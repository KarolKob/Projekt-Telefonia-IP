using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TalkaTIPClientV2
{
    class Communication
    {
        static bool Response(char answer)
        {
            if (answer == (char)5)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool Register(string login, string password, byte[] key)
        {
            char comm = (char)0;
            string message = comm + " " + Convert.ToBase64String(Program.security.EncryptMessage(key, login + " " + password)) + " <EOF>";

            Program.client.Send(message);
            return Response(Program.client.Receive()[0]);
        }

        public static bool LogIn(string login, string password, byte[] key)
        {
            char comm = (char)1;
            string message = comm + " " + Convert.ToBase64String(Program.security.EncryptMessage(key, login + " " + password)) + " <EOF>";

            Program.client.Send(message);

            return Response(Program.client.Receive()[0]);
        }

        public static void LogOut(string login)
        {
            char comm = (char)2;
            string message = comm + " " + Convert.ToBase64String(Program.security.EncryptMessage(Program.sessionKeyWithServer, login)) + " <EOF>";

            Program.client.Send(message);
            return;
        }

        public static bool AccDel(string login, string password)
        {
            char comm = (char)3;
            string message = comm + " " + Convert.ToBase64String(Program.security.EncryptMessage(Program.sessionKeyWithServer,
                login + " " + password)) + " <EOF>";
            Program.client.Send(message);
            return Response(Program.client.Receive()[0]);
        }

        public static bool PassChng(string login, string oldPassword, string newPassword)
        {
            char comm = (char)4;
            string message = comm + " " + Convert.ToBase64String(Program.security.EncryptMessage(Program.sessionKeyWithServer,
                login + " " + oldPassword + " " + newPassword)) + " <EOF>";
            Program.client.Send(message);
            var ans = Program.client.Receive();
            return Response(ans[0]);
        }

        public static bool AddFriend(string login, string friendLogin)
        {
            char comm = (char)8;
            string message = comm + " " + Convert.ToBase64String(Program.security.EncryptMessage(Program.sessionKeyWithServer,
                login + " " + friendLogin)) + " <EOF>";
            Program.client.Send(message);
            return Response(Program.client.Receive()[0]);
        }

        public static bool DelFriend(string login, string friendLogin)
        {
            char comm = (char)9;
            string message = comm + " " + Convert.ToBase64String(Program.security.EncryptMessage(Program.sessionKeyWithServer,
                login + " " + friendLogin)) + " <EOF>";
            Program.client.Send(message);
            return Response(Program.client.Receive()[0]);
        }

        public static bool BlockUser(string login, string userToBlock)
        {
            char comm = (char)21;
            string message = comm + " " + Convert.ToBase64String(Program.security.EncryptMessage(Program.sessionKeyWithServer,
                login + " " + userToBlock)) + " <EOF>";
            Program.client.Send(message);
            return Response(Program.client.Receive()[0]);
        }

        public static bool UnblockUser(string login, string userToUnblock)
        {
            char comm = (char)22;
            string message = comm + " " + Convert.ToBase64String(Program.security.EncryptMessage(Program.sessionKeyWithServer,
                login + " " + userToUnblock)) + " <EOF>";
            Program.client.Send(message);
            return Response(Program.client.Receive()[0]);
        }

        public static void CallState(string callerLogin, string receiverLogin, DateTime date, TimeSpan callTime)
        {
            char comm = (char)11;
            string dateString = date.ToString("yyyy-MM-dd-HH:mm:ss", CultureInfo.InvariantCulture);
            string callTimeString = string.Format("{0:D2}:{1:D2}:{2:D2}", callTime.Hours, callTime.Minutes, callTime.Seconds);
            string message = Convert.ToBase64String(Program.security.EncryptMessage(Program.sessionKeyWithServer,
                callerLogin + " " + receiverLogin + " " + dateString + " " + callTimeString));
            message = comm + " " + message + " <EOF>";
            Program.client.Send(message);
        }

        public static bool ChatMessage(string senderLogin, string receiverLogin, string chatMessage)
        {
            char comm = (char)20;
            string dateString = DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss", CultureInfo.InvariantCulture);
            string message = Convert.ToBase64String(Program.security.EncryptMessage(Program.sessionKeyWithServer,
                senderLogin + " " + receiverLogin + " " + dateString + " " + chatMessage));
            message = comm + " " + message + " <EOF>";
            Program.client.Send(message);
            return Response(Program.client.Receive()[0]);
        }

        public static byte[] KeyExchange()
        {
            var byteArray = Program.security.GetOwnerPublicKey().ToByteArray();

            string message = (char)17 + " " + Convert.ToBase64String(byteArray) + " <EOF>";
            Program.client.Send(message);
            message = Program.client.Receive();
            return Convert.FromBase64String(message.Substring(2, message.Length - 8));
        }

        public static void GetAllChatMessages(string senderLogin, string receiverLogin)
        {
            char comm = (char)23;
            string message = comm + " " + Convert.ToBase64String(Program.security.EncryptMessage(Program.sessionKeyWithServer,
                senderLogin + " " + receiverLogin)) + " <EOF>";
            Program.client.Send(message);
            message = Program.client.Receive();
            commFromServer(message.Substring(0, message.Length - 6));
        }

        public static void commFromServer(string messageFromServer)
        {
            // Decipher the message
            int comm = (int)messageFromServer[0];
            string message = Program.security.DecryptMessage(Convert.FromBase64String(messageFromServer.Substring(2)), Program.sessionKeyWithServer);
           
            switch (comm)
            {
                case 7:
                    LogIP(message);
                    break;
                case 13:
                    History(message);
                    break;
                case 14:
                    StateChng(message);
                    break;
                case 24:
                    RecieveChatMessage(message);
                    break;
                case 25:
                    RecieveAllChatMessages(message);
                    break;
                default:
                    break;
            }
        }

        static void RecieveAllChatMessages(string recievedMessages)
        {
            Program.mainWindow.Invoke((MethodInvoker)delegate
            {
                if (!Program.loginAndMessage.ContainsKey(Program.mainWindow.listView1.SelectedItems[0].Text))
                {
                    Program.loginAndMessage.Add(Program.mainWindow.listView1.SelectedItems[0].Text, recievedMessages);
                    Program.mainWindow.UpdateChatText(recievedMessages);
                }
                else
                {
                    if (Program.loginAndMessage[Program.mainWindow.listView1.SelectedItems[0].Text] != recievedMessages)
                    {
                        Program.mainWindow.UpdateChatText(recievedMessages);
                    }
                }
            });
        }

        // Recieve and display the message
        static void RecieveChatMessage(string chatMessage)
        {
            string[] param = chatMessage.Split(' ');

            string loginFrom = param[0];
            string loginTo = param[1];

            // Only recieve the messages meant for you (mistakes shouldn't happen unless on loopback)
            if (loginTo == Program.userLogin)
            {
                DateTime msgSentTime = DateTime.ParseExact(param[2], "yyyy-MM-dd-HH:mm:ss", CultureInfo.InvariantCulture);

                StringBuilder builder = new StringBuilder();
                for (int i = 3; i < param.Length - 1; i++)  // Ignore the <EOF>
                {
                    // Append each string to the StringBuilder overload
                    builder.Append(param[i]).Append(" ");
                }

                // Preparing the display format
                string message = builder.ToString();
                message = "\n" + loginFrom + " " + msgSentTime.ToString() + "\n" + message + "\n";

                // Saving messages to memory
                if (Program.loginAndMessage.ContainsKey(loginFrom))
                {
                    Program.loginAndMessage[loginFrom] += message;
                }
                else
                {
                    Program.loginAndMessage.Add(loginFrom, message);
                }

                // Display the messages or inform user
                Program.mainWindow.Invoke((MethodInvoker)delegate
                {
                    if (Program.mainWindow.listView1.SelectedItems[0].Text == loginFrom)
                    {
                        Program.mainWindow.AllMessages.Text += message;
                    }
                    else
                    {
                        bool found = false;
                        int index = 0;
                        foreach (ListViewItem item in Program.mainWindow.listView1.Items)
                        {
                            if (item.Text == loginFrom)
                            {
                                item.ForeColor = Color.Red;
                                found = true;
                                Program.mainWindow.listView1.Refresh();
                                break;
                            }
                            index++;
                        }

                        if (!found)
                        {
                            string[] friendDetails = { loginFrom, "0" };
                            ListViewItem friend = new ListViewItem(friendDetails, 0);
                            friend.ForeColor = Color.Red;
                            Program.mainWindow.listView1.Items.Add(friend);
                            Program.mainWindow.listView1.Refresh();
                        }
                    }
                });
            }
        }

        static void LogIP(string messageFromServer)
        {
            string[] friends = messageFromServer.Split(' ');
            ListViewItem friend;
            for (int i = 0; i < friends.Length - 1; i += 3)
            {
                // friends[i] login
                // friends[i+1] status
                // friends[i+2] IP
                string[] friendDetails = { friends[i], friends[i + 2] };
                if (friends[i + 1] == "0") // Unavailable
                {

                    friend = new ListViewItem(friendDetails, 0);
                }
                else
                {
                    friend = new ListViewItem(friendDetails, 1);
                }
                Program.mainWindow.Invoke((MethodInvoker)delegate
                {
                    Program.mainWindow.listView1.Items.Add(friend);
                });
            }
        }

        static void History(string messageFromServer)
        {
            string[] history = messageFromServer.Split(' ');
            string[] historyDetails;
            for (int i = 0; i < history.Length - 1; i += 5)
            {
                historyDetails = new string[3];
                historyDetails[0] = history[i];
                historyDetails[1] = history[i + 1] + " " + history[i + 2];
                historyDetails[2] = history[i + 4] == "00:00:00" ? "missed call" : history[i + 4];
                Program.mainWindow.Invoke((MethodInvoker)delegate
                {
                    Program.mainWindow.listView2.Items.Insert(0, new ListViewItem(historyDetails));
                });
            }
        }

        static void StateChng(string messageFromServer)
        {
            string[] friends = messageFromServer.Split(' ');
            for (int i = 0; i < friends.Length - 1; i += 2)
            {
                int index = Program.mainWindow.listView1.FindItemWithText(friends[i]).Index;
                Program.mainWindow.listView1.Items[index].SubItems[1].Text = friends[i + 1];
                if (friends[i + 1] == "0")
                {
                    Program.mainWindow.listView1.Items[index].ImageIndex = 0;
                }
                else
                {
                    Program.mainWindow.listView1.Items[index].ImageIndex = 1;
                }
            }
            Program.mainWindow.listView1.Refresh();
        }
    }
}
