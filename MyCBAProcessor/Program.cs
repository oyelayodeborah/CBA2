using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
//using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MyCBA.Core.Models;
using MyCBA.Data.Repositories;
using Trx.Messaging;
using Trx.Messaging.Channels;
using Trx.Messaging.FlowControl;
//using Trx.Messaging.FlowControl;
using Trx.Messaging.Iso8583;

namespace MyCBAProcessor
{
    class Program
    {
        public static Processor processor = new Processor();

        public static List<ClientPeer> ClientPeers = new List<ClientPeer>();

        public static SinkNodeRepository schemeRepo = new SinkNodeRepository();
        public static List<SinkNode> AllSinkNodes = new SinkNodeRepository().GetByStatus(Status.Active).ToList();


        static void Main(string[] args)
        {
            //InitializeTcpListener();

            //CbaListener.StartUpListener("1","192.168.43.160", 60012);
            //processor.BeginProcess();
            
            processor.BeginProcess();
            
            //InitializeTcpListener();
            // Console.WriteLine("Started the processor");
            Console.Read();
        }

        public static void ConnectSinkNode(int sinkNodeId)
        {
            try
            {
                SinkNode sink = new SinkNodeRepository().Get(sinkNodeId);
                CbaListener.ClientPeer(sink, ClientPeers);
                sink.Status = Status.Active;
                new SinkNodeRepository().Update(sink);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public static void ConnectSinkNode()
        {
            try
            {
                foreach (var sink in AllSinkNodes)
                {
                    CbaListener.ClientPeer(sink, ClientPeers);
                    sink.Status = Status.Active;
                    new SinkNodeRepository().Update(sink);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //private static void InitializeTcpListener()
        //{
        //  TcpListener server = null;
        //    try
        //    {
        //        // Set the TcpListener on port 13000. 
        //        //Int32 port = 60000;
        //        Int32 port = 60012;
        //        string ipAdd = "192.168.43.160";
        //        //Int32 port = 1500;
        //        IPAddress localAddr = IPAddress.Parse(ipAdd);
        //        //IPAddress localAddr = IPAddress.Parse("127.0.0.1");

        //        server = new /*System.Net.Sockets.*/TcpListener(localAddr, port);

        //        // Start listening for client requests.
        //        server.Start();
        //        Console.WriteLine($"Server with port: {port} and ipAddress: {ipAdd} has started");

        //        // Buffer for reading d ata
        //        Byte[] bytes = new Byte[256];
        //        String data = null;

        //        TcpClient client;

        //        // Enter the listening loop.
        //        while (true)
        //        {
        //            Console.WriteLine("Waiting for a connection... ");

        //            // Perform a blocking call to accept requests.
        //            // You could also user server.AcceptSocket() here.
        //            Thread.Sleep(2000);

        //            try
        //            {
        //                client = server.AcceptTcpClient();

        //            Console.WriteLine("Connected!");

        //            data = null;

        //            // Get a stream object for reading and writing
        //            NetworkStream stream = client.GetStream();

        //            int i;
        //            // Loop to receive all the data sent by the client.
        //            while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
        //            {
        //                // Translate data bytes to a ASCII string.
        //                data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
        //                Console.WriteLine("Received: {0}", data);

        //                // Process the data sent by the client.

        //                // Send back a response.
        //                byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);
        //                stream.Write(msg, 0, msg.Length);
        //                Console.WriteLine("Sent: {0}", data);

        //            }
        //            }
        //            catch (SocketException ex)
        //            {
        //                Console.WriteLine("Socket Exception: " + ex);
        //            }

        //        }
        //    }
        //    catch (SocketException e)
        //    {
        //        Console.WriteLine("SocketException: {0}", e);
        //    }
        //    finally
        //    {
        //        // Stop listening for new clients.
        //        server.Stop();
        //    }


        //    Console.WriteLine("\nHit enter to continue...");
        //    Console.Read();
        //}


    }
}
