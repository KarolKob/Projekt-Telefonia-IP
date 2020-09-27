using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Security.Cryptography;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

namespace TalkaTIPSerwer
{
    class Program
    {
        // Dictionary of logged in users
        public static Dictionary<long, ClientClass> onlineUsers = new Dictionary<long, ClientClass>();
        public static Security security = new Security();

        static void Main(string[] args)
        {
            string thisprocessname = Process.GetCurrentProcess().ProcessName;

            if (Process.GetProcesses().Count(p => p.ProcessName == thisprocessname) > 1)
                return;

            SQLiteConnection m_dbConnection;
            m_dbConnection = new SQLiteConnection("Data Source=TalkaTIP.sqlite; Version=3;");
            m_dbConnection.Open();

            // Create DataBase file if it doesn't exists
            DataBase.CreateDataBase(m_dbConnection);
            Communication.AddDelegateToDictionary();
            m_dbConnection.Close();

            security.CreatePublicKey();
            AsynchronousServer.StartListening();
        }
    }
}
