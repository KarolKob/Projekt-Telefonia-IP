namespace TalkaTIPClientV2
{
    partial class MainWindow
    {
        /// <summary>
        /// Wymagana zmienna projektanta.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Wyczyść wszystkie używane zasoby.
        /// </summary>
        /// <param name="disposing">prawda, jeżeli zarządzane zasoby powinny zostać zlikwidowane; Fałsz w przeciwnym wypadku.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kod generowany przez Projektanta formularzy systemu Windows

        /// <summary>
        /// Metoda wymagana do obsługi projektanta — nie należy modyfikować
        /// jej zawartości w edytorze kodu.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AllMessages = new System.Windows.Forms.TextBox();
            this.InsertMessageText = new System.Windows.Forms.TextBox();
            this.SendButton = new System.Windows.Forms.Button();
            this.BlockButton = new System.Windows.Forms.Button();
            this.ContactName = new System.Windows.Forms.TextBox();
            this.CallButton = new System.Windows.Forms.Button();
            this.UserSearch = new System.Windows.Forms.TextBox();
            this.SearchButton = new System.Windows.Forms.Button();
            this.FriendButton = new System.Windows.Forms.Button();
            this.LogOutButton = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.listView2 = new System.Windows.Forms.ListView();
            this.timerIAM = new System.Windows.Forms.Timer(this.components);
            this.gbCall = new System.Windows.Forms.GroupBox();
            this.cancelCallButton = new System.Windows.Forms.Button();
            this.labelCallUN = new System.Windows.Forms.Label();
            this.gbAnswerCall = new System.Windows.Forms.GroupBox();
            this.declineButton = new System.Windows.Forms.Button();
            this.labelCallingUN = new System.Windows.Forms.Label();
            this.answerButton = new System.Windows.Forms.Button();
            this.gbInCall = new System.Windows.Forms.GroupBox();
            this.labelInCallUN = new System.Windows.Forms.Label();
            this.disconnectButton = new System.Windows.Forms.Button();
            this.deleteFriendButton = new System.Windows.Forms.Button();
            this.UnblockButton = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.addGameAPIText = new System.Windows.Forms.TextBox();
            this.serverAddressLabel = new System.Windows.Forms.Label();
            this.addAPIButton = new System.Windows.Forms.Button();
            this.gameAPIList = new System.Windows.Forms.ListBox();
            this.searchOpponentButton = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.gbCall.SuspendLayout();
            this.gbAnswerCall.SuspendLayout();
            this.gbInCall.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // AllMessages
            // 
            this.AllMessages.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.AllMessages.Location = new System.Drawing.Point(213, 1);
            this.AllMessages.Multiline = true;
            this.AllMessages.Name = "AllMessages";
            this.AllMessages.ReadOnly = true;
            this.AllMessages.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.AllMessages.Size = new System.Drawing.Size(597, 376);
            this.AllMessages.TabIndex = 0;
            // 
            // InsertMessageText
            // 
            this.InsertMessageText.Location = new System.Drawing.Point(215, 422);
            this.InsertMessageText.Multiline = true;
            this.InsertMessageText.Name = "InsertMessageText";
            this.InsertMessageText.Size = new System.Drawing.Size(595, 69);
            this.InsertMessageText.TabIndex = 2;
            this.InsertMessageText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.InsertMessageText_KeyDown);
            // 
            // SendButton
            // 
            this.SendButton.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.SendButton.Location = new System.Drawing.Point(215, 384);
            this.SendButton.Name = "SendButton";
            this.SendButton.Size = new System.Drawing.Size(58, 32);
            this.SendButton.TabIndex = 3;
            this.SendButton.Text = "Send";
            this.SendButton.UseVisualStyleBackColor = false;
            this.SendButton.Click += new System.EventHandler(this.SendButton_Click);
            // 
            // BlockButton
            // 
            this.BlockButton.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.BlockButton.Location = new System.Drawing.Point(279, 384);
            this.BlockButton.Name = "BlockButton";
            this.BlockButton.Size = new System.Drawing.Size(58, 32);
            this.BlockButton.TabIndex = 4;
            this.BlockButton.Text = "Block";
            this.BlockButton.UseVisualStyleBackColor = false;
            this.BlockButton.Click += new System.EventHandler(this.BlockButton_Click);
            // 
            // ContactName
            // 
            this.ContactName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.ContactName.Location = new System.Drawing.Point(432, 386);
            this.ContactName.Name = "ContactName";
            this.ContactName.ReadOnly = true;
            this.ContactName.Size = new System.Drawing.Size(218, 26);
            this.ContactName.TabIndex = 5;
            // 
            // CallButton
            // 
            this.CallButton.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.CallButton.Location = new System.Drawing.Point(61, 31);
            this.CallButton.Name = "CallButton";
            this.CallButton.Size = new System.Drawing.Size(58, 32);
            this.CallButton.TabIndex = 6;
            this.CallButton.Text = "Call";
            this.CallButton.UseVisualStyleBackColor = false;
            this.CallButton.Click += new System.EventHandler(this.CallButton_Click);
            // 
            // UserSearch
            // 
            this.UserSearch.Location = new System.Drawing.Point(12, 12);
            this.UserSearch.Name = "UserSearch";
            this.UserSearch.Size = new System.Drawing.Size(195, 20);
            this.UserSearch.TabIndex = 7;
            // 
            // SearchButton
            // 
            this.SearchButton.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.SearchButton.Location = new System.Drawing.Point(73, 38);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(58, 28);
            this.SearchButton.TabIndex = 8;
            this.SearchButton.Text = "Search";
            this.SearchButton.UseVisualStyleBackColor = false;
            this.SearchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // FriendButton
            // 
            this.FriendButton.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.FriendButton.Location = new System.Drawing.Point(343, 384);
            this.FriendButton.Name = "FriendButton";
            this.FriendButton.Size = new System.Drawing.Size(69, 32);
            this.FriendButton.TabIndex = 9;
            this.FriendButton.Text = "Add friend";
            this.FriendButton.UseVisualStyleBackColor = false;
            this.FriendButton.Click += new System.EventHandler(this.FriendButton_Click);
            // 
            // LogOutButton
            // 
            this.LogOutButton.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.LogOutButton.Location = new System.Drawing.Point(9, 38);
            this.LogOutButton.Name = "LogOutButton";
            this.LogOutButton.Size = new System.Drawing.Size(58, 28);
            this.LogOutButton.TabIndex = 10;
            this.LogOutButton.Text = "Log out";
            this.LogOutButton.UseVisualStyleBackColor = false;
            this.LogOutButton.Click += new System.EventHandler(this.LogOutButton_Click);
            // 
            // listView1
            // 
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(187, 385);
            this.listView1.TabIndex = 11;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(12, 76);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(195, 327);
            this.tabControl1.TabIndex = 12;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.listView1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(187, 301);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Contacts";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.listView2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(187, 301);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "History";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // listView2
            // 
            this.listView2.HideSelection = false;
            this.listView2.Location = new System.Drawing.Point(0, 0);
            this.listView2.Name = "listView2";
            this.listView2.Size = new System.Drawing.Size(187, 381);
            this.listView2.TabIndex = 0;
            this.listView2.UseCompatibleStateImageBehavior = false;
            // 
            // timerIAM
            // 
            this.timerIAM.Interval = 60000;
            // 
            // gbCall
            // 
            this.gbCall.BackColor = System.Drawing.Color.DarkGray;
            this.gbCall.Controls.Add(this.cancelCallButton);
            this.gbCall.Controls.Add(this.labelCallUN);
            this.gbCall.Controls.Add(this.CallButton);
            this.gbCall.Location = new System.Drawing.Point(12, 405);
            this.gbCall.Name = "gbCall";
            this.gbCall.Size = new System.Drawing.Size(191, 73);
            this.gbCall.TabIndex = 12;
            this.gbCall.TabStop = false;
            // 
            // cancelCallButton
            // 
            this.cancelCallButton.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.cancelCallButton.Location = new System.Drawing.Point(61, 31);
            this.cancelCallButton.Name = "cancelCallButton";
            this.cancelCallButton.Size = new System.Drawing.Size(58, 32);
            this.cancelCallButton.TabIndex = 7;
            this.cancelCallButton.Text = "Cancel";
            this.cancelCallButton.UseVisualStyleBackColor = false;
            this.cancelCallButton.Visible = false;
            this.cancelCallButton.Click += new System.EventHandler(this.cancelCallButton_Click);
            // 
            // labelCallUN
            // 
            this.labelCallUN.AutoSize = true;
            this.labelCallUN.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelCallUN.Location = new System.Drawing.Point(69, 0);
            this.labelCallUN.Name = "labelCallUN";
            this.labelCallUN.Size = new System.Drawing.Size(40, 18);
            this.labelCallUN.TabIndex = 0;
            this.labelCallUN.Text = "User";
            // 
            // gbAnswerCall
            // 
            this.gbAnswerCall.BackColor = System.Drawing.Color.DarkGray;
            this.gbAnswerCall.Controls.Add(this.declineButton);
            this.gbAnswerCall.Controls.Add(this.labelCallingUN);
            this.gbAnswerCall.Controls.Add(this.answerButton);
            this.gbAnswerCall.Location = new System.Drawing.Point(12, 405);
            this.gbAnswerCall.Name = "gbAnswerCall";
            this.gbAnswerCall.Size = new System.Drawing.Size(191, 73);
            this.gbAnswerCall.TabIndex = 13;
            this.gbAnswerCall.TabStop = false;
            this.gbAnswerCall.Visible = false;
            // 
            // declineButton
            // 
            this.declineButton.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.declineButton.Location = new System.Drawing.Point(125, 31);
            this.declineButton.Name = "declineButton";
            this.declineButton.Size = new System.Drawing.Size(58, 32);
            this.declineButton.TabIndex = 7;
            this.declineButton.Text = "Decline";
            this.declineButton.UseVisualStyleBackColor = false;
            this.declineButton.Click += new System.EventHandler(this.declineButton_Click);
            // 
            // labelCallingUN
            // 
            this.labelCallingUN.AutoSize = true;
            this.labelCallingUN.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelCallingUN.Location = new System.Drawing.Point(69, 0);
            this.labelCallingUN.Name = "labelCallingUN";
            this.labelCallingUN.Size = new System.Drawing.Size(40, 18);
            this.labelCallingUN.TabIndex = 0;
            this.labelCallingUN.Text = "User";
            // 
            // answerButton
            // 
            this.answerButton.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.answerButton.Location = new System.Drawing.Point(10, 31);
            this.answerButton.Name = "answerButton";
            this.answerButton.Size = new System.Drawing.Size(58, 32);
            this.answerButton.TabIndex = 6;
            this.answerButton.Text = "Answer";
            this.answerButton.UseVisualStyleBackColor = false;
            this.answerButton.Click += new System.EventHandler(this.answerButton_Click);
            // 
            // gbInCall
            // 
            this.gbInCall.BackColor = System.Drawing.Color.DarkGray;
            this.gbInCall.Controls.Add(this.labelInCallUN);
            this.gbInCall.Controls.Add(this.disconnectButton);
            this.gbInCall.Location = new System.Drawing.Point(12, 405);
            this.gbInCall.Name = "gbInCall";
            this.gbInCall.Size = new System.Drawing.Size(191, 73);
            this.gbInCall.TabIndex = 13;
            this.gbInCall.TabStop = false;
            this.gbInCall.Visible = false;
            // 
            // labelInCallUN
            // 
            this.labelInCallUN.AutoSize = true;
            this.labelInCallUN.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelInCallUN.Location = new System.Drawing.Point(69, 0);
            this.labelInCallUN.Name = "labelInCallUN";
            this.labelInCallUN.Size = new System.Drawing.Size(40, 18);
            this.labelInCallUN.TabIndex = 0;
            this.labelInCallUN.Text = "User";
            // 
            // disconnectButton
            // 
            this.disconnectButton.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.disconnectButton.Location = new System.Drawing.Point(47, 31);
            this.disconnectButton.Name = "disconnectButton";
            this.disconnectButton.Size = new System.Drawing.Size(87, 32);
            this.disconnectButton.TabIndex = 6;
            this.disconnectButton.Text = "Disconnect";
            this.disconnectButton.UseVisualStyleBackColor = false;
            this.disconnectButton.Click += new System.EventHandler(this.disconnectButton_Click);
            // 
            // deleteFriendButton
            // 
            this.deleteFriendButton.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.deleteFriendButton.Location = new System.Drawing.Point(343, 383);
            this.deleteFriendButton.Name = "deleteFriendButton";
            this.deleteFriendButton.Size = new System.Drawing.Size(69, 32);
            this.deleteFriendButton.TabIndex = 14;
            this.deleteFriendButton.Text = "Unfriend";
            this.deleteFriendButton.UseVisualStyleBackColor = false;
            this.deleteFriendButton.Visible = false;
            this.deleteFriendButton.Click += new System.EventHandler(this.deleteFriendButton_Click);
            // 
            // UnblockButton
            // 
            this.UnblockButton.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.UnblockButton.Location = new System.Drawing.Point(279, 384);
            this.UnblockButton.Name = "UnblockButton";
            this.UnblockButton.Size = new System.Drawing.Size(58, 32);
            this.UnblockButton.TabIndex = 15;
            this.UnblockButton.Text = "Unblock";
            this.UnblockButton.UseVisualStyleBackColor = false;
            this.UnblockButton.Visible = false;
            this.UnblockButton.Click += new System.EventHandler(this.UnblockButton_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage3.Controls.Add(this.searchOpponentButton);
            this.tabPage3.Controls.Add(this.gameAPIList);
            this.tabPage3.Controls.Add(this.addAPIButton);
            this.tabPage3.Controls.Add(this.serverAddressLabel);
            this.tabPage3.Controls.Add(this.addGameAPIText);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(187, 301);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Games";
            // 
            // addGameAPIText
            // 
            this.addGameAPIText.Location = new System.Drawing.Point(9, 25);
            this.addGameAPIText.Name = "addGameAPIText";
            this.addGameAPIText.Size = new System.Drawing.Size(172, 20);
            this.addGameAPIText.TabIndex = 0;
            // 
            // serverAddressLabel
            // 
            this.serverAddressLabel.AutoSize = true;
            this.serverAddressLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.serverAddressLabel.Location = new System.Drawing.Point(31, 6);
            this.serverAddressLabel.Name = "serverAddressLabel";
            this.serverAddressLabel.Size = new System.Drawing.Size(122, 16);
            this.serverAddressLabel.TabIndex = 1;
            this.serverAddressLabel.Text = "Game API address";
            // 
            // addAPIButton
            // 
            this.addAPIButton.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.addAPIButton.Location = new System.Drawing.Point(57, 51);
            this.addAPIButton.Name = "addAPIButton";
            this.addAPIButton.Size = new System.Drawing.Size(58, 28);
            this.addAPIButton.TabIndex = 16;
            this.addAPIButton.Text = "Add API";
            this.addAPIButton.UseVisualStyleBackColor = false;
            this.addAPIButton.Click += new System.EventHandler(this.addAPIButton_Click);
            // 
            // gameAPIList
            // 
            this.gameAPIList.FormattingEnabled = true;
            this.gameAPIList.HorizontalScrollbar = true;
            this.gameAPIList.Location = new System.Drawing.Point(4, 92);
            this.gameAPIList.Name = "gameAPIList";
            this.gameAPIList.Size = new System.Drawing.Size(180, 173);
            this.gameAPIList.TabIndex = 17;
            // 
            // searchOpponentButton
            // 
            this.searchOpponentButton.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.searchOpponentButton.Location = new System.Drawing.Point(43, 267);
            this.searchOpponentButton.Name = "searchOpponentButton";
            this.searchOpponentButton.Size = new System.Drawing.Size(106, 28);
            this.searchOpponentButton.TabIndex = 18;
            this.searchOpponentButton.Text = "Search opponent";
            this.searchOpponentButton.UseVisualStyleBackColor = false;
            this.searchOpponentButton.Click += new System.EventHandler(this.searchOpponentButton_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(822, 491);
            this.Controls.Add(this.UnblockButton);
            this.Controls.Add(this.deleteFriendButton);
            this.Controls.Add(this.gbAnswerCall);
            this.Controls.Add(this.gbInCall);
            this.Controls.Add(this.gbCall);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.LogOutButton);
            this.Controls.Add(this.FriendButton);
            this.Controls.Add(this.SearchButton);
            this.Controls.Add(this.UserSearch);
            this.Controls.Add(this.ContactName);
            this.Controls.Add(this.BlockButton);
            this.Controls.Add(this.SendButton);
            this.Controls.Add(this.InsertMessageText);
            this.Controls.Add(this.AllMessages);
            this.Name = "MainWindow";
            this.Text = "TalkaTIP";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainWindow_FormClosed);
            this.Shown += new System.EventHandler(this.MainWindow_Shown);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.gbCall.ResumeLayout(false);
            this.gbCall.PerformLayout();
            this.gbAnswerCall.ResumeLayout(false);
            this.gbAnswerCall.PerformLayout();
            this.gbInCall.ResumeLayout(false);
            this.gbInCall.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox InsertMessageText;
        private System.Windows.Forms.Button SendButton;
        private System.Windows.Forms.Button BlockButton;
        private System.Windows.Forms.TextBox ContactName;
        private System.Windows.Forms.Button CallButton;
        private System.Windows.Forms.TextBox UserSearch;
        private System.Windows.Forms.Button SearchButton;
        private System.Windows.Forms.Button FriendButton;
        private System.Windows.Forms.Button LogOutButton;
        public System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        public System.Windows.Forms.ListView listView2;
        private System.Windows.Forms.Timer timerIAM;
        private System.Windows.Forms.Button disconnectButton;
        private System.Windows.Forms.Button declineButton;
        private System.Windows.Forms.Button answerButton;
        private System.Windows.Forms.Button cancelCallButton;
        public System.Windows.Forms.GroupBox gbCall;
        public System.Windows.Forms.GroupBox gbAnswerCall;
        public System.Windows.Forms.GroupBox gbInCall;
        public System.Windows.Forms.Label labelInCallUN;
        public System.Windows.Forms.Label labelCallingUN;
        public System.Windows.Forms.Label labelCallUN;
        private System.Windows.Forms.Button deleteFriendButton;
        private System.Windows.Forms.Button UnblockButton;
        public System.Windows.Forms.TextBox AllMessages;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.ListBox gameAPIList;
        private System.Windows.Forms.Button addAPIButton;
        private System.Windows.Forms.Label serverAddressLabel;
        private System.Windows.Forms.TextBox addGameAPIText;
        private System.Windows.Forms.Button searchOpponentButton;
    }
}

