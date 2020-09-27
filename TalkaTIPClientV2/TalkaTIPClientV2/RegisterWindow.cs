using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TalkaTIPClientV2
{
    public partial class RegisterWindow : Form
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, EventArgs e)
        {
            if (LoginText.Text == "Login" || PasswordText.Text == "Password" || RepeatPasswordText.Text == "Password" || ServeerIPText.Text == "Server IP")
            {
                MessageBox.Show("You need to fill all the fields.", "Error");
            }
            else if (PasswordText.Text != RepeatPasswordText.Text)
            {
                MessageBox.Show("The passwords don't match.", "Error");
            }
            else
            {
                // Password must contain:
                // Minimum eight characters, at least one uppercase letter, one lowercase letter, one number and one special character
                Regex regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&]{8,}$");

                Match match = regex.Match(PasswordText.Text);
                if (match.Success)
                {
                    try
                    {
                        Program.client = new Client(ServeerIPText.Text);
                        var serverKey = Communication.KeyExchange();
                        if (serverKey != null)
                        {
                            var sessKey = Program.security.SetSessionKey(serverKey);
                            if (Communication.Register(LoginText.Text, PasswordText.Text, sessKey) == true)
                            {
                                MessageBox.Show("User registration complete.", "Success");
                                DialogResult = DialogResult.No;
                                Close();
                            }
                            else
                            {
                                MessageBox.Show("The username is taken already, please enter another one.", "Error");
                                Program.client.Disconnect();
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
                else
                {
                    MessageBox.Show("The password must contain minimum eight characters," +
                        " at least one uppercase letter, one lowercase letter, one number and one special character.", "Error");
                }

            }
        }

        private void LogInButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
            Close();
        }

        private void Textbox_Enter(object sender, EventArgs e)
        {
            if ((((TextBox)sender).Name == "RepeatPasswordText" && ((TextBox)sender).Text == "Password") || 
                (((TextBox)sender).Name == "LoginText" && ((TextBox)sender).Text == "Login") ||
                (((TextBox)sender).Name == "ServeerIPText" && ((TextBox)sender).Text == "Server IP") ||
                (((TextBox)sender).Name == "PasswordText" && ((TextBox)sender).Text == "Password"))
            {
                ((TextBox)sender).Text = "";
                ((TextBox)sender).ForeColor = SystemColors.WindowText;
            }
        }

        private void Textbox_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(((TextBox)sender).Text))
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
                else if (((TextBox)sender).Name == "RepeatPasswordText")
                {
                    ((TextBox)sender).Text = "Password";
                    ((TextBox)sender).ForeColor = Color.Gray;
                }
                else if (((TextBox)sender).Name == "ServeerIPText")
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
                RegisterButton.PerformClick();
            }
        }
    }
}
