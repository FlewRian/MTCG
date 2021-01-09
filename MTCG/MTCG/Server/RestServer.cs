using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace MTCG
{
    class RestServer
    {
        static readonly SemaphoreSlim UserThreads = new SemaphoreSlim(2);

        static void Main(string[] args)
        {
            TcpListener listener = new TcpListener(IPAddress.Loopback, 10001);
            listener.Start(5);
            var tasks = new List<Task>();
            Console.CancelKeyPress += (sender, e) => Environment.Exit(0);

            while (true)
            {
                try
                {
                    UserThreads.Wait();
                    tasks.Add(Task.Run(() =>  ConnectUsers(listener)));
                }
                catch (Exception exc)
                {
                    Console.WriteLine("error occurred: " + exc.Message);
                }
            }
        }

        private static void ConnectUsers(TcpListener listener)
        {
            //Console.WriteLine("Hallo ich bin ein Client");
            List<string> messagesList = new List<string>();
            var socket = listener.AcceptTcpClient();
            var message = new MessageInput(socket, messagesList);
            socket.Close();
            UserThreads.Release();
        }
    }
}

