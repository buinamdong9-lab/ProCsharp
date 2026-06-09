namespace FrmProject
{
    partial class UcNguoidung
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            panelHeader = new Panel();
            lblTitle = new Label();
            grpThongTin = new GroupBox();
            cmbTrangThai = new ComboBox();
            pnlActions = new Panel();
            btnThem = new Button();
            btnSua = new Button();
            btnXoa = new Button();
            btnLuu = new Button();
            btnDatLaiMk = new Button();
            btnLamMoi = new Button();
            btnXuatExcel = new Button();
            this.txtMatKhau = new TextBox();
            this.txtSoDienThoai = new TextBox();
            cmbVaiTro = new ComboBox();
            this.txtTenDangNhap = new TextBox();
            this.lblMatKhau = new Label();
            this.lblTenDangNhap = new Label();
            this.lblTrangThai = new Label();
            this.lblSoDienThoai = new Label();
            this.lblVaiTro = new Label();
            lblKhoaBoMon = new Label();
            txtKhoaBoMon = new TextBox();
            txtEmail = new TextBox();
            txtHoTen = new TextBox();
            txtMaNguoiDung = new TextBox();
            lblEmail = new Label();
            lblHoTen = new Label();
            lblMaNguoiDung = new Label();
            dgvNguoiDung = new DataGridView();
            pnlFilter = new Panel();
            textBox8 = new TextBox();
            comboBox3 = new ComboBox();
            comboBox4 = new ComboBox();
            btnLoc = new Button();
            pnlPaging = new Panel();
            btnFirst = new Button();
            btnPrev = new Button();
            lblPageInfo = new Label();
            btnNext = new Button();
            btnLast = new Button();
            panelHeader.SuspendLayout();
            grpThongTin.SuspendLayout();
            pnlActions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvNguoiDung).BeginInit();
            pnlFilter.SuspendLayout();
            pnlPaging.SuspendLayout();
            SuspendLayout();
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.FromArgb(30, 58, 138);
            panelHeader.Controls.Add(lblTitle);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(1257, 70);
            panelHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(25, 18);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(310, 35);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "👤  Quản Lý Người Dùng";
            // 
            // grpThongTin
            // 
            grpThongTin.Controls.Add(cmbTrangThai);
            grpThongTin.Controls.Add(pnlActions);
            grpThongTin.Controls.Add(this.txtMatKhau);
            grpThongTin.Controls.Add(this.txtSoDienThoai);
            grpThongTin.Controls.Add(cmbVaiTro);
            grpThongTin.Controls.Add(this.txtTenDangNhap);
            grpThongTin.Controls.Add(this.lblMatKhau);
            grpThongTin.Controls.Add(this.lblTenDangNhap);
            grpThongTin.Controls.Add(this.lblTrangThai);
            grpThongTin.Controls.Add(this.lblSoDienThoai);
            grpThongTin.Controls.Add(this.lblVaiTro);
            grpThongTin.Controls.Add(lblKhoaBoMon);
            grpThongTin.Controls.Add(txtKhoaBoMon);
            grpThongTin.Controls.Add(txtEmail);
            grpThongTin.Controls.Add(txtHoTen);
            grpThongTin.Controls.Add(txtMaNguoiDung);
            grpThongTin.Controls.Add(lblEmail);
            grpThongTin.Controls.Add(lblHoTen);
            grpThongTin.Controls.Add(lblMaNguoiDung);
            grpThongTin.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            grpThongTin.Location = new Point(0, 70);
            grpThongTin.Name = "grpThongTin";
            grpThongTin.Size = new Size(1257, 320);
            grpThongTin.TabIndex = 1;
            grpThongTin.TabStop = false;
            grpThongTin.Text = "🖹  Thông tin người dùng";
            // 
            // cmbTrangThai
            // 
            cmbTrangThai.Font = new Font("Segoe UI", 9.5F);
            cmbTrangThai.FormattingEnabled = true;
            cmbTrangThai.Items.AddRange(new object[] { "Hoạt động", "Ngừng hoạt động", "Bị khóa" });
            cmbTrangThai.Location = new Point(703, 136);
            cmbTrangThai.Name = "cmbTrangThai";
            cmbTrangThai.Size = new Size(403, 29);
            cmbTrangThai.TabIndex = 22;
            cmbTrangThai.Text = "Hoạt động";
            // 
            // pnlActions
            // 
            pnlActions.Controls.Add(btnThem);
            pnlActions.Controls.Add(btnSua);
            pnlActions.Controls.Add(btnXoa);
            pnlActions.Controls.Add(btnLuu);
            pnlActions.Controls.Add(btnDatLaiMk);
            pnlActions.Controls.Add(btnLamMoi);
            pnlActions.Controls.Add(btnXuatExcel);
            pnlActions.Location = new Point(29, 261);
            pnlActions.Name = "pnlActions";
            pnlActions.Size = new Size(1216, 47);
            pnlActions.TabIndex = 21;
            // 
            // btnThem
            // 
            btnThem.BackColor = Color.FromArgb(22, 163, 74);
            btnThem.Cursor = Cursors.Hand;
            btnThem.FlatAppearance.BorderSize = 0;
            btnThem.FlatStyle = FlatStyle.Flat;
            btnThem.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnThem.ForeColor = Color.White;
            btnThem.Location = new Point(8, 1);
            btnThem.Name = "btnThem";
            btnThem.Size = new Size(105, 44);
            btnThem.TabIndex = 0;
            btnThem.Text = "➕ Thêm";
            btnThem.UseVisualStyleBackColor = false;
            // 
            // btnSua
            // 
            btnSua.BackColor = Color.FromArgb(234, 88, 12);
            btnSua.Cursor = Cursors.Hand;
            btnSua.FlatAppearance.BorderSize = 0;
            btnSua.FlatStyle = FlatStyle.Flat;
            btnSua.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnSua.ForeColor = Color.White;
            btnSua.Location = new Point(118, 1);
            btnSua.Name = "btnSua";
            btnSua.Size = new Size(105, 44);
            btnSua.TabIndex = 1;
            btnSua.Text = "✏ Sửa";
            btnSua.UseVisualStyleBackColor = false;
            // 
            // btnXoa
            // 
            btnXoa.BackColor = Color.FromArgb(220, 38, 38);
            btnXoa.Cursor = Cursors.Hand;
            btnXoa.FlatAppearance.BorderSize = 0;
            btnXoa.FlatStyle = FlatStyle.Flat;
            btnXoa.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnXoa.ForeColor = Color.White;
            btnXoa.Location = new Point(228, 1);
            btnXoa.Name = "btnXoa";
            btnXoa.Size = new Size(105, 44);
            btnXoa.TabIndex = 2;
            btnXoa.Text = "🗑 Xóa";
            btnXoa.UseVisualStyleBackColor = false;
            // 
            // btnLuu
            // 
            btnLuu.BackColor = Color.FromArgb(37, 99, 235);
            btnLuu.Cursor = Cursors.Hand;
            btnLuu.FlatAppearance.BorderSize = 0;
            btnLuu.FlatStyle = FlatStyle.Flat;
            btnLuu.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnLuu.ForeColor = Color.White;
            btnLuu.Location = new Point(338, 1);
            btnLuu.Name = "btnLuu";
            btnLuu.Size = new Size(105, 44);
            btnLuu.TabIndex = 3;
            btnLuu.Text = "💾 Lưu";
            btnLuu.UseVisualStyleBackColor = false;
            // 
            // btnDatLaiMk
            // 
            btnDatLaiMk.BackColor = Color.FromArgb(109, 40, 217);
            btnDatLaiMk.Cursor = Cursors.Hand;
            btnDatLaiMk.FlatAppearance.BorderSize = 0;
            btnDatLaiMk.FlatStyle = FlatStyle.Flat;
            btnDatLaiMk.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnDatLaiMk.ForeColor = Color.White;
            btnDatLaiMk.Location = new Point(448, 1);
            btnDatLaiMk.Name = "btnDatLaiMk";
            btnDatLaiMk.Size = new Size(140, 44);
            btnDatLaiMk.TabIndex = 4;
            btnDatLaiMk.Text = "🔑 Đặt lại MK";
            btnDatLaiMk.UseVisualStyleBackColor = false;
            // 
            // btnLamMoi
            // 
            btnLamMoi.BackColor = Color.FromArgb(107, 114, 128);
            btnLamMoi.Cursor = Cursors.Hand;
            btnLamMoi.FlatAppearance.BorderSize = 0;
            btnLamMoi.FlatStyle = FlatStyle.Flat;
            btnLamMoi.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnLamMoi.ForeColor = Color.White;
            btnLamMoi.Location = new Point(596, 1);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new Size(105, 44);
            btnLamMoi.TabIndex = 5;
            btnLamMoi.Text = "↺ Làm mới";
            btnLamMoi.UseVisualStyleBackColor = false;
            // 
            // btnXuatExcel
            // 
            btnXuatExcel.BackColor = Color.FromArgb(14, 165, 233);
            btnXuatExcel.Cursor = Cursors.Hand;
            btnXuatExcel.FlatAppearance.BorderSize = 0;
            btnXuatExcel.FlatStyle = FlatStyle.Flat;
            btnXuatExcel.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnXuatExcel.ForeColor = Color.White;
            btnXuatExcel.Location = new Point(706, 1);
            btnXuatExcel.Name = "btnXuatExcel";
            btnXuatExcel.Size = new Size(140, 44);
            btnXuatExcel.TabIndex = 6;
            btnXuatExcel.Text = "🖨 Xuất Excel";
            btnXuatExcel.UseVisualStyleBackColor = false;
            // 
            // txtMatKhau
            // 
            this.txtMatKhau.Location = new Point(703, 178);
            this.txtMatKhau.Name = "txtMatKhau";
            this.txtMatKhau.Size = new Size(403, 30);
            this.txtMatKhau.TabIndex = 20;
            this.txtMatKhau.UseSystemPasswordChar = true;
            // 
            // txtSoDienThoai
            // 
            this.txtSoDienThoai.Location = new Point(703, 84);
            this.txtSoDienThoai.MaxLength = 10;
            this.txtSoDienThoai.Name = "txtSoDienThoai";
            this.txtSoDienThoai.Size = new Size(403, 30);
            this.txtSoDienThoai.TabIndex = 18;
            // 
            // cmbVaiTro
            // 
            cmbVaiTro.Cursor = Cursors.Hand;
            cmbVaiTro.Font = new Font("Segoe UI", 9.5F);
            cmbVaiTro.FormattingEnabled = true;
            cmbVaiTro.Items.AddRange(new object[] { "Admin", "Thủ kho", "Giảng viên", "Sinh viên" });
            cmbVaiTro.Location = new Point(703, 37);
            cmbVaiTro.Name = "cmbVaiTro";
            cmbVaiTro.Size = new Size(403, 29);
            cmbVaiTro.TabIndex = 16;
            cmbVaiTro.Text = "Chọn vai trò";
            // 
            // txtTenDangNhap
            // 
            this.txtTenDangNhap.Location = new Point(150, 225);
            this.txtTenDangNhap.Name = "txtTenDangNhap";
            this.txtTenDangNhap.Size = new Size(355, 30);
            this.txtTenDangNhap.TabIndex = 15;
            // 
            // lblMatKhau
            // 
            this.lblMatKhau.AutoSize = true;
            this.lblMatKhau.Font = new Font("Segoe UI", 9.5F);
            this.lblMatKhau.ForeColor = Color.FromArgb(55, 65, 81);
            this.lblMatKhau.Location = new Point(598, 187);
            this.lblMatKhau.Name = "lblMatKhau";
            this.lblMatKhau.Size = new Size(78, 21);
            this.lblMatKhau.TabIndex = 14;
            this.lblMatKhau.Text = "Mật khẩu:";
            // 
            // lblTenDangNhap
            // 
            this.lblTenDangNhap.AutoSize = true;
            this.lblTenDangNhap.Font = new Font("Segoe UI", 9.5F);
            this.lblTenDangNhap.ForeColor = Color.FromArgb(55, 65, 81);
            this.lblTenDangNhap.Location = new Point(29, 228);
            this.lblTenDangNhap.Name = "lblTenDangNhap";
            this.lblTenDangNhap.Size = new Size(114, 21);
            this.lblTenDangNhap.TabIndex = 13;
            this.lblTenDangNhap.Text = "Tên đăng nhập:";
            // 
            // lblTrangThai
            // 
            this.lblTrangThai.AutoSize = true;
            this.lblTrangThai.Font = new Font("Segoe UI", 9.5F);
            this.lblTrangThai.ForeColor = Color.FromArgb(55, 65, 81);
            this.lblTrangThai.Location = new Point(598, 136);
            this.lblTrangThai.Name = "lblTrangThai";
            this.lblTrangThai.Size = new Size(82, 21);
            this.lblTrangThai.TabIndex = 12;
            this.lblTrangThai.Text = "Trạng thái:";
            // 
            // lblSoDienThoai
            // 
            this.lblSoDienThoai.AutoSize = true;
            this.lblSoDienThoai.Font = new Font("Segoe UI", 9.5F);
            this.lblSoDienThoai.ForeColor = Color.FromArgb(55, 65, 81);
            this.lblSoDienThoai.Location = new Point(598, 85);
            this.lblSoDienThoai.Name = "lblSoDienThoai";
            this.lblSoDienThoai.Size = new Size(104, 21);
            this.lblSoDienThoai.TabIndex = 11;
            this.lblSoDienThoai.Text = "Số điện thoại:";
            // 
            // lblVaiTro
            // 
            this.lblVaiTro.AutoSize = true;
            this.lblVaiTro.Font = new Font("Segoe UI", 9.5F);
            this.lblVaiTro.ForeColor = Color.FromArgb(55, 65, 81);
            this.lblVaiTro.Location = new Point(598, 42);
            this.lblVaiTro.Name = "lblVaiTro";
            this.lblVaiTro.Size = new Size(58, 21);
            this.lblVaiTro.TabIndex = 9;
            this.lblVaiTro.Text = "Vai trò:";
            // 
            // lblKhoaBoMon
            // 
            lblKhoaBoMon.AutoSize = true;
            lblKhoaBoMon.Font = new Font("Segoe UI", 9.5F);
            lblKhoaBoMon.ForeColor = Color.FromArgb(55, 65, 81);
            lblKhoaBoMon.Location = new Point(29, 182);
            lblKhoaBoMon.Name = "lblKhoaBoMon";
            lblKhoaBoMon.Size = new Size(116, 21);
            lblKhoaBoMon.TabIndex = 8;
            lblKhoaBoMon.Text = "Khoa / Bộ môn:";
            // 
            // txtKhoaBoMon
            // 
            txtKhoaBoMon.Location = new Point(150, 179);
            txtKhoaBoMon.Name = "txtKhoaBoMon";
            txtKhoaBoMon.Size = new Size(355, 30);
            txtKhoaBoMon.TabIndex = 7;
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(150, 132);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(355, 30);
            txtEmail.TabIndex = 6;
            // 
            // txtHoTen
            // 
            txtHoTen.Location = new Point(149, 85);
            txtHoTen.Name = "txtHoTen";
            txtHoTen.Size = new Size(356, 30);
            txtHoTen.TabIndex = 5;
            // 
            // txtMaNguoiDung
            // 
            txtMaNguoiDung.Location = new Point(150, 37);
            txtMaNguoiDung.Name = "txtMaNguoiDung";
            txtMaNguoiDung.Size = new Size(355, 30);
            txtMaNguoiDung.TabIndex = 3;
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.Font = new Font("Segoe UI", 9.5F);
            lblEmail.ForeColor = Color.FromArgb(55, 65, 81);
            lblEmail.Location = new Point(86, 135);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(51, 21);
            lblEmail.TabIndex = 2;
            lblEmail.Text = "Email:";
            // 
            // lblHoTen
            // 
            lblHoTen.AutoSize = true;
            lblHoTen.Font = new Font("Segoe UI", 9.5F);
            lblHoTen.ForeColor = Color.FromArgb(55, 65, 81);
            lblHoTen.Location = new Point(86, 88);
            lblHoTen.Name = "lblHoTen";
            lblHoTen.Size = new Size(59, 21);
            lblHoTen.TabIndex = 1;
            lblHoTen.Text = "Họ tên:";
            // 
            // lblMaNguoiDung
            // 
            lblMaNguoiDung.AutoSize = true;
            lblMaNguoiDung.Font = new Font("Segoe UI", 9.5F);
            lblMaNguoiDung.ForeColor = Color.FromArgb(55, 65, 81);
            lblMaNguoiDung.Location = new Point(29, 40);
            lblMaNguoiDung.Name = "lblMaNguoiDung";
            lblMaNguoiDung.Size = new Size(120, 21);
            lblMaNguoiDung.TabIndex = 0;
            lblMaNguoiDung.Text = "Mã người dùng:";
            // 
            // dgvNguoiDung
            // 
            dgvNguoiDung.AllowUserToAddRows = false;
            dgvNguoiDung.BackgroundColor = Color.White;
            dgvNguoiDung.BorderStyle = BorderStyle.None;
            dgvNguoiDung.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvNguoiDung.GridColor = Color.FromArgb(229, 231, 235);
            dgvNguoiDung.Location = new Point(12, 445);
            dgvNguoiDung.Name = "dgvNguoiDung";
            dgvNguoiDung.ReadOnly = true;
            dgvNguoiDung.RowHeadersVisible = false;
            dgvNguoiDung.RowHeadersWidth = 51;
            dgvNguoiDung.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvNguoiDung.Size = new Size(1233, 480);
            dgvNguoiDung.TabIndex = 2;
            // 
            // pnlFilter
            // 
            pnlFilter.BackColor = Color.FromArgb(249, 250, 251);
            pnlFilter.Controls.Add(textBox8);
            pnlFilter.Controls.Add(comboBox3);
            pnlFilter.Controls.Add(comboBox4);
            pnlFilter.Controls.Add(btnLoc);
            pnlFilter.Location = new Point(12, 395);
            pnlFilter.Name = "pnlFilter";
            pnlFilter.Size = new Size(1233, 45);
            pnlFilter.TabIndex = 3;
            // 
            // textBox8
            // 
            textBox8.Location = new Point(8, 8);
            textBox8.Name = "textBox8";
            textBox8.PlaceholderText = "🔍 Tìm kiếm...";
            textBox8.Size = new Size(230, 27);
            textBox8.TabIndex = 0;
            // 
            // comboBox3
            // 
            comboBox3.FormattingEnabled = true;
            comboBox3.Items.AddRange(new object[] { "Tất cả vai trò", "Admin", "Thủ kho", "Giảng viên", "Sinh viên" });
            comboBox3.Location = new Point(250, 8);
            comboBox3.Name = "comboBox3";
            comboBox3.Size = new Size(200, 28);
            comboBox3.TabIndex = 1;
            comboBox3.Text = "Tất cả vai trò";
            // 
            // comboBox4
            // 
            comboBox4.FormattingEnabled = true;
            comboBox4.Items.AddRange(new object[] { "Tất cả trạng thái", "Hoạt động", "Ngừng hoạt động", "Bị khóa" });
            comboBox4.Location = new Point(462, 8);
            comboBox4.Name = "comboBox4";
            comboBox4.Size = new Size(200, 28);
            comboBox4.TabIndex = 2;
            comboBox4.Text = "Tất cả trạng thái";
            // 
            // btnLoc
            // 
            btnLoc.BackColor = Color.FromArgb(22, 163, 74);
            btnLoc.Cursor = Cursors.Hand;
            btnLoc.FlatAppearance.BorderSize = 0;
            btnLoc.FlatStyle = FlatStyle.Flat;
            btnLoc.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnLoc.ForeColor = Color.White;
            btnLoc.Location = new Point(675, 7);
            btnLoc.Name = "btnLoc";
            btnLoc.Size = new Size(75, 30);
            btnLoc.TabIndex = 3;
            btnLoc.Text = "⚡ Lọc";
            btnLoc.UseVisualStyleBackColor = false;
            // 
            // pnlPaging
            // 
            pnlPaging.BackColor = Color.FromArgb(243, 244, 246);
            pnlPaging.Controls.Add(btnFirst);
            pnlPaging.Controls.Add(btnPrev);
            pnlPaging.Controls.Add(lblPageInfo);
            pnlPaging.Controls.Add(btnNext);
            pnlPaging.Controls.Add(btnLast);
            pnlPaging.Location = new Point(12, 932);
            pnlPaging.Name = "pnlPaging";
            pnlPaging.Size = new Size(1233, 44);
            pnlPaging.TabIndex = 4;
            // 
            // btnFirst
            // 
            btnFirst.BackColor = Color.FromArgb(37, 99, 235);
            btnFirst.Cursor = Cursors.Hand;
            btnFirst.FlatAppearance.BorderSize = 0;
            btnFirst.FlatStyle = FlatStyle.Flat;
            btnFirst.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnFirst.ForeColor = Color.White;
            btnFirst.Location = new Point(8, 6);
            btnFirst.Name = "btnFirst";
            btnFirst.Size = new Size(75, 32);
            btnFirst.TabIndex = 0;
            btnFirst.Text = "⏮ Đầu";
            btnFirst.UseVisualStyleBackColor = false;
            // 
            // btnPrev
            // 
            btnPrev.BackColor = Color.FromArgb(37, 99, 235);
            btnPrev.Cursor = Cursors.Hand;
            btnPrev.FlatAppearance.BorderSize = 0;
            btnPrev.FlatStyle = FlatStyle.Flat;
            btnPrev.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnPrev.ForeColor = Color.White;
            btnPrev.Location = new Point(90, 6);
            btnPrev.Name = "btnPrev";
            btnPrev.Size = new Size(75, 32);
            btnPrev.TabIndex = 1;
            btnPrev.Text = "◀ Trước";
            btnPrev.UseVisualStyleBackColor = false;
            // 
            // lblPageInfo
            // 
            lblPageInfo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblPageInfo.ForeColor = Color.FromArgb(30, 58, 138);
            lblPageInfo.Location = new Point(172, 10);
            lblPageInfo.Name = "lblPageInfo";
            lblPageInfo.Size = new Size(78, 24);
            lblPageInfo.TabIndex = 2;
            lblPageInfo.Text = "Trang 1/1";
            lblPageInfo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnNext
            // 
            btnNext.BackColor = Color.FromArgb(37, 99, 235);
            btnNext.Cursor = Cursors.Hand;
            btnNext.FlatAppearance.BorderSize = 0;
            btnNext.FlatStyle = FlatStyle.Flat;
            btnNext.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnNext.ForeColor = Color.White;
            btnNext.Location = new Point(252, 6);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(75, 32);
            btnNext.TabIndex = 3;
            btnNext.Text = "Tiếp ▶";
            btnNext.UseVisualStyleBackColor = false;
            // 
            // btnLast
            // 
            btnLast.BackColor = Color.FromArgb(37, 99, 235);
            btnLast.Cursor = Cursors.Hand;
            btnLast.FlatAppearance.BorderSize = 0;
            btnLast.FlatStyle = FlatStyle.Flat;
            btnLast.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnLast.ForeColor = Color.White;
            btnLast.Location = new Point(334, 6);
            btnLast.Name = "btnLast";
            btnLast.Size = new Size(75, 32);
            btnLast.TabIndex = 4;
            btnLast.Text = "Cuối ⏭";
            btnLast.UseVisualStyleBackColor = false;
            // 
            // UcNguoidung
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            AutoScrollMinSize = new Size(0, 980);
            BackColor = Color.White;
            ClientSize = new Size(1257, 988);
            Controls.Add(pnlPaging);
            Controls.Add(pnlFilter);
            Controls.Add(dgvNguoiDung);
            Controls.Add(grpThongTin);
            Controls.Add(panelHeader);
            Name = "UcNguoidung";
            Text = "Quản Lý Người Dùng";
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            grpThongTin.ResumeLayout(false);
            grpThongTin.PerformLayout();
            pnlActions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvNguoiDung).EndInit();
            pnlFilter.ResumeLayout(false);
            pnlFilter.PerformLayout();
            pnlPaging.ResumeLayout(false);
            ResumeLayout(false);
        }
        #endregion

        private Panel panelHeader;
        private Label lblTitle;
        private GroupBox grpThongTin;
        private Label lblEmail, lblHoTen, lblMaNguoiDung, lblKhoaBoMon;
        private Label lblMatKhau, lblTenDangNhap, lblTrangThai, lblSoDienThoai, lblVaiTro;
        private TextBox txtMaNguoiDung, txtHoTen, txtEmail, txtKhoaBoMon;
        private TextBox txtMatKhau, txtSoDienThoai, txtTenDangNhap;
        private ComboBox cmbVaiTro, cmbTrangThai;
        private Panel pnlActions;
        private Button btnThem, btnSua, btnXoa, btnLuu, btnDatLaiMk, btnLamMoi, btnXuatExcel;
        private DataGridView dgvNguoiDung;
        private Panel pnlFilter;
        private TextBox textBox8;
        private ComboBox comboBox3, comboBox4;
        private Button btnLoc;
        // Pagination
        private Panel pnlPaging;
        private Button btnFirst, btnPrev, btnNext, btnLast;
        private Label lblPageInfo;
    }
}


