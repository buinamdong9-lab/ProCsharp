namespace FrmProject.GUI
{
    partial class UcDanhsachphieu
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
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            lblDanhSachPhieuMuon = new Label();
            pnlContainerQuaHan5 = new Panel();
            lblQuaHan5 = new Label();
            lblDangMuon = new Label();
            lblChoDuyet3 = new Label();
            lblTatCa128 = new Label();
            pnlFilter = new Panel();
            btnReset = new Button();
            btnLoc = new Button();
            dtpDenNgay = new DateTimePicker();
            lblDenNgay = new Label();
            dtpTuNgay = new DateTimePicker();
            lblTuNgay = new Label();
            cmbDangMuon = new ComboBox();
            txtTatCa128 = new TextBox();
            dgvData = new DataGridView();
            colTicketID = new DataGridViewTextBoxColumn();
            colSoPhieu = new DataGridViewTextBoxColumn();
            colNgayLap = new DataGridViewTextBoxColumn();
            ColNguoimuon = new DataGridViewTextBoxColumn();
            colPhong = new DataGridViewTextBoxColumn();
            colSoluong = new DataGridViewTextBoxColumn();
            colNgayMuon = new DataGridViewTextBoxColumn();
            colHanTra = new DataGridViewTextBoxColumn();
            colNgaytra = new DataGridViewTextBoxColumn();
            colTrangThai = new DataGridViewTextBoxColumn();
            colThaoTac = new DataGridViewTextBoxColumn();
            pnlActions = new Panel();
            btnPheDuyet = new Button();
            btnTuChoi = new Button();
            lblChonPhieu = new Label();
            pnlChiTietPhieu = new Panel();
            dgvChiTietThietBi = new DataGridView();
            lblCTTrangThai = new Label();
            lblCTTrangThaiTitle = new Label();
            lblCTNguoiDuyet = new Label();
            lblCTNguoiDuyetTitle = new Label();
            lblCTHanTra = new Label();
            lblCTHanTraTitle = new Label();
            lblCTNgayMuon = new Label();
            lblCTNgayMuonTitle = new Label();
            lblCTMucDich = new Label();
            lblCTMucDichTitle = new Label();
            lblCTPhong = new Label();
            lblCTPhongTitle = new Label();
            lblCTNguoiMuon = new Label();
            lblCTNguoiMuonTitle = new Label();
            lblCTSoPhieu = new Label();
            lblCTSoPhieuTitle = new Label();
            dgvThongTinPhieu = new DataGridView();
            pnlChiTietLine = new Panel();
            lblChiTietTitle = new Label();
            colCTMaTB = new DataGridViewTextBoxColumn();
            colCTTenThietBi = new DataGridViewTextBoxColumn();
            colCTSoLuong = new DataGridViewTextBoxColumn();
            colCTTinhTrang = new DataGridViewTextBoxColumn();
            pnlContainerQuaHan5.SuspendLayout();
            pnlFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvData).BeginInit();
            pnlChiTietPhieu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvChiTietThietBi).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvThongTinPhieu).BeginInit();
            SuspendLayout();
            // 
            // lblDanhSachPhieuMuon
            // 
            lblDanhSachPhieuMuon.AutoSize = true;
            lblDanhSachPhieuMuon.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDanhSachPhieuMuon.ForeColor = Color.FromArgb(27, 94, 60);
            lblDanhSachPhieuMuon.Location = new Point(11, 9);
            lblDanhSachPhieuMuon.Name = "lblDanhSachPhieuMuon";
            lblDanhSachPhieuMuon.Size = new Size(310, 31);
            lblDanhSachPhieuMuon.TabIndex = 0;
            lblDanhSachPhieuMuon.Text = "🗒  Danh Sách Phiếu Mượn";
            // 
            // pnlContainerQuaHan5
            // 
            pnlContainerQuaHan5.Controls.Add(lblQuaHan5);
            pnlContainerQuaHan5.Controls.Add(lblDangMuon);
            pnlContainerQuaHan5.Controls.Add(lblChoDuyet3);
            pnlContainerQuaHan5.Controls.Add(lblTatCa128);
            pnlContainerQuaHan5.Location = new Point(11, 59);
            pnlContainerQuaHan5.Name = "pnlContainerQuaHan5";
            pnlContainerQuaHan5.Size = new Size(579, 48);
            pnlContainerQuaHan5.TabIndex = 1;
            // 
            // lblQuaHan5
            // 
            lblQuaHan5.BackColor = Color.FromArgb(220, 53, 69);
            lblQuaHan5.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblQuaHan5.Location = new Point(473, 0);
            lblQuaHan5.Name = "lblQuaHan5";
            lblQuaHan5.Size = new Size(101, 48);
            lblQuaHan5.TabIndex = 3;
            lblQuaHan5.Text = "Quá hạn: 5";
            lblQuaHan5.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblDangMuon
            // 
            lblDangMuon.BackColor = Color.FromArgb(40, 167, 69);
            lblDangMuon.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDangMuon.Location = new Point(309, 0);
            lblDangMuon.Name = "lblDangMuon";
            lblDangMuon.Size = new Size(114, 48);
            lblDangMuon.TabIndex = 2;
            lblDangMuon.Text = "Đang mượn:";
            lblDangMuon.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblChoDuyet3
            // 
            lblChoDuyet3.BackColor = Color.FromArgb(255, 193, 7);
            lblChoDuyet3.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblChoDuyet3.ForeColor = Color.White;
            lblChoDuyet3.Location = new Point(143, 0);
            lblChoDuyet3.Name = "lblChoDuyet3";
            lblChoDuyet3.Size = new Size(121, 48);
            lblChoDuyet3.TabIndex = 1;
            lblChoDuyet3.Text = "Chờ duyệt: 3";
            lblChoDuyet3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTatCa128
            // 
            lblTatCa128.BackColor = Color.FromArgb(108, 117, 125);
            lblTatCa128.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTatCa128.Location = new Point(3, 0);
            lblTatCa128.Name = "lblTatCa128";
            lblTatCa128.Size = new Size(98, 48);
            lblTatCa128.TabIndex = 0;
            lblTatCa128.Text = "Tất cả: 128";
            lblTatCa128.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlFilter
            // 
            pnlFilter.Controls.Add(btnReset);
            pnlFilter.Controls.Add(btnLoc);
            pnlFilter.Controls.Add(dtpDenNgay);
            pnlFilter.Controls.Add(lblDenNgay);
            pnlFilter.Controls.Add(dtpTuNgay);
            pnlFilter.Controls.Add(lblTuNgay);
            pnlFilter.Controls.Add(cmbDangMuon);
            pnlFilter.Controls.Add(txtTatCa128);
            pnlFilter.Location = new Point(11, 133);
            pnlFilter.Name = "pnlFilter";
            pnlFilter.Size = new Size(1265, 49);
            pnlFilter.TabIndex = 2;
            // 
            // btnReset
            // 
            btnReset.BackColor = Color.FromArgb(108, 117, 125);
            btnReset.Cursor = Cursors.Hand;
            btnReset.FlatAppearance.BorderSize = 0;
            btnReset.FlatStyle = FlatStyle.Flat;
            btnReset.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnReset.ForeColor = Color.White;
            btnReset.Location = new Point(1138, 12);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(78, 27);
            btnReset.TabIndex = 11;
            btnReset.Text = "↺ Reset";
            btnReset.UseVisualStyleBackColor = false;
            // 
            // btnLoc
            // 
            btnLoc.BackColor = Color.FromArgb(40, 167, 69);
            btnLoc.Cursor = Cursors.Hand;
            btnLoc.FlatAppearance.BorderSize = 0;
            btnLoc.FlatStyle = FlatStyle.Flat;
            btnLoc.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnLoc.ForeColor = Color.White;
            btnLoc.Location = new Point(1038, 10);
            btnLoc.Name = "btnLoc";
            btnLoc.Size = new Size(72, 29);
            btnLoc.TabIndex = 7;
            btnLoc.Text = "⚡ Lọc";
            btnLoc.UseVisualStyleBackColor = false;
            // 
            // dtpDenNgay
            // 
            dtpDenNgay.Format = DateTimePickerFormat.Short;
            dtpDenNgay.Location = new Point(859, 13);
            dtpDenNgay.Name = "dtpDenNgay";
            dtpDenNgay.Size = new Size(125, 27);
            dtpDenNgay.TabIndex = 6;
            // 
            // lblDenNgay
            // 
            lblDenNgay.AutoSize = true;
            lblDenNgay.ForeColor = SystemColors.ControlText;
            lblDenNgay.Location = new Point(767, 18);
            lblDenNgay.Name = "lblDenNgay";
            lblDenNgay.Size = new Size(36, 20);
            lblDenNgay.TabIndex = 5;
            lblDenNgay.Text = "Đến";
            // 
            // dtpTuNgay
            // 
            dtpTuNgay.Format = DateTimePickerFormat.Short;
            dtpTuNgay.Location = new Point(628, 12);
            dtpTuNgay.Name = "dtpTuNgay";
            dtpTuNgay.Size = new Size(124, 27);
            dtpTuNgay.TabIndex = 4;
            // 
            // lblTuNgay
            // 
            lblTuNgay.AutoSize = true;
            lblTuNgay.ForeColor = Color.Black;
            lblTuNgay.Location = new Point(580, 15);
            lblTuNgay.Name = "lblTuNgay";
            lblTuNgay.Size = new Size(26, 20);
            lblTuNgay.TabIndex = 3;
            lblTuNgay.Text = "Từ";
            // 
            // cmbDangMuon
            // 
            cmbDangMuon.FormattingEnabled = true;
            cmbDangMuon.Items.AddRange(new object[] { "Tất cả", "Chờ duyệt", "Đang mượn", "Quá hạn", "Đã trả", "Từ chối" });
            cmbDangMuon.Location = new Point(320, 13);
            cmbDangMuon.Name = "cmbDangMuon";
            cmbDangMuon.Size = new Size(232, 28);
            cmbDangMuon.TabIndex = 1;
            // 
            // txtTatCa128
            // 
            txtTatCa128.Location = new Point(8, 13);
            txtTatCa128.Name = "txtTatCa128";
            txtTatCa128.PlaceholderText = "🔍 Tìm theo số phiếu, người mượn...";
            txtTatCa128.Size = new Size(276, 27);
            txtTatCa128.TabIndex = 0;
            // 
            // dgvData
            // 
            dgvData.AllowUserToAddRows = false;
            dgvData.AllowUserToResizeColumns = false;
            dgvData.BackgroundColor = Color.White;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.White;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(225, 239, 252);
            dataGridViewCellStyle1.SelectionForeColor = Color.Black;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvData.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvData.Columns.AddRange(new DataGridViewColumn[] { colTicketID, colSoPhieu, colNgayLap, ColNguoimuon, colPhong, colSoluong, colNgayMuon, colHanTra, colNgaytra, colTrangThai, colThaoTac });
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(225, 239, 252);
            dataGridViewCellStyle2.SelectionForeColor = Color.Black;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvData.DefaultCellStyle = dataGridViewCellStyle2;
            dgvData.EnableHeadersVisualStyles = false;
            dgvData.GridColor = Color.White;
            dgvData.Location = new Point(11, 188);
            dgvData.Name = "dgvData";
            dgvData.ReadOnly = true;
            dgvData.RowHeadersVisible = false;
            dgvData.RowHeadersWidth = 51;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.ForeColor = Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(225, 239, 252);
            dataGridViewCellStyle3.SelectionForeColor = Color.Black;
            dgvData.RowsDefaultCellStyle = dataGridViewCellStyle3;
            dgvData.Size = new Size(1355, 342);
            dgvData.TabIndex = 3;
            // 
            // colTicketID
            // 
            colTicketID.HeaderText = "ID phiếu";
            colTicketID.MinimumWidth = 6;
            colTicketID.Name = "colTicketID";
            colTicketID.ReadOnly = true;
            colTicketID.Width = 80;
            // 
            // colSoPhieu
            // 
            colSoPhieu.HeaderText = "Số phiếu";
            colSoPhieu.MinimumWidth = 6;
            colSoPhieu.Name = "colSoPhieu";
            colSoPhieu.ReadOnly = true;
            colSoPhieu.Width = 125;
            // 
            // colNgayLap
            // 
            colNgayLap.HeaderText = "Ngày lập";
            colNgayLap.MinimumWidth = 6;
            colNgayLap.Name = "colNgayLap";
            colNgayLap.ReadOnly = true;
            colNgayLap.Width = 125;
            // 
            // ColNguoimuon
            // 
            ColNguoimuon.HeaderText = "Người mượn";
            ColNguoimuon.MinimumWidth = 6;
            ColNguoimuon.Name = "ColNguoimuon";
            ColNguoimuon.ReadOnly = true;
            ColNguoimuon.Width = 150;
            // 
            // colPhong
            // 
            colPhong.HeaderText = "Phòng";
            colPhong.MinimumWidth = 6;
            colPhong.Name = "colPhong";
            colPhong.ReadOnly = true;
            colPhong.Width = 150;
            // 
            // colSoluong
            // 
            colSoluong.HeaderText = "Số lượng";
            colSoluong.MinimumWidth = 6;
            colSoluong.Name = "colSoluong";
            colSoluong.ReadOnly = true;
            colSoluong.Width = 60;
            // 
            // colNgayMuon
            // 
            colNgayMuon.HeaderText = "Ngày mượn";
            colNgayMuon.MinimumWidth = 6;
            colNgayMuon.Name = "colNgayMuon";
            colNgayMuon.ReadOnly = true;
            colNgayMuon.Width = 125;
            // 
            // colHanTra
            // 
            colHanTra.HeaderText = "Hạn trả";
            colHanTra.MinimumWidth = 6;
            colHanTra.Name = "colHanTra";
            colHanTra.ReadOnly = true;
            colHanTra.Width = 125;
            // 
            // colNgaytra
            // 
            colNgaytra.HeaderText = "Ngày trả";
            colNgaytra.MinimumWidth = 6;
            colNgaytra.Name = "colNgaytra";
            colNgaytra.ReadOnly = true;
            colNgaytra.Width = 125;
            // 
            // colTrangThai
            // 
            colTrangThai.HeaderText = "Trạng thái";
            colTrangThai.MinimumWidth = 6;
            colTrangThai.Name = "colTrangThai";
            colTrangThai.ReadOnly = true;
            colTrangThai.Width = 125;
            // 
            // colThaoTac
            // 
            colThaoTac.HeaderText = "Thao tác";
            colThaoTac.MinimumWidth = 6;
            colThaoTac.Name = "colThaoTac";
            colThaoTac.ReadOnly = true;
            colThaoTac.Width = 150;
            // 
            // pnlActions
            // 
            pnlActions.Location = new Point(11, 59);
            pnlActions.Name = "pnlActions";
            pnlActions.Size = new Size(102, 48);
            pnlActions.TabIndex = 4;
            // 
            // btnPheDuyet
            // 
            btnPheDuyet.Location = new Point(0, 0);
            btnPheDuyet.Name = "btnPheDuyet";
            btnPheDuyet.Size = new Size(75, 23);
            btnPheDuyet.TabIndex = 0;
            // 
            // btnTuChoi
            // 
            btnTuChoi.Location = new Point(0, 0);
            btnTuChoi.Name = "btnTuChoi";
            btnTuChoi.Size = new Size(75, 23);
            btnTuChoi.TabIndex = 0;
            // 
            // lblChonPhieu
            // 
            lblChonPhieu.Location = new Point(0, 0);
            lblChonPhieu.Name = "lblChonPhieu";
            lblChonPhieu.Size = new Size(100, 23);
            lblChonPhieu.TabIndex = 0;
            // 
            // pnlChiTietPhieu
            // 
            pnlChiTietPhieu.BackColor = Color.White;
            pnlChiTietPhieu.BorderStyle = BorderStyle.FixedSingle;
            pnlChiTietPhieu.Controls.Add(dgvChiTietThietBi);
            pnlChiTietPhieu.Controls.Add(lblCTTrangThai);
            pnlChiTietPhieu.Controls.Add(lblCTTrangThaiTitle);
            pnlChiTietPhieu.Controls.Add(lblCTNguoiDuyet);
            pnlChiTietPhieu.Controls.Add(lblCTNguoiDuyetTitle);
            pnlChiTietPhieu.Controls.Add(lblCTHanTra);
            pnlChiTietPhieu.Controls.Add(lblCTHanTraTitle);
            pnlChiTietPhieu.Controls.Add(lblCTNgayMuon);
            pnlChiTietPhieu.Controls.Add(lblCTNgayMuonTitle);
            pnlChiTietPhieu.Controls.Add(lblCTMucDich);
            pnlChiTietPhieu.Controls.Add(lblCTMucDichTitle);
            pnlChiTietPhieu.Controls.Add(lblCTPhong);
            pnlChiTietPhieu.Controls.Add(lblCTPhongTitle);
            pnlChiTietPhieu.Controls.Add(lblCTNguoiMuon);
            pnlChiTietPhieu.Controls.Add(lblCTNguoiMuonTitle);
            pnlChiTietPhieu.Controls.Add(lblCTSoPhieu);
            pnlChiTietPhieu.Controls.Add(lblCTSoPhieuTitle);
            pnlChiTietPhieu.Controls.Add(dgvThongTinPhieu);
            pnlChiTietPhieu.Controls.Add(pnlChiTietLine);
            pnlChiTietPhieu.Controls.Add(lblChiTietTitle);
            pnlChiTietPhieu.Location = new Point(11, 537);
            pnlChiTietPhieu.Margin = new Padding(3, 4, 3, 4);
            pnlChiTietPhieu.Name = "pnlChiTietPhieu";
            pnlChiTietPhieu.Padding = new Padding(14, 16, 14, 16);
            pnlChiTietPhieu.Size = new Size(1355, 357);
            pnlChiTietPhieu.TabIndex = 5;
            pnlChiTietPhieu.Visible = false;
            // 
            // dgvChiTietThietBi
            // 
            dgvChiTietThietBi.AllowUserToAddRows = false;
            dgvChiTietThietBi.AllowUserToDeleteRows = false;
            dgvChiTietThietBi.AllowUserToResizeRows = false;
            dgvChiTietThietBi.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgvChiTietThietBi.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvChiTietThietBi.BackgroundColor = Color.White;
            dgvChiTietThietBi.BorderStyle = BorderStyle.None;
            dgvChiTietThietBi.ColumnHeadersHeight = 34;
            dgvChiTietThietBi.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvChiTietThietBi.Columns.AddRange(new DataGridViewColumn[] { colCTMaTB, colCTTenThietBi, colCTSoLuong, colCTTinhTrang });
            dgvChiTietThietBi.EnableHeadersVisualStyles = false;
            dgvChiTietThietBi.GridColor = Color.FromArgb(235, 239, 245);
            dgvChiTietThietBi.Location = new Point(15, 241);
            dgvChiTietThietBi.Margin = new Padding(3, 4, 3, 4);
            dgvChiTietThietBi.Name = "dgvChiTietThietBi";
            dgvChiTietThietBi.ReadOnly = true;
            dgvChiTietThietBi.RowHeadersVisible = false;
            dgvChiTietThietBi.RowHeadersWidth = 51;
            dgvChiTietThietBi.RowTemplate.Height = 30;
            dgvChiTietThietBi.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvChiTietThietBi.Size = new Size(1238, 96);
            dgvChiTietThietBi.TabIndex = 18;
            // 
            // lblCTTrangThai
            // 
            lblCTTrangThai.AutoSize = true;
            lblCTTrangThai.BackColor = Color.FromArgb(225, 245, 234);
            lblCTTrangThai.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblCTTrangThai.ForeColor = Color.FromArgb(25, 135, 84);
            lblCTTrangThai.Location = new Point(669, 164);
            lblCTTrangThai.Name = "lblCTTrangThai";
            lblCTTrangThai.Padding = new Padding(9, 3, 9, 3);
            lblCTTrangThai.Size = new Size(33, 26);
            lblCTTrangThai.TabIndex = 17;
            lblCTTrangThai.Text = "-";
            // 
            // lblCTTrangThaiTitle
            // 
            lblCTTrangThaiTitle.AutoSize = true;
            lblCTTrangThaiTitle.ForeColor = Color.FromArgb(85, 105, 135);
            lblCTTrangThaiTitle.Location = new Point(571, 167);
            lblCTTrangThaiTitle.Name = "lblCTTrangThaiTitle";
            lblCTTrangThaiTitle.Size = new Size(78, 20);
            lblCTTrangThaiTitle.TabIndex = 16;
            lblCTTrangThaiTitle.Text = "Trạng thái:";
            // 
            // lblCTNguoiDuyet
            // 
            lblCTNguoiDuyet.AutoSize = true;
            lblCTNguoiDuyet.ForeColor = Color.FromArgb(10, 24, 50);
            lblCTNguoiDuyet.Location = new Point(669, 139);
            lblCTNguoiDuyet.Name = "lblCTNguoiDuyet";
            lblCTNguoiDuyet.Size = new Size(15, 20);
            lblCTNguoiDuyet.TabIndex = 15;
            lblCTNguoiDuyet.Text = "-";
            // 
            // lblCTNguoiDuyetTitle
            // 
            lblCTNguoiDuyetTitle.AutoSize = true;
            lblCTNguoiDuyetTitle.ForeColor = Color.FromArgb(85, 105, 135);
            lblCTNguoiDuyetTitle.Location = new Point(571, 139);
            lblCTNguoiDuyetTitle.Name = "lblCTNguoiDuyetTitle";
            lblCTNguoiDuyetTitle.Size = new Size(95, 20);
            lblCTNguoiDuyetTitle.TabIndex = 14;
            lblCTNguoiDuyetTitle.Text = "Người duyệt:";
            // 
            // lblCTHanTra
            // 
            lblCTHanTra.AutoSize = true;
            lblCTHanTra.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblCTHanTra.ForeColor = Color.FromArgb(10, 24, 50);
            lblCTHanTra.Location = new Point(669, 108);
            lblCTHanTra.Name = "lblCTHanTra";
            lblCTHanTra.Size = new Size(16, 21);
            lblCTHanTra.TabIndex = 13;
            lblCTHanTra.Text = "-";
            // 
            // lblCTHanTraTitle
            // 
            lblCTHanTraTitle.AutoSize = true;
            lblCTHanTraTitle.ForeColor = Color.FromArgb(85, 105, 135);
            lblCTHanTraTitle.Location = new Point(571, 111);
            lblCTHanTraTitle.Name = "lblCTHanTraTitle";
            lblCTHanTraTitle.Size = new Size(61, 20);
            lblCTHanTraTitle.TabIndex = 12;
            lblCTHanTraTitle.Text = "Hạn trả:";
            // 
            // lblCTNgayMuon
            // 
            lblCTNgayMuon.AutoSize = true;
            lblCTNgayMuon.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblCTNgayMuon.ForeColor = Color.FromArgb(10, 24, 50);
            lblCTNgayMuon.Location = new Point(669, 80);
            lblCTNgayMuon.Name = "lblCTNgayMuon";
            lblCTNgayMuon.Size = new Size(16, 21);
            lblCTNgayMuon.TabIndex = 11;
            lblCTNgayMuon.Text = "-";
            // 
            // lblCTNgayMuonTitle
            // 
            lblCTNgayMuonTitle.AutoSize = true;
            lblCTNgayMuonTitle.ForeColor = Color.FromArgb(85, 105, 135);
            lblCTNgayMuonTitle.Location = new Point(571, 83);
            lblCTNgayMuonTitle.Name = "lblCTNgayMuonTitle";
            lblCTNgayMuonTitle.Size = new Size(90, 20);
            lblCTNgayMuonTitle.TabIndex = 10;
            lblCTNgayMuonTitle.Text = "Ngày mượn:";
            // 
            // lblCTMucDich
            // 
            lblCTMucDich.AutoSize = true;
            lblCTMucDich.ForeColor = Color.FromArgb(10, 24, 50);
            lblCTMucDich.Location = new Point(111, 167);
            lblCTMucDich.Name = "lblCTMucDich";
            lblCTMucDich.Size = new Size(15, 20);
            lblCTMucDich.TabIndex = 9;
            lblCTMucDich.Text = "-";
            // 
            // lblCTMucDichTitle
            // 
            lblCTMucDichTitle.AutoSize = true;
            lblCTMucDichTitle.ForeColor = Color.FromArgb(85, 105, 135);
            lblCTMucDichTitle.Location = new Point(16, 167);
            lblCTMucDichTitle.Name = "lblCTMucDichTitle";
            lblCTMucDichTitle.Size = new Size(72, 20);
            lblCTMucDichTitle.TabIndex = 8;
            lblCTMucDichTitle.Text = "Mục đích:";
            // 
            // lblCTPhong
            // 
            lblCTPhong.AutoSize = true;
            lblCTPhong.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblCTPhong.ForeColor = Color.FromArgb(10, 24, 50);
            lblCTPhong.Location = new Point(111, 136);
            lblCTPhong.Name = "lblCTPhong";
            lblCTPhong.Size = new Size(16, 21);
            lblCTPhong.TabIndex = 7;
            lblCTPhong.Text = "-";
            // 
            // lblCTPhongTitle
            // 
            lblCTPhongTitle.AutoSize = true;
            lblCTPhongTitle.ForeColor = Color.FromArgb(85, 105, 135);
            lblCTPhongTitle.Location = new Point(16, 139);
            lblCTPhongTitle.Name = "lblCTPhongTitle";
            lblCTPhongTitle.Size = new Size(54, 20);
            lblCTPhongTitle.TabIndex = 6;
            lblCTPhongTitle.Text = "Phòng:";
            // 
            // lblCTNguoiMuon
            // 
            lblCTNguoiMuon.AutoSize = true;
            lblCTNguoiMuon.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblCTNguoiMuon.ForeColor = Color.FromArgb(10, 24, 50);
            lblCTNguoiMuon.Location = new Point(111, 108);
            lblCTNguoiMuon.Name = "lblCTNguoiMuon";
            lblCTNguoiMuon.Size = new Size(16, 21);
            lblCTNguoiMuon.TabIndex = 5;
            lblCTNguoiMuon.Text = "-";
            // 
            // lblCTNguoiMuonTitle
            // 
            lblCTNguoiMuonTitle.AutoSize = true;
            lblCTNguoiMuonTitle.ForeColor = Color.FromArgb(85, 105, 135);
            lblCTNguoiMuonTitle.Location = new Point(16, 111);
            lblCTNguoiMuonTitle.Name = "lblCTNguoiMuonTitle";
            lblCTNguoiMuonTitle.Size = new Size(97, 20);
            lblCTNguoiMuonTitle.TabIndex = 4;
            lblCTNguoiMuonTitle.Text = "Người mượn:";
            // 
            // lblCTSoPhieu
            // 
            lblCTSoPhieu.AutoSize = true;
            lblCTSoPhieu.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblCTSoPhieu.ForeColor = Color.FromArgb(10, 24, 50);
            lblCTSoPhieu.Location = new Point(111, 80);
            lblCTSoPhieu.Name = "lblCTSoPhieu";
            lblCTSoPhieu.Size = new Size(16, 21);
            lblCTSoPhieu.TabIndex = 3;
            lblCTSoPhieu.Text = "-";
            // 
            // lblCTSoPhieuTitle
            // 
            lblCTSoPhieuTitle.AutoSize = true;
            lblCTSoPhieuTitle.ForeColor = Color.FromArgb(85, 105, 135);
            lblCTSoPhieuTitle.Location = new Point(16, 83);
            lblCTSoPhieuTitle.Name = "lblCTSoPhieuTitle";
            lblCTSoPhieuTitle.Size = new Size(70, 20);
            lblCTSoPhieuTitle.TabIndex = 2;
            lblCTSoPhieuTitle.Text = "Số phiếu:";
            // 
            // dgvThongTinPhieu
            // 
            dgvThongTinPhieu.AllowUserToAddRows = false;
            dgvThongTinPhieu.AllowUserToDeleteRows = false;
            dgvThongTinPhieu.AllowUserToResizeRows = false;
            dgvThongTinPhieu.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgvThongTinPhieu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvThongTinPhieu.BackgroundColor = Color.White;
            dgvThongTinPhieu.BorderStyle = BorderStyle.None;
            dgvThongTinPhieu.ColumnHeadersHeight = 34;
            dgvThongTinPhieu.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvThongTinPhieu.EnableHeadersVisualStyles = false;
            dgvThongTinPhieu.GridColor = Color.FromArgb(235, 239, 245);
            dgvThongTinPhieu.Location = new Point(15, 76);
            dgvThongTinPhieu.Margin = new Padding(3, 4, 3, 4);
            dgvThongTinPhieu.MultiSelect = false;
            dgvThongTinPhieu.Name = "dgvThongTinPhieu";
            dgvThongTinPhieu.ReadOnly = true;
            dgvThongTinPhieu.RowHeadersVisible = false;
            dgvThongTinPhieu.RowHeadersWidth = 51;
            dgvThongTinPhieu.RowTemplate.Height = 24;
            dgvThongTinPhieu.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvThongTinPhieu.Size = new Size(1238, 151);
            dgvThongTinPhieu.TabIndex = 19;
            // 
            // pnlChiTietLine
            // 
            pnlChiTietLine.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlChiTietLine.BackColor = Color.FromArgb(27, 94, 60);
            pnlChiTietLine.Location = new Point(15, 59);
            pnlChiTietLine.Margin = new Padding(3, 4, 3, 4);
            pnlChiTietLine.Name = "pnlChiTietLine";
            pnlChiTietLine.Size = new Size(1238, 1);
            pnlChiTietLine.TabIndex = 1;
            // 
            // lblChiTietTitle
            // 
            lblChiTietTitle.AutoSize = true;
            lblChiTietTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblChiTietTitle.ForeColor = Color.FromArgb(10, 24, 50);
            lblChiTietTitle.Location = new Point(15, 15);
            lblChiTietTitle.Name = "lblChiTietTitle";
            lblChiTietTitle.Size = new Size(174, 28);
            lblChiTietTitle.TabIndex = 0;
            lblChiTietTitle.Text = "📋 Chi tiết phiếu";
            // 
            // colCTMaTB
            // 
            colCTMaTB.DataPropertyName = "DeviceCode";
            colCTMaTB.FillWeight = 90F;
            colCTMaTB.HeaderText = "Mã TB";
            colCTMaTB.MinimumWidth = 6;
            colCTMaTB.Name = "colCTMaTB";
            colCTMaTB.ReadOnly = true;
            // 
            // colCTTenThietBi
            // 
            colCTTenThietBi.DataPropertyName = "DeviceName";
            colCTTenThietBi.FillWeight = 180F;
            colCTTenThietBi.HeaderText = "Tên thiết bị";
            colCTTenThietBi.MinimumWidth = 6;
            colCTTenThietBi.Name = "colCTTenThietBi";
            colCTTenThietBi.ReadOnly = true;
            // 
            // colCTSoLuong
            // 
            colCTSoLuong.DataPropertyName = "Quantity";
            colCTSoLuong.FillWeight = 70F;
            colCTSoLuong.HeaderText = "Số lượng";
            colCTSoLuong.MinimumWidth = 6;
            colCTSoLuong.Name = "colCTSoLuong";
            colCTSoLuong.ReadOnly = true;
            // 
            // colCTTinhTrang
            // 
            colCTTinhTrang.DataPropertyName = "BorrowCondition";
            colCTTinhTrang.FillWeight = 145F;
            colCTTinhTrang.HeaderText = "Tình trạng khi trả";
            colCTTinhTrang.MinimumWidth = 6;
            colCTTinhTrang.Name = "colCTTinhTrang";
            colCTTinhTrang.ReadOnly = true;
            // 
            // UcDanhsachphieu
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            AutoScrollMinSize = new Size(0, 930);
            Controls.Add(pnlChiTietPhieu);
            Controls.Add(dgvData);
            Controls.Add(pnlFilter);
            Controls.Add(pnlContainerQuaHan5);
            Controls.Add(pnlActions);
            Controls.Add(lblDanhSachPhieuMuon);
            ForeColor = Color.Black;
            Name = "UcDanhsachphieu";
            Size = new Size(1410, 906);
            pnlContainerQuaHan5.ResumeLayout(false);
            pnlFilter.ResumeLayout(false);
            pnlFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvData).EndInit();
            pnlChiTietPhieu.ResumeLayout(false);
            pnlChiTietPhieu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvChiTietThietBi).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvThongTinPhieu).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        // pnlActions injected programmatically
        private void ConfigureActionPanel()
        {
            // pnlActions - Action bar
            pnlActions.BackColor = Color.FromArgb(245, 248, 245);
            pnlActions.BorderStyle = BorderStyle.FixedSingle;
            pnlActions.Controls.Add(lblChonPhieu);
            pnlActions.Controls.Add(btnPheDuyet);
            pnlActions.Controls.Add(btnTuChoi);
            pnlActions.Location = new Point(0, 700);
            pnlActions.Name = "pnlActions";
            pnlActions.Dock = DockStyle.Bottom;
            pnlActions.Height = 60;
            pnlActions.Visible = false;

            // lblChonPhieu
            lblChonPhieu.AutoSize = false;
            lblChonPhieu.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblChonPhieu.ForeColor = Color.FromArgb(60, 60, 60);
            lblChonPhieu.AutoEllipsis = true;
            lblChonPhieu.Location = new Point(12, 18);
            lblChonPhieu.Name = "lblChonPhieu";
            lblChonPhieu.Size = new Size(330, 20);
            lblChonPhieu.Text = "Phiếu đã chọn:";

            // btnPheDuyet
            btnPheDuyet.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnPheDuyet.BackColor = Color.FromArgb(27, 94, 60);
            btnPheDuyet.FlatStyle = FlatStyle.Flat;
            btnPheDuyet.FlatAppearance.BorderSize = 0;
            btnPheDuyet.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnPheDuyet.ForeColor = Color.White;
            btnPheDuyet.Location = new Point(1012, 12);
            btnPheDuyet.Name = "btnPheDuyet";
            btnPheDuyet.Size = new Size(56, 36);
            btnPheDuyet.Text = "✓";
            btnPheDuyet.Cursor = Cursors.Hand;
            btnPheDuyet.Click += BtnPheDuyet_Click;

            // btnTuChoi
            btnTuChoi.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnTuChoi.BackColor = Color.FromArgb(180, 30, 30);
            btnTuChoi.FlatStyle = FlatStyle.Flat;
            btnTuChoi.FlatAppearance.BorderSize = 0;
            btnTuChoi.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnTuChoi.ForeColor = Color.White;
            btnTuChoi.Location = new Point(1078, 12);
            btnTuChoi.Name = "btnTuChoi";
            btnTuChoi.Size = new Size(56, 36);
            btnTuChoi.Text = "X";
            btnTuChoi.Cursor = Cursors.Hand;
            btnTuChoi.Click += BtnTuChoi_Click;
        }

        #endregion

        private Label lblDanhSachPhieuMuon;
        private Panel pnlContainerQuaHan5;
        private Label lblTatCa128;
        private Label lblChoDuyet3;
        private Label lblQuaHan5;
        private Label lblDangMuon;
        private Panel pnlFilter;
        private DateTimePicker dtpTuNgay;
        private Label lblTuNgay;
        private ComboBox cmbDangMuon;
        private TextBox txtTatCa128;
        private DateTimePicker dtpDenNgay;
        private Label lblDenNgay;
        private Button btnLoc;
        private Button btnReset;
        private DataGridView dgvData;
        private DataGridViewTextBoxColumn colTicketID;
        private DataGridViewTextBoxColumn colSoPhieu;
        private DataGridViewTextBoxColumn colNgayLap;
        private DataGridViewTextBoxColumn ColNguoimuon;
        private DataGridViewTextBoxColumn colPhong;
        private DataGridViewTextBoxColumn colSoluong;
        private DataGridViewTextBoxColumn colNgayMuon;
        private DataGridViewTextBoxColumn colHanTra;
        private DataGridViewTextBoxColumn colNgaytra;
        private DataGridViewTextBoxColumn colTrangThai;
        private DataGridViewTextBoxColumn colThaoTac;
        private Panel pnlActions;
        private Button btnPheDuyet;
        private Button btnTuChoi;
        private Label lblChonPhieu;
        private Panel pnlChiTietPhieu;
        private Label lblChiTietTitle;
        private Panel pnlChiTietLine;
        private Label lblCTSoPhieuTitle;
        private Label lblCTSoPhieu;
        private Label lblCTNguoiMuonTitle;
        private Label lblCTNguoiMuon;
        private Label lblCTPhongTitle;
        private Label lblCTPhong;
        private Label lblCTMucDichTitle;
        private Label lblCTMucDich;
        private Label lblCTNgayMuonTitle;
        private Label lblCTNgayMuon;
        private Label lblCTHanTraTitle;
        private Label lblCTHanTra;
        private Label lblCTNguoiDuyetTitle;
        private Label lblCTNguoiDuyet;
        private Label lblCTTrangThaiTitle;
        private Label lblCTTrangThai;
        private DataGridView dgvThongTinPhieu;
        private DataGridViewTextBoxColumn colInfoField1;
        private DataGridViewTextBoxColumn colInfoValue1;
        private DataGridViewTextBoxColumn colInfoField2;
        private DataGridViewTextBoxColumn colInfoValue2;
        private DataGridView dgvChiTietThietBi;
        private DataGridViewTextBoxColumn colCTMaTB;
        private DataGridViewTextBoxColumn colCTTenThietBi;
        private DataGridViewTextBoxColumn colCTSoLuong;
        private DataGridViewTextBoxColumn colCTTinhTrang;
    }
}






