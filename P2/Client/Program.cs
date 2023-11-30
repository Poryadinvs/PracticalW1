using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ClientConnect();
        }

        static void ClientConnect()
        {
            TcpClient tcpClient = new TcpClient();

            try
            {
                tcpClient.Connect("127.0.0.1", 9000);
                Console.WriteLine("Connection to server...");

                while(true)
                {
                    NetworkStream networkStream = tcpClient.GetStream();
                    Console.WriteLine("Enter 'date' or 'time' for request");

                    string request = Console.ReadLine();

                    byte[] buffer = Encoding.UTF8.GetBytes(request);
                    networkStream.Write(buffer,0,buffer.Length);

                    buffer = new byte[1024];
                    int byteRead = networkStream.Read(buffer, 0, buffer.Length);
                    string response = Encoding.UTF8.GetString(buffer,0,byteRead);

                    Console.WriteLine($"Got from server: {response}");
                    //networkStream.Close();

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка клиента{ex.Message}");
            }
            //finally
            //{
            //    tcpClient.Close();
            //}
        }
    }
}
