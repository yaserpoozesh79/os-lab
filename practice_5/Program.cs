using System;

namespace TcpServerClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Run Server or Client? [S/c]: ");
            string? input = Console.ReadLine()??"";

            if (input.ToLower().Equals("c"))
            {
                Client.StartClient();
            }
            else
            {
                Server.StartServer();
            }
        }
    }
}