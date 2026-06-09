namespace FrmProject.GUI
{
    partial class UcQuanLyCaThe
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            dgvInstances = new DataGridView();
            txtAssetCode = new TextBox();
            cmbStatus = new ComboBox();
            txtCondition = new TextBox();
            btnThem = new Button();
            btnXoa = new Button();
            btnCapNhat = new Button();
            pnlTop = new Panel();
            lblAssetCode = new Label();
            lblNextCode = new Label();
            lblStatus = new Label();
            lblCondition = new Label();
            pnlInfo = new Panel();
            lblDeviceInfo = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvInstances).BeginInit();
            pnlTop.SuspendLayout();
            pnlInfo.SuspendLayout();
            SuspendLayout();
            // 
            // dgvInstances
            // 
            dgvInstances.AllowUserToAddRows = false;
            dgvInstances.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvInstances.BackgroundColor = Color.White;
            dgvInstances.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvInstances.ColumnHeadersHeight = 34;
            dgvInstances.Dock = DockStyle.Fill;
            dgvInstances.Location = new Point(0, 160);
            dgvInstances.Name = "dgvInstances";
            dgvInstances.ReadOnly = true;
            dgvInstances.RowHeadersVisible = false;
            dgvInstances.RowHeadersWidth = 51;
            dgvInstances.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvInstances.Size = new Size(1079, 440);
            dgvInstances.TabIndex = 0;
            // 
            // txtAssetCode
            // 
            txtAssetCode.BackColor = Color.FromArgb(236, 250, 240);
            txtAssetCode.BorderStyle = BorderStyle.FixedSingle;
            txtAssetCode.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            txtAssetCode.ForeColor = Color.FromArgb(26, 92, 56);
            txtAssetCode.Location = new Point(182, 12);
            txtAssetCode.Name = "txtAssetCode";
            txtAssetCode.ReadOnly = true;
            txtAssetCode.Size = new Size(177, 30);
            txtAssetCode.TabIndex = 0;
            txtAssetCode.TabStop = false;
            // 
            // cmbStatus
            // 
            cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStatus.Font = new Font("Segoe UI", 9.5F);
            cmbStatus.Items.AddRange(new object[] { "Có sẵn", "Đang mượn", "Hỏng", "Mất", "Đang bảo trì", "Ngừng sử dụng" });
            cmbStatus.Location = new Point(165, 64);
            cmbStatus.Name = "cmbStatus";
            cmbStatus.Size = new Size(170, 29);
            cmbStatus.TabIndex = 1;
            // 
            // txtCondition
            // 
            txtCondition.Font = new Font("Segoe UI", 9.5F);
            txtCondition.Location = new Point(510, 12);
            txtCondition.Name = "txtCondition";
            txtCondition.PlaceholderText = "VD: Mới, Còn tốt, Bị xước...";
            txtCondition.Size = new Size(300, 29);
            txtCondition.TabIndex = 2;
            // 
            // btnThem
            // 
            btnThem.BackColor = Color.FromArgb(40, 167, 69);
            btnThem.Cursor = Cursors.Hand;
            btnThem.FlatAppearance.BorderSize = 0;
            btnThem.FlatStyle = FlatStyle.Flat;
            btnThem.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnThem.ForeColor = Color.White;
            btnThem.Location = new Point(510, 58);
            btnThem.Name = "btnThem";
            btnThem.Size = new Size(105, 36);
            btnThem.TabIndex = 3;
            btnThem.Text = "➕ Thêm mới";
            btnThem.UseVisualStyleBackColor = false;
            btnThem.Click += BtnThem_Click;
            // 
            // btnXoa
            // 
            btnXoa.BackColor = Color.FromArgb(220, 53, 69);
            btnXoa.Cursor = Cursors.Hand;
            btnXoa.FlatAppearance.BorderSize = 0;
            btnXoa.FlatStyle = FlatStyle.Flat;
            btnXoa.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnXoa.ForeColor = Color.White;
            btnXoa.Location = new Point(762, 60);
            btnXoa.Name = "btnXoa";
            btnXoa.Size = new Size(105, 36);
            btnXoa.TabIndex = 5;
            btnXoa.Text = "🗑 Xóa";
            btnXoa.UseVisualStyleBackColor = false;
            btnXoa.Click += BtnXoa_Click;
            // 
            // btnCapNhat
            // 
            btnCapNhat.BackColor = Color.FromArgb(255, 128, 0);
            btnCapNhat.Cursor = Cursors.Hand;
            btnCapNhat.Enabled = false;
            btnCapNhat.FlatAppearance.BorderSize = 0;
            btnCapNhat.FlatStyle = FlatStyle.Flat;
            btnCapNhat.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnCapNhat.ForeColor = Color.White;
            btnCapNhat.Location = new Point(625, 58);
            btnCapNhat.Name = "btnCapNhat";
            btnCapNhat.Size = new Size(122, 36);
            btnCapNhat.TabIndex = 4;
            btnCapNhat.Text = "💾 Cập nhật";
            btnCapNhat.UseVisualStyleBackColor = false;
            btnCapNhat.Click += BtnCapNhat_Click;
            // 
            // pnlTop
            // 
            pnlTop.BackColor = Color.White;
            pnlTop.Controls.Add(lblAssetCode);
            pnlTop.Controls.Add(txtAssetCode);
            pnlTop.Controls.Add(lblNextCode);
            pnlTop.Controls.Add(lblStatus);
            pnlTop.Controls.Add(cmbStatus);
            pnlTop.Controls.Add(lblCondition);
            pnlTop.Controls.Add(txtCondition);
            pnlTop.Controls.Add(btnThem);
            pnlTop.Controls.Add(btnCapNhat);
            pnlTop.Controls.Add(btnXoa);
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Location = new Point(0, 46);
            pnlTop.Name = "pnlTop";
            pnlTop.Padding = new Padding(14, 8, 14, 8);
            pnlTop.Size = new Size(1079, 114);
            pnlTop.TabIndex = 1;
            // 
            // lblAssetCode
            // 
            lblAssetCode.AutoSize = true;
            lblAssetCode.Font = new Font("Segoe UI", 9.5F);
            lblAssetCode.ForeColor = Color.FromArgb(80, 80, 80);
            lblAssetCode.Location = new Point(14, 16);
            lblAssetCode.Name = "lblAssetCode";
            lblAssetCode.Size = new Size(145, 21);
            lblAssetCode.TabIndex = 0;
            lblAssetCode.Text = "Mã cá thể tiếp theo:";
            // 
            // lblNextCode
            // 
            lblNextCode.AutoSize = true;
            lblNextCode.Font = new Font("Segoe UI", 8.5F, FontStyle.Italic);
            lblNextCode.ForeColor = Color.Gray;
            lblNextCode.Location = new Point(165, 44);
            lblNextCode.Name = "lblNextCode";
            lblNextCode.Size = new Size(286, 20);
            lblNextCode.TabIndex = 1;
            lblNextCode.Text = "← Mã được sinh tự động từ mã loại thiết bị";
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI", 9.5F);
            lblStatus.ForeColor = Color.FromArgb(80, 80, 80);
            lblStatus.Location = new Point(14, 68);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(82, 21);
            lblStatus.TabIndex = 2;
            lblStatus.Text = "Trạng thái:";
            // 
            // lblCondition
            // 
            lblCondition.AutoSize = true;
            lblCondition.Font = new Font("Segoe UI", 9.5F);
            lblCondition.ForeColor = Color.FromArgb(80, 80, 80);
            lblCondition.Location = new Point(365, 16);
            lblCondition.Name = "lblCondition";
            lblCondition.Size = new Size(138, 21);
            lblCondition.TabIndex = 3;
            lblCondition.Text = "Tình trạng (mô tả):";
            // 
            // pnlInfo
            // 
            pnlInfo.BackColor = Color.FromArgb(240, 247, 244);
            pnlInfo.Controls.Add(lblDeviceInfo);
            pnlInfo.Dock = DockStyle.Top;
            pnlInfo.Location = new Point(0, 0);
            pnlInfo.Name = "pnlInfo";
            pnlInfo.Padding = new Padding(14, 8, 14, 8);
            pnlInfo.Size = new Size(1079, 46);
            pnlInfo.TabIndex = 2;
            // 
            // lblDeviceInfo
            // 
            lblDeviceInfo.Dock = DockStyle.Fill;
            lblDeviceInfo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblDeviceInfo.ForeColor = Color.FromArgb(26, 92, 56);
            lblDeviceInfo.Location = new Point(14, 8);
            lblDeviceInfo.Name = "lblDeviceInfo";
            lblDeviceInfo.Size = new Size(1051, 30);
            lblDeviceInfo.TabIndex = 0;
            lblDeviceInfo.Text = "Đang tải...";
            lblDeviceInfo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // UcQuanLyCaThe
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1079, 600);
            Controls.Add(dgvInstances);
            Controls.Add(pnlTop);
            Controls.Add(pnlInfo);
            MinimumSize = new Size(700, 500);
            Name = "UcQuanLyCaThe";
            Text = "Quản lý mã cá thể";
            ((System.ComponentModel.ISupportInitialize)dgvInstances).EndInit();
            pnlTop.ResumeLayout(false);
            pnlTop.PerformLayout();
            pnlInfo.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.DataGridView dgvInstances;
        private System.Windows.Forms.TextBox txtAssetCode;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.TextBox txtCondition;
        private System.Windows.Forms.Button btnThem;
        private System.Windows.Forms.Button btnXoa;
        private System.Windows.Forms.Button btnCapNhat;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Panel pnlInfo;
        private System.Windows.Forms.Label lblDeviceInfo;
        private System.Windows.Forms.Label lblNextCode;
        private System.Windows.Forms.Label lblAssetCode;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblCondition;
    }
}



