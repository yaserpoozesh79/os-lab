using System.Net.Sockets;
using System.Text;
using System.Net;
using System.Text.Json;
using practice_5;

namespace TcpServerClient
{
    static class Server
    {
        public static void StartServer()
        {
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1"); 
            int port = 12345; 
            IPEndPoint serverEndPoint = new IPEndPoint(ipAddress, port);

            Socket serverSocket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            serverSocket.Bind(serverEndPoint);
            serverSocket.Listen(10);

            Console.WriteLine("Server is running and waiting for connections...");

            Socket clientSocket = serverSocket.Accept();
            Console.WriteLine("Client connected.");

            byte[] buffer = new byte[2*1024*1024];
            while (true)
            {
                int bytesReceived = clientSocket.Receive(buffer);
                string receivedData = Encoding.ASCII.GetString(buffer, 0, bytesReceived);
                if(receivedData.Equals("exit"))
                    break;

                string fileName;
                byte[] fileData;
                try
                {
                    MyFile fileObject = JsonSerializer.Deserialize<MyFile>(receivedData);
                    fileName = fileObject.name;
                    fileData = Convert.FromBase64String(fileObject.content);
                }
                catch (Exception)
                {
                    Console.WriteLine("Error: problem in the received message!!");
                    continue;
                }

                // Save the file to the local directory
                string localFilePath = Path.Combine(Environment.CurrentDirectory, fileName);
                File.WriteAllBytes(localFilePath, fileData);

                Console.WriteLine($"File '{fileName}' received and saved.");
            }

            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
            System.Console.WriteLine("server down.");
        }
    }
}