namespace FrmProject.GUI
{
    partial class UcRecycleBin
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            DataGridViewCellStyle headerStyle = new DataGridViewCellStyle();
            DataGridViewCellStyle cellStyle = new DataGridViewCellStyle();
            pnlHeader = new Panel();
            lblTitle = new Label();
            pnlToolbar = new Panel();
            lblTong = new Label();
            btnXoaVinhVien = new Button();
            btnKhoiPhuc = new Button();
            btnLamMoi = new Button();
            txtTimKiem = new TextBox();
            cboLoai = new ComboBox();
            lblLoai = new Label();
            dgvRecycleBin = new DataGridView();
            pnlHeader.SuspendLayout();
            pnlToolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvRecycleBin).BeginInit();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(27, 94, 60);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1180, 68);
            pnlHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 16.2F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(22, 15);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(251, 38);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "🗑  Thùng Rác";
            // 
            // pnlToolbar
            // 
            pnlToolbar.BackColor = Color.White;
            pnlToolbar.Controls.Add(lblTong);
            pnlToolbar.Controls.Add(btnXoaVinhVien);
            pnlToolbar.Controls.Add(btnKhoiPhuc);
            pnlToolbar.Controls.Add(btnLamMoi);
            pnlToolbar.Controls.Add(txtTimKiem);
            pnlToolbar.Controls.Add(cboLoai);
            pnlToolbar.Controls.Add(lblLoai);
            pnlToolbar.Dock = DockStyle.Top;
            pnlToolbar.Location = new Point(0, 68);
            pnlToolbar.Name = "pnlToolbar";
            pnlToolbar.Padding = new Padding(16, 12, 16, 8);
            pnlToolbar.Size = new Size(1180, 72);
            pnlToolbar.TabIndex = 1;
            // 
            // lblTong
            // 
            lblTong.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblTong.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblTong.ForeColor = Color.FromArgb(55, 65, 81);
            lblTong.Location = new Point(1052, 24);
            lblTong.Name = "lblTong";
            lblTong.Size = new Size(100, 23);
            lblTong.TabIndex = 6;
            lblTong.Text = "Tổng: 0";
            lblTong.TextAlign = ContentAlignment.MiddleRight;
            // 
            // btnXoaVinhVien
            // 
            btnXoaVinhVien.BackColor = Color.FromArgb(220, 53, 69);
            btnXoaVinhVien.Cursor = Cursors.Hand;
            btnXoaVinhVien.FlatAppearance.BorderSize = 0;
            btnXoaVinhVien.FlatStyle = FlatStyle.Flat;
            btnXoaVinhVien.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnXoaVinhVien.ForeColor = Color.White;
            btnXoaVinhVien.Location = new Point(760, 16);
            btnXoaVinhVien.Name = "btnXoaVinhVien";
            btnXoaVinhVien.Size = new Size(156, 40);
            btnXoaVinhVien.TabIndex = 5;
            btnXoaVinhVien.Text = "Xóa vĩnh viễn";
            btnXoaVinhVien.UseVisualStyleBackColor = false;
            // 
            // btnKhoiPhuc
            // 
            btnKhoiPhuc.BackColor = Color.FromArgb(40, 167, 69);
            btnKhoiPhuc.Cursor = Cursors.Hand;
            btnKhoiPhuc.FlatAppearance.BorderSize = 0;
            btnKhoiPhuc.FlatStyle = FlatStyle.Flat;
            btnKhoiPhuc.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnKhoiPhuc.ForeColor = Color.White;
            btnKhoiPhuc.Location = new Point(632, 16);
            btnKhoiPhuc.Name = "btnKhoiPhuc";
            btnKhoiPhuc.Size = new Size(112, 40);
            btnKhoiPhuc.TabIndex = 4;
            btnKhoiPhuc.Text = "Khôi phục";
            btnKhoiPhuc.UseVisualStyleBackColor = false;
            // 
            // btnLamMoi
            // 
            btnLamMoi.BackColor = Color.FromArgb(108, 117, 125);
            btnLamMoi.Cursor = Cursors.Hand;
            btnLamMoi.FlatAppearance.BorderSize = 0;
            btnLamMoi.FlatStyle = FlatStyle.Flat;
            btnLamMoi.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnLamMoi.ForeColor = Color.White;
            btnLamMoi.Location = new Point(522, 16);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new Size(94, 40);
            btnLamMoi.TabIndex = 3;
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.UseVisualStyleBackColor = false;
            // 
            // txtTimKiem
            // 
            txtTimKiem.Font = new Font("Segoe UI", 10F);
            txtTimKiem.Location = new Point(269, 21);
            txtTimKiem.Name = "txtTimKiem";
            txtTimKiem.PlaceholderText = "Tìm theo mã, tên, ghi chú...";
            txtTimKiem.Size = new Size(236, 30);
            txtTimKiem.TabIndex = 2;
            // 
            // cboLoai
            // 
            cboLoai.DropDownStyle = ComboBoxStyle.DropDownList;
            cboLoai.Font = new Font("Segoe UI", 10F);
            cboLoai.FormattingEnabled = true;
            cboLoai.Items.AddRange(new object[] { "Tất cả", "Thiết bị", "Mã cá thể", "Phòng học", "Người dùng" });
            cboLoai.Location = new Point(72, 20);
            cboLoai.Name = "cboLoai";
            cboLoai.Size = new Size(174, 31);
            cboLoai.TabIndex = 1;
            // 
            // lblLoai
            // 
            lblLoai.AutoSize = true;
            lblLoai.Font = new Font("Segoe UI", 10F);
            lblLoai.Location = new Point(18, 24);
            lblLoai.Name = "lblLoai";
            lblLoai.Size = new Size(47, 23);
            lblLoai.TabIndex = 0;
            lblLoai.Text = "Loại:";
            // 
            // dgvRecycleBin
            // 
            dgvRecycleBin.AllowUserToAddRows = false;
            dgvRecycleBin.AllowUserToDeleteRows = false;
            dgvRecycleBin.BackgroundColor = Color.White;
            dgvRecycleBin.BorderStyle = BorderStyle.None;
            headerStyle.BackColor = Color.FromArgb(235, 240, 248);
            headerStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            headerStyle.ForeColor = Color.FromArgb(45, 55, 72);
            headerStyle.SelectionBackColor = Color.FromArgb(235, 240, 248);
            headerStyle.SelectionForeColor = Color.FromArgb(45, 55, 72);
            dgvRecycleBin.ColumnHeadersDefaultCellStyle = headerStyle;
            dgvRecycleBin.ColumnHeadersHeight = 34;
            dgvRecycleBin.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            cellStyle.Font = new Font("Segoe UI", 9.5F);
            cellStyle.SelectionBackColor = Color.FromArgb(225, 239, 252);
            cellStyle.SelectionForeColor = Color.Black;
            dgvRecycleBin.DefaultCellStyle = cellStyle;
            dgvRecycleBin.Dock = DockStyle.Fill;
            dgvRecycleBin.EnableHeadersVisualStyles = false;
            dgvRecycleBin.Location = new Point(0, 140);
            dgvRecycleBin.MultiSelect = false;
            dgvRecycleBin.Name = "dgvRecycleBin";
            dgvRecycleBin.ReadOnly = true;
            dgvRecycleBin.RowHeadersVisible = false;
            dgvRecycleBin.RowHeadersWidth = 51;
            dgvRecycleBin.RowTemplate.Height = 30;
            dgvRecycleBin.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRecycleBin.Size = new Size(1180, 580);
            dgvRecycleBin.TabIndex = 2;
            // 
            // UcRecycleBin
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1180, 720);
            Controls.Add(dgvRecycleBin);
            Controls.Add(pnlToolbar);
            Controls.Add(pnlHeader);
            Name = "UcRecycleBin";
            Text = "Thùng Rác";
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlToolbar.ResumeLayout(false);
            pnlToolbar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvRecycleBin).EndInit();
            ResumeLayout(false);
        }

        private Panel pnlHeader;
        private Label lblTitle;
        private Panel pnlToolbar;
        private Label lblLoai;
        private ComboBox cboLoai;
        private TextBox txtTimKiem;
        private Button btnLamMoi;
        private Button btnKhoiPhuc;
        private Button btnXoaVinhVien;
        private Label lblTong;
        private DataGridView dgvRecycleBin;
    }
}


