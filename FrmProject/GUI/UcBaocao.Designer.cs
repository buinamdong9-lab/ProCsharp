namespace FrmProject.GUI
{
    partial class UcBaocao
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
            lblBaoCaoThongKe = new Label();
            pnlContainerInBaoCao = new Panel();
            btnXuatExcel = new Button();
            btnXemBaoCao = new Button();
            dtpDenNgay = new DateTimePicker();
            lblDenNgay = new Label();
            dtpTuNgay = new DateTimePicker();
            lblTuNgay = new Label();
            cmbLoaiBaoCao = new ComboBox();
            lblLoaiBaoCao = new Label();
            pnlGridData3 = new Panel();
            dgvData3 = new DataGridView();
            colSoLuong = new DataGridViewTextBoxColumn();
            colColumn2 = new DataGridViewTextBoxColumn();
            dgvData = new DataGridView();
            lblThongKeMuonTra = new Label();
            pnlGridData2 = new Panel();
            dgvData2 = new DataGridView();
            colHang = new DataGridViewTextBoxColumn();
            colThietBi = new DataGridViewTextBoxColumn();
            colLuotMuon = new DataGridViewTextBoxColumn();
            colTiLe = new DataGridViewTextBoxColumn();
            lblThietBiSuDung = new Label();
            pnlGridData4 = new Panel();
            dgvData4 = new DataGridView();
            colSoPhieu = new DataGridViewTextBoxColumn();
            colNguoiMuon = new DataGridViewTextBoxColumn();
            colPhong = new DataGridViewTextBoxColumn();
            colNgayMuon = new DataGridViewTextBoxColumn();
            colHanTra = new DataGridViewTextBoxColumn();
            colSoNgayQua = new DataGridViewTextBoxColumn();
            colTinhTrang = new DataGridViewTextBoxColumn();
            lblDanhSachThietBi = new Label();
            pnlContainerInBaoCao.SuspendLayout();
            pnlGridData3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvData3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvData).BeginInit();
            pnlGridData2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvData2).BeginInit();
            pnlGridData4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvData4).BeginInit();
            SuspendLayout();
            // 
            // lblBaoCaoThongKe
            // 
            lblBaoCaoThongKe.AutoSize = true;
            lblBaoCaoThongKe.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblBaoCaoThongKe.ForeColor = Color.FromArgb(27, 94, 60);
            lblBaoCaoThongKe.Location = new Point(10, 7);
            lblBaoCaoThongKe.Name = "lblBaoCaoThongKe";
            lblBaoCaoThongKe.Size = new Size(212, 25);
            lblBaoCaoThongKe.TabIndex = 0;
            lblBaoCaoThongKe.Text = "📊  Báo Cáo & Thống Kê";
            // 
            // pnlContainerInBaoCao
            // 
            pnlContainerInBaoCao.Controls.Add(btnXuatExcel);
            pnlContainerInBaoCao.Controls.Add(btnXemBaoCao);
            pnlContainerInBaoCao.Controls.Add(dtpDenNgay);
            pnlContainerInBaoCao.Controls.Add(lblDenNgay);
            pnlContainerInBaoCao.Controls.Add(dtpTuNgay);
            pnlContainerInBaoCao.Controls.Add(lblTuNgay);
            pnlContainerInBaoCao.Controls.Add(cmbLoaiBaoCao);
            pnlContainerInBaoCao.Controls.Add(lblLoaiBaoCao);
            pnlContainerInBaoCao.Location = new Point(10, 41);
            pnlContainerInBaoCao.Margin = new Padding(3, 2, 3, 2);
            pnlContainerInBaoCao.Name = "pnlContainerInBaoCao";
            pnlContainerInBaoCao.Size = new Size(1101, 93);
            pnlContainerInBaoCao.TabIndex = 1;
            // 
            // btnXuatExcel
            // 
            btnXuatExcel.BackColor = Color.White;
            btnXuatExcel.Cursor = Cursors.Hand;
            btnXuatExcel.FlatAppearance.BorderSize = 0;
            btnXuatExcel.FlatStyle = FlatStyle.Flat;
            btnXuatExcel.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnXuatExcel.ForeColor = Color.ForestGreen;
            btnXuatExcel.Location = new Point(175, 53);
            btnXuatExcel.Margin = new Padding(3, 2, 3, 2);
            btnXuatExcel.Name = "btnXuatExcel";
            btnXuatExcel.Size = new Size(116, 32);
            btnXuatExcel.TabIndex = 6;
            btnXuatExcel.Text = "📅 Xuất Excel";
            btnXuatExcel.UseVisualStyleBackColor = false;
            // 
            // btnXemBaoCao
            // 
            btnXemBaoCao.BackColor = Color.ForestGreen;
            btnXemBaoCao.Cursor = Cursors.Hand;
            btnXemBaoCao.FlatAppearance.BorderSize = 0;
            btnXemBaoCao.FlatStyle = FlatStyle.Flat;
            btnXemBaoCao.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnXemBaoCao.ForeColor = Color.White;
            btnXemBaoCao.Location = new Point(15, 53);
            btnXemBaoCao.Margin = new Padding(3, 2, 3, 2);
            btnXemBaoCao.Name = "btnXemBaoCao";
            btnXemBaoCao.Size = new Size(137, 32);
            btnXemBaoCao.TabIndex = 2;
            btnXemBaoCao.Text = "📊 Xem báo cáo";
            btnXemBaoCao.UseVisualStyleBackColor = false;
            // 
            // dtpDenNgay
            // 
            dtpDenNgay.Format = DateTimePickerFormat.Short;
            dtpDenNgay.Location = new Point(679, 13);
            dtpDenNgay.Margin = new Padding(3, 2, 3, 2);
            dtpDenNgay.Name = "dtpDenNgay";
            dtpDenNgay.Size = new Size(114, 23);
            dtpDenNgay.TabIndex = 5;
            // 
            // lblDenNgay
            // 
            lblDenNgay.AutoSize = true;
            lblDenNgay.Location = new Point(601, 16);
            lblDenNgay.Name = "lblDenNgay";
            lblDenNgay.Size = new Size(57, 15);
            lblDenNgay.TabIndex = 4;
            lblDenNgay.Text = "Đến ngày";
            // 
            // dtpTuNgay
            // 
            dtpTuNgay.Format = DateTimePickerFormat.Short;
            dtpTuNgay.Location = new Point(468, 15);
            dtpTuNgay.Margin = new Padding(3, 2, 3, 2);
            dtpTuNgay.Name = "dtpTuNgay";
            dtpTuNgay.Size = new Size(114, 23);
            dtpTuNgay.TabIndex = 3;
            // 
            // lblTuNgay
            // 
            lblTuNgay.AutoSize = true;
            lblTuNgay.Location = new Point(409, 16);
            lblTuNgay.Name = "lblTuNgay";
            lblTuNgay.Size = new Size(50, 15);
            lblTuNgay.TabIndex = 2;
            lblTuNgay.Text = "Từ ngày";
            // 
            // cmbLoaiBaoCao
            // 
            cmbLoaiBaoCao.FormattingEnabled = true;
            cmbLoaiBaoCao.Location = new Point(108, 14);
            cmbLoaiBaoCao.Margin = new Padding(3, 2, 3, 2);
            cmbLoaiBaoCao.Name = "cmbLoaiBaoCao";
            cmbLoaiBaoCao.Size = new Size(213, 23);
            cmbLoaiBaoCao.TabIndex = 1;
            // 
            // lblLoaiBaoCao
            // 
            lblLoaiBaoCao.AutoSize = true;
            lblLoaiBaoCao.Location = new Point(15, 16);
            lblLoaiBaoCao.Name = "lblLoaiBaoCao";
            lblLoaiBaoCao.Size = new Size(77, 15);
            lblLoaiBaoCao.TabIndex = 0;
            lblLoaiBaoCao.Text = "Loại báo cáo:";
            // 
            // pnlGridData3
            // 
            pnlGridData3.Controls.Add(dgvData3);
            pnlGridData3.Controls.Add(dgvData);
            pnlGridData3.Controls.Add(lblThongKeMuonTra);
            pnlGridData3.Location = new Point(10, 139);
            pnlGridData3.Margin = new Padding(3, 2, 3, 2);
            pnlGridData3.Name = "pnlGridData3";
            pnlGridData3.Size = new Size(463, 334);
            pnlGridData3.TabIndex = 2;
            // 
            // dgvData3
            // 
            dgvData3.AllowUserToAddRows = false;
            dgvData3.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvData3.Columns.AddRange(new DataGridViewColumn[] { colSoLuong, colColumn2 });
            dgvData3.Location = new Point(3, 367);
            dgvData3.Margin = new Padding(3, 2, 3, 2);
            dgvData3.Name = "dgvData3";
            dgvData3.ReadOnly = true;
            dgvData3.RowHeadersWidth = 51;
            dgvData3.Size = new Size(275, 62);
            dgvData3.TabIndex = 1;
            // 
            // colSoLuong
            // 
            colSoLuong.HeaderText = "Số lượng";
            colSoLuong.MinimumWidth = 6;
            colSoLuong.Name = "colSoLuong";
            colSoLuong.ReadOnly = true;
            colSoLuong.Width = 125;
            // 
            // colColumn2
            // 
            colColumn2.HeaderText = "colColumn2";
            colColumn2.MinimumWidth = 6;
            colColumn2.Name = "colColumn2";
            colColumn2.ReadOnly = true;
            colColumn2.Width = 125;
            // 
            // dgvData
            // 
            dgvData.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvData.Location = new Point(3, 40);
            dgvData.Margin = new Padding(3, 2, 3, 2);
            dgvData.Name = "dgvData";
            dgvData.RowHeadersWidth = 51;
            dgvData.Size = new Size(458, 291);
            dgvData.TabIndex = 1;
            // 
            // lblThongKeMuonTra
            // 
            lblThongKeMuonTra.AutoSize = true;
            lblThongKeMuonTra.BackColor = SystemColors.Control;
            lblThongKeMuonTra.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblThongKeMuonTra.ForeColor = Color.FromArgb(27, 94, 60);
            lblThongKeMuonTra.Location = new Point(3, 12);
            lblThongKeMuonTra.Name = "lblThongKeMuonTra";
            lblThongKeMuonTra.Size = new Size(287, 19);
            lblThongKeMuonTra.TabIndex = 0;
            lblThongKeMuonTra.Text = "📊  Thống kê mượn trả theo tháng (2026)";
            // 
            // pnlGridData2
            // 
            pnlGridData2.Controls.Add(dgvData2);
            pnlGridData2.Controls.Add(lblThietBiSuDung);
            pnlGridData2.Location = new Point(479, 139);
            pnlGridData2.Margin = new Padding(3, 2, 3, 2);
            pnlGridData2.Name = "pnlGridData2";
            pnlGridData2.Size = new Size(634, 334);
            pnlGridData2.TabIndex = 3;
            // 
            // dgvData2
            // 
            dgvData2.AllowUserToAddRows = false;
            dgvData2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvData2.Columns.AddRange(new DataGridViewColumn[] { colHang, colThietBi, colLuotMuon, colTiLe });
            dgvData2.Location = new Point(3, 40);
            dgvData2.Margin = new Padding(3, 2, 3, 2);
            dgvData2.Name = "dgvData2";
            dgvData2.ReadOnly = true;
            dgvData2.RowHeadersVisible = false;
            dgvData2.RowHeadersWidth = 51;
            dgvData2.Size = new Size(629, 291);
            dgvData2.TabIndex = 1;
            // 
            // colHang
            // 
            colHang.HeaderText = "Hạng";
            colHang.MinimumWidth = 6;
            colHang.Name = "colHang";
            colHang.ReadOnly = true;
            colHang.Width = 125;
            // 
            // colThietBi
            // 
            colThietBi.HeaderText = "Thiết bị";
            colThietBi.MinimumWidth = 6;
            colThietBi.Name = "colThietBi";
            colThietBi.ReadOnly = true;
            colThietBi.Width = 250;
            // 
            // colLuotMuon
            // 
            colLuotMuon.HeaderText = "Lượt mượn";
            colLuotMuon.MinimumWidth = 6;
            colLuotMuon.Name = "colLuotMuon";
            colLuotMuon.ReadOnly = true;
            colLuotMuon.Width = 125;
            // 
            // colTiLe
            // 
            colTiLe.HeaderText = "Tỉ lệ";
            colTiLe.MinimumWidth = 6;
            colTiLe.Name = "colTiLe";
            colTiLe.ReadOnly = true;
            colTiLe.Width = 125;
            // 
            // lblThietBiSuDung
            // 
            lblThietBiSuDung.AutoSize = true;
            lblThietBiSuDung.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblThietBiSuDung.ForeColor = Color.Coral;
            lblThietBiSuDung.Location = new Point(3, 12);
            lblThietBiSuDung.Name = "lblThietBiSuDung";
            lblThietBiSuDung.Size = new Size(217, 19);
            lblThietBiSuDung.TabIndex = 0;
            lblThietBiSuDung.Text = "🔥  Thiết bị sử dụng nhiều nhất";
            // 
            // pnlGridData4
            // 
            pnlGridData4.Controls.Add(dgvData4);
            pnlGridData4.Controls.Add(lblDanhSachThietBi);
            pnlGridData4.Location = new Point(10, 477);
            pnlGridData4.Margin = new Padding(3, 2, 3, 2);
            pnlGridData4.Name = "pnlGridData4";
            pnlGridData4.Size = new Size(1103, 252);
            pnlGridData4.TabIndex = 4;
            // 
            // dgvData4
            // 
            dgvData4.AllowUserToAddRows = false;
            dataGridViewCellStyle1.BackColor = Color.White;
            dataGridViewCellStyle1.ForeColor = Color.Red;
            dgvData4.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(255, 235, 235);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(180, 40, 40);
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvData4.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvData4.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvData4.Columns.AddRange(new DataGridViewColumn[] { colSoPhieu, colNguoiMuon, colPhong, colNgayMuon, colHanTra, colSoNgayQua, colTinhTrang });
            dgvData4.Location = new Point(3, 31);
            dgvData4.Margin = new Padding(3, 2, 3, 2);
            dgvData4.Name = "dgvData4";
            dgvData4.ReadOnly = true;
            dgvData4.RowHeadersVisible = false;
            dgvData4.RowHeadersWidth = 51;
            dgvData4.Size = new Size(1097, 217);
            dgvData4.TabIndex = 1;
            // 
            // colSoPhieu
            // 
            colSoPhieu.HeaderText = "Số phiếu";
            colSoPhieu.MinimumWidth = 6;
            colSoPhieu.Name = "colSoPhieu";
            colSoPhieu.ReadOnly = true;
            // 
            // colNguoiMuon
            // 
            colNguoiMuon.HeaderText = "Người mượn";
            colNguoiMuon.MinimumWidth = 6;
            colNguoiMuon.Name = "colNguoiMuon";
            colNguoiMuon.ReadOnly = true;
            colNguoiMuon.Width = 150;
            // 
            // colPhong
            // 
            colPhong.HeaderText = "Phòng";
            colPhong.MinimumWidth = 6;
            colPhong.Name = "colPhong";
            colPhong.ReadOnly = true;
            colPhong.Width = 80;
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
            // colSoNgayQua
            // 
            colSoNgayQua.HeaderText = "Số ngày quá hạn";
            colSoNgayQua.MinimumWidth = 6;
            colSoNgayQua.Name = "colSoNgayQua";
            colSoNgayQua.ReadOnly = true;
            colSoNgayQua.Width = 136;
            // 
            // colTinhTrang
            // 
            colTinhTrang.HeaderText = "Tình trạng";
            colTinhTrang.MinimumWidth = 6;
            colTinhTrang.Name = "colTinhTrang";
            colTinhTrang.ReadOnly = true;
            colTinhTrang.Width = 421;
            // 
            // lblDanhSachThietBi
            // 
            lblDanhSachThietBi.AutoSize = true;
            lblDanhSachThietBi.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDanhSachThietBi.ForeColor = Color.FromArgb(220, 50, 50);
            lblDanhSachThietBi.Location = new Point(3, 9);
            lblDanhSachThietBi.Name = "lblDanhSachThietBi";
            lblDanhSachThietBi.Size = new Size(288, 19);
            lblDanhSachThietBi.TabIndex = 0;
            lblDanhSachThietBi.Text = "📋  Danh sách thiết bị quá hạn / cần chú ý";
            // 
            // UcBaocao
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            AutoScrollMinSize = new Size(0, 790);
            ClientSize = new Size(1246, 749);
            Controls.Add(pnlGridData4);
            Controls.Add(pnlGridData2);
            Controls.Add(pnlGridData3);
            Controls.Add(pnlContainerInBaoCao);
            Controls.Add(lblBaoCaoThongKe);
            Margin = new Padding(3, 2, 3, 2);
            Name = "UcBaocao";
            Text = "Báo Cáo Thống Kê";
            pnlContainerInBaoCao.ResumeLayout(false);
            pnlContainerInBaoCao.PerformLayout();
            pnlGridData3.ResumeLayout(false);
            pnlGridData3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvData3).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvData).EndInit();
            pnlGridData2.ResumeLayout(false);
            pnlGridData2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvData2).EndInit();
            pnlGridData4.ResumeLayout(false);
            pnlGridData4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvData4).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblBaoCaoThongKe;
        private Panel pnlContainerInBaoCao;
        private Button btnXuatExcel;
        private Button btnXemBaoCao;
        private DateTimePicker dtpDenNgay;
        private Label lblDenNgay;
        private DateTimePicker dtpTuNgay;
        private Label lblTuNgay;
        private ComboBox cmbLoaiBaoCao;
        private Label lblLoaiBaoCao;
        private Panel pnlGridData3;
        private Label lblThongKeMuonTra;
        private Panel pnlGridData2;
        private DataGridView dgvData3;
        private DataGridView dgvData;
        private DataGridView dgvData2;
        private Label lblThietBiSuDung;
        private Panel pnlGridData4;
        private Label lblDanhSachThietBi;
        private DataGridViewTextBoxColumn colSoLuong;
        private DataGridViewTextBoxColumn colColumn2;
        private DataGridView dgvData4;
        private DataGridViewTextBoxColumn colSoPhieu;
        private DataGridViewTextBoxColumn colNguoiMuon;
        private DataGridViewTextBoxColumn colPhong;
        private DataGridViewTextBoxColumn colNgayMuon;
        private DataGridViewTextBoxColumn colHanTra;
        private DataGridViewTextBoxColumn colSoNgayQua;
        private DataGridViewTextBoxColumn colTinhTrang;
        private DataGridViewTextBoxColumn colHang;
        private DataGridViewTextBoxColumn colThietBi;
        private DataGridViewTextBoxColumn colLuotMuon;
        private DataGridViewTextBoxColumn colTiLe;
    }
}






