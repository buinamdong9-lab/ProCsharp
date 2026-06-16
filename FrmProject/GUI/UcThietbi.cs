using System.Data;
using System.Diagnostics;

namespace FrmProject.GUI
{
    public partial class UcThietbi : UserControl
    {
        private IDeviceService DeviceService => AppServiceProvider.Get<IDeviceService>();

        private int _selectedDeviceID = -1;
        private int _selectedTotalQuantity;
        private int _selectedAvailableQuantity;
        private int _currentPage = 1;
        private const int PageSize = 20;
        private int _totalPages = 1;
        private string _keyword = string.Empty;
        private string _loai = string.Empty;
        private string _trangThai = string.Empty;

        public UcThietbi()
        {
            InitializeComponent();      
            ViewHelper.ApplyBaseStyle(this);
            this.Load += UcThietbi_Load;
            
            // Gán sự kiện các nút
            btnThem.Click += BtnThem_Click;   // Thêm
            btnSua.Click += BtnSua_Click;    // Sửa
            btnXoa.Click += BtnXoa_Click;    // Xóa
            btnLuu.Click += BtnLuu_Click;    // Lưu
            btnLamMoi.Click += BtnLamMoi_Click; // Làm mới
            // btnTaiDuLieu.Click += BtnTaiDL_Click;  // Tải dữ liệu
            // Click vào dòng trên grid
            dgvDevices.CellClick += DgvDevices_CellClick;
            
            // Tìm kiếm realtime
            txtQuanLyThietBi.TextChanged += (s, e) => ApplyFilterAndReload();
            comboBox1.SelectedIndexChanged += (s, e) => ApplyFilterAndReload();
            comboBox2.SelectedIndexChanged += (s, e) => ApplyFilterAndReload();

            // Phân trang
            btnFirst.Click += (s, e) => GoToPage(1);
            btnPrev.Click += (s, e) => GoToPage(_currentPage - 1);
            btnNext.Click += (s, e) => GoToPage(_currentPage + 1);
            btnLast.Click += (s, e) => GoToPage(_totalPages);
        }

        private void UcThietbi_Load(object sender, EventArgs e)
        {
            LoadComboBoxLoai();
            LoadComboBoxTrangThai();
            LoadComboBoxTimKiem();
            ApplyFilterAndReload();
            SetFormReadOnly(true);
        }

        // ===== LOAD DỮ LIỆU & PHÂN TRANG =====
        private void ApplyFilterAndReload()
        {
            _keyword = txtQuanLyThietBi.Text.Trim();
            _loai = comboBox1.SelectedIndex > 0 ? comboBox1.SelectedItem?.ToString() ?? "" : "";
            _trangThai = comboBox2.SelectedIndex > 0 ? comboBox2.SelectedItem?.ToString() ?? "" : "";
            _currentPage = 1;
            LoadPage();
        }

        private void LoadPage()
        {
            try
            {
                int total = DeviceService.GetTotalDevicesCount(_keyword, _loai, _trangThai);
                _totalPages = Math.Max(1, (int)Math.Ceiling((double)total / PageSize));

                if (_currentPage > _totalPages) _currentPage = _totalPages;
                if (_currentPage < 1) _currentPage = 1;

                DataTable dt = DeviceService.GetDevicesPaged(_currentPage, PageSize, _keyword, _loai, _trangThai);

                dgvDevices.AutoGenerateColumns = true;
                dgvDevices.DataSource = dt;

                if (dgvDevices.Columns.Contains("DeviceID"))
                    dgvDevices.Columns["DeviceID"].Visible = false;
                if (dgvDevices.Columns.Contains("DeviceCode"))
                    dgvDevices.Columns["DeviceCode"].Visible = false;
                if (dgvDevices.Columns.Contains("AvailableQuantity"))
                    dgvDevices.Columns["AvailableQuantity"].Visible = false;

                lblPageInfo.Text = $"Trang {_currentPage}/{_totalPages}";
                btnFirst.Enabled = _currentPage > 1;
                btnPrev.Enabled = _currentPage > 1;
                btnNext.Enabled = _currentPage < _totalPages;
                btnLast.Enabled = _currentPage < _totalPages;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu:\n" + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GoToPage(int page)
        {
            _currentPage = Math.Clamp(page, 1, _totalPages);
            LoadPage();
        }

        private void LoadComboBoxLoai()
        {
            try
            {
                DataTable dt = DeviceService.GetCategories();

                // ComboBox loại thiết bị trong form nhập
                txtLoaiThietBi.Tag = dt; // Dùng combobox thay textbox6 nếu muốn, tạm dùng text
                
                // ComboBox tìm kiếm
                comboBox1.Items.Clear();
                comboBox1.Items.Add("-- Tất cả loại --");
                foreach (DataRow row in dt.Rows)
                    comboBox1.Items.Add(row["CategoryName"]?.ToString() ?? string.Empty);
                comboBox1.SelectedIndex = 0;
            }
            catch (Exception ex) { AppLogger.Error($"[UcThietbi] Lỗi LoadComboBoxLoai", ex); }
        }

        private void LoadComboBoxTrangThai()
        {
            comboBox2.Items.Clear();
            comboBox2.Items.Add("-- Tất cả trạng thái --");
                comboBox2.Items.Add("Tốt");
                comboBox2.Items.Add("Đang mượn");
                comboBox2.Items.Add("Hỏng");
                comboBox2.Items.Add("Bảo trì");
                comboBox2.Items.Add("Ngừng sử dụng");
                comboBox2.SelectedIndex = 0;

            // txtTinhTrang là tình trạng trong form nhập
        }

        private void LoadComboBoxTimKiem()
        {
            // đã load trong LoadComboBoxLoai và LoadComboBoxTrangThai
        }

        // ===== TÌM KIẾM =====
        // Đã thay thế bằng ApplyFilterAndReload (Server-side)

        // ===== CLICK DÒNG TRÊN GRID =====
        private void DgvDevices_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvDevices.Rows[e.RowIndex];

            // Lấy DeviceID từ DataTable ẩn
            if (dgvDevices.DataSource is DataTable dt)
            {
                var dataRow = dt.DefaultView[e.RowIndex].Row;
                _selectedDeviceID = Convert.ToInt32(dataRow[Col.DeviceID]);
                _selectedTotalQuantity = Convert.ToInt32(dataRow[Col.SoLuong]);
                _selectedAvailableQuantity = Convert.ToInt32(dataRow[Col.AvailableQuantity]);

                txtMaThietBi.Text = dataRow[Col.MaThietBi]?.ToString();
                txtTenThietBi.Text = dataRow[Col.TenThietBi]?.ToString();
                txtViTri.Text = dataRow[Col.ViTri]?.ToString();
                txtLoaiThietBi.Text = dataRow[Col.LoaiThietBi]?.ToString();
                txtTinhTrang.Text = dataRow[Col.TinhTrang]?.ToString();
                txtGhiChu.Text = dataRow[Col.GhiChu]?.ToString();
                SetQuantityValue(Convert.ToDecimal(dataRow[Col.SoLuong]));
            }
        }

        // ===== CÁC NÚT =====
        private void BtnThem_Click(object sender, EventArgs e)
        {
            SetFormReadOnly(false);
            ClearForm();
            _selectedDeviceID = -1;
            // Ẩn nudSoLuong khi thêm mới — số lượng sẽ do hệ thống cá thể quản lý
            nudSoLuong.Enabled = false;
            nudSoLuong.Value = 0;
            txtMaThietBi.ReadOnly = true;
            try
            {
                txtMaThietBi.Text = DeviceService.GenerateDeviceCode();
            }
            catch { }
            txtTenThietBi.Focus();
        }

        private void BtnSua_Click(object sender, EventArgs e)
        {
            if (_selectedDeviceID < 0)
            {
                MessageBox.Show("Vui lòng chọn thiết bị cần sửa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            SetFormReadOnly(false);
            txtMaThietBi.ReadOnly = true;
            txtTenThietBi.Focus();
        }

        private void BtnLuu_Click(object sender, EventArgs e)
        {
            // Centralized validation using ValidationHelper
            if (!ValidationHelper.ValidateRequired(
                (txtTenThietBi.Text, "Tên thiết bị"),
                (txtMaThietBi.Text, "Mã thiết bị")))
                return;
            if (string.IsNullOrWhiteSpace(txtLoaiThietBi.Text))
            {
                MessageBox.Show("Vui lòng nhập loại thiết bị!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                bool isAdding = _selectedDeviceID < 0;

                DeviceService.SaveDevice(
                    _selectedDeviceID,
                    txtMaThietBi.Text.Trim(),
                    txtTenThietBi.Text.Trim(),
                    txtLoaiThietBi.Text.Trim(),
                    txtViTri.Text.Trim(),
                    (int)nudSoLuong.Value,
                    _selectedTotalQuantity,
                    _selectedAvailableQuantity,
                    txtTinhTrang.Text.Trim(),
                    txtGhiChu.Text.Trim());

                if (isAdding)
                {
                    MessageBox.Show(
                        "✅ Thêm thiết bị thành công!\n\n" +
                        "💡 Tiếp theo: Chọn thiết bị vừa thêm, sau đó bấm\n" +
                        "   [🏷 Mã cá thể] để thêm từng cá thể cho loại thiết bị này.",
                        "Thêm thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("✅ Cập nhật thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                SetFormReadOnly(true);
                ClearForm();
                LoadPage();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lưu dữ liệu:\n" + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            if (_selectedDeviceID < 0)
            {
                MessageBox.Show("Vui lòng chọn thiết bị cần xóa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Bạn có chắc muốn xóa mềm thiết bị này?\n\nThiết bị sẽ chuyển sang trạng thái Ngừng sử dụng và không còn được chọn để lập phiếu mới.", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            try
            {
                DeviceService.DeleteDevice(_selectedDeviceID);

                MessageBox.Show("Đã xóa mềm thiết bị thành công!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
                _selectedDeviceID = -1;
                LoadPage();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xóa:\n" + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnLamMoi_Click(object sender, EventArgs e)
        {
            ClearForm();
            SetFormReadOnly(true);
            _selectedDeviceID = -1;
            txtQuanLyThietBi.Text = "";
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            ApplyFilterAndReload();
        }

        private void BtnTaiDL_Click(object sender, EventArgs e) => LoadPage();

        private void BtnQuanLyCaThe_Click(object? sender, EventArgs e)
        {
            if (_selectedDeviceID < 0)
            {
                MessageBox.Show("Vui lòng chọn một loại thiết bị trước!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            using var view = new UcQuanLyCaThe(_selectedDeviceID, txtTenThietBi.Text);
            view.ShowDialog(this);
            LoadPage();
        }
        private void ClearForm()
        {
            txtMaThietBi.Text = "";
            txtTenThietBi.Text = "";
            txtViTri.Text = "";
            txtLoaiThietBi.Text = "";
            txtTinhTrang.Text = "";
            txtGhiChu.Text = "";
            SetQuantityValue(0);
            _selectedDeviceID = -1;
            _selectedTotalQuantity = 0;
            _selectedAvailableQuantity = 0;
        }

        private void SetQuantityValue(decimal value)
        {
            if (value > nudSoLuong.Maximum)
                nudSoLuong.Maximum = value;

            if (value < nudSoLuong.Minimum)
                nudSoLuong.Minimum = value;

            nudSoLuong.Value = value;
        }

        private void SetFormReadOnly(bool readOnly)
        {
            txtMaThietBi.ReadOnly = readOnly;
            txtTenThietBi.ReadOnly = readOnly;
            txtViTri.ReadOnly = readOnly;
            txtLoaiThietBi.ReadOnly = readOnly;
            txtTinhTrang.ReadOnly = readOnly;
            txtGhiChu.ReadOnly = readOnly;
            nudSoLuong.Enabled = false; 
            btnLuu.Enabled = !readOnly; 
        }
    }
}



