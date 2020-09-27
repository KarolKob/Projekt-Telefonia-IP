using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace TalkaTIPSerwer
{
    class DataBase
    {
        static void ExecuteCommand(string sql, SQLiteConnection m_dbConnection)
        {
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
        }

        static void DropTable(string tableName, SQLiteConnection m_dbConnection)
        {
            string sql = "DROP TABLE " + tableName;
            ExecuteCommand(sql, m_dbConnection);
        }

        static void CreateUsersTable(SQLiteConnection m_dbConnection)
        {
            string sql = "CREATE TABLE IF NOT EXISTS Users(" +
                                            "UserID INTEGER PRIMARY KEY AUTOINCREMENT," +
                                            "Login VARCHAR(50) UNIQUE NOT NULL," +
                                            "Password CHAR(64) NOT NULL," +
                                            "LastLogoutDate DATETIME NOT NULL," +
                                            "RegistrationDate DATETIME NOT NULL);";

            ExecuteCommand(sql, m_dbConnection);
        }

        static void CreateHistoriesTable(SQLiteConnection m_dbConnection)
        {
            string sql = "CREATE TABLE IF NOT EXISTS Histories(" +
                                            "HistoryID INTEGER PRIMARY KEY AUTOINCREMENT," +
                                            "UserSenderID INTEGER REFERENCES Users(UserID)," +
                                            "UserReceiverID INTEGER REFERENCES Users(UserID)," +
                                            "Start DATETIME NOT NULL," +
                                            "Duration TIME);";
            ExecuteCommand(sql, m_dbConnection);
        }

        static void CreateFriendsTable(SQLiteConnection m_dbConnection)
        {
            string sql = "CREATE TABLE IF NOT EXISTS Friends(" +
                                            "FriendID INTEGER PRIMARY KEY AUTOINCREMENT," +
                                            "UserID1 INTEGER NOT NULL REFERENCES Users(UserID)," +
                                            "UserID2 INTEGER NOT NULL REFERENCES Users(UserID));";
            ExecuteCommand(sql, m_dbConnection);
        }

        static void CreateBlockedTable(SQLiteConnection m_dbConnection)
        {
            string sql = "CREATE TABLE IF NOT EXISTS Blocked(" +
                                            "BlockID INTEGER PRIMARY KEY AUTOINCREMENT," +
                                            "UserID1 INTEGER NOT NULL REFERENCES Users(UserID)," +
                                            "UserID2 INTEGER NOT NULL REFERENCES Users(UserID));";
            ExecuteCommand(sql, m_dbConnection);
        }

        static void CreateMessagesTable(SQLiteConnection m_dbConnection)
        {
            string sql = "CREATE TABLE IF NOT EXISTS Messages(" +
                                            "ChatID INTEGER PRIMARY KEY AUTOINCREMENT," +
                                            "UserSenderID INTEGER NOT NULL REFERENCES Users(UserID)," +
                                            "UserReceiverID INTEGER NOT NULL REFERENCES Users(UserID)," +
                                            "SendTime DATETIME NOT NULL," +
                                            "Message VARCHAR(200) NOT NULL);";
            ExecuteCommand(sql, m_dbConnection);
        }

        public static void CreateDataBase(SQLiteConnection m_dbConnection)
        {
            if (!File.Exists("TalkaTIP.sqlite"))
            {
                SQLiteConnection.CreateFile("TalkaTIP.sqlite");
            }

            CreateUsersTable(m_dbConnection);
            CreateFriendsTable(m_dbConnection);
            CreateHistoriesTable(m_dbConnection);
            CreateMessagesTable(m_dbConnection);
            CreateBlockedTable(m_dbConnection);
        }
    }
}
