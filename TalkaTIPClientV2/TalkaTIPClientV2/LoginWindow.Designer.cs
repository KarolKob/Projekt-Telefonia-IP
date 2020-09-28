namespace TalkaTIPClientV2
{
    partial class LoginWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ServerIPText = new System.Windows.Forms.TextBox();
            this.PasswordText = new System.Windows.Forms.TextBox();
            this.LoginText = new System.Windows.Forms.TextBox();
            this.LogInButton = new System.Windows.Forms.Button();
            this.RegisterButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ServerIPText);
            this.groupBox1.Controls.Add(this.PasswordText);
            this.groupBox1.Controls.Add(this.LoginText);
            this.groupBox1.Location = new System.Drawing.Point(73, 41);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 134);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Log in";
            // 
            // ServerIPText
            // 
            this.ServerIPText.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.ServerIPText.ForeColor = System.Drawing.Color.Gray;
            this.ServerIPText.Location = new System.Drawing.Point(6, 100);
            this.ServerIPText.Name = "ServerIPText";
            this.ServerIPText.Size = new System.Drawing.Size(187, 24);
            this.ServerIPText.TabIndex = 4;
            this.ServerIPText.Text = "Server IP";
            this.ServerIPText.Enter += new System.EventHandler(this.Textbox_Enter);
            this.ServerIPText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Enter_KeyDown);
            this.ServerIPText.Leave += new System.EventHandler(this.Textbox_Leave);
            // 
            // PasswordText
            // 
            this.PasswordText.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.PasswordText.ForeColor = System.Drawing.Color.Gray;
            this.PasswordText.Location = new System.Drawing.Point(7, 61);
            this.PasswordText.Name = "PasswordText";
            this.PasswordText.Size = new System.Drawing.Size(187, 24);
            this.PasswordText.TabIndex = 3;
            this.PasswordText.Text = "Password";
            this.PasswordText.UseSystemPasswordChar = true;
            this.PasswordText.Enter += new System.EventHandler(this.Textbox_Enter);
            this.PasswordText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Enter_KeyDown);
            this.PasswordText.Leave += new System.EventHandler(this.Textbox_Leave);
            // 
            // LoginText
            // 
            this.LoginText.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.LoginText.ForeColor = System.Drawing.Color.Gray;
            this.LoginText.Location = new System.Drawing.Point(7, 20);
            this.LoginText.Name = "LoginText";
            this.LoginText.Size = new System.Drawing.Size(187, 24);
            this.LoginText.TabIndex = 2;
            this.LoginText.Text = "Login";
            this.LoginText.TextChanged += new System.EventHandler(this.LoginText_TextChanged);
            this.LoginText.Enter += new System.EventHandler(this.Textbox_Enter);
            this.LoginText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Enter_KeyDown);
            this.LoginText.Leave += new System.EventHandler(this.Textbox_Leave);
            // 
            // LogInButton
            // 
            this.LogInButton.Location = new System.Drawing.Point(80, 201);
            this.LogInButton.Name = "LogInButton";
            this.LogInButton.Size = new System.Drawing.Size(75, 23);
            this.LogInButton.TabIndex = 1;
            this.LogInButton.Text = "Log in";
            this.LogInButton.UseVisualStyleBackColor = true;
            this.LogInButton.Click += new System.EventHandler(this.LogInButton_Click);
            // 
            // RegisterButton
            // 
            this.RegisterButton.Location = new System.Drawing.Point(192, 201);
            this.RegisterButton.Name = "RegisterButton";
            this.RegisterButton.Size = new System.Drawing.Size(75, 23);
            this.RegisterButton.TabIndex = 2;
            this.RegisterButton.Text = "Register";
            this.RegisterButton.UseVisualStyleBackColor = true;
            this.RegisterButton.Click += new System.EventHandler(this.RegisterButton_Click);
            // 
            // LoginWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(349, 256);
            this.Controls.Add(this.RegisterButton);
            this.Controls.Add(this.LogInButton);
            this.Controls.Add(this.groupBox1);
            this.Name = "LoginWindow";
            this.Text = "TalkaTIP";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox PasswordText;
        private System.Windows.Forms.TextBox LoginText;
        private System.Windows.Forms.Button LogInButton;
        private System.Windows.Forms.Button RegisterButton;
        public System.Windows.Forms.TextBox ServerIPText;
    }
}