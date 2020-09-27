using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TalkaTIPClientV2
{
    class Client
    {
        Socket socket;

        // Data buffer for incoming data
        byte[] bytes;

        public Client(string AddressIP)
        {
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse(AddressIP), 11000);

            // Create a TCP/IP socket
            socket = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

            // Connect the socket to the remote endpoint. Catch any errors
            try
            {
                socket.Connect(remoteEP);
            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                throw;
            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketException : {0}", se.ToString());
                throw;
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
                throw;
            }
        }

        public string Receive()
        {
            bytes = new byte[1024];
            int bytesRec = 0;
            string answer = string.Empty;
            try
            {
                while (!answer.Contains("<EOF>"))
                {
                    // Receive the response from the remote device
                    bytesRec = socket.Receive(bytes);
                    answer += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                }
            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                throw;
            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketException : {0}", se.ToString());
                throw;
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
                throw;
            }
            return answer;
        }

        public void Send(String data)
        {
            try
            {
                // Encode the data string into a byte array
                byte[] msg = Encoding.ASCII.GetBytes(data);

                // Send the data through the socket
                int bytesSent = socket.Send(msg);
            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                throw;
            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketException : {0}", se.ToString());
                throw;
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
                throw;
            }
        }

        public void Disconnect()
        {
            try
            {
                // Release the socket
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                throw;
            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketException : {0}", se.ToString());
                throw;
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
                throw;
            }
        }
    }
}
