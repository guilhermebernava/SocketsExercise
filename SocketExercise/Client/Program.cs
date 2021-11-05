using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IPEndPoint ipep = new IPEndPoint(IPAddress.Loopback, 9000);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Console.WriteLine("PRESS <ENTER> TO CONNECT WITH SERVER");
            Console.ReadLine();
            try
            {
                socket.Connect(ipep);
                Console.WriteLine("CONNECTION SUCESSFULL");
            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            byte[] bufferSend = new byte[1024];
            byte[] bufferReceived = new byte[1024];
            while (socket.Connected)
            {
                Console.Write("WRITE: ");
                var textSend = Console.ReadLine();
                bufferSend = Encoding.UTF8.GetBytes(textSend);
                socket.Send(bufferSend);
                Array.Clear(bufferSend, 0, bufferSend.Length);

                socket.Receive(bufferReceived);
                var textReceived = Encoding.UTF8.GetString(bufferReceived);
                Console.Write($"SERVER: {textReceived}"); 
            }
            Console.ReadLine();
        }
    }
}
