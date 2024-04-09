using System;
using System.Diagnostics;
using System.Management;
using System.Text.RegularExpressions;

namespace ActionsExecuter{
    partial class GetProcessTree : Action{

        public string getTitle(){
            return "get process Tree";
        }

        public void execute(){
            Console.WriteLine("enter process id:");
            string? entry = Console.ReadLine();
            int pid = int.Parse(entry);
            int? parentId = getProcessParentId(pid);
            if(parentId==null){
                Console.WriteLine("could not find the parent!");
                return;
            }
            TreeNode<int> root = new((int)parentId);
            Tree<int> processTree = new(root);
            foreach(int siblingId in GetProcessChildren((int)parentId))
                root.AddChild(siblingId);

            Console.WriteLine(processTree);
        }

        private int? getProcessParentId(int pid)
        {
            string? parentId = getProcessProperty(pid,"parentprocessid");
            return parentId==null? null:int.Parse(parentId);
        }

        private string? getProcessProperty(int pid, string propertyName)
        {
            string cmdOutput = cmdExecute($"wmic process where processid={pid} get {propertyName}");
            Match propertyMatch = MyRegex().Match(cmdOutput);
            if (propertyMatch.Success)
                return propertyMatch.Groups[1].Value;
            else
                return null;
        }

        private int[] GetProcessChildren(int pid)
        {
            string cmdOutput = cmdExecute($"wmic process where parentprocessid={pid} get processId");
            MatchCollection matches = MyRegex().Matches(cmdOutput);
            int[] result = [0];
            if (matches.Count > 0){
                result = new int[matches.Count];
                for (int i = 0; i<result.Length; i++)
                    result[i] = int.Parse(matches[i].Groups[1].Value);
            }
            return result;
        }

        private string cmdExecute(string command)
        {
            Process cmd = new();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();
            cmd.StandardInput.WriteLine(command);
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
            return cmd.StandardOutput.ReadToEnd();
        }

        [GeneratedRegex(@"[\n|\r](\d+)")]
        private static partial Regex MyRegex();
    }
    
    public class TreeNode<T>{
        public T Value { get; set; }
        public List<TreeNode<T>> Children { get; set; }

        public TreeNode(T value)
        {
            Value = value;
            Children = [];
        }

        public TreeNode<T> AddChild(T childValue)
        {
            TreeNode<T> childNode = new TreeNode<T>(childValue);
            Children.Add(childNode);
            return childNode;
        }
    }

    public class Tree<T>(TreeNode<T> root)
    {
        public TreeNode<T> Root { get; } = root;

        public override string ToString(){
            return Tree<T>.ToString(Root, "");
        }

        private static string ToString(TreeNode<T> node, string indent)
        {
            string buffer = "";
            buffer+=node.Value.ToString();
            if(node.Children.Count > 0)
                buffer+="\n";
            indent += "|-";
            foreach (TreeNode<T> child in node.Children)
                buffer+= indent + Tree<T>.ToString(child, indent)+"\n";
            return buffer;
        }
    }

}