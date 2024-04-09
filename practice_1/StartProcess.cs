using System;
using System.Diagnostics;

namespace ActionsExecuter{
    class StartProcess : Action{

        public string getTitle(){
            return "start a new process";
        }

        public void execute(){
            Console.WriteLine("enter process name:");
            string? pName = Console.ReadLine();
            Process.Start(pName);
        }   
    }
}