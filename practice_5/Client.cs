using System.Net.Sockets;
using System.Text;
using System;
using System.Net.Mime;
using System.IO;
using System.Text.Json;
using practice_5;

namespace TcpServerClient
{
    class Client
    {
        public static void StartClient()
        {
            string serverIp = "127.0.0.1";
            int port = 12345;

            Socket clientSocket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                clientSocket.Connect(serverIp, port);
                Console.WriteLine("Connected to server.");

                while (true)
                {
                    Console.Write("Enter a file path to transfer (or 'exit' to quit): ");
                    string input = Console.ReadLine()??"";
                    if (input.ToLower().Equals("exit")){
                        clientSocket.Send(
                            Encoding.UTF8.GetBytes(input)
                        );
                        break;
                    }
                    if(!File.Exists(input)){
                        Console.WriteLine("could not find the file!!");
                        continue;
                    }
                    
                    byte[] fileData = File.ReadAllBytes(input);
                    string fileName = Path.GetFileName(input);
                    MyFile fileObject = new();
                    fileObject.name = fileName;
                    fileObject.content = Convert.ToBase64String(fileData);

                    string jsonData = JsonSerializer.Serialize(fileObject);
                    byte[] messageBytes = Encoding.ASCII.GetBytes(jsonData);
                    clientSocket.Send(messageBytes);
                }

                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
                System.Console.WriteLine("client down.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}