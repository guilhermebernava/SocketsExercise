using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Client
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
        #endregion //SEND AND RECEIVE MESSAGES
        #region//START CLIENT
        static void StartClient()
        {
            IPEndPoint ipep = new IPEndPoint(IPAddress.Loopback, 9000);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Console.WriteLine("PRESS <ENTER> TO CONNECT WITH SERVER");
            Console.ReadLine();
            try
            {
                socket.Connect(ipep);
                Console.WriteLine("CONNECTION SUCESSFULL");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            var networkStream = new NetworkStream(socket);
            while (socket.Connected)
            {
                var receive = Task.Run(() => (ReceivedMessage(networkStream)));
                var send = Task.Run(() => (SendMessage(networkStream)));
            }

        }
        #endregion //START CLIENT
        static void Main(string[] args)
        {
            var Tclient = new Thread(StartClient);
            StartClient();
            Console.ReadLine();
        }
    }
}
