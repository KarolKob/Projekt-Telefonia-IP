using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TalkaTIPClientV2
{
    static class Program
    {
        public static string userLogin;
        public static string serverAddress;
        public static Client client;
        public static MainWindow mainWindow;
        public static Security security = new Security();
        public static CallHandler callHandler;

        // A dictionary containing user names and their messages with the application user
        public static Dictionary<string, string> loginAndMessage = new Dictionary<string, string>();

        public static byte[] sessionKeyWithServer = null;
        public static byte[] sessionKeyWithClient = null;

        /// <summary>
        /// Główny punkt wejścia dla aplikacji.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string thisprocessname = Process.GetCurrentProcess().ProcessName;

            if (Process.GetProcesses().Count(p => p.ProcessName == thisprocessname) > 1)
            {
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            userLogin = "";
            serverAddress = "";

            LoginWindow logIn;
            RegisterWindow register;
            security.CreatePublicKey();

            DialogResult dialogResult = DialogResult.No;

            while (dialogResult != DialogResult.Cancel)
            {
                // The user is not logged in
                if (dialogResult == DialogResult.No)
                {
                    logIn = new LoginWindow();
                    logIn.ServerIPText.Text = "127.0.0.1";
                    dialogResult = logIn.ShowDialog();
                }

                // The user is logged in
                if (dialogResult == DialogResult.Yes)
                {
                    mainWindow = new MainWindow();
                    callHandler = new CallHandler();
                    dialogResult = mainWindow.ShowDialog();
                }

                // The user wants to register
                if (dialogResult == DialogResult.OK)
                {
                    register = new RegisterWindow();
                    dialogResult = register.ShowDialog();
                }
            }
        }
    }
}
