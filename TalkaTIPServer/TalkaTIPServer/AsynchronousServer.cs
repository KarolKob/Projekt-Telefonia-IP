using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Timers;

namespace TalkaTIPSerwer
{
    class AsynchronousServer
    {
        private static System.Timers.Timer serverTimer;
        private static Socket messageSocket = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

        private static void SetTimer()
        {
            // Create a timer with a 61 seconds interval
            serverTimer = new System.Timers.Timer(61000);

            // Hook up the Elapsed event for the timer 
            serverTimer.Elapsed += OnTimedEvent;
            serverTimer.AutoReset = true;
            serverTimer.Enabled = true;
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            DateTime time = DateTime.Now;
            List<long> usersIDToRemove = new List<long>();
            foreach (var item in Program.onlineUsers)
            {
                if ((time - item.Value.iAM).TotalSeconds > 60)
                {
                    usersIDToRemove.Add(item.Key);
                }
            }
            foreach (var item in usersIDToRemove)
            {
                Program.onlineUsers.Remove(item);
            }
        }

        // Thread signal
        public static ManualResetEvent allDone = new ManualResetEvent(false);

        public static void StartListening()
        {
            // Data buffer for incoming data
            byte[] bytes = new Byte[1024];

            // Establish the local endpoint for the socket
            // The DNS name of the computer
            IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            int i = 0;
  
            Console.WriteLine("Choose the server IPv4 address:");

            foreach (var item in ipHostInfo.AddressList)
            {
                Console.WriteLine(i + " - " + item);
                i++;
            }

            Console.WriteLine(i + " - " + IPAddress.Loopback);
            i++;

            do
            {
                Console.Write("Type: ");
                i = (int)(Console.ReadKey().KeyChar) - 48;
                Console.WriteLine();
            } while (i < 0 || i >= ipHostInfo.AddressList.Count() + 1);
            
            IPEndPoint localEndPoint;
            IPAddress ipAddress;


            if (i == ipHostInfo.AddressList.Count())
            {
                ipAddress = IPAddress.Loopback;
                localEndPoint = new IPEndPoint(ipAddress, 11000);
            }
            else
            {
                ipAddress = ipHostInfo.AddressList[i];
                localEndPoint = new IPEndPoint(ipAddress, 11000);
            }

            // Create a TCP/IP socket
            Socket listener = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);

                Console.WriteLine("Server is running (" + ipAddress + ")");
                SetTimer();
                while (true)
                {
                    // Set the event to nonsignaled state
                    allDone.Reset();

                    // Start an asynchronous socket to listen for connections          
                    listener.BeginAccept(
                        new AsyncCallback(AcceptCallback),
                        listener);

                    // Wait until a connection is made before continuing
                    allDone.WaitOne();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();

        }

        public static void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue
            allDone.Set();

            // Get the socket that handles the client request
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            // Create the state object
            StateObject state = new StateObject();
            state.workSocket = handler;
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReadCallback), state);
        }

        public static void ReadCallback(IAsyncResult ar)
        {
            String content = String.Empty;

            // Retrieve the state object and the handler socket from the asynchronous state object
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;
            byte[] sessionKey = null;
            int bytesRead = 0;

            // Read data from the client socket
            bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                // There  might be more data, so store the data received so far
                state.sb.Append(Encoding.ASCII.GetString(
                    state.buffer, 0, bytesRead));

                // Check for end-of-file tag. If it is not there, read more data
                content = state.sb.ToString();
                if (content.IndexOf("<EOF>") > -1)
                {
                    // All the data has been read from the client
                    // Display it on the console
                    Console.WriteLine("Read {0} bytes from socket. \nData : {1}", content.Length, content);
                    string messageBits = Utilities.getBinaryMessage(content);

                    // Take 8 bits to recognize the communique
                    int bits8 = Convert.ToInt32(messageBits.Substring(0, 8), 2);    // Decimal value

                    if (bits8 == 17)
                    {
                        sessionKey = Program.security.SetSessionKey(Convert.FromBase64String(content.Substring(2, content.Length - 8)));
                        state.sessionKey = sessionKey;
                        Send(handler, (char)17 + " " + Convert.ToBase64String(Program.security.GetOwnerPublicKey().ToByteArray()) + " <EOF>");
                        state.buffer = new byte[1024];
                        state.sb = new StringBuilder();
                        handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                        new AsyncCallback(ReadCallback), state);
                    }
                    else
                    {
                        content = Communication.ChooseCommunique(content, state.sessionKey, handler);

                        // Echo the data back to the client
                        if (bits8 != 2 && bits8 != 11)
                        {
                            Send(handler, content);
                        }

                        if (bits8 == 1 && content == ((char)5).ToString() + " <EOF>") // logIn
                        {

                            string userAddressIP = ((IPEndPoint)handler.RemoteEndPoint).Address.ToString();
                            long userID = Communication.getUserIDHavingAdressIP(userAddressIP);
                            if (userID == -1)
                            {
                                Send(handler, Communication.Fail());
                            }
                            {
                                Send(handler, Communication.LogIP(userID));     // Data about the friend
                                Thread.Sleep(250);
                                Send(handler, Communication.History(userID));   // userID history
                            }
                        }

                        Thread.Sleep(100);
                        handler.Shutdown(SocketShutdown.Both);
                        handler.Close();

                        if (bits8 == 0)
                        {

                        }
                    }

                }
                else
                {
                    // Not all data received - get more
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReadCallback), state);
                }
            }
        }

        private static void Send(Socket handler, string data)
        {
            // Convert the string data to byte data using ASCII encoding
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            // Begin sending the data to the remote device
            handler.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), handler);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device
                int bytesSent = handler.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to client.", bytesSent);


            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static void SendMessage(string message, EndPoint sendToEP)
        {
            // TODO: Send message to a client available on the online list in ClientClass
            try
            {
                byte[] msg = Encoding.ASCII.GetBytes(message);

                // Send the message asynchronously
                messageSocket.BeginSendTo(msg, 0, message.Length, SocketFlags.None, sendToEP, new AsyncCallback(OnSend), null);
            }
            catch (Exception)
            {
                
            }
        }

        private static void OnSend(IAsyncResult ar)
        {
            try
            {
                messageSocket.EndSendTo(ar);
            }
            catch (Exception)
            {
                
            }
        }
    }

    class StateObject
    {
        // Client  socket
        public Socket workSocket = null;

        // Size of receive buffer
        public const int BufferSize = 1024;

        // Receive buffer
        public byte[] buffer = new byte[BufferSize];

        // Received data string
        public StringBuilder sb = new StringBuilder();
        public byte[] sessionKey = null;
    }
}
