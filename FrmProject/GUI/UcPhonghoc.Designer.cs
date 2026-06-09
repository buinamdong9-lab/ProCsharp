namespace FrmProject.GUI
{
    partial class UcPhonghoc
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
            pnlHeader = new Panel();
            lblQuanLyPhongHoc = new Label();
            pnlInputGhiChu = new Panel();
            cmbTang = new ComboBox();
            cmbToaNha = new ComboBox();
            btnButton = new Button();
            btnButton2 = new Button();
            cmbTrangThai = new ComboBox();
            cmbLoaiPhong = new ComboBox();
            lblGhiChu = new Label();
            lblSucChua = new Label();
            lblTang = new Label();
            lblToaNha = new Label();
            lblTrangThai = new Label();
            lblLoaiPhong = new Label();
            lblTenPhong = new Label();
            lblMaPhong = new Label();
            txtGhiChu = new TextBox();
            txtSucChua = new TextBox();
            txtTenPhong = new TextBox();
            txtMaPhong = new TextBox();
            pnlActions = new Panel();
            btnLamMoi = new Button();
            btnLuu = new Button();
            btnXoa = new Button();
            btnSua = new Button();
            btnThem = new Button();
            pnlGridPhongHoc = new Panel();
            dgvPhongHoc = new DataGridView();
            pnlFilter = new Panel();
            btnLoc = new Button();
            cmbQuanLyPhongHoc = new ComboBox();
            cmbQuanLyPhongHoc2 = new ComboBox();
            txtDanhSachPhongHoc = new TextBox();
            lblDanhSachPhongHoc = new Label();
            pnlGridThietBiPhong = new Panel();
            dgvThietBiPhong = new DataGridView();
            lblThietBiTrongPhongTitle = new Label();
            pnlHeader.SuspendLayout();
            pnlInputGhiChu.SuspendLayout();
            pnlActions.SuspendLayout();
            pnlGridPhongHoc.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPhongHoc).BeginInit();
            pnlFilter.SuspendLayout();
            pnlGridThietBiPhong.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvThietBiPhong).BeginInit();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(27, 94, 60);
            pnlHeader.Controls.Add(lblQuanLyPhongHoc);
            pnlHeader.Location = new Point(-1, 0);
            pnlHeader.Margin = new Padding(3, 2, 3, 2);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1202, 58);
            pnlHeader.TabIndex = 0;
            // 
            // lblQuanLyPhongHoc
            // 
            lblQuanLyPhongHoc.AutoSize = true;
            lblQuanLyPhongHoc.Font = new Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblQuanLyPhongHoc.ForeColor = Color.White;
            lblQuanLyPhongHoc.Location = new Point(35, 15);
            lblQuanLyPhongHoc.Name = "lblQuanLyPhongHoc";
            lblQuanLyPhongHoc.Size = new Size(251, 30);
            lblQuanLyPhongHoc.TabIndex = 0;
            lblQuanLyPhongHoc.Text = "🏫  Quản lý phòng học";
            // 
            // pnlInputGhiChu
            // 
            pnlInputGhiChu.BackColor = Color.White;
            pnlInputGhiChu.Controls.Add(cmbTang);
            pnlInputGhiChu.Controls.Add(cmbToaNha);
            pnlInputGhiChu.Controls.Add(btnButton);
            pnlInputGhiChu.Controls.Add(btnButton2);
            pnlInputGhiChu.Controls.Add(cmbTrangThai);
            pnlInputGhiChu.Controls.Add(cmbLoaiPhong);
            pnlInputGhiChu.Controls.Add(lblGhiChu);
            pnlInputGhiChu.Controls.Add(lblSucChua);
            pnlInputGhiChu.Controls.Add(lblTang);
            pnlInputGhiChu.Controls.Add(lblToaNha);
            pnlInputGhiChu.Controls.Add(lblTrangThai);
            pnlInputGhiChu.Controls.Add(lblLoaiPhong);
            pnlInputGhiChu.Controls.Add(lblTenPhong);
            pnlInputGhiChu.Controls.Add(lblMaPhong);
            pnlInputGhiChu.Controls.Add(txtGhiChu);
            pnlInputGhiChu.Controls.Add(txtSucChua);
            pnlInputGhiChu.Controls.Add(txtTenPhong);
            pnlInputGhiChu.Controls.Add(txtMaPhong);
            pnlInputGhiChu.Location = new Point(-1, 63);
            pnlInputGhiChu.Margin = new Padding(3, 2, 3, 2);
            pnlInputGhiChu.Name = "pnlInputGhiChu";
            pnlInputGhiChu.Size = new Size(1202, 194);
            pnlInputGhiChu.TabIndex = 1;
            // 
            // cmbTang
            // 
            cmbTang.FormattingEnabled = true;
            cmbTang.Location = new Point(724, 66);
            cmbTang.Margin = new Padding(3, 2, 3, 2);
            cmbTang.Name = "cmbTang";
            cmbTang.Size = new Size(260, 23);
            cmbTang.TabIndex = 22;
            // 
            // cmbToaNha
            // 
            cmbToaNha.FormattingEnabled = true;
            cmbToaNha.Location = new Point(724, 29);
            cmbToaNha.Margin = new Padding(3, 2, 3, 2);
            cmbToaNha.Name = "cmbToaNha";
            cmbToaNha.Size = new Size(260, 23);
            cmbToaNha.TabIndex = 21;
            // 
            // btnButton
            // 
            btnButton.Cursor = Cursors.Hand;
            btnButton.FlatAppearance.BorderSize = 0;
            btnButton.FlatStyle = FlatStyle.Flat;
            btnButton.Location = new Point(810, 95);
            btnButton.Margin = new Padding(3, 2, 3, 2);
            btnButton.Name = "btnButton";
            btnButton.Size = new Size(28, 22);
            btnButton.TabIndex = 20;
            btnButton.Text = "+";
            btnButton.UseVisualStyleBackColor = true;
            // 
            // btnButton2
            // 
            btnButton2.Cursor = Cursors.Hand;
            btnButton2.FlatAppearance.BorderSize = 0;
            btnButton2.FlatStyle = FlatStyle.Flat;
            btnButton2.Location = new Point(746, 96);
            btnButton2.Margin = new Padding(3, 2, 3, 2);
            btnButton2.Name = "btnButton2";
            btnButton2.Size = new Size(28, 22);
            btnButton2.TabIndex = 19;
            btnButton2.Text = "-";
            btnButton2.UseVisualStyleBackColor = true;
            // 
            // cmbTrangThai
            // 
            cmbTrangThai.FormattingEnabled = true;
            cmbTrangThai.Items.AddRange(new object[] { "Hoạt động", "Đang sửa chữa", "Bảo trì", "Ngừng sử dụng" });
            cmbTrangThai.Location = new Point(193, 138);
            cmbTrangThai.Margin = new Padding(3, 2, 3, 2);
            cmbTrangThai.Name = "cmbTrangThai";
            cmbTrangThai.Size = new Size(226, 23);
            cmbTrangThai.TabIndex = 17;
            // 
            // cmbLoaiPhong
            // 
            cmbLoaiPhong.FormattingEnabled = true;
            cmbLoaiPhong.Items.AddRange(new object[] { "Phòng thực hành", "Phòng lý thuyết", "Lab", "Hội trường" });
            cmbLoaiPhong.Location = new Point(193, 101);
            cmbLoaiPhong.Margin = new Padding(3, 2, 3, 2);
            cmbLoaiPhong.Name = "cmbLoaiPhong";
            cmbLoaiPhong.Size = new Size(226, 23);
            cmbLoaiPhong.TabIndex = 16;
            // 
            // lblGhiChu
            // 
            lblGhiChu.AutoSize = true;
            lblGhiChu.Location = new Point(618, 146);
            lblGhiChu.Name = "lblGhiChu";
            lblGhiChu.Size = new Size(51, 15);
            lblGhiChu.TabIndex = 15;
            lblGhiChu.Text = "Ghi chú:";
            // 
            // lblSucChua
            // 
            lblSucChua.AutoSize = true;
            lblSucChua.Location = new Point(618, 101);
            lblSucChua.Name = "lblSucChua";
            lblSucChua.Size = new Size(58, 15);
            lblSucChua.TabIndex = 14;
            lblSucChua.Text = "Sức chứa:";
            // 
            // lblTang
            // 
            lblTang.AutoSize = true;
            lblTang.Location = new Point(618, 63);
            lblTang.Name = "lblTang";
            lblTang.Size = new Size(37, 15);
            lblTang.TabIndex = 13;
            lblTang.Text = "Tầng:";
            // 
            // lblToaNha
            // 
            lblToaNha.AutoSize = true;
            lblToaNha.Location = new Point(618, 32);
            lblToaNha.Name = "lblToaNha";
            lblToaNha.Size = new Size(52, 15);
            lblToaNha.TabIndex = 12;
            lblToaNha.Text = "Tòa nhà:";
            // 
            // lblTrangThai
            // 
            lblTrangThai.AutoSize = true;
            lblTrangThai.Location = new Point(59, 144);
            lblTrangThai.Name = "lblTrangThai";
            lblTrangThai.Size = new Size(63, 15);
            lblTrangThai.TabIndex = 11;
            lblTrangThai.Text = "Trạng thái:";
            // 
            // lblLoaiPhong
            // 
            lblLoaiPhong.AutoSize = true;
            lblLoaiPhong.Location = new Point(55, 101);
            lblLoaiPhong.Name = "lblLoaiPhong";
            lblLoaiPhong.Size = new Size(70, 15);
            lblLoaiPhong.TabIndex = 10;
            lblLoaiPhong.Text = "Loại phòng:";
            // 
            // lblTenPhong
            // 
            lblTenPhong.AutoSize = true;
            lblTenPhong.Location = new Point(55, 63);
            lblTenPhong.Name = "lblTenPhong";
            lblTenPhong.Size = new Size(67, 15);
            lblTenPhong.TabIndex = 9;
            lblTenPhong.Text = "Tên phòng:";
            // 
            // lblMaPhong
            // 
            lblMaPhong.AutoSize = true;
            lblMaPhong.Location = new Point(55, 26);
            lblMaPhong.Name = "lblMaPhong";
            lblMaPhong.Size = new Size(65, 15);
            lblMaPhong.TabIndex = 8;
            lblMaPhong.Text = "Mã phòng:";
            // 
            // txtGhiChu
            // 
            txtGhiChu.Location = new Point(724, 144);
            txtGhiChu.Margin = new Padding(3, 2, 3, 2);
            txtGhiChu.Name = "txtGhiChu";
            txtGhiChu.Size = new Size(260, 23);
            txtGhiChu.TabIndex = 6;
            // 
            // txtSucChua
            // 
            txtSucChua.Location = new Point(780, 97);
            txtSucChua.Margin = new Padding(3, 2, 3, 2);
            txtSucChua.Name = "txtSucChua";
            txtSucChua.Size = new Size(26, 23);
            txtSucChua.TabIndex = 5;
            // 
            // txtTenPhong
            // 
            txtTenPhong.Location = new Point(193, 61);
            txtTenPhong.Margin = new Padding(3, 2, 3, 2);
            txtTenPhong.Name = "txtTenPhong";
            txtTenPhong.Size = new Size(224, 23);
            txtTenPhong.TabIndex = 1;
            // 
            // txtMaPhong
            // 
            txtMaPhong.Location = new Point(193, 20);
            txtMaPhong.Margin = new Padding(3, 2, 3, 2);
            txtMaPhong.Name = "txtMaPhong";
            txtMaPhong.Size = new Size(224, 23);
            txtMaPhong.TabIndex = 0;
            // 
            // pnlActions
            // 
            pnlActions.BackColor = Color.White;
            pnlActions.Controls.Add(btnLamMoi);
            pnlActions.Controls.Add(btnLuu);
            pnlActions.Controls.Add(btnXoa);
            pnlActions.Controls.Add(btnSua);
            pnlActions.Controls.Add(btnThem);
            pnlActions.Location = new Point(-1, 262);
            pnlActions.Margin = new Padding(3, 2, 3, 2);
            pnlActions.Name = "pnlActions";
            pnlActions.Size = new Size(1202, 54);
            pnlActions.TabIndex = 2;
            // 
            // btnLamMoi
            // 
            btnLamMoi.BackColor = Color.FromArgb(108, 117, 125);
            btnLamMoi.Cursor = Cursors.Hand;
            btnLamMoi.FlatAppearance.BorderSize = 0;
            btnLamMoi.FlatStyle = FlatStyle.Flat;
            btnLamMoi.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            btnLamMoi.ForeColor = Color.White;
            btnLamMoi.Location = new Point(536, 10);
            btnLamMoi.Margin = new Padding(3, 2, 3, 2);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new Size(106, 35);
            btnLamMoi.TabIndex = 10;
            btnLamMoi.Text = "↺ Làm mới";
            btnLamMoi.UseVisualStyleBackColor = false;
            // 
            // btnLuu
            // 
            btnLuu.BackColor = Color.FromArgb(0, 123, 255);
            btnLuu.Cursor = Cursors.Hand;
            btnLuu.FlatAppearance.BorderSize = 0;
            btnLuu.FlatStyle = FlatStyle.Flat;
            btnLuu.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            btnLuu.ForeColor = Color.White;
            btnLuu.Location = new Point(414, 10);
            btnLuu.Margin = new Padding(3, 2, 3, 2);
            btnLuu.Name = "btnLuu";
            btnLuu.Size = new Size(82, 35);
            btnLuu.TabIndex = 9;
            btnLuu.Text = "💾 Lưu";
            btnLuu.UseVisualStyleBackColor = false;
            // 
            // btnXoa
            // 
            btnXoa.BackColor = Color.FromArgb(220, 53, 69);
            btnXoa.Cursor = Cursors.Hand;
            btnXoa.FlatAppearance.BorderSize = 0;
            btnXoa.FlatStyle = FlatStyle.Flat;
            btnXoa.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            btnXoa.ForeColor = Color.White;
            btnXoa.Location = new Point(292, 10);
            btnXoa.Margin = new Padding(3, 2, 3, 2);
            btnXoa.Name = "btnXoa";
            btnXoa.Size = new Size(76, 35);
            btnXoa.TabIndex = 8;
            btnXoa.Text = "🗑 Xóa";
            btnXoa.UseVisualStyleBackColor = false;
            // 
            // btnSua
            // 
            btnSua.BackColor = Color.FromArgb(253, 126, 20);
            btnSua.Cursor = Cursors.Hand;
            btnSua.FlatAppearance.BorderSize = 0;
            btnSua.FlatStyle = FlatStyle.Flat;
            btnSua.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            btnSua.ForeColor = Color.White;
            btnSua.Location = new Point(169, 10);
            btnSua.Margin = new Padding(3, 2, 3, 2);
            btnSua.Name = "btnSua";
            btnSua.Size = new Size(82, 35);
            btnSua.TabIndex = 7;
            btnSua.Text = "✏ Sửa";
            btnSua.UseVisualStyleBackColor = false;
            // 
            // btnThem
            // 
            btnThem.BackColor = Color.FromArgb(40, 167, 69);
            btnThem.Cursor = Cursors.Hand;
            btnThem.FlatAppearance.BorderSize = 0;
            btnThem.FlatStyle = FlatStyle.Flat;
            btnThem.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            btnThem.ForeColor = Color.White;
            btnThem.Location = new Point(35, 10);
            btnThem.Margin = new Padding(3, 2, 3, 2);
            btnThem.Name = "btnThem";
            btnThem.Size = new Size(100, 35);
            btnThem.TabIndex = 6;
            btnThem.Text = "➕ Thêm";
            btnThem.UseVisualStyleBackColor = false;
            // 
            // pnlGridPhongHoc
            // 
            pnlGridPhongHoc.BackColor = Color.White;
            pnlGridPhongHoc.Controls.Add(dgvPhongHoc);
            pnlGridPhongHoc.Controls.Add(pnlFilter);
            pnlGridPhongHoc.Controls.Add(lblDanhSachPhongHoc);
            pnlGridPhongHoc.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            pnlGridPhongHoc.Location = new Point(-1, 320);
            pnlGridPhongHoc.Margin = new Padding(3, 2, 3, 2);
            pnlGridPhongHoc.Name = "pnlGridPhongHoc";
            pnlGridPhongHoc.Size = new Size(743, 414);
            pnlGridPhongHoc.TabIndex = 3;
            // 
            // dgvPhongHoc
            // 
            dgvPhongHoc.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPhongHoc.Location = new Point(4, 77);
            dgvPhongHoc.Margin = new Padding(3, 2, 3, 2);
            dgvPhongHoc.Name = "dgvPhongHoc";
            dgvPhongHoc.RowHeadersWidth = 51;
            dgvPhongHoc.Size = new Size(736, 335);
            dgvPhongHoc.TabIndex = 2;
            dgvPhongHoc.CellClick += dgvPhongHoc_CellClick;
            // 
            // pnlFilter
            // 
            pnlFilter.BackColor = Color.White;
            pnlFilter.Controls.Add(btnLoc);
            pnlFilter.Controls.Add(cmbQuanLyPhongHoc);
            pnlFilter.Controls.Add(cmbQuanLyPhongHoc2);
            pnlFilter.Controls.Add(txtDanhSachPhongHoc);
            pnlFilter.Location = new Point(4, 38);
            pnlFilter.Margin = new Padding(3, 2, 3, 2);
            pnlFilter.Name = "pnlFilter";
            pnlFilter.Size = new Size(736, 35);
            pnlFilter.TabIndex = 1;
            // 
            // btnLoc
            // 
            btnLoc.BackColor = Color.FromArgb(40, 167, 69);
            btnLoc.Cursor = Cursors.Hand;
            btnLoc.FlatAppearance.BorderSize = 0;
            btnLoc.FlatStyle = FlatStyle.Flat;
            btnLoc.ForeColor = Color.White;
            btnLoc.Location = new Point(674, 7);
            btnLoc.Margin = new Padding(3, 2, 3, 2);
            btnLoc.Name = "btnLoc";
            btnLoc.Size = new Size(59, 23);
            btnLoc.TabIndex = 4;
            btnLoc.Text = "⚡ Lọc";
            btnLoc.UseVisualStyleBackColor = false;
            // 
            // cmbQuanLyPhongHoc
            // 
            cmbQuanLyPhongHoc.FormattingEnabled = true;
            cmbQuanLyPhongHoc.Location = new Point(491, 7);
            cmbQuanLyPhongHoc.Margin = new Padding(3, 2, 3, 2);
            cmbQuanLyPhongHoc.Name = "cmbQuanLyPhongHoc";
            cmbQuanLyPhongHoc.Size = new Size(177, 23);
            cmbQuanLyPhongHoc.TabIndex = 2;
            // 
            // cmbQuanLyPhongHoc2
            // 
            cmbQuanLyPhongHoc2.FormattingEnabled = true;
            cmbQuanLyPhongHoc2.Location = new Point(277, 8);
            cmbQuanLyPhongHoc2.Margin = new Padding(3, 2, 3, 2);
            cmbQuanLyPhongHoc2.Name = "cmbQuanLyPhongHoc2";
            cmbQuanLyPhongHoc2.Size = new Size(208, 23);
            cmbQuanLyPhongHoc2.TabIndex = 1;
            // 
            // txtDanhSachPhongHoc
            // 
            txtDanhSachPhongHoc.Location = new Point(9, 7);
            txtDanhSachPhongHoc.Margin = new Padding(3, 2, 3, 2);
            txtDanhSachPhongHoc.Name = "txtDanhSachPhongHoc";
            txtDanhSachPhongHoc.PlaceholderText = "🔍 Tìm kiếm...";
            txtDanhSachPhongHoc.Size = new Size(263, 23);
            txtDanhSachPhongHoc.TabIndex = 0;
            // 
            // lblDanhSachPhongHoc
            // 
            lblDanhSachPhongHoc.AutoSize = true;
            lblDanhSachPhongHoc.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDanhSachPhongHoc.ForeColor = Color.FromArgb(27, 94, 60);
            lblDanhSachPhongHoc.Location = new Point(0, 8);
            lblDanhSachPhongHoc.Name = "lblDanhSachPhongHoc";
            lblDanhSachPhongHoc.Size = new Size(189, 20);
            lblDanhSachPhongHoc.TabIndex = 0;
            lblDanhSachPhongHoc.Text = "📋  Danh sách phòng học";
            // 
            // pnlGridThietBiPhong
            // 
            pnlGridThietBiPhong.BackColor = Color.White;
            pnlGridThietBiPhong.Controls.Add(dgvThietBiPhong);
            pnlGridThietBiPhong.Controls.Add(lblThietBiTrongPhongTitle);
            pnlGridThietBiPhong.Location = new Point(745, 320);
            pnlGridThietBiPhong.Margin = new Padding(3, 2, 3, 2);
            pnlGridThietBiPhong.Name = "pnlGridThietBiPhong";
            pnlGridThietBiPhong.Size = new Size(456, 414);
            pnlGridThietBiPhong.TabIndex = 4;
            // 
            // dgvThietBiPhong
            // 
            dgvThietBiPhong.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvThietBiPhong.Location = new Point(3, 38);
            dgvThietBiPhong.Margin = new Padding(3, 2, 3, 2);
            dgvThietBiPhong.Name = "dgvThietBiPhong";
            dgvThietBiPhong.RowHeadersWidth = 51;
            dgvThietBiPhong.Size = new Size(450, 374);
            dgvThietBiPhong.TabIndex = 2;
            // 
            // lblThietBiTrongPhongTitle
            // 
            lblThietBiTrongPhongTitle.AutoSize = true;
            lblThietBiTrongPhongTitle.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblThietBiTrongPhongTitle.ForeColor = Color.FromArgb(27, 94, 60);
            lblThietBiTrongPhongTitle.Location = new Point(30, 8);
            lblThietBiTrongPhongTitle.Name = "lblThietBiTrongPhongTitle";
            lblThietBiTrongPhongTitle.Size = new Size(184, 20);
            lblThietBiTrongPhongTitle.TabIndex = 1;
            lblThietBiTrongPhongTitle.Text = "🛠️  Thiết bị trong phòng";
            // 
            // UcPhonghoc
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            AutoScrollMinSize = new Size(0, 1020);
            BackColor = Color.White;
            ClientSize = new Size(1288, 750);
            Controls.Add(pnlGridThietBiPhong);
            Controls.Add(pnlGridPhongHoc);
            Controls.Add(pnlActions);
            Controls.Add(pnlInputGhiChu);
            Controls.Add(pnlHeader);
            Margin = new Padding(3, 2, 3, 2);
            Name = "UcPhonghoc";
            Text = "Quản lý phòng học";
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlInputGhiChu.ResumeLayout(false);
            pnlInputGhiChu.PerformLayout();
            pnlActions.ResumeLayout(false);
            pnlGridPhongHoc.ResumeLayout(false);
            pnlGridPhongHoc.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPhongHoc).EndInit();
            pnlFilter.ResumeLayout(false);
            pnlFilter.PerformLayout();
            pnlGridThietBiPhong.ResumeLayout(false);
            pnlGridThietBiPhong.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvThietBiPhong).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlHeader;
        private Label lblQuanLyPhongHoc;
        private Panel pnlInputGhiChu;
        private TextBox txtTenPhong;
        private TextBox txtMaPhong;
        private TextBox txtSucChua;
        private TextBox txtGhiChu;
        private Label lblGhiChu;
        private Label lblSucChua;
        private Label lblTang;
        private Label lblToaNha;
        private Label lblTrangThai;
        private Label lblLoaiPhong;
        private Label lblTenPhong;
        private Label lblMaPhong;
        private Button btnButton;
        private Button btnButton2;
        private ComboBox cmbTrangThai;
        private ComboBox cmbLoaiPhong;
        private ComboBox cmbTang;
        private ComboBox cmbToaNha;
        private Panel pnlActions;
        private Button btnThem;
        private Button btnSua;
        private Button btnXoa;
        private Button btnLuu;
        private Button btnLamMoi;
        private Panel pnlGridPhongHoc;
        private Panel pnlGridThietBiPhong;
        private Panel pnlFilter;
        private Label lblDanhSachPhongHoc;
        private ComboBox cmbQuanLyPhongHoc;
        private ComboBox cmbQuanLyPhongHoc2;
        private TextBox txtDanhSachPhongHoc;
        private Button btnLoc;
        private DataGridView dgvPhongHoc;
        private DataGridView dgvThietBiPhong;
        private Label lblThietBiTrongPhongTitle;
    }
}






