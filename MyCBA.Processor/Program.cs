using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Trx.Messaging;
using Trx.Messaging.Channels;
using Trx.Messaging.FlowControl;
using Trx.Messaging.Iso8583;

namespace MyCBAProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            // CbaListener.StartUpListener("1","192.168.43.160", 61004);
            //InitializeTcpListener();
            Console.Read();
        }

        private static void InitializeTcpListener()
        {
            System.Net.Sockets.TcpListener server = null;
            try
            {
                // Set the TcpListener on port 13000.
                Int32 port = 61004;
                string ipAdd = "192.168.43.160";
                //Int32 port = 1500;
                IPAddress localAddr = IPAddress.Parse(ipAdd);
                //IPAddress localAddr = IPAddress.Parse("127.0.0.1");

                server = new System.Net.Sockets.TcpListener(localAddr, port);

                // Start listening for client requests.
                server.Start();

                // Buffer for reading data
                Byte[] bytes = new Byte[256];
                String data = null;

                // Enter the listening loop.
                while (true)
                {
                    Console.Write("Waiting for a connection... ");

                    // Perform a blocking call to accept requests.
                    // You could also user server.AcceptSocket() here.
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");

                    data = null;

                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();

                    int i;
                    // Loop to receive all the data sent by the client.
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        Console.WriteLine("Received: {0}", data);

                        // Process the data sent by the client.

                        // Send back a response.
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);
                        stream.Write(msg, 0, msg.Length);
                        Console.WriteLine("Sent: {0}", data);

                    }

                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
            }


            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
        }

       
    }
}
