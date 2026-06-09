namespace FrmProject.GUI
{
    partial class FrmMain
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            pnlPagingDashboard = new Panel();
            btnFirstDashboard = new Button();
            btnPrevDashboard = new Button();
            lblPageInfoDashboard = new Label();
            btnNextDashboard = new Button();
            btnLastDashboard = new Button();
            pnlContainerelContent = new Panel();
            pnlGridData = new Panel();
            dgvData = new DataGridView();
            lblThietBiDangMuon = new Label();
            pnlContainer0 = new Panel();
            lbl0 = new Label();
            lblHuBaoTri = new Label();
            pnlContainer02 = new Panel();
            lbl02 = new Label();
            lblQuaHan = new Label();
            pnlContainer03 = new Panel();
            lbl03 = new Label();
            lblDangMuon = new Label();
            pnlContainer04 = new Panel();
            lbl04 = new Label();
            lblTongThietBi = new Label();
            pnlContainerel3 = new Panel();
            lblNgayGio = new Label();
            lblHeThongQuanLy = new Label();
            pn1Sidebar = new Panel();
            btnDangXuat = new Button();
            btnThungRac = new Button();
            btnCauHinh = new Button();
            btnBaoCao = new Button();
            btnDanhSachPhieu = new Button();
            btnTraThietBi = new Button();
            btnLapPhieuMuon = new Button();
            btnNguoiDung = new Button();
            btnPhongHoc = new Button();
            btnThietBi = new Button();
            btnTongQuan = new Button();
            lblQuanLyThietBi = new Label();
            panelContent = new Panel();
            pnlContainerel12 = new Panel();
            flpNotifications = new FlowLayoutPanel();
            lblCanhBaoThongBao = new Label();
            pnlContainerMuonTheoThang = new Panel();
            lblMuonTheoThang = new Label();
            pnlPagingDashboard.SuspendLayout();
            pnlContainerelContent.SuspendLayout();
            pnlGridData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvData).BeginInit();
            pnlContainer0.SuspendLayout();
            pnlContainer02.SuspendLayout();
            pnlContainer03.SuspendLayout();
            pnlContainer04.SuspendLayout();
            pnlContainerel3.SuspendLayout();
            pn1Sidebar.SuspendLayout();
            panelContent.SuspendLayout();
            pnlContainerel12.SuspendLayout();
            pnlContainerMuonTheoThang.SuspendLayout();
            SuspendLayout();
            // 
            // pnlPagingDashboard
            // 
            pnlPagingDashboard.BackColor = Color.White;
            pnlPagingDashboard.Controls.Add(btnFirstDashboard);
            pnlPagingDashboard.Controls.Add(btnPrevDashboard);
            pnlPagingDashboard.Controls.Add(lblPageInfoDashboard);
            pnlPagingDashboard.Controls.Add(btnNextDashboard);
            pnlPagingDashboard.Controls.Add(btnLastDashboard);
            pnlPagingDashboard.Location = new Point(10, 503);
            pnlPagingDashboard.Name = "pnlPagingDashboard";
            pnlPagingDashboard.Size = new Size(608, 40);
            pnlPagingDashboard.TabIndex = 3;
            // 
            // btnFirstDashboard
            // 
            btnFirstDashboard.Location = new Point(85, 5);
            btnFirstDashboard.Name = "btnFirstDashboard";
            btnFirstDashboard.Size = new Size(75, 30);
            btnFirstDashboard.TabIndex = 0;
            btnFirstDashboard.Text = "⏮ Đầu";
            btnFirstDashboard.UseVisualStyleBackColor = true;
            // 
            // btnPrevDashboard
            // 
            btnPrevDashboard.Location = new Point(165, 5);
            btnPrevDashboard.Name = "btnPrevDashboard";
            btnPrevDashboard.Size = new Size(85, 30);
            btnPrevDashboard.TabIndex = 1;
            btnPrevDashboard.Text = "◀ Trước";
            btnPrevDashboard.UseVisualStyleBackColor = true;
            // 
            // lblPageInfoDashboard
            // 
            lblPageInfoDashboard.Location = new Point(255, 5);
            lblPageInfoDashboard.Name = "lblPageInfoDashboard";
            lblPageInfoDashboard.Size = new Size(100, 30);
            lblPageInfoDashboard.TabIndex = 2;
            lblPageInfoDashboard.Text = "Trang 1 / 1";
            lblPageInfoDashboard.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnNextDashboard
            // 
            btnNextDashboard.Location = new Point(360, 5);
            btnNextDashboard.Name = "btnNextDashboard";
            btnNextDashboard.Size = new Size(80, 30);
            btnNextDashboard.TabIndex = 3;
            btnNextDashboard.Text = "Tiếp ▶";
            btnNextDashboard.UseVisualStyleBackColor = true;
            // 
            // btnLastDashboard
            // 
            btnLastDashboard.Location = new Point(445, 5);
            btnLastDashboard.Name = "btnLastDashboard";
            btnLastDashboard.Size = new Size(85, 30);
            btnLastDashboard.TabIndex = 4;
            btnLastDashboard.Text = "Cuối ⏭";
            btnLastDashboard.UseVisualStyleBackColor = true;
            // 
            // pnlContainerelContent
            // 
            pnlContainerelContent.AutoScroll = true;
            pnlContainerelContent.BackColor = Color.FromArgb(245, 246, 248);
            pnlContainerelContent.Controls.Add(pnlGridData);
            pnlContainerelContent.Controls.Add(pnlContainer0);
            pnlContainerelContent.Controls.Add(pnlContainer02);
            pnlContainerelContent.Controls.Add(pnlContainer03);
            pnlContainerelContent.Controls.Add(pnlContainer04);
            pnlContainerelContent.Controls.Add(pnlContainerel3);
            pnlContainerelContent.Controls.Add(pn1Sidebar);
            pnlContainerelContent.Controls.Add(panelContent);
            pnlContainerelContent.Dock = DockStyle.Fill;
            pnlContainerelContent.Location = new Point(0, 0);
            pnlContainerelContent.Name = "pnlContainerelContent";
            pnlContainerelContent.Size = new Size(1411, 782);
            pnlContainerelContent.TabIndex = 0;
            // 
            // pnlGridData
            // 
            pnlGridData.BackColor = Color.White;
            pnlGridData.Controls.Add(dgvData);
            pnlGridData.Controls.Add(lblThietBiDangMuon);
            pnlGridData.Controls.Add(pnlPagingDashboard);
            pnlGridData.Location = new Point(211, 168);
            pnlGridData.Name = "pnlGridData";
            pnlGridData.Size = new Size(629, 553);
            pnlGridData.TabIndex = 6;
            // 
            // dgvData
            // 
            dgvData.AllowUserToAddRows = false;
            dgvData.BackgroundColor = Color.White;
            dgvData.BorderStyle = BorderStyle.None;
            dgvData.ColumnHeadersHeight = 29;
            dgvData.Location = new Point(10, 35);
            dgvData.Name = "dgvData";
            dgvData.ReadOnly = true;
            dgvData.RowHeadersVisible = false;
            dgvData.RowHeadersWidth = 51;
            dgvData.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvData.Size = new Size(608, 462);
            dgvData.TabIndex = 1;
            // 
            // lblThietBiDangMuon
            // 
            lblThietBiDangMuon.AutoSize = true;
            lblThietBiDangMuon.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            lblThietBiDangMuon.Location = new Point(10, 8);
            lblThietBiDangMuon.Name = "lblThietBiDangMuon";
            lblThietBiDangMuon.Size = new Size(211, 25);
            lblThietBiDangMuon.TabIndex = 2;
            lblThietBiDangMuon.Text = "📋 Thiết bị đang mượn";
            // 
            // pnlContainer0
            // 
            pnlContainer0.BackColor = Color.FromArgb(230, 126, 34);
            pnlContainer0.Controls.Add(lbl0);
            pnlContainer0.Controls.Add(lblHuBaoTri);
            pnlContainer0.Location = new Point(1070, 60);
            pnlContainer0.Name = "pnlContainer0";
            pnlContainer0.Size = new Size(290, 99);
            pnlContainer0.TabIndex = 5;
            // 
            // lbl0
            // 
            lbl0.AutoSize = true;
            lbl0.Font = new Font("Segoe UI", 28.2F, FontStyle.Bold);
            lbl0.ForeColor = Color.White;
            lbl0.Location = new Point(60, 35);
            lbl0.Name = "lbl0";
            lbl0.Size = new Size(54, 62);
            lbl0.TabIndex = 0;
            lbl0.Text = "0";
            // 
            // lblHuBaoTri
            // 
            lblHuBaoTri.AutoSize = true;
            lblHuBaoTri.ForeColor = Color.White;
            lblHuBaoTri.Location = new Point(60, 15);
            lblHuBaoTri.Name = "lblHuBaoTri";
            lblHuBaoTri.Size = new Size(100, 20);
            lblHuBaoTri.TabIndex = 1;
            lblHuBaoTri.Text = "HƯ / BẢO TRÌ";
            // 
            // pnlContainer02
            // 
            pnlContainer02.BackColor = Color.FromArgb(231, 76, 60);
            pnlContainer02.Controls.Add(lbl02);
            pnlContainer02.Controls.Add(lblQuaHan);
            pnlContainer02.Location = new Point(770, 60);
            pnlContainer02.Name = "pnlContainer02";
            pnlContainer02.Size = new Size(290, 99);
            pnlContainer02.TabIndex = 4;
            // 
            // lbl02
            // 
            lbl02.AutoSize = true;
            lbl02.Font = new Font("Segoe UI", 28.2F, FontStyle.Bold);
            lbl02.ForeColor = Color.White;
            lbl02.Location = new Point(60, 35);
            lbl02.Name = "lbl02";
            lbl02.Size = new Size(54, 62);
            lbl02.TabIndex = 0;
            lbl02.Text = "0";
            // 
            // lblQuaHan
            // 
            lblQuaHan.AutoSize = true;
            lblQuaHan.ForeColor = Color.White;
            lblQuaHan.Location = new Point(60, 15);
            lblQuaHan.Name = "lblQuaHan";
            lblQuaHan.Size = new Size(76, 20);
            lblQuaHan.TabIndex = 1;
            lblQuaHan.Text = "QUÁ HẠN";
            // 
            // pnlContainer03
            // 
            pnlContainer03.BackColor = Color.FromArgb(39, 174, 96);
            pnlContainer03.Controls.Add(lbl03);
            pnlContainer03.Controls.Add(lblDangMuon);
            pnlContainer03.Location = new Point(489, 60);
            pnlContainer03.Name = "pnlContainer03";
            pnlContainer03.Size = new Size(271, 99);
            pnlContainer03.TabIndex = 3;
            // 
            // lbl03
            // 
            lbl03.AutoSize = true;
            lbl03.Font = new Font("Segoe UI", 28.2F, FontStyle.Bold);
            lbl03.ForeColor = Color.White;
            lbl03.Location = new Point(60, 35);
            lbl03.Name = "lbl03";
            lbl03.Size = new Size(54, 62);
            lbl03.TabIndex = 0;
            lbl03.Text = "0";
            // 
            // lblDangMuon
            // 
            lblDangMuon.AutoSize = true;
            lblDangMuon.ForeColor = Color.White;
            lblDangMuon.Location = new Point(67, 15);
            lblDangMuon.Name = "lblDangMuon";
            lblDangMuon.Size = new Size(101, 20);
            lblDangMuon.TabIndex = 1;
            lblDangMuon.Text = "ĐANG MƯỢN";
            // 
            // pnlContainer04
            // 
            pnlContainer04.BackColor = Color.FromArgb(58, 123, 213);
            pnlContainer04.Controls.Add(lbl04);
            pnlContainer04.Controls.Add(lblTongThietBi);
            pnlContainer04.Location = new Point(208, 60);
            pnlContainer04.Name = "pnlContainer04";
            pnlContainer04.Size = new Size(263, 99);
            pnlContainer04.TabIndex = 2;
            // 
            // lbl04
            // 
            lbl04.AutoSize = true;
            lbl04.Font = new Font("Segoe UI", 28.2F, FontStyle.Bold);
            lbl04.ForeColor = Color.White;
            lbl04.Location = new Point(60, 35);
            lbl04.Name = "lbl04";
            lbl04.Size = new Size(54, 62);
            lbl04.TabIndex = 0;
            lbl04.Text = "0";
            // 
            // lblTongThietBi
            // 
            lblTongThietBi.ForeColor = Color.White;
            lblTongThietBi.Location = new Point(60, 15);
            lblTongThietBi.Name = "lblTongThietBi";
            lblTongThietBi.Size = new Size(123, 23);
            lblTongThietBi.TabIndex = 1;
            lblTongThietBi.Text = "TỔNG THIẾT BỊ";
            // 
            // pnlContainerel3
            // 
            pnlContainerel3.BackColor = Color.FromArgb(26, 92, 53);
            pnlContainerel3.Controls.Add(lblNgayGio);
            pnlContainerel3.Controls.Add(lblHeThongQuanLy);
            pnlContainerel3.Location = new Point(205, 0);
            pnlContainerel3.Name = "pnlContainerel3";
            pnlContainerel3.Size = new Size(1155, 55);
            pnlContainerel3.TabIndex = 1;
            // 
            // lblNgayGio
            // 
            lblNgayGio.AutoSize = true;
            lblNgayGio.Font = new Font("Segoe UI", 10.2F);
            lblNgayGio.ForeColor = Color.White;
            lblNgayGio.Location = new Point(900, 15);
            lblNgayGio.Name = "lblNgayGio";
            lblNgayGio.Size = new Size(0, 23);
            lblNgayGio.TabIndex = 0;
            // 
            // lblHeThongQuanLy
            // 
            lblHeThongQuanLy.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold);
            lblHeThongQuanLy.ForeColor = Color.White;
            lblHeThongQuanLy.Location = new Point(3, 8);
            lblHeThongQuanLy.Name = "lblHeThongQuanLy";
            lblHeThongQuanLy.Size = new Size(1102, 45);
            lblHeThongQuanLy.TabIndex = 1;
            lblHeThongQuanLy.Text = "🏫 Hệ Thống Quản Lý Mượn Trả Thiết Bị Phòng Học";
            // 
            // pn1Sidebar
            // 
            pn1Sidebar.BackColor = Color.FromArgb(27, 94, 60);
            pn1Sidebar.Controls.Add(btnDangXuat);
            pn1Sidebar.Controls.Add(btnThungRac);
            pn1Sidebar.Controls.Add(btnCauHinh);
            pn1Sidebar.Controls.Add(btnBaoCao);
            pn1Sidebar.Controls.Add(btnDanhSachPhieu);
            pn1Sidebar.Controls.Add(btnTraThietBi);
            pn1Sidebar.Controls.Add(btnLapPhieuMuon);
            pn1Sidebar.Controls.Add(btnNguoiDung);
            pn1Sidebar.Controls.Add(btnPhongHoc);
            pn1Sidebar.Controls.Add(btnThietBi);
            pn1Sidebar.Controls.Add(btnTongQuan);
            pn1Sidebar.Controls.Add(lblQuanLyThietBi);
            pn1Sidebar.Location = new Point(0, 0);
            pn1Sidebar.Name = "pn1Sidebar";
            pn1Sidebar.Size = new Size(199, 860);
            pn1Sidebar.TabIndex = 0;
            // 
            // btnDangXuat
            // 
            btnDangXuat.BackColor = Color.Transparent;
            btnDangXuat.Cursor = Cursors.Hand;
            btnDangXuat.FlatAppearance.BorderSize = 0;
            btnDangXuat.FlatStyle = FlatStyle.Flat;
            btnDangXuat.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDangXuat.ForeColor = Color.White;
            btnDangXuat.Location = new Point(12, 686);
            btnDangXuat.Name = "btnDangXuat";
            btnDangXuat.Size = new Size(160, 48);
            btnDangXuat.TabIndex = 10;
            btnDangXuat.Text = "🚪 Đăng Xuất";
            btnDangXuat.UseVisualStyleBackColor = false;
            // 
            // btnThungRac
            // 
            btnThungRac.BackColor = Color.FromArgb(27, 94, 60);
            btnThungRac.Cursor = Cursors.Hand;
            btnThungRac.FlatAppearance.BorderSize = 0;
            btnThungRac.FlatStyle = FlatStyle.Flat;
            btnThungRac.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            btnThungRac.ForeColor = Color.White;
            btnThungRac.Location = new Point(0, 648);
            btnThungRac.Name = "btnThungRac";
            btnThungRac.Size = new Size(199, 48);
            btnThungRac.TabIndex = 12;
            btnThungRac.Text = "🗑 Thùng Rác";
            btnThungRac.UseVisualStyleBackColor = false;
            // 
            // btnCauHinh
            // 
            btnCauHinh.BackColor = Color.FromArgb(27, 94, 60);
            btnCauHinh.Cursor = Cursors.Hand;
            btnCauHinh.FlatAppearance.BorderSize = 0;
            btnCauHinh.FlatStyle = FlatStyle.Flat;
            btnCauHinh.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            btnCauHinh.ForeColor = Color.White;
            btnCauHinh.Location = new Point(0, 600);
            btnCauHinh.Name = "btnCauHinh";
            btnCauHinh.Size = new Size(199, 48);
            btnCauHinh.TabIndex = 9;
            btnCauHinh.Text = "🔧 Cấu Hình";
            btnCauHinh.UseVisualStyleBackColor = false;
            // 
            // btnBaoCao
            // 
            btnBaoCao.BackColor = Color.FromArgb(27, 94, 60);
            btnBaoCao.Cursor = Cursors.Hand;
            btnBaoCao.FlatAppearance.BorderSize = 0;
            btnBaoCao.FlatStyle = FlatStyle.Flat;
            btnBaoCao.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            btnBaoCao.ForeColor = Color.White;
            btnBaoCao.Location = new Point(-3, 556);
            btnBaoCao.Name = "btnBaoCao";
            btnBaoCao.Size = new Size(199, 48);
            btnBaoCao.TabIndex = 8;
            btnBaoCao.Text = "📊 Báo Cáo";
            btnBaoCao.UseVisualStyleBackColor = false;
            // 
            // btnDanhSachPhieu
            // 
            btnDanhSachPhieu.BackColor = Color.FromArgb(27, 94, 60);
            btnDanhSachPhieu.Cursor = Cursors.Hand;
            btnDanhSachPhieu.FlatAppearance.BorderSize = 0;
            btnDanhSachPhieu.FlatStyle = FlatStyle.Flat;
            btnDanhSachPhieu.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            btnDanhSachPhieu.ForeColor = Color.White;
            btnDanhSachPhieu.Location = new Point(3, 511);
            btnDanhSachPhieu.Name = "btnDanhSachPhieu";
            btnDanhSachPhieu.Size = new Size(199, 48);
            btnDanhSachPhieu.TabIndex = 7;
            btnDanhSachPhieu.Text = "📄 Danh Sách Phiếu";
            btnDanhSachPhieu.UseVisualStyleBackColor = false;
            // 
            // btnTraThietBi
            // 
            btnTraThietBi.BackColor = Color.FromArgb(27, 94, 60);
            btnTraThietBi.Cursor = Cursors.Hand;
            btnTraThietBi.FlatAppearance.BorderSize = 0;
            btnTraThietBi.FlatStyle = FlatStyle.Flat;
            btnTraThietBi.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            btnTraThietBi.ForeColor = Color.White;
            btnTraThietBi.Location = new Point(3, 457);
            btnTraThietBi.Name = "btnTraThietBi";
            btnTraThietBi.Size = new Size(199, 48);
            btnTraThietBi.TabIndex = 6;
            btnTraThietBi.Text = "↩ Trả Thiết Bị";
            btnTraThietBi.UseVisualStyleBackColor = false;
            // 
            // btnLapPhieuMuon
            // 
            btnLapPhieuMuon.BackColor = Color.FromArgb(27, 94, 60);
            btnLapPhieuMuon.Cursor = Cursors.Hand;
            btnLapPhieuMuon.FlatAppearance.BorderSize = 0;
            btnLapPhieuMuon.FlatStyle = FlatStyle.Flat;
            btnLapPhieuMuon.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            btnLapPhieuMuon.ForeColor = Color.White;
            btnLapPhieuMuon.Location = new Point(0, 403);
            btnLapPhieuMuon.Name = "btnLapPhieuMuon";
            btnLapPhieuMuon.Size = new Size(199, 48);
            btnLapPhieuMuon.TabIndex = 5;
            btnLapPhieuMuon.Text = "📋 Lập Phiếu Mượn";
            btnLapPhieuMuon.UseVisualStyleBackColor = false;
            // 
            // btnNguoiDung
            // 
            btnNguoiDung.BackColor = Color.FromArgb(27, 94, 60);
            btnNguoiDung.Cursor = Cursors.Hand;
            btnNguoiDung.FlatAppearance.BorderSize = 0;
            btnNguoiDung.FlatStyle = FlatStyle.Flat;
            btnNguoiDung.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            btnNguoiDung.ForeColor = Color.White;
            btnNguoiDung.Location = new Point(0, 349);
            btnNguoiDung.Name = "btnNguoiDung";
            btnNguoiDung.Size = new Size(199, 48);
            btnNguoiDung.TabIndex = 4;
            btnNguoiDung.Text = "👥 Người Dùng";
            btnNguoiDung.UseVisualStyleBackColor = false;
            // 
            // btnPhongHoc
            // 
            btnPhongHoc.BackColor = Color.FromArgb(27, 94, 60);
            btnPhongHoc.Cursor = Cursors.Hand;
            btnPhongHoc.FlatAppearance.BorderSize = 0;
            btnPhongHoc.FlatStyle = FlatStyle.Flat;
            btnPhongHoc.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            btnPhongHoc.ForeColor = Color.White;
            btnPhongHoc.Location = new Point(0, 295);
            btnPhongHoc.Name = "btnPhongHoc";
            btnPhongHoc.Size = new Size(199, 48);
            btnPhongHoc.TabIndex = 3;
            btnPhongHoc.Text = "🏫 Phòng Học";
            btnPhongHoc.UseVisualStyleBackColor = false;
            // 
            // btnThietBi
            // 
            btnThietBi.BackColor = Color.FromArgb(27, 94, 60);
            btnThietBi.Cursor = Cursors.Hand;
            btnThietBi.FlatAppearance.BorderSize = 0;
            btnThietBi.FlatStyle = FlatStyle.Flat;
            btnThietBi.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnThietBi.ForeColor = Color.White;
            btnThietBi.Location = new Point(0, 242);
            btnThietBi.Name = "btnThietBi";
            btnThietBi.Size = new Size(199, 48);
            btnThietBi.TabIndex = 2;
            btnThietBi.Text = "⚙ Thiết Bị";
            btnThietBi.UseVisualStyleBackColor = false;
            // 
            // btnTongQuan
            // 
            btnTongQuan.BackColor = Color.FromArgb(27, 94, 60);
            btnTongQuan.Cursor = Cursors.Hand;
            btnTongQuan.FlatAppearance.BorderSize = 0;
            btnTongQuan.FlatStyle = FlatStyle.Flat;
            btnTongQuan.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnTongQuan.ForeColor = Color.White;
            btnTongQuan.Location = new Point(0, 188);
            btnTongQuan.Name = "btnTongQuan";
            btnTongQuan.Size = new Size(199, 48);
            btnTongQuan.TabIndex = 1;
            btnTongQuan.Text = "🏠 Tổng Quan";
            btnTongQuan.UseVisualStyleBackColor = false;
            // 
            // lblQuanLyThietBi
            // 
            lblQuanLyThietBi.BackColor = Color.FromArgb(27, 94, 60);
            lblQuanLyThietBi.Font = new Font("Segoe UI", 16.2F, FontStyle.Bold);
            lblQuanLyThietBi.ForeColor = Color.White;
            lblQuanLyThietBi.Location = new Point(0, 0);
            lblQuanLyThietBi.Name = "lblQuanLyThietBi";
            lblQuanLyThietBi.Size = new Size(199, 159);
            lblQuanLyThietBi.TabIndex = 11;
            lblQuanLyThietBi.Text = "🏫 Quản Lý Thiết Bị Phòng Học";
            lblQuanLyThietBi.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panelContent
            // 
            panelContent.BackColor = Color.White;
            panelContent.Controls.Add(pnlContainerel12);
            panelContent.Controls.Add(pnlContainerMuonTheoThang);
            panelContent.Location = new Point(208, 165);
            panelContent.Name = "panelContent";
            panelContent.Size = new Size(1191, 569);
            panelContent.TabIndex = 9;
            // 
            // pnlContainerel12
            // 
            pnlContainerel12.BackColor = Color.White;
            pnlContainerel12.Controls.Add(flpNotifications);
            pnlContainerel12.Controls.Add(lblCanhBaoThongBao);
            pnlContainerel12.Location = new Point(645, 3);
            pnlContainerel12.Name = "pnlContainerel12";
            pnlContainerel12.Size = new Size(496, 283);
            pnlContainerel12.TabIndex = 7;
            // 
            // flpNotifications
            // 
            flpNotifications.AutoScroll = true;
            flpNotifications.Dock = DockStyle.Fill;
            flpNotifications.Location = new Point(0, 30);
            flpNotifications.Name = "flpNotifications";
            flpNotifications.Padding = new Padding(5);
            flpNotifications.Size = new Size(496, 253);
            flpNotifications.TabIndex = 6;
            // 
            // lblCanhBaoThongBao
            // 
            lblCanhBaoThongBao.AutoSize = true;
            lblCanhBaoThongBao.Dock = DockStyle.Top;
            lblCanhBaoThongBao.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCanhBaoThongBao.Location = new Point(0, 0);
            lblCanhBaoThongBao.Name = "lblCanhBaoThongBao";
            lblCanhBaoThongBao.Padding = new Padding(5, 5, 0, 5);
            lblCanhBaoThongBao.Size = new Size(188, 30);
            lblCanhBaoThongBao.TabIndex = 5;
            lblCanhBaoThongBao.Text = "⚠ Cảnh báo  Thông báo";
            // 
            // pnlContainerMuonTheoThang
            // 
            pnlContainerMuonTheoThang.BackColor = Color.White;
            pnlContainerMuonTheoThang.Controls.Add(lblMuonTheoThang);
            pnlContainerMuonTheoThang.Location = new Point(645, 298);
            pnlContainerMuonTheoThang.Name = "pnlContainerMuonTheoThang";
            pnlContainerMuonTheoThang.Size = new Size(496, 258);
            pnlContainerMuonTheoThang.TabIndex = 8;
            // 
            // lblMuonTheoThang
            // 
            lblMuonTheoThang.AutoSize = true;
            lblMuonTheoThang.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            lblMuonTheoThang.Location = new Point(5, 5);
            lblMuonTheoThang.Name = "lblMuonTheoThang";
            lblMuonTheoThang.Size = new Size(180, 23);
            lblMuonTheoThang.TabIndex = 0;
            lblMuonTheoThang.Text = "📊 Mượn theo tháng";
            // 
            // FrmMain
            // 
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1411, 782);
            Controls.Add(pnlContainerelContent);
            Name = "FrmMain";
            Text = "Quản Lý Mượn Trả Thiết Bị";
            WindowState = FormWindowState.Maximized;
            pnlPagingDashboard.ResumeLayout(false);
            pnlContainerelContent.ResumeLayout(false);
            pnlGridData.ResumeLayout(false);
            pnlGridData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvData).EndInit();
            pnlContainer0.ResumeLayout(false);
            pnlContainer0.PerformLayout();
            pnlContainer02.ResumeLayout(false);
            pnlContainer02.PerformLayout();
            pnlContainer03.ResumeLayout(false);
            pnlContainer03.PerformLayout();
            pnlContainer04.ResumeLayout(false);
            pnlContainer04.PerformLayout();
            pnlContainerel3.ResumeLayout(false);
            pnlContainerel3.PerformLayout();
            pn1Sidebar.ResumeLayout(false);
            panelContent.ResumeLayout(false);
            pnlContainerel12.ResumeLayout(false);
            pnlContainerel12.PerformLayout();
            pnlContainerMuonTheoThang.ResumeLayout(false);
            pnlContainerMuonTheoThang.PerformLayout();
            ResumeLayout(false);
        }

        private Panel pnlContainerelContent, pnlContainerel3, pnlContainer04, pnlContainer03, pnlContainer02, pnlContainer0, pnlGridData;
        private Panel pnlContainerel12, pnlContainerMuonTheoThang;
        private FlowLayoutPanel flpNotifications;
        private Label lblQuanLyThietBi, lblHeThongQuanLy, lblNgayGio, lblTongThietBi, lbl04, lblDangMuon, lbl03;
        private Label lblQuaHan, lbl02, lblHuBaoTri, lbl0, lblThietBiDangMuon, lblCanhBaoThongBao;
        private Label lblMuonTheoThang;
        private Button btnTongQuan, btnThietBi, btnPhongHoc, btnNguoiDung, btnLapPhieuMuon;
        private Button btnTraThietBi, btnDanhSachPhieu, btnBaoCao, btnCauHinh, btnThungRac, btnDangXuat;
        private DataGridView dgvData;
        private Panel pn1Sidebar;
        private Panel panelContent;
        private System.Windows.Forms.Panel pnlPagingDashboard;
        private System.Windows.Forms.Button btnFirstDashboard;
        private System.Windows.Forms.Button btnPrevDashboard;
        private System.Windows.Forms.Button btnNextDashboard;
        private System.Windows.Forms.Button btnLastDashboard;
        private System.Windows.Forms.Label lblPageInfoDashboard;
    }
}





