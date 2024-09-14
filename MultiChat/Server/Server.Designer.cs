namespace Server
{
    partial class Server
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnSend = new Button();
            txbMessage = new TextBox();
            lsvMessage = new ListView();
            ListClient = new ListBox();
            SuspendLayout();
            // 
            // btnSend
            // 
            btnSend.Location = new Point(606, 388);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(182, 50);
            btnSend.TabIndex = 5;
            btnSend.Text = "Send";
            btnSend.UseVisualStyleBackColor = true;
            btnSend.Click += btnSend_Click;
            // 
            // txbMessage
            // 
            txbMessage.Location = new Point(237, 388);
            txbMessage.Multiline = true;
            txbMessage.Name = "txbMessage";
            txbMessage.Size = new Size(363, 50);
            txbMessage.TabIndex = 4;
            txbMessage.TextChanged += txbMessage_TextChanged;
            // 
            // lsvMessage
            // 
            lsvMessage.Location = new Point(237, 12);
            lsvMessage.Name = "lsvMessage";
            lsvMessage.Size = new Size(551, 370);
            lsvMessage.TabIndex = 3;
            lsvMessage.UseCompatibleStateImageBehavior = false;
            lsvMessage.View = View.List;
            // 
            // ListClient
            // 
            ListClient.FormattingEnabled = true;
            ListClient.Location = new Point(12, 12);
            ListClient.Name = "ListClient";
            ListClient.Size = new Size(219, 424);
            ListClient.TabIndex = 6;
            ListClient.SelectedIndexChanged += ListClient_SelectedIndexChanged;
            // 
            // Server
            // 
            AcceptButton = btnSend;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(ListClient);
            Controls.Add(btnSend);
            Controls.Add(txbMessage);
            Controls.Add(lsvMessage);
            Name = "Server";
            Text = "Server";
            FormClosed += Server_FormClosed;
            Load += btnSend_Click;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnSend;
        private TextBox txbMessage;
        private ListView lsvMessage;
        private ListBox ListClient;
    }
}
