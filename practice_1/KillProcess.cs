using System;
using System.Diagnostics;

namespace ActionsExecuter{
    class KillProcess : Action{

        public string getTitle(){
            return "kill a process";
        }

        public void execute(){
            Console.WriteLine("enter process id:");
            string? entry = Console.ReadLine();
            int pid = int.Parse(entry);
            Process.GetProcessById(pid).Kill();
        }   
    }
}