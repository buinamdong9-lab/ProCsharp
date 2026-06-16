using FrmProject.GUI;
using System.Data;
using System.Diagnostics;

namespace FrmProject.GUI
{
    public partial class UcPhonghoc : UserControl
    {
        private IRoomService RoomService => AppServiceProvider.Get<IRoomService>();
        private IDeviceService DeviceService => AppServiceProvider.Get<IDeviceService>();

        private int _selectedRoomID = -1;
        private bool _isAdding = false;

        public UcPhonghoc()
        {
            InitializeComponent();
            ViewHelper.ApplyBaseStyle(this);
            this.Load += UcPhonghoc_Load;

            // CRUD buttons
            btnThem.Click += BtnThem_Click;     // ➕ Thêm
            btnSua.Click += BtnSua_Click;      // ✏ Sửa
            btnXoa.Click += BtnXoa_Click;      // 🗑 Xóa
            btnLuu.Click += BtnLuu_Click;      // 💾 Lưu
            btnLamMoi.Click += BtnLamMoi_Click;   // ↺ Làm mới

            // Filter
            btnLoc.Click += BtnLoc_Click;
            txtDanhSachPhongHoc.TextChanged += (s, e) => TimKiem();

            // Capacity +/-
            btnButton.Click += (s, e) =>
            {
                if (int.TryParse(txtSucChua.Text, out int val))
                    txtSucChua.Text = (val + 1).ToString();
                else
                    txtSucChua.Text = "1";
            };
            btnButton2.Click += (s, e) =>
            {
                if (int.TryParse(txtSucChua.Text, out int val) && val > 0)
                    txtSucChua.Text = (val - 1).ToString();
            };

            // Grid click
            dgvPhongHoc.CellClick += dgvPhongHoc_CellClick;
        }

        private void UcPhonghoc_Load(object sender, EventArgs e)
        {
            LoadLookupValues();
            LoadData();
            SetFormReadOnly(true);
            pnlGridThietBiPhong.Visible = false;
        }

        private void LoadData()
        {
            try
            {
                dgvPhongHoc.DataSource = null;
                dgvPhongHoc.Columns.Clear();
                dgvPhongHoc.AutoGenerateColumns = true;
                dgvPhongHoc.DataSource = RoomService.GetAllRooms();

                if (dgvPhongHoc.Columns.Count > 0)
                {
                    dgvPhongHoc.Columns["Mã phòng"].Width = 90;
                    dgvPhongHoc.Columns["Tên phòng"].Width = 180;
                    dgvPhongHoc.Columns["Loại"].Width = 150;
                    dgvPhongHoc.Columns["Tầng"].Width = 60;
                    dgvPhongHoc.Columns["Sức chứa"].Width = 90;
                    dgvPhongHoc.Columns["Trạng thái"].AutoSizeMode =
                        DataGridViewAutoSizeColumnMode.Fill;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvPhongHoc_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvPhongHoc.Rows[e.RowIndex];

            string roomCode = row.Cells["Mã phòng"].Value?.ToString() ?? "";
            txtMaPhong.Text = roomCode;
            txtTenPhong.Text = row.Cells["Tên phòng"].Value?.ToString() ?? "";
            cmbLoaiPhong.Text = row.Cells["Loại"].Value?.ToString() ?? "";
            cmbTrangThai.Text = row.Cells["Trạng thái"].Value?.ToString() ?? "";

            // Get RoomID via RoomDao (Task 5)
            try
            {
                var roomInfo = RoomService.GetRoomByCode(roomCode);
                if (roomInfo.HasValue)
                {
                    _selectedRoomID = roomInfo.Value.RoomID;
                    cmbTang.Text = roomInfo.Value.Floor;
                    txtSucChua.Text = roomInfo.Value.Capacity;

                    string fullNote = roomInfo.Value.Note ?? string.Empty;
                    if (fullNote.StartsWith("Tòa nhà: "))
                    {
                        int pipeIndex = fullNote.IndexOf('|');
                        if (pipeIndex >= 0)
                        {
                            cmbToaNha.Text = fullNote.Substring(9, pipeIndex - 9).Trim();
                            txtGhiChu.Text = fullNote.Substring(pipeIndex + 1).Trim();
                        }
                        else
                        {
                            cmbToaNha.Text = fullNote.Substring(9).Trim();
                            txtGhiChu.Text = string.Empty;
                        }
                    }
                    else
                    {
                        cmbToaNha.SelectedIndex = -1;
                        txtGhiChu.Text = fullNote;
                    }
                }
                else
                {
                    _selectedRoomID = -1;
                }
            }
            catch (Exception ex) { AppLogger.Error($"[UcPhonghoc] Lỗi lấy RoomID", ex); _selectedRoomID = -1; }

            // Load devices in room
            lblThietBiTrongPhongTitle.Text = "🔧  Thiết bị trong phòng " + roomCode;
            dgvThietBiPhong.DataSource = null;
            dgvThietBiPhong.Columns.Clear();
            dgvThietBiPhong.AutoGenerateColumns = true;
            try
            {
                dgvThietBiPhong.DataSource = DeviceService.GetDevicesByRoom(roomCode);
                if (dgvThietBiPhong.Columns.Count > 0)
                {
                    dgvThietBiPhong.Columns["Mã TB"].Width = 100;
                    dgvThietBiPhong.Columns["Tên thiết bị"].Width = 200;
                    dgvThietBiPhong.Columns["SL"].Width = 60;
                    dgvThietBiPhong.Columns["Tình trạng"].AutoSizeMode =
                        DataGridViewAutoSizeColumnMode.Fill;
                }
            }
            catch (Exception ex) { AppLogger.Error($"[UcPhonghoc] Lỗi load thiết bị trong phòng", ex); }

            pnlGridThietBiPhong.Visible = true;
        }

        private void BtnThem_Click(object sender, EventArgs e)
        {
            _isAdding = true;
            _selectedRoomID = -1;
            ClearForm();
            SetFormReadOnly(false);
            txtMaPhong.Focus();
        }

        private void BtnSua_Click(object sender, EventArgs e)
        {
            if (_selectedRoomID < 0)
            {
                MessageBox.Show("Vui lòng chọn phòng cần sửa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            _isAdding = false;
            SetFormReadOnly(false);
            txtTenPhong.Focus();
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            if (_selectedRoomID < 0)
            {
                MessageBox.Show("Vui lòng chọn phòng cần xóa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("Bạn có chắc muốn xóa mềm phòng này?\n\nPhòng sẽ chuyển sang trạng thái Ngừng sử dụng và không còn được chọn khi lập phiếu mới.", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            try
            {
                RoomService.DeleteRoom(_selectedRoomID);

                MessageBox.Show("Đã xóa mềm phòng thành công!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
                _selectedRoomID = -1;
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xóa: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnLuu_Click(object sender, EventArgs e)
        {
            // Centralized validation using ValidationHelper
            if (!ValidationHelper.ValidateRequired(
                (txtMaPhong.Text, "Mã phòng"),
                (txtTenPhong.Text, "Tên phòng")))
                return;

            try
            {
                int capacity = 0;
                int.TryParse(txtSucChua.Text, out capacity);

                if (_isAdding || _selectedRoomID < 0)
                {
                    // Task 5: Use RoomDao
                    RoomService.InsertRoom(
                        txtMaPhong.Text.Trim(),
                        txtTenPhong.Text.Trim(),
                        cmbLoaiPhong.Text.Trim(),
                        cmbTang.Text.Trim(),
                        capacity,
                        cmbTrangThai.Text.Trim(),
                        BuildRoomNote());

                    MessageBox.Show("Thêm phòng thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Task 5: Use RoomDao
                    RoomService.UpdateRoom(
                        _selectedRoomID,
                        txtMaPhong.Text.Trim(),
                        txtTenPhong.Text.Trim(),
                        cmbLoaiPhong.Text.Trim(),
                        cmbTang.Text.Trim(),
                        capacity,
                        cmbTrangThai.Text.Trim(),
                        BuildRoomNote());

                    MessageBox.Show("Cập nhật phòng thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                SetFormReadOnly(true);
                ClearForm();
                _selectedRoomID = -1;
                _isAdding = false;
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lưu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnLamMoi_Click(object sender, EventArgs e)
        {
            if (HasUnsavedChanges() &&
                MessageBox.Show("Dữ liệu đang nhập sẽ bị mất. Bạn có muốn làm mới không?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            ClearForm();
            SetFormReadOnly(true);
            _selectedRoomID = -1;
            _isAdding = false;
            txtDanhSachPhongHoc.Text = "";
            LoadData();
        }

        private void BtnLoc_Click(object sender, EventArgs e)
        {
            TimKiem();
        }

        private void TimKiem()
        {
            try
            {
                // Task 5: Use RoomService.SearchRooms
                string keyword = txtDanhSachPhongHoc.Text.Trim();
                string type = cmbQuanLyPhongHoc2.Text;
                string status = cmbQuanLyPhongHoc.Text;
                dgvPhongHoc.DataSource = RoomService.SearchRooms(keyword, type, status);
            }
            catch (Exception ex) { AppLogger.Error($"[UcPhonghoc] Lỗi tìm kiếm", ex); }
        }

        private void ClearForm()
        {
            txtMaPhong.Text = "";
            txtTenPhong.Text = "";
            txtSucChua.Text = "";
            txtGhiChu.Text = "";
            cmbLoaiPhong.SelectedIndex = -1;
            cmbTrangThai.SelectedIndex = -1;
            cmbToaNha.SelectedIndex = -1;
            cmbTang.Text = "";
        }

        private void SetFormReadOnly(bool readOnly)
        {
            txtMaPhong.ReadOnly = readOnly;
            txtTenPhong.ReadOnly = readOnly;
            txtSucChua.ReadOnly = readOnly;
            txtGhiChu.ReadOnly = readOnly;
            cmbLoaiPhong.Enabled = !readOnly;
            cmbTrangThai.Enabled = !readOnly;
            cmbToaNha.Enabled = !readOnly;
            cmbTang.Enabled = !readOnly;
            btnLuu.Enabled = !readOnly; // Lưu
        }

        private void LoadLookupValues()
        {
            cmbQuanLyPhongHoc2.Items.Clear();
            cmbQuanLyPhongHoc2.Items.AddRange(new object[] { "Tất cả loại", "Phòng thực hành", "Phòng lý thuyết", "Lab", "Hội trường" });
            cmbQuanLyPhongHoc2.SelectedIndex = 0;

            cmbQuanLyPhongHoc.Items.Clear();
            cmbQuanLyPhongHoc.Items.AddRange(new object[] { "Tất cả trạng thái", "Hoạt động", "Đang sửa chữa", "Bảo trì", "Ngừng sử dụng" });
            cmbQuanLyPhongHoc.SelectedIndex = 0;

            cmbToaNha.Items.Clear();
            cmbToaNha.Items.AddRange(new object[] { "A", "B", "C", "D" });
            cmbTang.Items.Clear();
            cmbTang.Items.AddRange(new object[] { "1", "2", "3", "4", "5" });
        }

        private string BuildRoomNote()
        {
            string note = txtGhiChu.Text.Trim();
            if (string.IsNullOrWhiteSpace(cmbToaNha.Text))
                return note;

            return string.IsNullOrWhiteSpace(note)
                ? $"Tòa nhà: {cmbToaNha.Text.Trim()}"
                : $"Tòa nhà: {cmbToaNha.Text.Trim()} | {note}";
        }

        private bool HasUnsavedChanges()
        {
            if (!btnLuu.Enabled)
                return false;

            return !string.IsNullOrWhiteSpace(txtMaPhong.Text)
                || !string.IsNullOrWhiteSpace(txtTenPhong.Text)
                || !string.IsNullOrWhiteSpace(txtSucChua.Text)
                || !string.IsNullOrWhiteSpace(txtGhiChu.Text)
                || !string.IsNullOrWhiteSpace(cmbLoaiPhong.Text)
                || !string.IsNullOrWhiteSpace(cmbTrangThai.Text)
                || !string.IsNullOrWhiteSpace(cmbToaNha.Text)
                || !string.IsNullOrWhiteSpace(cmbTang.Text);
        }
    }
}



