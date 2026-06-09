namespace FrmProject
{
    partial class FrmLogin
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
            panelLeft = new Panel();
            lblSubtitle = new Label();
            picBoxIcon = new PictureBox();
            lblTitle = new Label();
            panelRight = new Panel();
            lblVersion = new Label();
            labelSub = new Label();
            btnLogin = new Button();
            chkRemember = new CheckBox();
            pnlContainerText = new Panel();
            lblText = new Label();
            txtControl = new TextBox();
            btnShowPass = new Button();
            panelUsername = new Panel();
            lblIconUser = new Label();
            txtUsername = new TextBox();
            panel1 = new Panel();
            lblDangNhap = new Label();
            panelLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picBoxIcon).BeginInit();
            panelRight.SuspendLayout();
            pnlContainerText.SuspendLayout();
            panelUsername.SuspendLayout();
            SuspendLayout();
            // 
            // panelLeft
            // 
            panelLeft.BackColor = Color.FromArgb(26, 92, 56);
            panelLeft.Controls.Add(lblSubtitle);
            panelLeft.Controls.Add(picBoxIcon);
            panelLeft.Controls.Add(lblTitle);
            panelLeft.Dock = DockStyle.Left;
            panelLeft.Location = new Point(0, 0);
            panelLeft.Name = "panelLeft";
            panelLeft.Size = new Size(477, 760);
            panelLeft.TabIndex = 0;
            // 
            // lblSubtitle
            // 
            lblSubtitle.BackColor = Color.FromArgb(26, 92, 56);
            lblSubtitle.Font = new Font("Segoe UI", 11F, FontStyle.Italic);
            lblSubtitle.ForeColor = Color.FromArgb(180, 230, 180);
            lblSubtitle.Location = new Point(88, 645);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new Size(306, 76);
            lblSubtitle.TabIndex = 2;
            lblSubtitle.Text = "Quản lý thiết bị hiệu quả tiện lợi và minh bạch";
            lblSubtitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // picBoxIcon
            // 
            picBoxIcon.BackColor = Color.Transparent;
            picBoxIcon.Image = Properties.Resources.box;
            picBoxIcon.Location = new Point(160, 372);
            picBoxIcon.Name = "picBoxIcon";
            picBoxIcon.Size = new Size(145, 145);
            picBoxIcon.SizeMode = PictureBoxSizeMode.Zoom;
            picBoxIcon.TabIndex = 1;
            picBoxIcon.TabStop = false;
            // 
            // lblTitle
            // 
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(77, 117);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(328, 176);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "🏫 Hệ Thống Quản Lý Mượn Trả Thiết Bị Phòng Học";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panelRight
            // 
            panelRight.Controls.Add(lblVersion);
            panelRight.Controls.Add(labelSub);
            panelRight.Controls.Add(btnLogin);
            panelRight.Controls.Add(chkRemember);
            panelRight.Controls.Add(pnlContainerText);
            panelRight.Controls.Add(panelUsername);
            panelRight.Controls.Add(lblDangNhap);
            panelRight.Cursor = Cursors.Hand;
            panelRight.Dock = DockStyle.Fill;
            panelRight.Location = new Point(477, 0);
            panelRight.Name = "panelRight";
            panelRight.Size = new Size(483, 760);
            panelRight.TabIndex = 1;
            // 
            // lblVersion
            // 
            lblVersion.ForeColor = Color.LightGray;
            lblVersion.Location = new Point(113, 705);
            lblVersion.Name = "lblVersion";
            lblVersion.Size = new Size(304, 25);
            lblVersion.TabIndex = 6;
            lblVersion.Text = "v1.0.0 © 2026 Hệ Thống Quản Lý Thiết Bị";
            // 
            // labelSub
            // 
            labelSub.Font = new Font("Segoe UI", 10F);
            labelSub.ForeColor = Color.Gray;
            labelSub.Location = new Point(97, 218);
            labelSub.Name = "labelSub";
            labelSub.Size = new Size(320, 30);
            labelSub.TabIndex = 5;
            labelSub.Text = "Nhập thông tin tài khoản để tiếp tục";
            // 
            // btnLogin
            // 
            btnLogin.BackColor = Color.FromArgb(26, 92, 56);
            btnLogin.Cursor = Cursors.Hand;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.Font = new Font("Segoe UI", 13.2000008F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnLogin.ForeColor = Color.White;
            btnLogin.Location = new Point(97, 545);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(299, 55);
            btnLogin.TabIndex = 4;
            btnLogin.Text = "ĐĂNG NHẬP";
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.Click += btnLogin_Click;
            // 
            // chkRemember
            // 
            chkRemember.Font = new Font("Segoe UI", 10F);
            chkRemember.Location = new Point(70, 482);
            chkRemember.Name = "chkRemember";
            chkRemember.Size = new Size(205, 35);
            chkRemember.TabIndex = 3;
            chkRemember.Text = "Ghi nhớ đăng nhập";
            chkRemember.UseVisualStyleBackColor = true;
            // 
            // pnlContainerText
            // 
            pnlContainerText.BackColor = Color.WhiteSmoke;
            pnlContainerText.Controls.Add(lblText);
            pnlContainerText.Controls.Add(txtControl);
            pnlContainerText.Controls.Add(btnShowPass);
            pnlContainerText.Location = new Point(70, 399);
            pnlContainerText.Name = "pnlContainerText";
            pnlContainerText.Size = new Size(350, 74);
            pnlContainerText.TabIndex = 2;
            // 
            // lblText
            // 
            lblText.Font = new Font("Segoe UI Emoji", 14F);
            lblText.Location = new Point(16, 21);
            lblText.Name = "lblText";
            lblText.Size = new Size(41, 38);
            lblText.TabIndex = 5;
            lblText.Text = "🔒";
            // 
            // txtControl
            // 
            txtControl.BackColor = Color.FromArgb(245, 248, 245);
            txtControl.BorderStyle = BorderStyle.None;
            txtControl.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtControl.Location = new Point(63, 25);
            txtControl.Name = "txtControl";
            txtControl.PasswordChar = '*';
            txtControl.PlaceholderText = "Mật khẩu";
            txtControl.Size = new Size(244, 24);
            txtControl.TabIndex = 4;
            // 
            // btnShowPass
            // 
            btnShowPass.BackColor = Color.Transparent;
            btnShowPass.Cursor = Cursors.Hand;
            btnShowPass.FlatAppearance.BorderSize = 0;
            btnShowPass.FlatStyle = FlatStyle.Flat;
            btnShowPass.Location = new Point(307, 17);
            btnShowPass.Name = "btnShowPass";
            btnShowPass.Size = new Size(40, 40);
            btnShowPass.TabIndex = 0;
            btnShowPass.Text = "👁";
            btnShowPass.UseVisualStyleBackColor = false;
            // 
            // panelUsername
            // 
            panelUsername.BackColor = Color.WhiteSmoke;
            panelUsername.Controls.Add(lblIconUser);
            panelUsername.Controls.Add(txtUsername);
            panelUsername.Controls.Add(panel1);
            panelUsername.Location = new Point(70, 275);
            panelUsername.Name = "panelUsername";
            panelUsername.Size = new Size(350, 74);
            panelUsername.TabIndex = 1;
            // 
            // lblIconUser
            // 
            lblIconUser.Font = new Font("Segoe UI Emoji", 14F);
            lblIconUser.Location = new Point(16, 21);
            lblIconUser.Name = "lblIconUser";
            lblIconUser.Size = new Size(41, 40);
            lblIconUser.TabIndex = 4;
            lblIconUser.Text = "👤";
            // 
            // txtUsername
            // 
            txtUsername.BackColor = Color.FromArgb(245, 248, 245);
            txtUsername.BorderStyle = BorderStyle.None;
            txtUsername.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtUsername.Location = new Point(63, 21);
            txtUsername.Name = "txtUsername";
            txtUsername.PlaceholderText = "Tên đăng nhập";
            txtUsername.Size = new Size(263, 24);
            txtUsername.TabIndex = 3;
            // 
            // panel1
            // 
            panel1.BackColor = Color.WhiteSmoke;
            panel1.Location = new Point(0, 97);
            panel1.Name = "panel1";
            panel1.Size = new Size(350, 76);
            panel1.TabIndex = 2;
            // 
            // lblDangNhap
            // 
            lblDangNhap.Font = new Font("Segoe UI", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDangNhap.ForeColor = Color.FromArgb(27, 94, 60);
            lblDangNhap.Location = new Point(114, 156);
            lblDangNhap.Name = "lblDangNhap";
            lblDangNhap.Size = new Size(245, 62);
            lblDangNhap.TabIndex = 0;
            lblDangNhap.Text = "Đăng Nhập";
            // 
            // FrmLogin
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(960, 760);
            Controls.Add(panelRight);
            Controls.Add(panelLeft);
            Name = "FrmLogin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Đăng nhập";
            panelLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picBoxIcon).EndInit();
            panelRight.ResumeLayout(false);
            pnlContainerText.ResumeLayout(false);
            pnlContainerText.PerformLayout();
            panelUsername.ResumeLayout(false);
            panelUsername.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelLeft;
        private Label lblTitle;
        private Label lblSubtitle;
        private PictureBox picBoxIcon;
        private Panel panelRight;
        private Label lblDangNhap;
        private Panel panelUsername;
        private TextBox txtUsername;
        private Panel panel1;
        private Panel pnlContainerText;
        private Button btnShowPass;
        private CheckBox chkRemember;
        private TextBox txtControl;
        private Button btnLogin;
        private Label lblVersion;
        private Label labelSub;
        private Label lblText;
        private Label lblIconUser;
    }
}





