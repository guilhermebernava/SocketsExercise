using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Server
{
    internal class Program
    {
        #region//SEND AND RECEIVE MESSAGES
        public static async Task SendMessage(NetworkStream networkStream)
        {
            byte[] bufferSend = new byte[128];
            Console.WriteLine();
            Console.WriteLine("WRITE: ");
            var textSend = Console.ReadLine();
            bufferSend = Encoding.UTF8.GetBytes(textSend);
            await networkStream.WriteAsync(bufferSend, 0, bufferSend.Length);
        }
        private static async Task ReceivedMessage(NetworkStream networkStream)
        {
            byte[] bufferReceived = new byte[1024];
            Array.Clear(bufferReceived, 0, bufferReceived.Length);
            await networkStream.ReadAsync(bufferReceived, 0, bufferReceived.Length);
            string textReceived = Encoding.UTF8.GetString(bufferReceived);
            Console.Write($"Server: {textReceived}\n");
        }
        #endregion
        #region//START SERVER
        public static void StartServer()
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
            var networkStream = new NetworkStream(client);
            Console.WriteLine("CLIENT ACCEPT!");

            while (client.Connected)
            {
                var receive = Task.Run(() => (ReceivedMessage(networkStream)));
                var send = Task.Run(() => (SendMessage(networkStream)));
            }
        }
        #endregion
        
        static async Task Main(string[] args)
        {
            var Tclient = new Thread(StartServer);
            StartServer();
            Console.ReadLine();
        }
    }
}
