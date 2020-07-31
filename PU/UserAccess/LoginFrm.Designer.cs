namespace PU
{
    partial class LoginFrm
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
            this.radButton1 = new Telerik.WinControls.UI.RadButton();
            this.radButton2 = new Telerik.WinControls.UI.RadButton();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel2 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel3 = new Telerik.WinControls.UI.RadLabel();
            this.loginTextBox = new Telerik.WinControls.UI.RadTextBox();
            this.passwordTextBox = new Telerik.WinControls.UI.RadTextBox();
            this.rememberNameCheckBox = new Telerik.WinControls.UI.RadCheckBox();
            this.loginErrorLabel = new Telerik.WinControls.UI.RadLabel();
            ((System.ComponentModel.ISupportInitialize)(this.radButton1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.loginTextBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.passwordTextBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rememberNameCheckBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.loginErrorLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radButton1
            // 
            this.radButton1.Location = new System.Drawing.Point(15, 182);
            this.radButton1.Name = "radButton1";
            this.radButton1.Size = new System.Drawing.Size(110, 24);
            this.radButton1.TabIndex = 3;
            this.radButton1.Text = "Отмена";
            this.radButton1.ThemeName = "Office2013Light";
            this.radButton1.Click += new System.EventHandler(this.radButton1_Click);
            // 
            // radButton2
            // 
            this.radButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.radButton2.Location = new System.Drawing.Point(198, 182);
            this.radButton2.Name = "radButton2";
            this.radButton2.Size = new System.Drawing.Size(110, 24);
            this.radButton2.TabIndex = 2;
            this.radButton2.Text = "Вход";
            this.radButton2.ThemeName = "Office2013Light";
            this.radButton2.Click += new System.EventHandler(this.radButton2_Click);
            // 
            // radLabel1
            // 
            this.radLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radLabel1.AutoSize = false;
            this.radLabel1.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.radLabel1.Location = new System.Drawing.Point(12, 12);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(296, 18);
            this.radLabel1.TabIndex = 3;
            this.radLabel1.Text = "Авторизация";
            this.radLabel1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.radLabel1.ThemeName = "Office2013Light";
            // 
            // radLabel2
            // 
            this.radLabel2.AutoSize = false;
            this.radLabel2.Location = new System.Drawing.Point(15, 84);
            this.radLabel2.Name = "radLabel2";
            this.radLabel2.Size = new System.Drawing.Size(60, 19);
            this.radLabel2.TabIndex = 4;
            this.radLabel2.Text = "Логин";
            this.radLabel2.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.radLabel2.ThemeName = "Office2013Light";
            // 
            // radLabel3
            // 
            this.radLabel3.AutoSize = false;
            this.radLabel3.Location = new System.Drawing.Point(15, 115);
            this.radLabel3.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.radLabel3.Name = "radLabel3";
            this.radLabel3.Size = new System.Drawing.Size(60, 18);
            this.radLabel3.TabIndex = 4;
            this.radLabel3.Text = "Пароль";
            this.radLabel3.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.radLabel3.ThemeName = "Office2013Light";
            // 
            // loginTextBox
            // 
            this.loginTextBox.Location = new System.Drawing.Point(93, 85);
            this.loginTextBox.Margin = new System.Windows.Forms.Padding(15, 3, 3, 3);
            this.loginTextBox.Name = "loginTextBox";
            this.loginTextBox.Size = new System.Drawing.Size(132, 21);
            this.loginTextBox.TabIndex = 0;
            this.loginTextBox.ThemeName = "Office2013Light";
            this.loginTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.loginTextBox_KeyPress);
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Location = new System.Drawing.Point(93, 115);
            this.passwordTextBox.Margin = new System.Windows.Forms.Padding(15, 3, 3, 3);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.PasswordChar = '*';
            this.passwordTextBox.Size = new System.Drawing.Size(132, 21);
            this.passwordTextBox.TabIndex = 1;
            this.passwordTextBox.ThemeName = "Office2013Light";
            this.passwordTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.passwordTextBox_KeyPress);
            // 
            // rememberNameCheckBox
            // 
            this.rememberNameCheckBox.Location = new System.Drawing.Point(231, 86);
            this.rememberNameCheckBox.Name = "rememberNameCheckBox";
            this.rememberNameCheckBox.Size = new System.Drawing.Size(84, 19);
            this.rememberNameCheckBox.TabIndex = 4;
            this.rememberNameCheckBox.Text = "запомнить";
            this.rememberNameCheckBox.ThemeName = "Office2013Light";
            // 
            // loginErrorLabel
            // 
            this.loginErrorLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.loginErrorLabel.AutoSize = false;
            this.loginErrorLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.loginErrorLabel.Location = new System.Drawing.Point(12, 142);
            this.loginErrorLabel.Name = "loginErrorLabel";
            this.loginErrorLabel.Size = new System.Drawing.Size(296, 34);
            this.loginErrorLabel.TabIndex = 8;
            this.loginErrorLabel.Text = "Ошибка авторизации!";
            this.loginErrorLabel.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.loginErrorLabel.ThemeName = "Office2013Light";
            this.loginErrorLabel.Visible = false;
            ((Telerik.WinControls.UI.RadLabelElement)(this.loginErrorLabel.GetChildAt(0))).TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            ((Telerik.WinControls.UI.RadLabelElement)(this.loginErrorLabel.GetChildAt(0))).Text = "Ошибка авторизации!";
            ((Telerik.WinControls.UI.RadLabelElement)(this.loginErrorLabel.GetChildAt(0))).ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            // 
            // LoginFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(320, 218);
            this.Controls.Add(this.loginErrorLabel);
            this.Controls.Add(this.rememberNameCheckBox);
            this.Controls.Add(this.passwordTextBox);
            this.Controls.Add(this.loginTextBox);
            this.Controls.Add(this.radLabel3);
            this.Controls.Add(this.radLabel2);
            this.Controls.Add(this.radLabel1);
            this.Controls.Add(this.radButton2);
            this.Controls.Add(this.radButton1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "LoginFrm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Авторизация";
            this.ThemeName = "Office2013Light";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LoginFrm_FormClosing);
            this.Load += new System.EventHandler(this.LoginFrm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radButton1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.loginTextBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.passwordTextBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rememberNameCheckBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.loginErrorLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadButton radButton1;
        private Telerik.WinControls.UI.RadButton radButton2;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadLabel radLabel2;
        private Telerik.WinControls.UI.RadLabel radLabel3;
        private Telerik.WinControls.UI.RadTextBox loginTextBox;
        private Telerik.WinControls.UI.RadTextBox passwordTextBox;
        private Telerik.WinControls.UI.RadLabel loginErrorLabel;
        public Telerik.WinControls.UI.RadCheckBox rememberNameCheckBox;
    }
}
