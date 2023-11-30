using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            StartClient();
        }

        static void StartClient()
        {
            TcpClient tcpClient = new TcpClient();

            try
            {
                tcpClient.Connect("127.0.0.1", 9000);
                Console.WriteLine("Connecting...");

                NetworkStream networkStream = tcpClient.GetStream();

                string message = "Hi, server!";
                byte[] buffer = Encoding.UTF8.GetBytes(message);
                networkStream.Write(buffer, 0, buffer.Length);

                Console.WriteLine("Sending message to server...");

                buffer = new byte[1024];
                int byteRead = networkStream.Read(buffer, 0, buffer.Length);
                string responseMessage = Encoding.UTF8.GetString(buffer, 0, byteRead);

                Console.WriteLine($"В {DateTime.Now:t} от {((System.Net.IPEndPoint)tcpClient.Client.RemoteEndPoint).Address} получена строка: {responseMessage}");

                networkStream.Close();
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Ошибка клиента: {ex.Message}");
            }
            finally { tcpClient.Close(); }
        }
    }
}
