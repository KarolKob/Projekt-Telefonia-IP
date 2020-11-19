using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Net.Sockets;
using System.Globalization;
using TalkaTIPServer;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

namespace TalkaTIPSerwer
{
    class Communication
    {
        static Dictionary<int, Delegate> communiqueDictionary = new Dictionary<int, Delegate>();

        public static void AddDelegateToDictionary()
        {
            // in, out
            communiqueDictionary[0] = new Func<List<string>, string>(Register);
            communiqueDictionary[1] = new Func<List<string>, byte[], Socket, string>(LogIn);
            communiqueDictionary[2] = new Func<List<string>, string>(LogOut);
            communiqueDictionary[3] = new Func<List<string>, string>(AccDel);
            communiqueDictionary[4] = new Func<List<string>, string>(PassChng);
            communiqueDictionary[8] = new Func<List<string>, string>(AddFriend);
            communiqueDictionary[9] = new Func<List<string>, string>(DelFriend);
            communiqueDictionary[11] = new Func<List<string>, string>(CallState);
            communiqueDictionary[13] = new Func<long, string>(History);
            communiqueDictionary[15] = new Func<List<string>, string>(Iam);
            communiqueDictionary[20] = new Func<List<string>, string>(ChatMessage);
            communiqueDictionary[21] = new Func<List<string>, string>(BlockUser);
            communiqueDictionary[22] = new Func<List<string>, string>(UnblockUser);
            communiqueDictionary[23] = new Func<List<string>, string>(GetChatMessages);
        }

        // Incoming messages
        public static string Register(List<string> param)
        {
            // Communication with DB
            using (var ctx = new TalkaTipDB())
            {
                var newUser = new Users();
                string login = param[0]; // login

                int loginUnique = ctx.Users.Where(x => x.Login == login).Count(); // Check if the login is unique
                if (loginUnique == 0)
                {
                    newUser.Login = login;
                    newUser.Password = Utilities.hashBytePassHex(param[1] + login); // 256 bit hash password          
                    newUser.LastLogoutDate = DateTime.Now;
                    newUser.RegistrationDate = DateTime.Now;

                    ctx.Users.Add(newUser);
                    try
                    {
                        ctx.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        return Fail(); ;
                    }
                }
                else
                {
                    return Fail();
                }
            }
            return OK();
        }

        public static string LogIn(List<string> param, byte[] sessionKey, Socket client)
        {
            string login = param[0];

            string password = Utilities.hashBytePassHex(param[1] + login);

            // users -> online
            using (var ctx = new TalkaTipDB())
            {
                var user = ctx.Users.Where(x => x.Login == login && x.Password == password).FirstOrDefault();
                if (user != null)
                {
                    if (Program.onlineUsers.ContainsKey(user.UserID))
                    {
                        return Fail();
                    }
                    ClientClass connectedClient = new ClientClass();
                    connectedClient.login = login;
                    connectedClient.iAM = DateTime.Now;
                    connectedClient.addressIP = ((IPEndPoint)client.RemoteEndPoint).Address.ToString();

                    connectedClient.sessionKey = sessionKey;
                    connectedClient.friendWithStateDict = new Dictionary<string, string>();
                    connectedClient.blockedUsersDict = new Dictionary<string, string>();

                    // Add to dictionary
                    Program.onlineUsers[user.UserID] = connectedClient;
                    
                    // Find people who have friend that is logged in
                    IQueryable<long> friends = ctx.Friends.Where(x => x.UserID2 == user.UserID).Select(x => x.UserID1);

                    // Find friends in the online dict and add/change the IP address
                    foreach (long item in friends)
                    {
                        if (Program.onlineUsers.ContainsKey(item))
                        {
                            Program.onlineUsers[item].friendWithStateDict[login] = connectedClient.addressIP;
                        }
                    }

                    IQueryable<long> blocked = ctx.Blocked.Where(x => x.UserID2 == user.UserID).Select(x => x.UserID1);
                    
                    foreach (long item in blocked)
                    {
                        if (Program.onlineUsers.ContainsKey(item))
                        {
                            Program.onlineUsers[item].blockedUsersDict[login] = connectedClient.addressIP;
                        }
                    }

                    return OK();
                }
                else
                {
                    return Fail();
                }
            }
        }

        public static string LogOut(List<string> param)
        {
            string login = param[0];

            using (var ctx = new TalkaTipDB())
            {
                var userToLogOut = ctx.Users.Where(x => x.Login == login).FirstOrDefault();
                if (userToLogOut != null)
                {
                    userToLogOut.LastLogoutDate = DateTime.Now;
                    try
                    {
                        Program.onlineUsers.Remove(userToLogOut.UserID); // Delete from the dictionary
                        ctx.Entry(userToLogOut).State = System.Data.Entity.EntityState.Modified;
                        ctx.SaveChanges();

                        // Find people who have friend that is logged in
                        IQueryable<long> friends = ctx.Friends.Where(x => x.UserID2 == userToLogOut.UserID).Select(x => x.UserID1);

                        // Find friends in the online dict and add/change the IP address
                        foreach (long item in friends)
                        {
                            if (Program.onlineUsers.ContainsKey(item))
                            {
                                Program.onlineUsers[item].friendWithStateDict[login] = "0";
                            }
                        }

                        IQueryable<long> blocked = ctx.Blocked.Where(x => x.UserID2 == userToLogOut.UserID).Select(x => x.UserID1); ;

                        foreach (long item in blocked)
                        {
                            if (Program.onlineUsers.ContainsKey(item))
                            {
                                Program.onlineUsers[item].blockedUsersDict[login] = "0";
                            }
                        }

                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        return Fail();
                    }
                }
                else
                {
                    return Fail();
                }
            }
            return OK();
        }

        public static string AccDel(List<string> param)
        {
            string login = param[0];
            string password = Utilities.hashBytePassHex(param[1] + login);
            using (var ctx = new TalkaTipDB())
            {
                var userToDelete = ctx.Users.Where(x => x.Login == login && x.Password == password).FirstOrDefault();
                if (userToDelete != null)
                {
                    try
                    {
                        // 1. set, in history, id to null, it can be first or second
                        IQueryable<Histories> userHistory = ctx.Histories.Where(x => x.UserReceiverID == userToDelete.UserID);//first
                        foreach (Histories item in userHistory)
                        {
                            item.UserReceiverID = null;
                            ctx.Entry(item).State = System.Data.Entity.EntityState.Modified;
                        }

                        userHistory = ctx.Histories.Where(x => x.UserSenderID == userToDelete.UserID);//second
                        foreach (Histories item in userHistory)
                        {
                            item.UserSenderID = null;
                            ctx.Entry(item).State = System.Data.Entity.EntityState.Modified;
                        }

                        // 2. delete acquaintances in both directions
                        IQueryable<Friends> userAcquaintances = ctx.Friends.Where(x => x.UserID1 == userToDelete.UserID);//first
                        foreach (Friends item in userAcquaintances)
                        {
                            ctx.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                        }

                        userAcquaintances = ctx.Friends.Where(x => x.UserID2 == userToDelete.UserID);
                        foreach (Friends item in userAcquaintances)
                        {
                            ctx.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                        }

                        // 3. delete blocked users in both directions
                        IQueryable<Blocked> blockedUsers = ctx.Blocked.Where(x => x.UserID1 == userToDelete.UserID);//first
                        foreach (Blocked item in blockedUsers)
                        {
                            ctx.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                        }

                        blockedUsers = ctx.Blocked.Where(x => x.UserID2 == userToDelete.UserID);
                        foreach (Blocked item in blockedUsers)
                        {
                            ctx.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                        }

                        // 4. delete messages sent in both directions
                        IQueryable<Messages> messages = ctx.Messages.Where(x => x.UserSenderID == userToDelete.UserID);
                        foreach (Messages item in messages)
                        {
                            ctx.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                        }

                        messages = ctx.Messages.Where(x => x.UserReceiverID == userToDelete.UserID);
                        foreach (Messages item in messages)
                        {
                            ctx.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                        }

                        // 5. delete user
                        ctx.Entry(userToDelete).State = System.Data.Entity.EntityState.Deleted;

                        ctx.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        return Fail();
                    }
                }
                else
                {
                    return Fail();
                }
            }
            return OK();
        }

        public static string PassChng(List<string> param)
        {
            string login = param[0];
            string password1 = Utilities.hashBytePassHex(param[1] + login);
            using (var ctx = new TalkaTipDB())
            {
                var userToChngPasswd = ctx.Users.Where(x => x.Login == login && x.Password == password1).FirstOrDefault();
                if (userToChngPasswd != null)
                {
                    userToChngPasswd.Password = Utilities.hashBytePassHex(param[2] + login);
                    try
                    {
                        ctx.Entry(userToChngPasswd).State = System.Data.Entity.EntityState.Modified;
                        ctx.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        return Fail();
                    }
                }
                else
                {
                    return Fail();
                }
            }
            return OK();
        }

        public static string AddFriend(List<string> param)
        {
            string login1 = param[0]; // User logged in
            string login2 = param[1]; // Friend of the logged in user
            using (var ctx = new TalkaTipDB())
            {
                var acquaintance = new Friends();

                // Find the proper users IDs
                var userID1 = ctx.Users.Where(x => x.Login == login1).Select(x => x.UserID).FirstOrDefault();
                if (userID1 != 0)
                {
                    var user2 = ctx.Users.Where(x => x.Login == login2).Select(x => new { x.UserID, x.Login }).FirstOrDefault();
                    if (user2 != null)
                    {
                        var acq = ctx.Friends.Where(x => x.UserID1 == userID1 && x.UserID2 == user2.UserID).FirstOrDefault();
                        if (acq == null)
                        {
                            var block = ctx.Blocked.Where(x => x.UserID1 == userID1 && x.UserID2 == user2.UserID).FirstOrDefault();
                            if (block == null)
                            {
                                acquaintance.UserID1 = userID1;
                                acquaintance.UserID2 = user2.UserID;
                                try
                                {
                                    ctx.Friends.Add(acquaintance);
                                    ctx.SaveChanges();
                                    string acqLogin = user2.Login;
                                    if (Program.onlineUsers.ContainsKey(user2.UserID))
                                    {
                                        Program.onlineUsers[userID1].friendWithStateDict.Add(acqLogin, Program.onlineUsers[user2.UserID].addressIP);
                                    }
                                    else
                                    {
                                        Program.onlineUsers[userID1].friendWithStateDict.Add(acqLogin, "0");
                                    }

                                }
                                catch (DbUpdateConcurrencyException)
                                {
                                    return Fail();
                                }
                            }
                            else
                            {
                                Blocked blockedUser = ctx.Blocked.Where(x => x.UserID1 == userID1 && x.UserID2 == user2.UserID).FirstOrDefault();
                                ctx.Entry(blockedUser).State = System.Data.Entity.EntityState.Deleted;
                                try
                                {
                                    ctx.SaveChanges();
                                    Program.onlineUsers[userID1].blockedUsersDict.Remove(user2.Login);
                                }
                                catch (DbUpdateConcurrencyException)
                                {
                                    return Fail();
                                }
                                acquaintance.UserID1 = userID1;
                                acquaintance.UserID2 = user2.UserID;
                                try
                                {
                                    ctx.Friends.Add(acquaintance);
                                    ctx.SaveChanges();
                                    string acqLogin = user2.Login;
                                    if (Program.onlineUsers.ContainsKey(user2.UserID))
                                    {
                                        Program.onlineUsers[userID1].friendWithStateDict.Add(acqLogin, Program.onlineUsers[user2.UserID].addressIP);
                                    }
                                    else
                                    {
                                        Program.onlineUsers[userID1].friendWithStateDict.Add(acqLogin, "0");
                                    }

                                }
                                catch (DbUpdateConcurrencyException)
                                {
                                    return Fail();
                                }
                            }
                        }
                        else
                        {
                            return Fail();
                        }
                    }
                    else
                    {
                        return Fail();
                    }
                }
                else
                {
                    return Fail();
                }
            }
            return OK();
        }

        public static string DelFriend(List<string> param)
        {
            string login1 = param[0]; // Logged in user
            string login2 = param[1];

            using (var ctx = new TalkaTipDB())
            {
                var userLoggedInID1 = ctx.Users.Where(x => x.Login == login1).Select(x => x.UserID).FirstOrDefault();
                if (userLoggedInID1 != 0)
                {
                    var user1Friend = ctx.Users.Where(x => x.Login == login2).Select(x => new { x.UserID, x.Login }).FirstOrDefault();
                    if (user1Friend.UserID != 0)
                    {
                        var friend = ctx.Friends.Where(x => x.UserID1 == userLoggedInID1 && x.UserID2 == user1Friend.UserID).FirstOrDefault();
                        ctx.Entry(friend).State = System.Data.Entity.EntityState.Deleted;
                        try
                        {
                            ctx.SaveChanges();
                            Program.onlineUsers[userLoggedInID1].friendWithStateDict.Remove(user1Friend.Login);
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            return Fail();
                        }
                    }
                    else
                    {
                        return Fail();
                    }
                }
                else
                {
                    return Fail();
                }
            }
            return OK();
        }

        public static string BlockUser(List<string> param)
        {
            string login1 = param[0]; // User logged in
            string login2 = param[1]; // Friend of the logged in user
            using (TalkaTipDB ctx = new TalkaTipDB())
            {
                Blocked block = new Blocked();
                // Find the proper users IDs
                long userID1 = ctx.Users.Where(x => x.Login == login1).Select(x => x.UserID).FirstOrDefault();
                if (userID1 != 0)
                {
                    var user2 = ctx.Users.Where(x => x.Login == login2).Select(x => new { x.UserID, x.Login }).FirstOrDefault();
                    if (user2 != null)
                    {
                        var blck = ctx.Blocked.Where(x => x.UserID1 == userID1 && x.UserID2 == user2.UserID).FirstOrDefault();
                        if (blck == null)
                        {
                            block.UserID1 = userID1;
                            block.UserID2 = user2.UserID;
                            try
                            {
                                ctx.Blocked.Add(block);
                                ctx.SaveChanges();
                                string blockedLogin = user2.Login;

                                if (Program.onlineUsers.ContainsKey(user2.UserID))
                                {
                                    Program.onlineUsers[userID1].blockedUsersDict.Add(blockedLogin, Program.onlineUsers[user2.UserID].addressIP);
                                }
                                else
                                {
                                    Program.onlineUsers[userID1].blockedUsersDict.Add(blockedLogin, "0");
                                }
                            }
                            catch (DbUpdateConcurrencyException)
                            {
                                return Fail();
                            }
                        }
                        else
                        {
                            return Fail();
                        }
                    }
                    else
                    {
                        return Fail();
                    }
                }
                else
                {
                    return Fail();
                }
            }
            return OK();
        }

        public static string UnblockUser(List<string> param)
        {
            string login1 = param[0]; // Logged in user
            string login2 = param[1];

            using (var ctx = new TalkaTipDB())
            {
                var userLoggedInID1 = ctx.Users.Where(x => x.Login == login1).Select(x => x.UserID).FirstOrDefault();
                if (userLoggedInID1 != 0)
                {
                    var user1Blocked = ctx.Users.Where(x => x.Login == login2).Select(x => new { x.UserID, x.Login }).FirstOrDefault();
                    if (user1Blocked.UserID != 0)
                    {
                        Blocked blockedUser = ctx.Blocked.Where(x => x.UserID1 == userLoggedInID1 && x.UserID2 == user1Blocked.UserID).FirstOrDefault();
                        ctx.Entry(blockedUser).State = System.Data.Entity.EntityState.Deleted;
                        try
                        {
                            ctx.SaveChanges();
                            Program.onlineUsers[userLoggedInID1].blockedUsersDict.Remove(user1Blocked.Login);
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            return Fail();
                        }
                    }
                    else
                    {
                        return Fail();
                    }
                }
                else
                {
                    return Fail();
                }
            }
            return OK();
        }

        public static string CallState(List<string> param)
        {
            string login1 = param[0];
            string login2 = param[1];

            using (TalkaTipDB ctx = new TalkaTipDB())
            {
                long SenderID = ctx.Users.Where(x => x.Login == login1).Select(x => x.UserID).FirstOrDefault();
                if (SenderID != 0)
                {
                    long ReceiverID = ctx.Users.Where(x => x.Login == login2).Select(x => x.UserID).FirstOrDefault();
                    if (ReceiverID != 0)
                    {
                        Histories history = new Histories();
                        history.UserSenderID = SenderID;
                        history.UserReceiverID = ReceiverID;
                        
                        //yyyy-MM-dd-HH:mm:ss
                        history.Start = DateTime.ParseExact(param[2], "yyyy-MM-dd-HH:mm:ss", CultureInfo.InvariantCulture);

                        //HH:mm:ss
                        history.Duration = new DateTime().Add(TimeSpan.Parse(param[3]));
                        try
                        {
                            ctx.Histories.Add(history);
                            ctx.SaveChanges();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            return Fail();
                        }
                    }
                    else
                    {
                        return Fail();
                    }
                }
                else
                {
                    return Fail();
                }
            }
            return OK();
        }
        public static string Iam(List<string> param)
        {
            string login = string.Empty;
            login = param[0];

            // Check if the user exists
            using (var ctx = new TalkaTipDB())
            {
                var user = ctx.Users.Where(x => x.Login == login).FirstOrDefault();
                if (user != null)
                {
                    if (Program.onlineUsers.ContainsKey(user.UserID))
                    {
                        Program.onlineUsers[user.UserID].iAM = DateTime.Now;
                        return StateChng(user.UserID);
                    }
                }
            }
            return " <EOF>";
        }

        // Function for recieving and saving chat messages between users
        public static string ChatMessage(List<string> param)
        {
            string loginFrom = param[0];
            string loginTo = param[1];
            DateTime msgSentTime = DateTime.ParseExact(param[2], "yyyy-MM-dd-HH:mm:ss", CultureInfo.InvariantCulture);

            StringBuilder builder = new StringBuilder();
            for(int i = 3; i < param.Count; i++)
            {
                // Append each string to the StringBuilder overload.
                builder.Append(param[i]).Append(" ");
            }
            string message = builder.ToString();

            using (TalkaTipDB ctx = new TalkaTipDB())
            {
                long SenderID = ctx.Users.Where(x => x.Login == loginFrom).Select(x => x.UserID).FirstOrDefault();
                if (SenderID != 0)
                {
                    long ReceiverID = ctx.Users.Where(x => x.Login == loginTo).Select(x => x.UserID).FirstOrDefault();
                    if (ReceiverID != 0)
                    {
                        Messages msg = new Messages();
                        msg.UserSenderID = SenderID;
                        msg.UserReceiverID = ReceiverID;
                        msg.SendTime = DateTime.ParseExact(param[2], "yyyy-MM-dd-HH:mm:ss", CultureInfo.InvariantCulture);
                        msg.Message = message;

                        try
                        {
                            ctx.Messages.Add(msg);
                            ctx.SaveChanges();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            return Fail();
                        }
                        var blck = ctx.Blocked.Where(x => x.UserID1 == ReceiverID && x.UserID2 == SenderID).FirstOrDefault();
                        if (blck == null)
                        {
                            // Send the message to reciever
                            if(Program.onlineUsers.ContainsKey(ReceiverID))
                            {
                                EndPoint endPoint = new IPEndPoint(IPAddress.Parse(Program.onlineUsers[ReceiverID].addressIP), 14450);
                                builder = new StringBuilder();
                                for (int i = 0; i < param.Count; i++)
                                {
                                    // Append each string to the StringBuilder overload.
                                    builder.Append(param[i]).Append(" ");
                                }
                                message = builder.ToString();

                                message = Convert.ToBase64String(Program.security.EncryptMessage(Program.onlineUsers[SenderID].sessionKey, message));
                                message = ((char)24).ToString() + ' ' + message;
                                message += " <EOF>";

                                AsynchronousServer.SendMessage(message, endPoint);
                            }
                            return OK();
                        }
                        else
                        {
                            return Fail();
                        }
                    }
                    else
                    {
                        return Fail();
                    }
                }
                else
                {
                    return Fail();
                }
            }
        }

        // Return in a format ready to display
        public static string GetChatMessages(List<string> param)
        {
            string allChatMessages = string.Empty;
            string loginFrom = param[0];
            string loginTo = param[1];

            using (TalkaTipDB ctx = new TalkaTipDB())
            {
                long User1ID = ctx.Users.Where(x => x.Login == loginFrom).Select(x => x.UserID).FirstOrDefault();
                if (User1ID != 0)
                {
                    long User2ID = ctx.Users.Where(x => x.Login == loginTo).Select(x => x.UserID).FirstOrDefault();
                    if (User2ID != 0)
                    {
                        var selectedRows = ctx.Messages
                            .Where(x => (x.UserReceiverID == User1ID && x.UserSenderID == User2ID)
                            || (x.UserReceiverID == User2ID && x.UserSenderID == User1ID)).Select(x => x);

                        if(selectedRows != null)
                        {
                            foreach(Messages msg in selectedRows)
                            {
                                if (msg.UserSenderID == User1ID)
                                {
                                    allChatMessages = allChatMessages + "\n" + loginFrom + " " + msg.SendTime.ToString() + "\n" + msg.Message + "\n";
                                }
                                else
                                {
                                    allChatMessages = allChatMessages + "\n" + loginTo + " " + msg.SendTime.ToString() + "\n" + msg.Message + "\n";
                                }
                            }

                            allChatMessages = Convert.ToBase64String(Program.security.EncryptMessage(
                                Program.onlineUsers[User1ID].sessionKey, allChatMessages));
                            allChatMessages = ((char)25).ToString() + ' ' + allChatMessages;
                            allChatMessages += " <EOF>";

                            return allChatMessages;
                        }
                        else
                        {
                            return Fail();
                        }
                    }
                    else
                    {
                        return Fail();
                    }
                }
                else
                {
                    return Fail();
                }
            }
        }

        // Outgoing messages
        public static string OK()
        {
            Console.WriteLine("OK");
            return ((char)5).ToString() + " <EOF>";
        }

        public static string Fail()
        {
            Console.WriteLine("Fail");
            return ((char)6).ToString() + " <EOF>";

        }

        public static string LogIP(long userID)
        {
            using (var ctx = new TalkaTipDB())
            {
                var user = ctx.Users.Where(x => x.UserID == userID).FirstOrDefault();
                if (user != null)
                {
                    string message = string.Empty;

                    // 0111 login_1 status_1 IP_1 login_2 status_2 IP_2...login_n status_n IP_n)
                    var friends = ctx.Friends.Where(x => x.UserID1 == userID);  // Returns all friends

                    foreach (var item in friends)
                    {
                        var friendLogin = ctx.Users.Where(x => x.UserID == item.UserID2).Select(x => x.Login).FirstOrDefault();
                        if (friendLogin != null)
                        {
                            message += friendLogin + " ";
                            if (Program.onlineUsers.ContainsKey(item.UserID2))
                            {
                                message += "1 ";
                                message += Program.onlineUsers[item.UserID2].addressIP + " ";
                            }
                            else
                            {
                                message += "0 ";
                                message += "0 ";
                            }
                        }
                        else
                        {
                            return Fail();
                        }
                    }

                    message = Convert.ToBase64String(Program.security.EncryptMessage(Program.onlineUsers[userID].sessionKey, message));
                    message = ((char)7).ToString() + ' ' + message;
                    message += " <EOF>";
                    return message;
                }
                else
                {
                    return Fail();
                }
            }
        }

        public static string History(long userID)
        {
            string history = string.Empty;
            using (var ctx = new TalkaTipDB())
            {
                var histories = ctx.Histories.Where(x => x.UserSenderID == userID || x.UserReceiverID == userID).OrderBy(x => x.Start).ToList();
                if (histories.Count != 0)
                {
                    foreach (var item in histories)
                    {
                        if (item.UserSenderID == userID)    // userID is the sender
                        {
                            var friendLoginR = ctx.Users.Where(x => x.UserID == item.UserReceiverID).Select(x => x.Login).FirstOrDefault();
                            if (friendLoginR != null)
                            {
                                history += friendLoginR + " " + item.Start.ToString() + " " + item.Duration.ToString() + " ";
                            }
                        }
                        else // userID is the receiver
                        {
                            var friendLoginS = ctx.Users.Where(x => x.UserID == item.UserSenderID).Select(x => x.Login).FirstOrDefault();
                            if (friendLoginS != null)
                            {
                                history += friendLoginS + " " + item.Start.ToString() + " " + item.Duration.ToString() + " ";
                            }
                        }
                    }
                }
                history = Convert.ToBase64String(Program.security.EncryptMessage(Program.onlineUsers[userID].sessionKey, history));
                history = ((char)13).ToString() + ' ' + history;
                return history + " <EOF>";
            }
        }

        public static string StateChng(long userID)
        {
            string message = string.Empty;
            foreach (var item in Program.onlineUsers[userID].friendWithStateDict)
            {
                message += item.Key + " " + item.Value + " ";
            }

            message = Convert.ToBase64String(Program.security.EncryptMessage(Program.onlineUsers[userID].sessionKey, message));
            message = (char)14 + " " + message;
            message += " <EOF>";

            // Remove from the dictionary
            Program.onlineUsers[userID].friendWithStateDict.Clear();

            return message;
        }

        public static string ChooseCommunique(string message, byte[] sessionKey, Socket client)
        {
            byte[] sessKey;
            long userID = 0;
            string result = string.Empty;
            char mess = message[0];

            if (message[0] == (char)1 || message[0] == (char)0)
            {
                sessKey = sessionKey;
                if (sessKey == null) { return Fail(); }
            }
            else
            {
                // Session key is in the onlineUsers dictionary
                userID = getUserIDHavingAdressIP(((IPEndPoint)client.RemoteEndPoint).Address.ToString());
                if (userID > 0)
                {
                    sessKey = Program.onlineUsers[userID].sessionKey;
                    if (sessKey == null) { return Fail(); }
                }
                else
                {
                    return Fail();
                }
            }

            // Decipher   
            string decryptedMessage = Program.security.DecryptMessage(Convert.FromBase64String(message.Substring(2, message.Length - 8)), sessKey);

            // Take 8 bits to recognize the communique
            int bits8 = (int)message[0];    // Decimal value

            // Parameters to send
            string[] sParameters = decryptedMessage.Split(' ');
            List<string> parameters = new List<string>();
            for (int i = 0; i < sParameters.Length; i++)
            {
                parameters.Add(sParameters[i]);
            }

            if (bits8 != 1) // If it isn't login
            {
                // Use the dictionary of methods to choose a proper response
                // http://stackoverflow.com/a/4233539
                result = (string)communiqueDictionary[bits8].DynamicInvoke(parameters);
            }
            else
            {
                result = (string)communiqueDictionary[bits8].DynamicInvoke(parameters, sessionKey, client);
            }
            return result;
        }

        public static long getUserIDHavingAdressIP(string addressIP)
        {
            long userID = -1;

            foreach (KeyValuePair<long, ClientClass> item in Program.onlineUsers)
            {
                if (item.Value.addressIP == addressIP)
                {
                    userID = item.Key;
                    return userID;
                }
            }
            return userID;
        }
    }
}
