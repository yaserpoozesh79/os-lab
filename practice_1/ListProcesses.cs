using System;
using System.Diagnostics;

namespace ActionsExecuter{
    class ListProcesses : Action{

        private String processFormat = "{0, 6}|| {1,-40}";

        public string getTitle(){
            return "list all processes";
        }

        public void execute(){
            Process[] allProcesses = Process.GetProcesses();
            Console.WriteLine(String.Format("{0} processes found.\n", allProcesses.Length));
            Console.WriteLine(String.Format(processFormat+"\n", "PID", "Process Name"));
            foreach(Process p in allProcesses)
                Console.WriteLine(String.Format(processFormat+"\n", p.Id, p.ProcessName));
        }   
    }
}