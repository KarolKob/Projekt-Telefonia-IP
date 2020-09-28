using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Windows.Forms;
using System.Diagnostics;
using NAudio.Wave;

namespace TalkaTIPClientV2
{
    class CallHandler
    {
        private AutoResetEvent autoResetEvent;
        private WaveFormat waveFormat;
        private int bufferSize;
        private UdpClient udpClient;                // Listens and sends data on port 1550, used in synchronous mode
        public Socket clientSocket;
        private volatile bool bStop;                // Flag to end the Start and Receive threads
        private IPEndPoint otherPartyIP;            // IP of the party we want to make a call with
        private EndPoint otherPartyEP;
        private volatile bool bIsCallActive;        // Tells whether we have an active call
        private Vocoder vocoder;
        private byte[] byteData = new byte[1024];   // Buffer to store the data received
        private volatile int nUdpClientFlag;        // Flag used to close the udpClient socket
        public System.Media.SoundPlayer player;

        private WaveIn recorder;
        private BufferedWaveProvider bufferedWaveProvider;
        private WaveOut voicePlayer;

        private Thread senderThread;
        private Thread receiverThread;

        public bool IsInCall() { return bIsCallActive;  }

        public CallHandler()
        {
            Initialize();
        }

        /*
         * Initializes all the data members.
         */
        private void Initialize()
        {
            try
            {
                short channels = 1; // Stereo
                short bitsPerSample = 16; // 16bit, alternatively use 8Bits
                int samplesPerSecond = 22050; // 11KHz use 11025 , 22KHz use 22050, 44KHz use 44100 etc

                // Set up the wave format to be captured
                waveFormat = new WaveFormat(samplesPerSecond, bitsPerSample, channels);

                // Set up the recorder
                recorder = new WaveIn();
                recorder.DataAvailable += Send;
                recorder.WaveFormat = waveFormat;

                // Set up the signal chain
                bufferedWaveProvider = new BufferedWaveProvider(waveFormat);

                // Set up voice player
                voicePlayer = new WaveOut();
                voicePlayer.Init(bufferedWaveProvider);

                bIsCallActive = false;
                nUdpClientFlag = 0;

                // Using UDP sockets
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                clientSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                EndPoint ourEP = new IPEndPoint(IPAddress.Any, 14450);

                // Listen asynchronously on port 14450 for coming messages
                clientSocket.Bind(ourEP);

                // Receive data from any IP
                EndPoint remoteEP = (new IPEndPoint(IPAddress.Any, 0));

                byteData = new byte[1024];

                // Receive data asynchornously
                clientSocket.BeginReceiveFrom(byteData,
                                           0, byteData.Length,
                                           SocketFlags.None,
                                           ref remoteEP,
                                           new AsyncCallback(OnReceive),
                                           null);
            }
            catch (Exception)
            {
                MessageBox.Show("callHandler init error!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Call()
        {
            try
            {
                // Get the IP we want to call 
                string ipAddress = Program.mainWindow.listView1.SelectedItems[0].SubItems[1].Text.ToString();
                otherPartyIP = new IPEndPoint(IPAddress.Parse(ipAddress), 14450);
                otherPartyEP = (EndPoint)otherPartyIP;

                // Get the vocoder to be used
                    vocoder = Vocoder.None;

                // Send an invite message
                char comm = (char)10;
                string message = comm + " " + Program.userLogin + " " + vocoder + " <EOF>";

                SendMessage(message, otherPartyEP);
                player = new System.Media.SoundPlayer();
                player.Stream = Properties.Resources.WaitSound;
                player.PlayLooping();
            }
            catch (Exception)
            {
                MessageBox.Show("Connection error!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void CancelCall()
        {
            try
            {
                string message = (char)6 + " <EOF>"; // FAIL
                SendMessage(message, otherPartyEP);
                Program.mainWindow.gbInCall.Visible = false;
                player.Stop();
            }
            catch (Exception)
            {
                MessageBox.Show("Connection error!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnSend(IAsyncResult ar)
        {
            try
            {
                clientSocket.EndSendTo(ar);
            }
            catch (Exception)
            {
                //MessageBox.Show("Packet sending error!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /*
         * Commands are received asynchronously.
         * OnReceive is the handler for them.
         */
        private void OnReceive(IAsyncResult ar)
        {
            try
            {
                EndPoint receivedFromEP = new IPEndPoint(IPAddress.Any, 0);

                // Get the IP from where we got a message
                clientSocket.EndReceiveFrom(ar, ref receivedFromEP);

                // Convert the bytes received into an object of type Data
                string message = Encoding.ASCII.GetString(byteData);

                // Act according to the received message
                switch (message[0])
                {
                    // Incoming call
                    case (char)10:
                        {
                            if (Program.mainWindow.gbCall.Visible == true)
                            {
                                Program.mainWindow.Invoke((MethodInvoker)delegate
                                {
                                    //Program.mainWindow.timerCallOut.Stop();
                                    Program.mainWindow.gbCall.Visible = false;
                                    player.Stop();
                                });
                            }
                            player = new System.Media.SoundPlayer();
                            player.Stream = Properties.Resources.CallSound;
                            player.PlayLooping();
                            string msgTmp = string.Empty;
                            if (bIsCallActive == false)
                            {
                                string[] msgTable = message.Split(' ');

                                Program.mainWindow.Invoke((MethodInvoker)delegate
                                {
                                    Program.mainWindow.gbAnswerCall.Visible = true;
                                    Program.mainWindow.labelCallingUN.Text = msgTable[1];
                                    //Program.mainWindow.gBChangePass.Enabled = false;
                                    //Program.mainWindow.gBDelAcc.Enabled = false;
                                });
                                Program.mainWindow.callerEndPoint = receivedFromEP;
                                Program.mainWindow.isReceiver = true;
                                //Program.mainWindow.Refresh();
                            }
                            else
                            {
                                DeclineCall(receivedFromEP);
                            }
                            break;
                        }

                    // OK is received in response to an Invite
                    case (char)5:
                        {
                            // Start a call
                            Program.mainWindow.Invoke((MethodInvoker)delegate
                            {
                                //Program.mainWindow.timerCallOut.Stop();
                            });
                            InitializeCall();
                            break;
                        }

                    // Remote party is busy
                    case (char)6: //FAIL
                        {
                            player.Stop();

                            // Send msg to DB with history
                            string[] historyDetails = new string[3];
                            if (Program.mainWindow.isReceiver == false) // Somebody calls - user declines
                            {
                                Program.client = new Client(Program.serverAddress);
                                Program.mainWindow.Invoke((MethodInvoker)delegate
                                {
                                    //Program.mainWindow.timerCallOut.Stop();
                                    Communication.CallState(Program.userLogin, Program.mainWindow.listView1.SelectedItems[0].Text, DateTime.Now, TimeSpan.Zero);
                                    historyDetails[0] = Program.mainWindow.listView1.SelectedItems[0].Text;
                                });
                                Program.client.Disconnect();
                            }
                            else
                            {
                                historyDetails[0] = Program.mainWindow.labelCallingUN.Text;
                            }

                            Program.mainWindow.isReceiver = false;

                            // Refresh user panel with histories
                            historyDetails[1] = DateTime.Now.ToString();
                            historyDetails[2] = "missed call";
                            Program.mainWindow.listView2.Items.Insert(0, (new ListViewItem(historyDetails)));
                            Program.mainWindow.listView2.Refresh();

                            // Close ring panel
                            Program.mainWindow.Invoke((MethodInvoker)delegate
                            {
                                Program.mainWindow.gbAnswerCall.Visible = false;
                            });
                            if (Program.mainWindow.gbCall.Visible == true)
                            {
                                Program.mainWindow.Invoke((MethodInvoker)delegate
                                {
                                    Program.mainWindow.gbCall.Visible = false;
                                });
                                //CancelCall();
                                MessageBox.Show("Call denied.", "TalkaTIP", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }
                            break;
                        }

                    case (char)12: // BYE
                        {
                            // Check if the Bye command has come from the user/IP with which we have an established call.
                            // This is used to prevent other users from sending a Bye, which would otherwise end the call
                            if (receivedFromEP.Equals(otherPartyEP) == true)
                            {
                                //Program.mainWindow.timerConv.Stop();

                                // End the call
                                UninitializeCall();
                            }
                            break;
                        }
                }

                byteData = new byte[1024];
                // Get ready to receive more commands
                clientSocket.BeginReceiveFrom(byteData, 0, byteData.Length, SocketFlags.None, ref receivedFromEP, new AsyncCallback(OnReceive), null);
            }
            catch (Exception)
            {
                //MessageBox.Show("Packet recieving error!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void AcceptCall(EndPoint receivedFromEP)
        {
            string msgTmp = (char)5 + " <EOF>";
            SendMessage(msgTmp, receivedFromEP);
            vocoder = Vocoder.None;
            otherPartyEP = receivedFromEP;
            otherPartyIP = (IPEndPoint)receivedFromEP;
            InitializeCall();
        }

        public void DeclineCall(EndPoint receivedFromEP)
        {
            player.Stop();
            string msgTmp = (char)6 + " <EOF>"; // FAIL
            SendMessage(msgTmp, receivedFromEP);

            // Caller date state
            string[] historyDetails = new string[3];
            historyDetails[0] = Program.mainWindow.labelCallingUN.Text; // Get the user who initialized call
            historyDetails[1] = DateTime.Now.ToString();
            historyDetails[2] = "missed call";
            Program.mainWindow.listView2.Items.Insert(0, (new ListViewItem(historyDetails)));
            Program.mainWindow.listView2.Refresh();
        }

        /*
         * Send synchronously sends data captured from microphone across the network on port 1550.
         */
        private void Send(object sender, WaveInEventArgs waveInEventArgs)
        {
            try
            {
                recorder.StartRecording();

                bStop = false;
                while (!bStop && Process.GetCurrentProcess().MainWindowHandle != IntPtr.Zero)
                {
                    autoResetEvent.WaitOne();

                    byte[] dataToWrite = waveInEventArgs.Buffer;
                    udpClient.Send(dataToWrite, dataToWrite.Length, otherPartyIP.Address.ToString(), 1550);
                }
            }
            catch (Exception)
            {
                //MessageBox.Show("Packet sending error!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Increment flag by one
                nUdpClientFlag += 1;

                // When flag is two then it means we have got out of loops in Send and Receive
                while (nUdpClientFlag != 2)
                { }

                // Clear the flag
                nUdpClientFlag = 0;

                // Close the socket
                udpClient.Close();

                recorder.StopRecording();
            }
        }

        /*
         * Receive audio data coming on port 1550 and feed it to the speakers to be played.
         */
        private void Receive()
        {
            try
            {
                bStop = false;
                IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);

                while (!bStop && Process.GetCurrentProcess().MainWindowHandle != IntPtr.Zero)
                {
                    // Receive data and put it into the buffer used by voicePlayer
                    byte[] byteData = udpClient.Receive(ref remoteEP);
                    bufferedWaveProvider.AddSamples(byteData, 0, byteData.Length);
                }
            }
            catch (Exception)
            {
                //MessageBox.Show("Packet recieving error!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                voicePlayer.Stop();
                nUdpClientFlag += 1;
            }
        }

        private void UninitializeCall()
        {
            Program.mainWindow.Invoke((MethodInvoker)delegate
            {
                Program.mainWindow.gbInCall.Visible = false;
                //Program.mainWindow.gBChangePass.Enabled = true;
                //Program.mainWindow.gBDelAcc.Enabled = true;
            });

            // Set the flag to end the Send and Receive threads
            bStop = true;

            bIsCallActive = false;

            string[] historyDetails = new string[3];
            historyDetails[0] = Program.mainWindow.labelInCallUN.Text.Remove(0, 2);
            historyDetails[1] = Program.mainWindow.begin.ToString();
            historyDetails[2] = (Program.mainWindow.end - Program.mainWindow.begin).ToString().Split('.')[0];
            Program.mainWindow.listView2.Items.Insert(0, (new ListViewItem(historyDetails)));
            Program.mainWindow.listView2.Refresh();

            bStop = true;
            if (receiverThread.IsAlive)
            {
                receiverThread.Join();
            }
            if (senderThread.IsAlive)
            {
                senderThread.Join();
            }
        }

        public void DropCall()
        {
            try
            {
                char comm = (char)12;
                string message = comm + " <EOF>";

                // Send a Bye message to the user to end the call
                SendMessage(message, otherPartyEP);
                UninitializeCall();
            }
            catch (Exception)
            {
                MessageBox.Show("There was an error during disconnecting", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeCall()
        {
            player.Stop();
            Program.mainWindow.Invoke((MethodInvoker)delegate
            {
                Program.mainWindow.gbCall.Visible = false;
                Program.mainWindow.gbAnswerCall.Visible = false;
            });
            try
            {
                // Start listening on port 1550
                udpClient = new UdpClient(1550);

                voicePlayer.Play();

                recorder.StartRecording();
                receiverThread = new Thread(new ThreadStart(Receive));

                bIsCallActive = true;

                // Start the receiver and sender thread
                receiverThread.Start();
                senderThread.Start();

                Program.mainWindow.begin = DateTime.Now;
                Program.mainWindow.end = DateTime.Now;
                Program.mainWindow.Invoke((MethodInvoker)delegate
                {
                    Program.mainWindow.gbInCall.Visible = true;
                    //Program.mainWindow.timerConv.Start();
                });

                if (Program.mainWindow.isReceiver == true)
                {
                    Program.mainWindow.Invoke((MethodInvoker)delegate
                    {
                        Program.mainWindow.labelInCallUN.Text = "with " + Program.mainWindow.labelCallingUN.Text;
                    });
                }
                else
                {
                    Program.mainWindow.Invoke((MethodInvoker)delegate
                    {
                        Program.mainWindow.labelInCallUN.Text = Program.mainWindow.labelCallUN.Text;
                    });
                }
            }
            catch (Exception)
            {
                Program.mainWindow.Invoke((MethodInvoker)delegate
                {
                    Program.mainWindow.gbCall.Visible = true;
                });
                voicePlayer.Stop();
                MessageBox.Show("Connection init error!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /*
         * Send a message to the remote party.
         */
        private void SendMessage(string message, EndPoint sendToEP)
        {
            try
            {
                byte[] msg = Encoding.ASCII.GetBytes(message);

                // Send the message asynchronously
                clientSocket.BeginSendTo(msg, 0, message.Length, SocketFlags.None, sendToEP, new AsyncCallback(OnSend), null);
            }
            catch (Exception)
            {
                MessageBox.Show("Packet sending error!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    enum Vocoder
    {
        None,
        ALaw,
        uLaw,
    }
}
