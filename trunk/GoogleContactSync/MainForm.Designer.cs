namespace GoogleContactSync
{
    partial class MainForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.myUsername = new System.Windows.Forms.TextBox();
            this.myPassword = new System.Windows.Forms.TextBox();
            this.mySyncButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Username";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Password";
            // 
            // myUsername
            // 
            this.myUsername.Location = new System.Drawing.Point(74, 10);
            this.myUsername.Name = "myUsername";
            this.myUsername.Size = new System.Drawing.Size(180, 20);
            this.myUsername.TabIndex = 2;
            // 
            // myPassword
            // 
            this.myPassword.Location = new System.Drawing.Point(74, 37);
            this.myPassword.Name = "myPassword";
            this.myPassword.PasswordChar = '*';
            this.myPassword.Size = new System.Drawing.Size(180, 20);
            this.myPassword.TabIndex = 3;
            // 
            // mySyncButton
            // 
            this.mySyncButton.Location = new System.Drawing.Point(179, 63);
            this.mySyncButton.Name = "mySyncButton";
            this.mySyncButton.Size = new System.Drawing.Size(75, 23);
            this.mySyncButton.TabIndex = 4;
            this.mySyncButton.Text = "Sync";
            this.mySyncButton.UseVisualStyleBackColor = true;
            this.mySyncButton.Click += new System.EventHandler(this.mySyncButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(266, 210);
            this.Controls.Add(this.mySyncButton);
            this.Controls.Add(this.myPassword);
            this.Controls.Add(this.myUsername);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Text = "Google Contact Sync";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox myUsername;
        private System.Windows.Forms.TextBox myPassword;
        private System.Windows.Forms.Button mySyncButton;
    }
}

