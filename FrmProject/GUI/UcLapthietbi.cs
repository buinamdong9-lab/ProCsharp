using FrmProject.GUI;
using System.Data;
using System.Drawing;

namespace FrmProject.GUI
{
    public partial class UcLapthietbi : UserControl
    {
        private ISettingsService SettingsService => AppServiceProvider.Get<ISettingsService>();
        private IBorrowApprovalService BorrowApprovalService => AppServiceProvider.Get<IBorrowApprovalService>();
        private IBorrowTicketService BorrowTicketService => AppServiceProvider.Get<IBorrowTicketService>();

        private DataTable _dtBorrowItems = new DataTable();
        private readonly int _currentUserId;
        private readonly string _currentUserName;
        private readonly AppRole _appRole;
        private readonly List<ComboItem> _borrowerItems = new();
        private int _defaultBorrowDays = 7;
        private int _maxDevicesPerTicket = 10;
        private bool _requireApproval = true;

        public UcLapthietbi() : this(0, string.Empty, AppRole.Staff)
        {
        }

        public UcLapthietbi(int currentUserId, string currentUserName, AppRole appRole)
        {
            _currentUserId = currentUserId;
            _currentUserName = currentUserName;
            _appRole = appRole;

            InitializeComponent();
            ViewHelper.ApplyBaseStyle(this);
            this.Load += UcLapthietbi_Load;

            // Buttons
            btnThemVaoPhieu.Click += BtnThemVaoPhieu_Click;  // ✔ Thêm vào phiếu
            btnLuuPhieu.Click += BtnLuuPhieu_Click;      // 💾 Lưu Phiếu
            btnHuy.Click += BtnHuy_Click;           // ✖ Hủy
            btnLamMoi.Click += BtnLamMoi_Click;        // ↺ Làm mới

        


            cmbNgayLap.SelectedIndexChanged += CmbDevice_SelectedIndexChanged;
            cmbMaThietBi.SelectedIndexChanged += (s, e) => UpdateQuantityLimit();
            dtpNgayMuon.ValueChanged += (s, e) => ApplyDefaultReturnDate();
        }

        private void UcLapthietbi_Load(object sender, EventArgs e)
        {
            LoadBorrowSettings();
            InitBorrowItemsGrid();
            LoadComboBoxes();
            ConfigureTicketNumberInput();
            GenerateTicketNumber();
            dtpNgayLap.Value = DateTime.Now;
            dtpNgayMuon.Value = DateTime.Now;
            ApplyDefaultReturnDate();
        }

        private void LoadBorrowSettings()
        {
            try
            {
                SettingsService.EnsureAppSettingsTable();
                _defaultBorrowDays = Math.Max(0, SettingsService.GetIntValue("CaiDat_ThoiHanMuon", 7));
                _maxDevicesPerTicket = Math.Max(1, SettingsService.GetIntValue("CaiDat_ToiDaThietBi", 10));
                _requireApproval = SettingsService.GetYesNoValue("CaiDat_CanPheDuyet", true);
            }
            catch (Exception ex)
            {
                AppLogger.Error("[UcLapthietbi] Không thể đọc cài đặt mượn trả.", ex);
                _defaultBorrowDays = 7;
                _maxDevicesPerTicket = 10;
                _requireApproval = true;
            }
        }

        private void ApplyDefaultReturnDate()
        {
            dtpHanTra.Value = dtpNgayMuon.Value.Date.AddDays(_defaultBorrowDays);
        }

        private void ConfigureTicketNumberInput()
        {
            txtPhieuMuonThietBi.ReadOnly = true;
            txtPhieuMuonThietBi.TabStop = false;
            txtPhieuMuonThietBi.BackColor = Color.FromArgb(235, 241, 245);
            txtPhieuMuonThietBi.ForeColor = Color.FromArgb(27, 94, 60);
            txtPhieuMuonThietBi.Cursor = Cursors.Default;
        }

        private void InitBorrowItemsGrid()
        {
            _dtBorrowItems = new DataTable();
            _dtBorrowItems.Columns.Add("DeviceID", typeof(int));
            _dtBorrowItems.Columns.Add("InstanceID", typeof(int));
            _dtBorrowItems.Columns.Add("Tên thiết bị", typeof(string));
            _dtBorrowItems.Columns.Add("Mã tài sản", typeof(string));
            _dtBorrowItems.Columns.Add("Số lượng", typeof(int));
            _dtBorrowItems.Columns.Add("Ghi chú", typeof(string));

            dgvData.DataSource = _dtBorrowItems;
            dgvData.AutoGenerateColumns = true;

            if (dgvData.Columns.Contains("DeviceID"))
                dgvData.Columns["DeviceID"].Visible = false;
            if (dgvData.Columns.Contains("InstanceID"))
                dgvData.Columns["InstanceID"].Visible = false;

            UpdateSummary();
        }

        private void LoadComboBoxes()
        {
            try
            {
                LoadBorrowerComboBox();

                cmbPhongSuDung.Items.Clear();
                foreach (LookupItem item in BorrowTicketService.GetRooms())
                    cmbPhongSuDung.Items.Add(new ComboItem(item.Id, item.Text));
                if (cmbPhongSuDung.Items.Count > 0) cmbPhongSuDung.SelectedIndex = 0;

                cmbNgayLap.Items.Clear();
                foreach (LookupItem item in BorrowTicketService.GetBorrowableDevices())
                    cmbNgayLap.Items.Add(new ComboItem(item.Id, item.Text));
                if (cmbNgayLap.Items.Count > 0) cmbNgayLap.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadBorrowerComboBox()
        {
            cmbNguoiMuon.Items.Clear();
            cmbNguoiMuon.AutoCompleteCustomSource.Clear();
            _borrowerItems.Clear();
            cmbNguoiMuon.Enabled = true;
            cmbNguoiMuon.DropDownStyle = ComboBoxStyle.DropDown;
            cmbNguoiMuon.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbNguoiMuon.AutoCompleteSource = AutoCompleteSource.CustomSource;
            cmbNguoiMuon.DropDownHeight = 240;
            cmbNguoiMuon.IntegralHeight = false;

            if (_appRole == AppRole.User)
            {
                cmbNguoiMuon.Items.Add(new ComboItem(_currentUserId, _currentUserName));
                cmbNguoiMuon.AutoCompleteCustomSource.Add(_currentUserName);
                cmbNguoiMuon.SelectedIndex = 0;
                cmbNguoiMuon.Enabled = false;
                return;
            }

            foreach (LookupItem item in BorrowTicketService.GetEnabledUsers())
            {
                ComboItem comboItem = new ComboItem(item.Id, item.Text);
                _borrowerItems.Add(comboItem);
                cmbNguoiMuon.Items.Add(comboItem);
                cmbNguoiMuon.AutoCompleteCustomSource.Add(item.Text);
            }

            if (cmbNguoiMuon.Items.Count > 0)
                cmbNguoiMuon.SelectedIndex = 0;
        }

        private void GenerateTicketNumber()
        {
            txtPhieuMuonThietBi.Text = "PM" + DateTime.Now.ToString("yyyyMMddHHmmss");
        }

        private void CmbDevice_SelectedIndexChanged(object? sender, EventArgs e)
        {
            cmbMaThietBi.Items.Clear();
            nudSoLuong.Value = 1;
            nudSoLuong.Maximum = 1;
            if (cmbNgayLap.SelectedItem is not ComboItem device) return;

            try
            {
                foreach (LookupItem item in BorrowTicketService.GetAvailableInstances(device.ID))
                    cmbMaThietBi.Items.Add(new ComboItem(item.Id, item.Text));
                if (cmbMaThietBi.Items.Count > 0) cmbMaThietBi.SelectedIndex = 0;
                UpdateQuantityLimit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải mã cá thể: " + ex.Message);
            }
        }

        private void BtnThemVaoPhieu_Click(object sender, EventArgs e)
        {
            if (cmbNgayLap.SelectedItem is not ComboItem device)
            {
                MessageBox.Show("Vui lòng chọn thiết bị!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbMaThietBi.SelectedItem is not ComboItem asset)
            {
                MessageBox.Show("Vui lòng chọn mã cá thể!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int requestedQuantity = (int)nudSoLuong.Value;
            int currentTotalQuantity = GetCurrentBorrowQuantity();
            if (currentTotalQuantity + requestedQuantity > _maxDevicesPerTicket)
            {
                MessageBox.Show(
                    $"Phiếu này chỉ được mượn tối đa {_maxDevicesPerTicket} thiết bị theo cấu hình hệ thống.\n" +
                    $"Hiện đã có {currentTotalQuantity}, bạn chỉ có thể thêm tối đa {Math.Max(0, _maxDevicesPerTicket - currentTotalQuantity)} thiết bị nữa.",
                    "Vượt giới hạn",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            List<ComboItem> availableAssets = GetAvailableAssetCandidates(asset);

            if (availableAssets.Count < requestedQuantity)
            {
                MessageBox.Show(
                    $"Chỉ còn {availableAssets.Count} mã cá thể khả dụng để thêm vào phiếu.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                UpdateQuantityLimit();
                return;
            }

            string note = txtGhiChu2.Text.Trim();
            foreach (ComboItem selectedAsset in availableAssets.Take(requestedQuantity))
                _dtBorrowItems.Rows.Add(device.ID, selectedAsset.ID, device.DisplayText, selectedAsset.DisplayText, 1, note);

            txtGhiChu2.Text = "";
            nudSoLuong.Value = 1;
            UpdateQuantityLimit();
            UpdateSummary();
        }

        private int GetCurrentBorrowQuantity()
        {
            int total = 0;
            foreach (DataRow row in _dtBorrowItems.Rows)
                total += Convert.ToInt32(row["Số lượng"]);

            return total;
        }

        private List<ComboItem> GetAvailableAssetCandidates(ComboItem selectedAsset)
        {
            List<ComboItem> candidates = new List<ComboItem>();

            if (!IsAssetAlreadyInBorrowList(selectedAsset.ID))
                candidates.Add(selectedAsset);

            foreach (ComboItem item in cmbMaThietBi.Items.OfType<ComboItem>())
            {
                if (item.ID == selectedAsset.ID || IsAssetAlreadyInBorrowList(item.ID))
                    continue;

                candidates.Add(item);
            }

            return candidates;
        }

        private bool IsAssetAlreadyInBorrowList(int instanceId)
        {
            foreach (DataRow row in _dtBorrowItems.Rows)
            {
                if (Convert.ToInt32(row["InstanceID"]) == instanceId)
                    return true;
            }

            return false;
        }

        private void UpdateQuantityLimit()
        {
            if (nudSoLuong == null)
                return;

            int availableCount = cmbMaThietBi.Items
                .OfType<ComboItem>()
                .Count(item => !IsAssetAlreadyInBorrowList(item.ID));

            int maxQuantity = Math.Max(1, availableCount);
            nudSoLuong.Maximum = maxQuantity;

            if (nudSoLuong.Value > maxQuantity)
                nudSoLuong.Value = maxQuantity;
        }

        private void UpdateSummary()
        {
            int totalTypes = _dtBorrowItems.Rows.Count;
            int totalQty = 0;
            foreach (DataRow row in _dtBorrowItems.Rows)
                totalQty += Convert.ToInt32(row["Số lượng"]);

            lblTong.Text = $"Tổng loại: {totalTypes}     Tổng số lượng: {totalQty}";
        }

        private void BtnLuuPhieu_Click(object sender, EventArgs e)
        {
            if (_dtBorrowItems.Rows.Count == 0)
            {
                MessageBox.Show("Vui lòng thêm thiết bị vào phiếu!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ComboItem? borrower = _appRole == AppRole.User
                ? new ComboItem(_currentUserId, _currentUserName)
                : ResolveSelectedBorrower();

            if (borrower == null || borrower.ID <= 0)
            {
                MessageBox.Show("Vui lòng chọn người mượn hợp lệ từ danh sách gợi ý!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbNguoiMuon.Focus();
                return;
            }

            if (dtpHanTra.Value.Date < dtpNgayMuon.Value.Date)
            {
                MessageBox.Show("Hạn trả phải lớn hơn hoặc bằng ngày mượn!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string roomName = (cmbPhongSuDung.SelectedItem as ComboItem)?.DisplayText ?? string.Empty;
                BorrowTicketDraft draft = new()
                {
                    BorrowerId = borrower.ID,
                    TicketCode = GetCurrentTicketCode(),
                    BorrowDate = dtpNgayMuon.Value,
                    ExpectedReturnDate = dtpHanTra.Value,
                    Purpose = txtMucDich.Text.Trim(),
                    Note = BuildTicketNote(roomName)
                };

                foreach (DataRow row in _dtBorrowItems.Rows)
                {
                    draft.Items.Add(new BorrowTicketDraftItem
                    {
                        DeviceId = Convert.ToInt32(row["DeviceID"]),
                        InstanceId = Convert.ToInt32(row["InstanceID"]),
                        Quantity = Convert.ToInt32(row["Số lượng"])
                    });
                }

                int ticketID = BorrowTicketService.CreatePendingTicket(draft);
                if (_requireApproval)
                {
                    MessageBox.Show(
                        $"✅ Đã gửi phiếu mượn #{ticketID} thành công!\n\nTrạng thái: Chờ phê duyệt\nVui lòng đợi Admin xét duyệt trong Danh Sách Phiếu.",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    BorrowApprovalService.ApproveBorrow(ticketID);
                    MessageBox.Show(
                        $"✅ Đã lập và duyệt phiếu mượn #{ticketID} thành công!\n\nTrạng thái: Đang mượn",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                BtnLamMoi_Click(null!, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lưu phiếu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnHuy_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Hủy phiếu hiện tại?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                BtnLamMoi_Click(sender, e);
            }
        }

        private void BtnLamMoi_Click(object sender, EventArgs e)
        {
            InitBorrowItemsGrid();
            GenerateTicketNumber();
            dtpNgayLap.Value = DateTime.Now;
            dtpNgayMuon.Value = DateTime.Now;
            ApplyDefaultReturnDate();
            txtMucDich.Text = "";
            txtGhiChu.Text = "";
            txtGhiChu2.Text = "";
            if (cmbPhongSuDung.Items.Count > 0) cmbPhongSuDung.SelectedIndex = 0;
            nudSoLuong.Value = 1;
            LoadComboBoxes();
        }

        private string GetCurrentTicketCode()
        {
            if (string.IsNullOrWhiteSpace(txtPhieuMuonThietBi.Text))
                GenerateTicketNumber();

            return txtPhieuMuonThietBi.Text.Trim();
        }



        private string BuildTicketNote(string roomName)
        {
            List<string> noteParts = new List<string>();

            if (!string.IsNullOrWhiteSpace(roomName))
                noteParts.Add($"Phòng sử dụng: {roomName.Trim()}");
            if (!string.IsNullOrWhiteSpace(txtGhiChu.Text))
                noteParts.Add(txtGhiChu.Text.Trim());

            return string.Join(" | ", noteParts);
        }

        private ComboItem? ResolveSelectedBorrower()
        {
            if (cmbNguoiMuon.SelectedItem is ComboItem selected)
                return selected;

            string typedText = cmbNguoiMuon.Text.Trim();
            if (string.IsNullOrWhiteSpace(typedText))
                return null;

            return _borrowerItems.FirstOrDefault(item =>
                item.DisplayText.Equals(typedText, StringComparison.OrdinalIgnoreCase));
        }

    }

    // Helper class for ComboBox items with ID
    public class ComboItem
    {
        public int ID { get; set; }
        public string DisplayText { get; set; }

        public ComboItem(int id, string displayText)
        {
            ID = id;
            DisplayText = displayText;
        }

        public override string ToString() => DisplayText;
    }
}



