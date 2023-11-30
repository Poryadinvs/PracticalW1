using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PracticalW1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            StartServer();
        }

        static void StartServer()
        {
            TcpListener tcpListener = new TcpListener(IPAddress.Any, 9000);
            try
            {
                tcpListener.Start();
                Console.WriteLine("Start");
                Console.WriteLine("Wait connection...");

                while(true)
                {
                    TcpClient tcpClient = tcpListener.AcceptTcpClient();
                    Console.WriteLine("Client is connected");

                    try
                    {
                        NetworkStream networkStream = tcpClient.GetStream();

                        byte[] buffer = new byte[1024];
                        int bytesRead = networkStream.Read(buffer, 0, buffer.Length);
                        string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                        Console.WriteLine($"В {DateTime.Now:t} от {((IPEndPoint)tcpClient.Client.RemoteEndPoint).Address} получена строка: {message}");

                        string serponseMessage = "Hi, client !";
                        byte[] responseBuffer = Encoding.UTF8.GetBytes(serponseMessage);
                        networkStream.Write(responseBuffer, 0, responseBuffer.Length);

                        Console.WriteLine("Response sent to client");

                        networkStream.Close();
                        tcpClient.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка обработки клиента: {ex.Message}");
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Ошибка запуска сервера: {ex.Message}");
            }
            finally
            {
                tcpListener.Stop();
            }
        }
    }
}
