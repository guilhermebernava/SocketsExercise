using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IPEndPoint ipep = new IPEndPoint(IPAddress.Loopback, 9000);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Console.WriteLine("WAITING FOR A CLIENT......");
            try
            {
                socket.Bind(ipep);
                socket.Listen(2);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            var client = socket.Accept();
            Console.WriteLine("CLIENT ACCEPT!");
            byte[] bufferSend = new byte[1024];
            byte[] bufferReceived = new byte[1024];

            while (client.Connected)
            {
                 client.Receive(bufferReceived);

                 string textReceived = Encoding.UTF8.GetString(bufferReceived);

                Console.Write($"Client: {textReceived}");

                Array.Clear(bufferReceived, 0, bufferReceived.Length);
                Console.Write("WRITE: ");
                var textSend = Console.ReadLine();
                if(textSend == "exit")
                {
                    break;
                }
                bufferSend = Encoding.UTF8.GetBytes(textSend);
                client.Send(bufferSend);
                Array.Clear(bufferSend, 0, bufferSend.Length);
            }

            Console.WriteLine("CONNECTION IS FINISHED!");
            Console.ReadLine();
        }
    }
}
