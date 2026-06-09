namespace FrmProject.GUI
{
    partial class UcThietbi
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
            panelMain = new Panel();
            dgvDevices = new DataGridView();
            labelTopTitle = new Label();
            grpThongtin = new GroupBox();
            nudSoLuong = new NumericUpDown();
            lblSoLuong = new Label();
            txtGhiChu = new TextBox();
            txtTinhTrang = new TextBox();
            txtLoaiThietBi = new TextBox();
            pnlContainerQuanLyThietBi = new Panel();
            txtQuanLyThietBi = new TextBox();
            comboBox2 = new ComboBox();
            comboBox1 = new ComboBox();
            lblGhiChu = new Label();
            lblTinhTrang = new Label();
            lblLoaiThietBi = new Label();
            pnlActions = new Panel();
            btnQuanLyCaThe = new Button();
            btnLamMoi = new Button();
            btnLuu = new Button();
            btnXoa = new Button();
            btnSua = new Button();
            btnThem = new Button();
            txtViTri = new TextBox();
            txtTenThietBi = new TextBox();
            txtMaThietBi = new TextBox();
            lblViTri = new Label();
            lblTenThietBi = new Label();
            lblMaThietBi = new Label();
            pnlPaging = new Panel();
            btnFirst = new Button();
            btnPrev = new Button();
            lblPageInfo = new Label();
            btnNext = new Button();
            btnLast = new Button();
            colMaThietBi = new DataGridViewTextBoxColumn();
            colLoaiThietBi = new DataGridViewTextBoxColumn();
            colVT = new DataGridViewTextBoxColumn();
            ColTinhtrang = new DataGridViewTextBoxColumn();
            ColSL = new DataGridViewTextBoxColumn();
            ColGhichu = new DataGridViewTextBoxColumn();
            panelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDevices).BeginInit();
            grpThongtin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudSoLuong).BeginInit();
            pnlContainerQuanLyThietBi.SuspendLayout();
            pnlActions.SuspendLayout();
            pnlPaging.SuspendLayout();
            SuspendLayout();
            // 
            // panelMain
            // 
            panelMain.Controls.Add(dgvDevices);
            panelMain.Controls.Add(labelTopTitle);
            panelMain.Controls.Add(grpThongtin);
            panelMain.Controls.Add(pnlPaging);
            panelMain.Dock = DockStyle.Fill;
            panelMain.Location = new Point(0, 0);
            panelMain.Name = "panelMain";
            panelMain.Size = new Size(1290, 731);
            panelMain.TabIndex = 0;
            // 
            // dgvDevices
            // 
            dgvDevices.AllowUserToAddRows = false;
            dgvDevices.AllowUserToResizeColumns = false;
            dgvDevices.BackgroundColor = Color.White;
            dgvDevices.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridViewCellStyle1.SelectionBackColor = Color.Honeydew;
            dataGridViewCellStyle1.SelectionForeColor = Color.FromArgb(44, 123, 229);
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvDevices.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvDevices.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvDevices.DefaultCellStyle = dataGridViewCellStyle2;
            dgvDevices.GridColor = Color.White;
            dgvDevices.Location = new Point(11, 433);
            dgvDevices.Name = "dgvDevices";
            dgvDevices.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = SystemColors.Control;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            dgvDevices.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            dgvDevices.RowHeadersVisible = false;
            dgvDevices.RowHeadersWidth = 51;
            dgvDevices.Size = new Size(1240, 238);
            dgvDevices.TabIndex = 2;
            // 
            // labelTopTitle
            // 
            labelTopTitle.BackColor = Color.FromArgb(27, 94, 60);
            labelTopTitle.Dock = DockStyle.Top;
            labelTopTitle.Font = new Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelTopTitle.ForeColor = Color.White;
            labelTopTitle.Location = new Point(0, 0);
            labelTopTitle.Name = "labelTopTitle";
            labelTopTitle.Size = new Size(1290, 69);
            labelTopTitle.TabIndex = 1;
            labelTopTitle.Text = "📦  Quản Lý Thiết Bị";
            labelTopTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // grpThongtin
            // 
            grpThongtin.Controls.Add(nudSoLuong);
            grpThongtin.Controls.Add(lblSoLuong);
            grpThongtin.Controls.Add(txtGhiChu);
            grpThongtin.Controls.Add(txtTinhTrang);
            grpThongtin.Controls.Add(txtLoaiThietBi);
            grpThongtin.Controls.Add(pnlContainerQuanLyThietBi);
            grpThongtin.Controls.Add(lblGhiChu);
            grpThongtin.Controls.Add(lblTinhTrang);
            grpThongtin.Controls.Add(lblLoaiThietBi);
            grpThongtin.Controls.Add(pnlActions);
            grpThongtin.Controls.Add(txtViTri);
            grpThongtin.Controls.Add(txtTenThietBi);
            grpThongtin.Controls.Add(txtMaThietBi);
            grpThongtin.Controls.Add(lblViTri);
            grpThongtin.Controls.Add(lblTenThietBi);
            grpThongtin.Controls.Add(lblMaThietBi);
            grpThongtin.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            grpThongtin.ForeColor = Color.FromArgb(26, 92, 56);
            grpThongtin.Location = new Point(11, 72);
            grpThongtin.Name = "grpThongtin";
            grpThongtin.Size = new Size(1240, 385);
            grpThongtin.TabIndex = 0;
            grpThongtin.TabStop = false;
            grpThongtin.Text = "🖹  Thông tin thiết bị";
            // 
            // nudSoLuong
            // 
            nudSoLuong.Location = new Point(205, 179);
            nudSoLuong.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            nudSoLuong.Name = "nudSoLuong";
            nudSoLuong.Size = new Size(55, 31);
            nudSoLuong.TabIndex = 19;
            // 
            // lblSoLuong
            // 
            lblSoLuong.AutoSize = true;
            lblSoLuong.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblSoLuong.Location = new Point(56, 183);
            lblSoLuong.Name = "lblSoLuong";
            lblSoLuong.Size = new Size(82, 23);
            lblSoLuong.TabIndex = 18;
            lblSoLuong.Text = "Số lượng:";
            // 
            // txtGhiChu
            // 
            txtGhiChu.Location = new Point(800, 126);
            txtGhiChu.Name = "txtGhiChu";
            txtGhiChu.Size = new Size(406, 31);
            txtGhiChu.TabIndex = 17;
            // 
            // txtTinhTrang
            // 
            txtTinhTrang.Location = new Point(800, 71);
            txtTinhTrang.Name = "txtTinhTrang";
            txtTinhTrang.Size = new Size(406, 31);
            txtTinhTrang.TabIndex = 16;
            // 
            // txtLoaiThietBi
            // 
            txtLoaiThietBi.Location = new Point(800, 34);
            txtLoaiThietBi.Name = "txtLoaiThietBi";
            txtLoaiThietBi.Size = new Size(406, 31);
            txtLoaiThietBi.TabIndex = 15;
            // 
            // pnlContainerQuanLyThietBi
            // 
            pnlContainerQuanLyThietBi.Controls.Add(txtQuanLyThietBi);
            pnlContainerQuanLyThietBi.Controls.Add(comboBox2);
            pnlContainerQuanLyThietBi.Controls.Add(comboBox1);
            pnlContainerQuanLyThietBi.Location = new Point(41, 295);
            pnlContainerQuanLyThietBi.Name = "pnlContainerQuanLyThietBi";
            pnlContainerQuanLyThietBi.Size = new Size(1167, 60);
            pnlContainerQuanLyThietBi.TabIndex = 14;
            // 
            // txtQuanLyThietBi
            // 
            txtQuanLyThietBi.Location = new Point(10, 16);
            txtQuanLyThietBi.Name = "txtQuanLyThietBi";
            txtQuanLyThietBi.PlaceholderText = "🔍 Tìm kiếm...";
            txtQuanLyThietBi.Size = new Size(243, 31);
            txtQuanLyThietBi.TabIndex = 3;
            // 
            // comboBox2
            // 
            comboBox2.FormattingEnabled = true;
            comboBox2.Location = new Point(510, 16);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(197, 33);
            comboBox2.TabIndex = 2;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(283, 16);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(201, 33);
            comboBox1.TabIndex = 1;
            // 
            // lblGhiChu
            // 
            lblGhiChu.AutoSize = true;
            lblGhiChu.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblGhiChu.Location = new Point(705, 131);
            lblGhiChu.Name = "lblGhiChu";
            lblGhiChu.Size = new Size(73, 23);
            lblGhiChu.TabIndex = 13;
            lblGhiChu.Text = "Ghi chú:";
            // 
            // lblTinhTrang
            // 
            lblTinhTrang.AutoSize = true;
            lblTinhTrang.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTinhTrang.Location = new Point(685, 79);
            lblTinhTrang.Name = "lblTinhTrang";
            lblTinhTrang.Size = new Size(93, 23);
            lblTinhTrang.TabIndex = 12;
            lblTinhTrang.Text = "Tình trạng:";
            // 
            // lblLoaiThietBi
            // 
            lblLoaiThietBi.AutoSize = true;
            lblLoaiThietBi.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblLoaiThietBi.Location = new Point(674, 34);
            lblLoaiThietBi.Name = "lblLoaiThietBi";
            lblLoaiThietBi.Size = new Size(104, 23);
            lblLoaiThietBi.TabIndex = 11;
            lblLoaiThietBi.Text = "Loại thiết bị:";
            // 
            // pnlActions
            // 
            pnlActions.Controls.Add(btnQuanLyCaThe);
            pnlActions.Controls.Add(btnLamMoi);
            pnlActions.Controls.Add(btnLuu);
            pnlActions.Controls.Add(btnXoa);
            pnlActions.Controls.Add(btnSua);
            pnlActions.Controls.Add(btnThem);
            pnlActions.Location = new Point(41, 222);
            pnlActions.Name = "pnlActions";
            pnlActions.Size = new Size(1167, 67);
            pnlActions.TabIndex = 10;
            // 
            // btnQuanLyCaThe
            // 
            btnQuanLyCaThe.BackColor = Color.FromArgb(108, 92, 231);
            btnQuanLyCaThe.Cursor = Cursors.Hand;
            btnQuanLyCaThe.FlatAppearance.BorderSize = 0;
            btnQuanLyCaThe.FlatStyle = FlatStyle.Flat;
            btnQuanLyCaThe.ForeColor = Color.White;
            btnQuanLyCaThe.Location = new Point(651, 11);
            btnQuanLyCaThe.Name = "btnQuanLyCaThe";
            btnQuanLyCaThe.Size = new Size(137, 47);
            btnQuanLyCaThe.TabIndex = 10;
            btnQuanLyCaThe.Text = "🏷 Mã cá thể";
            btnQuanLyCaThe.UseVisualStyleBackColor = false;
            btnQuanLyCaThe.Click += BtnQuanLyCaThe_Click;
            // 
            // btnLamMoi
            // 
            btnLamMoi.BackColor = Color.FromArgb(108, 117, 125);
            btnLamMoi.Cursor = Cursors.Hand;
            btnLamMoi.FlatAppearance.BorderSize = 0;
            btnLamMoi.FlatStyle = FlatStyle.Flat;
            btnLamMoi.ForeColor = Color.White;
            btnLamMoi.Location = new Point(509, 11);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new Size(121, 47);
            btnLamMoi.TabIndex = 9;
            btnLamMoi.Text = "↺ Làm mới";
            btnLamMoi.UseVisualStyleBackColor = false;
            // 
            // btnLuu
            // 
            btnLuu.BackColor = Color.FromArgb(0, 123, 255);
            btnLuu.Cursor = Cursors.Hand;
            btnLuu.FlatAppearance.BorderSize = 0;
            btnLuu.FlatStyle = FlatStyle.Flat;
            btnLuu.ForeColor = Color.White;
            btnLuu.Location = new Point(382, 11);
            btnLuu.Name = "btnLuu";
            btnLuu.Size = new Size(94, 47);
            btnLuu.TabIndex = 8;
            btnLuu.Text = "💾 Lưu";
            btnLuu.UseVisualStyleBackColor = false;
            // 
            // btnXoa
            // 
            btnXoa.BackColor = Color.FromArgb(220, 53, 69);
            btnXoa.Cursor = Cursors.Hand;
            btnXoa.FlatAppearance.BorderSize = 0;
            btnXoa.FlatStyle = FlatStyle.Flat;
            btnXoa.ForeColor = Color.White;
            btnXoa.Location = new Point(270, 11);
            btnXoa.Name = "btnXoa";
            btnXoa.Size = new Size(87, 47);
            btnXoa.TabIndex = 7;
            btnXoa.Text = "🗑 Xóa";
            btnXoa.UseVisualStyleBackColor = false;
            // 
            // btnSua
            // 
            btnSua.BackColor = Color.FromArgb(253, 126, 20);
            btnSua.Cursor = Cursors.Hand;
            btnSua.FlatAppearance.BorderSize = 0;
            btnSua.FlatStyle = FlatStyle.Flat;
            btnSua.ForeColor = Color.White;
            btnSua.Location = new Point(152, 11);
            btnSua.Name = "btnSua";
            btnSua.Size = new Size(94, 47);
            btnSua.TabIndex = 6;
            btnSua.Text = "✏ Sửa";
            btnSua.UseVisualStyleBackColor = false;
            // 
            // btnThem
            // 
            btnThem.BackColor = Color.FromArgb(40, 167, 69);
            btnThem.Cursor = Cursors.Hand;
            btnThem.FlatAppearance.BorderSize = 0;
            btnThem.FlatStyle = FlatStyle.Flat;
            btnThem.ForeColor = Color.White;
            btnThem.Location = new Point(17, 11);
            btnThem.Name = "btnThem";
            btnThem.Size = new Size(114, 47);
            btnThem.TabIndex = 5;
            btnThem.Text = "➕ Thêm";
            btnThem.UseVisualStyleBackColor = false;
            // 
            // txtViTri
            // 
            txtViTri.Location = new Point(144, 131);
            txtViTri.Name = "txtViTri";
            txtViTri.Size = new Size(373, 31);
            txtViTri.TabIndex = 6;
            // 
            // txtTenThietBi
            // 
            txtTenThietBi.Location = new Point(144, 85);
            txtTenThietBi.Name = "txtTenThietBi";
            txtTenThietBi.Size = new Size(373, 31);
            txtTenThietBi.TabIndex = 5;
            // 
            // txtMaThietBi
            // 
            txtMaThietBi.Location = new Point(144, 39);
            txtMaThietBi.Name = "txtMaThietBi";
            txtMaThietBi.Size = new Size(373, 31);
            txtMaThietBi.TabIndex = 4;
            // 
            // lblViTri
            // 
            lblViTri.AutoSize = true;
            lblViTri.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblViTri.Location = new Point(88, 135);
            lblViTri.Name = "lblViTri";
            lblViTri.Size = new Size(50, 23);
            lblViTri.TabIndex = 2;
            lblViTri.Text = "Vị trí:";
            // 
            // lblTenThietBi
            // 
            lblTenThietBi.AutoSize = true;
            lblTenThietBi.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTenThietBi.Location = new Point(39, 85);
            lblTenThietBi.Name = "lblTenThietBi";
            lblTenThietBi.Size = new Size(99, 23);
            lblTenThietBi.TabIndex = 1;
            lblTenThietBi.Text = "Tên thiết bị:";
            // 
            // lblMaThietBi
            // 
            lblMaThietBi.AutoSize = true;
            lblMaThietBi.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblMaThietBi.Location = new Point(41, 39);
            lblMaThietBi.Name = "lblMaThietBi";
            lblMaThietBi.Size = new Size(97, 23);
            lblMaThietBi.TabIndex = 0;
            lblMaThietBi.Text = "Mã thiết bị:";
            // 
            // pnlPaging
            // 
            pnlPaging.BackColor = Color.FromArgb(243, 244, 246);
            pnlPaging.Controls.Add(btnFirst);
            pnlPaging.Controls.Add(btnPrev);
            pnlPaging.Controls.Add(lblPageInfo);
            pnlPaging.Controls.Add(btnNext);
            pnlPaging.Controls.Add(btnLast);
            pnlPaging.Location = new Point(11, 680);
            pnlPaging.Name = "pnlPaging";
            pnlPaging.Size = new Size(1240, 44);
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
            // colMaThietBi
            // 
            colMaThietBi.HeaderText = "Mã thiết bị";
            colMaThietBi.MinimumWidth = 6;
            colMaThietBi.Name = "colMaThietBi";
            colMaThietBi.ReadOnly = true;
            colMaThietBi.Width = 125;
            // 
            // colLoaiThietBi
            // 
            colLoaiThietBi.HeaderText = "Loại thiết bị";
            colLoaiThietBi.MinimumWidth = 6;
            colLoaiThietBi.Name = "colLoaiThietBi";
            colLoaiThietBi.ReadOnly = true;
            colLoaiThietBi.Width = 135;
            // 
            // colVT
            // 
            colVT.HeaderText = "Vị trí";
            colVT.MinimumWidth = 6;
            colVT.Name = "colVT";
            colVT.ReadOnly = true;
            colVT.Width = 125;
            // 
            // ColTinhtrang
            // 
            ColTinhtrang.HeaderText = "Tình trạng";
            ColTinhtrang.MinimumWidth = 6;
            ColTinhtrang.Name = "ColTinhtrang";
            ColTinhtrang.ReadOnly = true;
            ColTinhtrang.Width = 150;
            // 
            // ColSL
            // 
            ColSL.HeaderText = "Số lượng";
            ColSL.MinimumWidth = 6;
            ColSL.Name = "ColSL";
            ColSL.ReadOnly = true;
            ColSL.Width = 125;
            // 
            // ColGhichu
            // 
            ColGhichu.HeaderText = "Ghi chú";
            ColGhichu.MinimumWidth = 6;
            ColGhichu.Name = "ColGhichu";
            ColGhichu.ReadOnly = true;
            ColGhichu.Width = 200;
            // 
            // UcThietbi
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1290, 731);
            Controls.Add(panelMain);
            Name = "UcThietbi";
            Text = "Quản Lý Thiết Bị";
            panelMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvDevices).EndInit();
            grpThongtin.ResumeLayout(false);
            grpThongtin.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudSoLuong).EndInit();
            pnlContainerQuanLyThietBi.ResumeLayout(false);
            pnlContainerQuanLyThietBi.PerformLayout();
            pnlActions.ResumeLayout(false);
            pnlPaging.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panelMain;
        private GroupBox grpThongtin;
        private Label labelTopTitle;
        private TextBox txtViTri;
        private TextBox txtTenThietBi;
        private TextBox txtMaThietBi;
        private Label lblSoLuong;
        private Label lblViTri;
        private Label lblTenThietBi;
        private Label lblMaThietBi;
        private Label lblGhiChu;
        private Label lblTinhTrang;
        private Label lblLoaiThietBi;
        private Panel pnlActions;
        private TextBox txtGhiChu;
        private TextBox txtTinhTrang;
        private TextBox txtLoaiThietBi;
        private Panel pnlContainerQuanLyThietBi;
        private ComboBox comboBox2;
        private ComboBox comboBox1;
        private Button btnThem;
        private Button btnSua;
        private Button btnXoa;
        private Button btnLuu;
        private Button btnLamMoi;
        private TextBox txtQuanLyThietBi;
        private DataGridView dgvDevices;
        private DataGridViewTextBoxColumn colMaThietBi;
        private DataGridViewTextBoxColumn colLoaiThietBi;
        private DataGridViewTextBoxColumn colVT;
        private DataGridViewTextBoxColumn ColTinhtrang;
        private DataGridViewTextBoxColumn ColSL;
        private DataGridViewTextBoxColumn ColGhichu;
        private Button btnQuanLyCaThe;
        private Panel pnlPaging;
        private Button btnFirst, btnPrev, btnNext, btnLast;
        private Label lblPageInfo;
        private NumericUpDown nudSoLuong;
        
    }
}






