using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace MTCG
{
    class RestServer
    {

        static void Main(string[] args)
        {
            TcpListener listener = new TcpListener(IPAddress.Loopback, 10001);
            listener.Start(5);

            List<string> messagesList = new List<string>();

            Console.CancelKeyPress += (sender, e) => Environment.Exit(0);

            while (true)
            {
                try
                {
                    var socket = listener.AcceptTcpClient();
                    var message = new MessageInput(socket, messagesList);
                    socket.Close();
                }
                catch (Exception exc)
                {
                    Console.WriteLine("error occurred: " + exc.Message);
                }
            }
        }
    }
}

