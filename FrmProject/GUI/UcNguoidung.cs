using FrmProject.GUI;
using System.Data;
using OfficeOpenXml;

namespace FrmProject
{
    public partial class UcNguoidung : UserControl
    {
        private IUserService UserService => AppServiceProvider.Get<IUserService>();

        // ═══ Trạng thái chọn dòng ═══
        private int _selectedUserID = -1;
        private bool _isAdding;

        // ═══ Trạng thái phân trang ═══
        private int _currentPage = 1;
        private const int PageSize = 20;
        private int _totalPages = 1;

        // ═══ Bộ lọc hiện tại ═══
        private string _keyword = string.Empty;
        private string _role    = string.Empty;
        private string _status  = string.Empty;

        public UcNguoidung()
        {
            InitializeComponent();
            ViewHelper.ApplyBaseStyle(this);

            // Load & grid
            this.Load              += UcNguoidung_Load;
            dgvNguoiDung.CellClick += DgvNguoiDung_CellClick;

            // CRUD
            btnThem.Click    += BtnThem_Click;
            btnSua.Click     += BtnSua_Click;
            btnXoa.Click     += BtnXoa_Click;
            btnLuu.Click     += BtnLuu_Click;
            btnDatLaiMk.Click += BtnResetMK_Click;
            btnLamMoi.Click  += BtnLamMoi_Click;
            btnXuatExcel.Click += BtnXuatExcel_Click;

            // Filter
            btnLoc.Click           += BtnLoc_Click;
            textBox8.TextChanged   += (s, e) => ApplyFilterAndReload();

            // Pagination
            btnFirst.Click += (s, e) => GoToPage(1);
            btnPrev.Click  += (s, e) => GoToPage(_currentPage - 1);
            btnNext.Click  += (s, e) => GoToPage(_currentPage + 1);
            btnLast.Click  += (s, e) => GoToPage(_totalPages);

            // Validation
            txtSoDienThoai.KeyPress += (s, e) =>
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            };
        }

        // ──────────────────────────────────────────────────────
        // LOAD & PHÂN TRANG
        // ──────────────────────────────────────────────────────

        private void UcNguoidung_Load(object sender, EventArgs e)
        {
            ApplyFilterAndReload();
            SetFormReadOnly(true);
        }

        /// <summary>
        /// Lấy bộ lọc từ UI, reset về trang 1, rồi tải dữ liệu.
        /// Gọi khi người dùng thay đổi điều kiện lọc / bấm Làm mới.
        /// </summary>
        private void ApplyFilterAndReload()
        {
            _keyword = textBox8.Text.Trim();
            _role    = comboBox3.Text.Contains("Tất cả") ? string.Empty : comboBox3.Text.Trim();
            _status  = comboBox4.Text.Contains("Tất cả") ? string.Empty : comboBox4.Text.Trim();
            _currentPage = 1;
            LoadPage();
        }

        /// <summary>
        /// Tải đúng một trang dữ liệu từ Repository (server-side pagination).
        /// Không reset bộ lọc — chỉ dùng _currentPage / _keyword / _role / _status hiện tại.
        /// </summary>
        private void LoadPage()
        {
            try
            {
                // 1. Đếm tổng (để tính số trang)
                int total  = UserService.GetTotalUsersCount(_keyword, _role, _status);
                _totalPages = Math.Max(1, (int)Math.Ceiling((double)total / PageSize));

                // Đảm bảo trang hiện tại không vượt
                if (_currentPage > _totalPages) _currentPage = _totalPages;
                if (_currentPage < 1)           _currentPage = 1;

                // 2. Lấy dữ liệu trang hiện tại
                DataTable dt = UserService.GetUsersPaged(
                    _currentPage, PageSize, _keyword, _role, _status);

                dgvNguoiDung.DataSource          = dt;
                dgvNguoiDung.AutoGenerateColumns = true;

                // 3. Cập nhật UI phân trang
                lblPageInfo.Text  = $"Trang {_currentPage} / {_totalPages}  ({total} người)";
                btnFirst.Enabled  = _currentPage > 1;
                btnPrev.Enabled   = _currentPage > 1;
                btnNext.Enabled   = _currentPage < _totalPages;
                btnLast.Enabled   = _currentPage < _totalPages;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>Chuyển sang trang chỉ định rồi tải lại.</summary>
        private void GoToPage(int page)
        {
            _currentPage = Math.Clamp(page, 1, _totalPages);
            LoadPage();
        }

        // ──────────────────────────────────────────────────────
        // GRID SELECTION
        // ──────────────────────────────────────────────────────

        private void DgvNguoiDung_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvNguoiDung.Rows[e.RowIndex];

            txtMaNguoiDung.Text  = row.Cells[Col.MaND]?.Value?.ToString() ?? "";
            txtHoTen.Text        = row.Cells[Col.HoTen]?.Value?.ToString() ?? "";
            txtEmail.Text        = row.Cells[Col.Email]?.Value?.ToString() ?? "";
            txtKhoaBoMon.Text    = row.Cells[Col.KhoaBoMon]?.Value?.ToString() ?? "";
            txtSoDienThoai.Text  = row.Cells[Col.SoDienThoai]?.Value?.ToString() ?? "";
            cmbVaiTro.Text       = row.Cells[Col.VaiTro]?.Value?.ToString() ?? "";
            cmbTrangThai.Text    = row.Cells[Col.TrangThai]?.Value?.ToString() ?? "";
            txtMatKhau.Text      = "";

            try
            {
                var identity = UserService.FindUserIdentity(
                    txtMaNguoiDung.Text.Trim(), "");
                if (identity != null)
                {
                    _selectedUserID   = identity.UserId;
                    txtTenDangNhap.Text = identity.Username;
                }
                else
                {
                    _selectedUserID     = -1;
                    txtTenDangNhap.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                AppLogger.Error("[UcNguoidung] Lỗi đồng bộ dòng chọn", ex);
                _selectedUserID = -1;
            }
        }

        // ──────────────────────────────────────────────────────
        // CRUD HANDLERS
        // ──────────────────────────────────────────────────────

        private void BtnThem_Click(object sender, EventArgs e)
        {
            _isAdding = true;
            _selectedUserID = -1;
            ClearForm();
            SetFormReadOnly(false);
            txtMaNguoiDung.ReadOnly = true; // Auto-generated
            try
            {
                txtMaNguoiDung.Text = UserService.GenerateUserCode();
            }
            catch { /* Ignore in design time or initial fail */ }
            txtHoTen.Focus();
        }

        private void BtnSua_Click(object sender, EventArgs e)
        {
            if (_selectedUserID < 0)
            {
                MessageBox.Show("Vui lòng chọn người dùng cần sửa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            _isAdding = false;
            SetFormReadOnly(false);
            txtMaNguoiDung.ReadOnly = true; // Protect auto-generated ID
            txtHoTen.Focus();
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            if (_selectedUserID < 0)
            {
                MessageBox.Show("Vui lòng chọn người dùng cần xóa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("Bạn có chắc muốn xóa mềm người dùng này?\n\nTài khoản sẽ ngừng hoạt động và không thể đăng nhập, nhưng lịch sử phiếu vẫn được giữ.", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            try
            {
                UserService.DeleteUser(_selectedUserID);
                MessageBox.Show("Đã xóa mềm người dùng thành công!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
                _selectedUserID = -1;
                LoadPage();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xóa: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnLuu_Click(object sender, EventArgs e)
        {
            if (!ValidationHelper.ValidateRequired(
                (txtMaNguoiDung.Text, "Mã người dùng"),
                (txtHoTen.Text,       "Họ tên"))) return;

            if (!ValidationHelper.ValidateEmail(txtEmail.Text))       return;
            if (!ValidationHelper.ValidatePhone(txtSoDienThoai.Text))  return;

            try
            {
                string username = string.IsNullOrWhiteSpace(txtTenDangNhap.Text)
                    ? txtMaNguoiDung.Text.Trim().ToLowerInvariant()
                    : txtTenDangNhap.Text.Trim();

                bool isAdding = _isAdding || _selectedUserID < 0;
                if (isAdding)
                {
                    if (string.IsNullOrWhiteSpace(txtMatKhau.Text))
                    {
                        MessageBox.Show("Vui lòng nhập mật khẩu cho người dùng mới!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    if (!ValidationHelper.ValidatePassword(txtMatKhau.Text)) return;
                }

                var user = new UserEditModel
                {
                    UserId       = _selectedUserID,
                    UserCode     = txtMaNguoiDung.Text.Trim(),
                    FullName     = txtHoTen.Text.Trim(),
                    Email        = txtEmail.Text.Trim(),
                    Department   = txtKhoaBoMon.Text.Trim(),
                    Phone        = txtSoDienThoai.Text.Trim(),
                    RoleName     = cmbVaiTro.Text.Trim(),
                    Username     = username,
                    IsActive     = !cmbTrangThai.Text.Contains("Ngừng", StringComparison.OrdinalIgnoreCase),
                    IsLocked     = cmbTrangThai.Text.Contains("khóa",   StringComparison.OrdinalIgnoreCase),
                    PasswordHash = string.IsNullOrWhiteSpace(txtMatKhau.Text)
                        ? null
                        : PasswordHelper.HashPassword(txtMatKhau.Text.Trim())
                };

                UserService.SaveUser(user, isAdding);
                MessageBox.Show(isAdding ? "Thêm thành công!" : "Cập nhật thành công!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                SetFormReadOnly(true);
                ClearForm();
                _selectedUserID = -1;
                _isAdding = false;
                LoadPage();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lưu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnResetMK_Click(object sender, EventArgs e)
        {
            if (_selectedUserID < 0)
            {
                MessageBox.Show("Vui lòng chọn người dùng!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string newPass = Microsoft.VisualBasic.Interaction.InputBox(
                "Nhập mật khẩu mới:", "Đặt lại mật khẩu", "");
            if (string.IsNullOrWhiteSpace(newPass)) return;
            if (!ValidationHelper.ValidatePassword(newPass)) return;

            try
            {
                UserService.ResetPassword(_selectedUserID, PasswordHelper.HashPassword(newPass));
                MessageBox.Show("Đã đặt lại mật khẩu thành công.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnLamMoi_Click(object sender, EventArgs e)
        {
            ClearForm();
            SetFormReadOnly(true);
            _selectedUserID = -1;
            _isAdding = false;
            textBox8.Text    = "";
            comboBox3.Text   = "Tất cả vai trò";
            comboBox4.Text   = "Tất cả trạng thái";
            ApplyFilterAndReload();
        }

        private void BtnLoc_Click(object sender, EventArgs e) => ApplyFilterAndReload();

        // ──────────────────────────────────────────────────────
        // XUẤT EXCEL
        // ──────────────────────────────────────────────────────

        private void BtnXuatExcel_Click(object? sender, EventArgs e)
        {
            if (dgvNguoiDung.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            ExcelPackage.License.SetNonCommercialPersonal("ExportUserList");
            using var sfd = new SaveFileDialog
            {
                Filter = "Excel Workbook|*.xlsx",
                Title  = "Xuất Danh Sách Người Dùng"
            };
            if (sfd.ShowDialog() != DialogResult.OK) return;

            try
            {
                using var package = new ExcelPackage(new System.IO.FileInfo(sfd.FileName));
                var ws = package.Workbook.Worksheets.Add("NguoiDung");

                int col = 1;
                for (int j = 0; j < dgvNguoiDung.Columns.Count; j++)
                {
                    if (!dgvNguoiDung.Columns[j].Visible) continue;
                    ws.Cells[1, col].Value          = dgvNguoiDung.Columns[j].HeaderText;
                    ws.Cells[1, col].Style.Font.Bold = true;
                    col++;
                }
                for (int i = 0; i < dgvNguoiDung.Rows.Count; i++)
                {
                    col = 1;
                    for (int j = 0; j < dgvNguoiDung.Columns.Count; j++)
                    {
                        if (!dgvNguoiDung.Columns[j].Visible) continue;
                        ws.Cells[i + 2, col].Value = dgvNguoiDung.Rows[i].Cells[j].Value?.ToString() ?? "";
                        col++;
                    }
                }
                ws.Cells.AutoFitColumns();
                package.Save();
                MessageBox.Show("Xuất file Excel thành công!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xuất file: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ──────────────────────────────────────────────────────
        // UI HELPERS
        // ──────────────────────────────────────────────────────

        private void ClearForm()
        {
            txtMaNguoiDung.Text = txtHoTen.Text = txtEmail.Text = "";
            txtKhoaBoMon.Text   = txtTenDangNhap.Text = "";
            txtSoDienThoai.Text = txtMatKhau.Text = "";
            cmbVaiTro.Text      = "Chọn vai trò";
            cmbTrangThai.Text   = "Hoạt động";
        }

        private void SetFormReadOnly(bool readOnly)
        {
            txtMaNguoiDung.ReadOnly  = readOnly;
            txtHoTen.ReadOnly        = readOnly;
            txtEmail.ReadOnly        = readOnly;
            txtKhoaBoMon.ReadOnly    = readOnly;
            txtTenDangNhap.ReadOnly  = readOnly;
            txtSoDienThoai.ReadOnly  = readOnly;
            txtMatKhau.ReadOnly      = readOnly;
            cmbVaiTro.Enabled        = !readOnly;
            cmbTrangThai.Enabled     = !readOnly;
            btnLuu.Enabled           = !readOnly;
        }
    }
}



