using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TalkaTIPClientV2
{
    public partial class LoginWindow : Form
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LogInButton_Click(object sender, EventArgs e)
        {
            if (LoginText.Text == "Login" || PasswordText.Text == "Password" || ServerIPText.Text == "Server IP")
            {
                MessageBox.Show("You need to enter both login and password.", "Error");
            }
            else
            {
                try
                {
                    Program.client = new Client(ServerIPText.Text);
                    var serverKey =  Communication.KeyExchange();
                    if (serverKey != null)
                    {
                        var sessKey = Program.security.SetSessionKey(serverKey);

                        if (Communication.LogIn(LoginText.Text, PasswordText.Text, sessKey) == true)
                        {
                            Program.userLogin = LoginText.Text;
                            Program.serverAddress = ServerIPText.Text;
                            DialogResult = DialogResult.Yes;
                            Program.sessionKeyWithServer = sessKey;
                            Close();
                        }
                        else
                        {
                            MessageBox.Show("Username or password incorrect.", "Error");
                        }
                    }
                    else
                    {
                        Program.client.Disconnect();
                        MessageBox.Show("Server connection attempt has failed.", "Error");
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Server connection attempt has failed.", "Error");
                }
            }
        }

        private void RegisterButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void LoginText_TextChanged(object sender, EventArgs e)
        {

        }

        private void Textbox_Enter(object sender, EventArgs e)
        {
            if ((((TextBox)sender).Name == "LoginText" && ((TextBox)sender).Text == "Login") ||
                (((TextBox)sender).Name == "PasswordText" && ((TextBox)sender).Text == "Password") ||
                (((TextBox)sender).Name == "ServerIPText" && ((TextBox)sender).Text == "Server IP"))
            {
                ((TextBox)sender).Text = "";
                ((TextBox)sender).ForeColor = SystemColors.WindowText;
            }
        }

        private void Textbox_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(((TextBox)sender).Text))
            {
                if (((TextBox)sender).Name == "LoginText")
                {
                    ((TextBox)sender).Text = "Login";
                    ((TextBox)sender).ForeColor = Color.Gray;
                }
                else if (((TextBox)sender).Name == "PasswordText")
                {
                    ((TextBox)sender).Text = "Password";
                    ((TextBox)sender).ForeColor = Color.Gray;
                }
                else if (((TextBox)sender).Name == "ServerIPText")
                {
                    ((TextBox)sender).Text = "Server IP";
                    ((TextBox)sender).ForeColor = Color.Gray;
                }
            }
        }

        private void Enter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LogInButton.PerformClick();
            }
        }
    }
}
