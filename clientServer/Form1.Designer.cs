namespace clientServer
{
    partial class Form1
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
            label1 = new Label();
            txtNick = new TextBox();
            lblStatus = new Label();
            txtGracze = new TextBox();
            label3 = new Label();
            btnDolacz = new Button();
            lblLocalIP = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(39, 32);
            label1.Name = "label1";
            label1.Size = new Size(34, 15);
            label1.TabIndex = 0;
            label1.Text = "Nick:";
            // 
            // txtNick
            // 
            txtNick.Location = new Point(39, 63);
            txtNick.Name = "txtNick";
            txtNick.Size = new Size(127, 23);
            txtNick.TabIndex = 1;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(82, 200);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(0, 15);
            lblStatus.TabIndex = 2;
            // 
            // txtGracze
            // 
            txtGracze.Location = new Point(355, 63);
            txtGracze.Multiline = true;
            txtGracze.Name = "txtGracze";
            txtGracze.ReadOnly = true;
            txtGracze.Size = new Size(319, 353);
            txtGracze.TabIndex = 3;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(355, 36);
            label3.Name = "label3";
            label3.Size = new Size(71, 15);
            label3.TabIndex = 4;
            label3.Text = "Lista graczy:";
            // 
            // btnDolacz
            // 
            btnDolacz.Location = new Point(91, 110);
            btnDolacz.Name = "btnDolacz";
            btnDolacz.Size = new Size(75, 23);
            btnDolacz.TabIndex = 5;
            btnDolacz.Text = "Dołącz";
            btnDolacz.UseVisualStyleBackColor = true;
            btnDolacz.Click += btnDolacz_Click;
            // 
            // lblLocalIP
            // 
            lblLocalIP.AutoSize = true;
            lblLocalIP.Location = new Point(12, 426);
            lblLocalIP.Name = "lblLocalIP";
            lblLocalIP.Size = new Size(48, 15);
            lblLocalIP.TabIndex = 6;
            lblLocalIP.Text = "Twój IP:";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(725, 450);
            Controls.Add(lblLocalIP);
            Controls.Add(btnDolacz);
            Controls.Add(label3);
            Controls.Add(txtGracze);
            Controls.Add(lblStatus);
            Controls.Add(txtNick);
            Controls.Add(label1);
            Name = "Form1";
            Text = "Client Server";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox txtNick;
        private Label lblStatus;
        private TextBox txtGracze;
        private Label label3;
        private Button btnDolacz;
        private Label lblLocalIP;
    }
}
