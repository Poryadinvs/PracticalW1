using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PracticalW2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ServerConnect();
        }

        static void ServerConnect()
        {
            TcpListener tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 9000);
            try
            {
                tcpListener.Start();
                Console.WriteLine("Start");
                Console.WriteLine("Wait to connection...");

                while(true)
                {
                    TcpClient tcpClient = tcpListener.AcceptTcpClient();
                    Console.WriteLine("Client is accepted");

                    try
                    {
                        NetworkStream networkStream = tcpClient.GetStream();

                        byte[] buffer = new byte[1024];
                        int byteRead = networkStream.Read(buffer, 0, buffer.Length);
                        string request = Encoding.UTF8.GetString(buffer,0,byteRead);

                        string response = GetResponse(request);

                        byte[] responseBuffer = Encoding.UTF8.GetBytes(response);
                        networkStream.Write(responseBuffer, 0, responseBuffer.Length);
                        Console.WriteLine("Response sent to client");

                        networkStream.Close();
                        tcpClient.Close();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка обработки клиента:{ex.Message}");
                    }

                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Ошибка запуска: {ex.Message}");
            }
            finally
            {
                tcpListener.Stop();
            }
        }
        
        static string GetResponse(string request)
        {
            while(true)
            {
                if (request.ToLower() == "date")
                {
                    return DateTime.Now.ToString("yyy-MM-dd");
                }
                else if (request.ToLower() == "time")
                {
                    return DateTime.Now.ToString("HH:mm:ss");
                }
                else
                {
                    return "Invalid request. Use 'date' or 'time'";
                }
            }
        }

    }
}
