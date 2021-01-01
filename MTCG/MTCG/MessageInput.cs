using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace MTCG
{
    class MessageInput
    {

        public MessageInput(TcpClient socket, List<string> messagesList)
        {
            string message = "";
            var stream = socket.GetStream();
            Thread.Sleep(200);

            while (stream.DataAvailable)
            {
                Byte[] bytes = new Byte[4096];
                int i = stream.Read(bytes, 0, bytes.Length);
                message += System.Text.Encoding.ASCII.GetString(bytes, 0, i);
            }

            Console.WriteLine("received: " + message);
            Console.WriteLine("-------------------------");

            var requestContext = new RequestContext(message, messagesList);
            using var writer = new StreamWriter(socket.GetStream()) { AutoFlush = true };
            writer.WriteLine(requestContext.Response);
        }
    }
}