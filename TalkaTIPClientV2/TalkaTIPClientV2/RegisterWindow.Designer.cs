namespace TalkaTIPClientV2
{
    partial class RegisterWindow
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
            this.ServeerIPText = new System.Windows.Forms.TextBox();
            this.RepeatPasswordText = new System.Windows.Forms.TextBox();
            this.PasswordText = new System.Windows.Forms.TextBox();
            this.LoginText = new System.Windows.Forms.TextBox();
            this.RegisterButton = new System.Windows.Forms.Button();
            this.LogInButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ServeerIPText);
            this.groupBox1.Controls.Add(this.RepeatPasswordText);
            this.groupBox1.Controls.Add(this.PasswordText);
            this.groupBox1.Controls.Add(this.LoginText);
            this.groupBox1.Location = new System.Drawing.Point(79, 24);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 178);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Register";
            // 
            // ServeerIPText
            // 
            this.ServeerIPText.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.ServeerIPText.ForeColor = System.Drawing.Color.Gray;
            this.ServeerIPText.Location = new System.Drawing.Point(6, 136);
            this.ServeerIPText.Name = "ServeerIPText";
            this.ServeerIPText.Size = new System.Drawing.Size(187, 24);
            this.ServeerIPText.TabIndex = 3;
            this.ServeerIPText.Text = "Server IP";
            this.ServeerIPText.Enter += new System.EventHandler(this.Textbox_Enter);
            this.ServeerIPText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Enter_KeyDown);
            this.ServeerIPText.Leave += new System.EventHandler(this.Textbox_Leave);
            // 
            // RepeatPasswordText
            // 
            this.RepeatPasswordText.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.RepeatPasswordText.ForeColor = System.Drawing.Color.Gray;
            this.RepeatPasswordText.Location = new System.Drawing.Point(6, 96);
            this.RepeatPasswordText.Name = "RepeatPasswordText";
            this.RepeatPasswordText.Size = new System.Drawing.Size(187, 24);
            this.RepeatPasswordText.TabIndex = 2;
            this.RepeatPasswordText.Text = "Password";
            this.RepeatPasswordText.UseSystemPasswordChar = true;
            this.RepeatPasswordText.Enter += new System.EventHandler(this.Textbox_Enter);
            this.RepeatPasswordText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Enter_KeyDown);
            this.RepeatPasswordText.Leave += new System.EventHandler(this.Textbox_Leave);
            // 
            // PasswordText
            // 
            this.PasswordText.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.PasswordText.ForeColor = System.Drawing.Color.Gray;
            this.PasswordText.Location = new System.Drawing.Point(7, 57);
            this.PasswordText.Name = "PasswordText";
            this.PasswordText.Size = new System.Drawing.Size(187, 24);
            this.PasswordText.TabIndex = 1;
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
            this.LoginText.TabIndex = 0;
            this.LoginText.Text = "Login";
            this.LoginText.Enter += new System.EventHandler(this.Textbox_Enter);
            this.LoginText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Enter_KeyDown);
            this.LoginText.Leave += new System.EventHandler(this.Textbox_Leave);
            // 
            // RegisterButton
            // 
            this.RegisterButton.Location = new System.Drawing.Point(86, 230);
            this.RegisterButton.Name = "RegisterButton";
            this.RegisterButton.Size = new System.Drawing.Size(75, 23);
            this.RegisterButton.TabIndex = 1;
            this.RegisterButton.Text = "Register";
            this.RegisterButton.UseVisualStyleBackColor = true;
            this.RegisterButton.Click += new System.EventHandler(this.RegisterButton_Click);
            // 
            // LogInButton
            // 
            this.LogInButton.Location = new System.Drawing.Point(197, 230);
            this.LogInButton.Name = "LogInButton";
            this.LogInButton.Size = new System.Drawing.Size(75, 23);
            this.LogInButton.TabIndex = 2;
            this.LogInButton.Text = "Log in";
            this.LogInButton.UseVisualStyleBackColor = true;
            this.LogInButton.Click += new System.EventHandler(this.LogInButton_Click);
            // 
            // RegisterWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(380, 288);
            this.Controls.Add(this.LogInButton);
            this.Controls.Add(this.RegisterButton);
            this.Controls.Add(this.groupBox1);
            this.Name = "RegisterWindow";
            this.Text = "TalkaTIP";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox RepeatPasswordText;
        private System.Windows.Forms.TextBox PasswordText;
        private System.Windows.Forms.TextBox LoginText;
        private System.Windows.Forms.Button RegisterButton;
        private System.Windows.Forms.Button LogInButton;
        private System.Windows.Forms.TextBox ServeerIPText;
    }
}