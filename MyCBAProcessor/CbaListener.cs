using MyCBA;
using MyCBA.Core.Models;
using MyCBA.Data.Repositories;
using MyCBA.Logic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Trx.Messaging;
using Trx.Messaging.Channels;
using Trx.Messaging.FlowControl;
using Trx.Messaging.Iso8583; 

namespace MyCBAProcessor
{
    public class CbaListener
    {
        //List<AtmTerminal> ourTerminals;
        public enum MessageSource
        {
            OnUs, RemoteOnUs, NotOnUs,InvalidMsgSource
        }

        UtilityLogic utility = new UtilityLogic();
        static void Listener_Receive(object sender, ReceiveEventArgs e)
        {
            try
            {
                UtilityLogic.LogMessage("Message received!");
                var client = sender as ClientPeer;
                Iso8583Message msg = e.Message as Iso8583Message;
                PeerRequest request = null;
                string InstitutionCode1 = Convert.ToString(msg.Fields[32]);
                string InstitutionCode2 = Convert.ToString(msg.Fields[33]);
                string transactionTypeCode = msg.Fields[MessageField.TRANSACTION_TYPE_FIELD].ToString().Substring(0, 2);

                switch (GetTransactionSource(msg))
                {
                    case MessageSource.OnUs:

                        msg = TransactionManager.ProcessMessage(msg, MessageSource.OnUs);
                        request = new PeerRequest(client, msg);
                        break;
                    case MessageSource.RemoteOnUs:

                        //msg = TransactionManager.ProcessMessage(msg, MessageSource.RemoteOnUs);
                        //request = new PeerRequest(client, msg);
                        //do nothing yet
                        var listpeerr = sender as ListenerPeer;
                        msg = TransactionManager.ProcessMessageNotOnUs(msg);
                        request = new PeerRequest(listpeerr, msg);
                        break;
                    case MessageSource.InvalidMsgSource:
                        if (InstitutionCode2 == Processor.InstitutionCode)
                        {
                            msg.Fields.Add(39, ResponseCode.INVALID_TRANSACTION);
                            var listpeer = sender as ListenerPeer;
                            request = new PeerRequest(listpeer, msg);
                        }
                        else if (InstitutionCode1 == Processor.InstitutionCode)
                        {
                            msg.Fields.Add(39, ResponseCode.INVALID_TRANSACTION);

                        }
                        break;
                    case MessageSource.NotOnUs:
                        //redirect to interswitch
                        //msg.Fields.Add(39, "31");   //bank not supported
                        if (transactionTypeCode.Equals(TransactionTypeCode.CASH_WITHDRAWAL) || transactionTypeCode.Equals(TransactionTypeCode.INTER_BANK_TRANSFER)
                            || transactionTypeCode.Equals(TransactionTypeCode.BALANCE_ENQUIRY))
                        {
                            if (InstitutionCode1 != null && InstitutionCode2 != null)
                            {
                                if (InstitutionCode2 == Processor.InstitutionCode)
                                {
                                    var listpeer = sender as ListenerPeer;
                                    msg = TransactionManager.ProcessMessageNotOnUs(msg);
                                    request = new PeerRequest(listpeer, msg);
                                }
                                else if (InstitutionCode1 == Processor.InstitutionCode)
                                {
                                    if (msg.MessageTypeIdentifier == 420)
                                    {
                                        msg.MessageTypeIdentifier = 430;
                                    }
                                    else if (msg.MessageTypeIdentifier == 200)
                                    {
                                        msg.MessageTypeIdentifier = 210;
                                    }
                                    msg.Fields.Add(39, ResponseCode.SUCCESS);
                                    Program.ConnectSinkNode();
                                    var sinkNode = new SinkNodeRepository().GetAll().ToList().FirstOrDefault();
                                    var clientPeer = Program.ClientPeers.Where(p => p.Name.Equals(sinkNode.Id.ToString())).FirstOrDefault();
                                    if (clientPeer == null)
                                    {
                                        ErrorLogger.LogMessage("Clientpeer is null");
                                        msg = TransactionManager.SetReponseMessage(msg, ResponseCode.ISSUER_OR_SWITCH_INOPERATIVE.ToString());
                                        SendMessage(client, msg);
                                    }
                                    //INVALID TRANSACTION
                                    TransactionManager.SendMessageToFEP(clientPeer, msg);
                                    string responseCode = Convert.ToString(msg.Fields[39]);
                                    if (responseCode == "00")
                                    {
                                        msg = TransactionManager.ProcessMessageNotOnUs(msg);
                                    }
                                    request = new PeerRequest(client, msg);

                                }
                            }
                        }
                        else
                        {
                            msg.Fields.Add(39, ResponseCode.INVALID_TRANSACTION);
                            var listpeer = sender as ListenerPeer;
                            request = new PeerRequest(listpeer, msg);
                        }
                        break;
                    default:
                        break;
                }
                request.Send();
                client.Close();
                client.Dispose();
            }
            catch (Exception ex)
            {
                UtilityLogic.LogError("Error processing the incoming meaasgae");
                UtilityLogic.LogError("Message: " + ex.Message + " \t InnerException " + ex.InnerException);
            }
        }

        static MessageSource GetTransactionSource(Iso8583Message msg)
        {
            string ourBIN = "111122";
            //string ourBIN = "506124";
            //string ourBIN = "519894";
            //string ourTerminalId = "60";
            string InstitutionCode1 = Convert.ToString(msg.Fields[32]);
            string InstitutionCode2 = Convert.ToString(msg.Fields[33]);
            string msgBin = msg.Fields[2].ToString().Substring(0, 6);   //first 6 digits of the card PAN
            string msgTerminalId = msg.Fields[41].ToString();  //.Substring(2, 2);   //the third and first digits

            if (msgBin.Equals(ourBIN) && IsValidTerminal(msgTerminalId))   //our terminal, our card
            {
                return MessageSource.OnUs;
            }
            else if (msgBin.Equals(ourBIN) && InstitutionCode2 == Processor.InstitutionCode)     //our card, not our terminal
            {
                return MessageSource.RemoteOnUs;
            }
            else if (IsValidTerminal(msgTerminalId) && InstitutionCode1 == Processor.InstitutionCode)
            {
                return MessageSource.NotOnUs;
            }
            else if (InstitutionCode2 == Processor.InstitutionCode)     //inst2 is our institutioncode
            {
                return MessageSource.RemoteOnUs;
            }
            else
            {
                return MessageSource.InvalidMsgSource;
            }
        }

        static bool IsValidTerminal(string terminalCode)
        {
            return Processor.OurTerminals.Any(t => t.Code == terminalCode);
        }

        static void Listener_Connected(object sender, EventArgs e)
        {
            var listener = sender as ListenerPeer;
            UtilityLogic.LogMessage(listener.Name + " is now connected");
            //Console.WriteLine("Client Connected!");
        }
        static void Listener_Disconnected(object sender, EventArgs e)
        {
            var listener = sender as ListenerPeer;
            UtilityLogic.LogMessage(listener.Name + " is disonnected");
            //Console.WriteLine("Client Disconnected!");
        }

        public static void StartUpListener(string name, string hostName, int port)     //create conn5
        {
            //Trx.Messaging.FlowControl.
            TcpListener tcpListener = new TcpListener(port);
            tcpListener.LocalInterface = hostName;
            ListenerPeer listener = new ListenerPeer(name,
                     new TwoBytesNboHeaderChannel(new Iso8583Ascii1987BinaryBitmapMessageFormatter()),
                     new BasicMessagesIdentifier(11, 41),
                     tcpListener);
            listener.Receive += new PeerReceiveEventHandler(Listener_Receive);
            listener.Connected += new PeerConnectedEventHandler(Listener_Connected);
            listener.Disconnected += new PeerDisconnectedEventHandler(Listener_Disconnected);
            listener.Connect();

            Console.WriteLine("Waiting for connection...");
        }

        static void Client_RequestCancelled(object sender, PeerRequestCancelledEventArgs e)
        {
            ///
            Console.WriteLine("Peer request cancelled");
        }

        static void Client_RequestDone(object sender, PeerRequestDoneEventArgs e)
        {
            //
            Console.WriteLine("Peer request done");
        }


        public static void ClientPeer(SinkNode sinkNodeToConnect, List<ClientPeer> clientPeers)    //join conn
        {
            ClientPeer client = new ClientPeer(sinkNodeToConnect.Id.ToString(),
                                new TwoBytesNboHeaderChannel(new Iso8583Ascii1987BinaryBitmapMessageFormatter(), sinkNodeToConnect.IPAddress, Convert.ToInt32(sinkNodeToConnect.Port)), new BasicMessagesIdentifier(11, 41));
            client.RequestDone += new PeerRequestDoneEventHandler(Client_RequestDone);
            client.RequestCancelled += new PeerRequestCancelledEventHandler(Client_RequestCancelled);
            client.Connected += client_Connected;
            client.Receive += new PeerReceiveEventHandler(Client_Receive);
            clientPeers.Add(client);
            client.Connect();

            Console.WriteLine("Waiting for connection..");
        }
        static void Client_Receive(object sender, ReceiveEventArgs e)
        {
            ClientPeer clientPeer = sender as ClientPeer;
            //logger.Log("Connected to ==> " + clientPeer.Name);

            Iso8583Message receivedMsg = e.Message as Iso8583Message;
        }
        static void client_Connected(object sender, EventArgs e)
        {
            Console.WriteLine("Client Connected");
        }

        static Message SendMessage(Peer peer, Message msg)
        {
            int maxRetries = 3; int numberOfRetries = 1;
            while (numberOfRetries < maxRetries)
            {
                if (peer.IsConnected)
                {
                    break;
                }
                peer.Close();
                numberOfRetries++;
                peer.Connect();
                Thread.Sleep(2000);
            }

            if (peer.IsConnected)
            {
                try
                {
                    var request = new PeerRequest(peer, msg);

                    request.Send();

                    //At this point, the message has been sent to the SINK for processing
                    int serverTimeout = 100000000;
                    request.WaitResponse(serverTimeout);

                    var response = request.ResponseMessage;
                    return response;
                }
                catch (Exception ex)
                {
                    msg.Fields.Add(39, "06"); // ERROR
                    ErrorLogger.LogError("Error sending message " + ex.Message + "   Inner Exception:  " + ex.InnerException);
                    return msg;
                }
            }
            else
            {
                msg.Fields.Add(39, ResponseCode.ISSUER_OR_SWITCH_INOPERATIVE); // ERROR
                return msg;
            }

        }

    }
}
