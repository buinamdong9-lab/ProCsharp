using FrmProject.GUI;
using System.Data;

namespace FrmProject.GUI
{
    public partial class UcTrathietbi : UserControl
    {
        private readonly int _currentUserId;
        private readonly AppRole _appRole;
        private int _selectedTicketID = -1;
        private bool _allowPartialReturn;
        private bool _hasLoaded;

        // Admin: pending return approval.
        private int _pendingSelectedTicketID = -1;

        public UcTrathietbi(int currentUserId = 0, AppRole appRole = AppRole.Staff)
        {
            _currentUserId = currentUserId;
            _appRole = appRole;
            InitializeComponent();
            ViewHelper.ApplyBaseStyle(this);
            this.Load += UcTrathietbi_Load;
            this.Resize += (_, _) => { if (_pnlPendingReturns != null) LayoutPendingPanel(); };

            btnTim.Click += BtnTimPhieu_Click;     // 🔍 Tìm
            btnXacNhanTra.Click += BtnXacNhanTra_Click;   // ✓ Xác nhận trả
            btnTraThieuLoi.Click += BtnTraThieuLoi_Click;  // ▲ Trả thiếu / lỗi
            btnInBienBan.Click += BtnInBienBan_Click;      // 🖨 In biên bản
            btnHuy.Click += BtnHuy_Click;          // Hủy

            dgvData.CellValidating += DgvData_CellValidating;
            cmbNgayMuon.SelectedIndexChanged += (s, e) =>
            {
                if (cmbNgayMuon.SelectedItem is ComboItem item)
                    LoadTicketDetails(item.ID);
            };
        }

        private void UcTrathietbi_Load(object sender, EventArgs e)
        {
            dtpNgayTra.Value = DateTime.Now;
            lblPhieuNayDaQua.Visible = false;
            txtHanTra2.Visible = false;
            txtHanTra2.Text = string.Empty;
            txtHanTra2.PlaceholderText = "Nhập ghi chú thiếu/hỏng rồi chỉnh cột SL trả nếu cần.";
            dgvData.AllowUserToAddRows = false;
            btnXacNhanTra.Text = "✓ Gửi yêu cầu trả";
            lblTraThietBi.Text = _appRole == AppRole.User ? "🔵  Gửi Yêu Cầu Trả Thiết Bị" : "🔵  Tiếp Nhận Yêu Cầu Trả";
            ConfigureTicketLookupUi();
            LoadBorrowingTickets();

            // Admin/Staff: show pending return approval panel
            if (_appRole != AppRole.User)
                SetupPendingReturnPanel();

            _hasLoaded = true;
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);

            if (!Visible || !_hasLoaded || IsDisposed)
                return;

            LoadBorrowingTickets(selectFirst: _selectedTicketID <= 0);
            if (_appRole != AppRole.User)
                LoadPendingReturnTickets();
        }


        // ═══════════════════════════════════════════════════════════════
        //  Existing functionality (with ownership verification)
        // ═══════════════════════════════════════════════════════════════

        private void LoadBorrowingTickets(bool selectFirst = true)
        {
            try
            {
                cmbNgayMuon.Items.Clear();
                foreach (LookupItem item in ReturnTicketRepository.SearchBorrowingTickets(_currentUserId, _appRole))
                    cmbNgayMuon.Items.Add(new ComboItem(item.Id, item.Text));

                if (cmbNgayMuon.Items.Count > 0)
                {
                    lblTraCuuPhieuMuon.Text = _appRole == AppRole.User
                        ? "📋  Phiếu mượn của bạn"
                        : "🔍  Tra cứu phiếu mượn";
                    if (selectFirst)
                        cmbNgayMuon.SelectedIndex = 0;
                }
                else
                {
                    ClearForm();
                    lblTraCuuPhieuMuon.Text = _appRole == AppRole.User
                        ? "📋  Bạn hiện không có phiếu mượn nào đang giữ"
                        : "🔍  Tra cứu phiếu mượn";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải phiếu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnTimPhieu_Click(object sender, EventArgs e)
        {
            if (_appRole == AppRole.User)
            {
                LoadBorrowingTickets();
                return;
            }

            string keyword = txtSoPhieu.Text.Trim();
            if (string.IsNullOrEmpty(keyword))
            {
                LoadBorrowingTickets();
                return;
            }

            try
            {
                cmbNgayMuon.Items.Clear();
                foreach (LookupItem item in ReturnTicketRepository.SearchBorrowingTickets(_currentUserId, _appRole, keyword))
                    cmbNgayMuon.Items.Add(new ComboItem(item.Id, item.Text));

                if (cmbNgayMuon.Items.Count > 0)
                    cmbNgayMuon.SelectedIndex = 0;
                else
                    MessageBox.Show("Không tìm thấy phiếu nào!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadTicketDetails(int ticketID, bool isPendingApprovalView = false)
        {
            _selectedTicketID = ticketID;
            try
            {
                ReturnTicketDetails? details = ReturnTicketRepository.GetTicketDetails(ticketID);
                if (details == null)
                    return;

                txtSoPhieu2.Text = details.TicketDisplay;
                txtNgayMuon.Text = details.BorrowerName;
                txtNgayMuon2.Text = details.BorrowDate.ToString("dd/MM/yyyy");
                txtHanTra.Text = details.ExpectedReturnDate.ToString("dd/MM/yyyy");
                txtGhiChuTra.Text = string.Empty;

                if (details.ExpectedReturnDate < DateTime.Now)
                {
                    int overdueDays = (DateTime.Now - details.ExpectedReturnDate).Days;
                    lblPhieuNayDaQua.Text = $"⚠  Phiếu này đã quá hạn {overdueDays} ngày. Cần xử lý vi phạm.";
                    lblPhieuNayDaQua.Visible = true;
                }
                else
                {
                    lblPhieuNayDaQua.Visible = false;
                }

                DataTable dt = details.Items;
                foreach (DataColumn col in dt.Columns)
                    col.ReadOnly = false;

                bool isPending = string.Equals(details.Status, BorrowTicketStatus.ReturnPending, StringComparison.OrdinalIgnoreCase);
                if (isPendingApprovalView || isPending)
                {
                    ReturnTicketRepository.ApplyPendingReturnQuantities(ticketID, dt);
                    if (ReturnTicketRepository.TryLoadPendingReturnRequest(ticketID, out DateTime requestedAt, out List<ReturnRequestItem> pendingItems))
                    {
                        dtpNgayTra.Value = requestedAt == DateTime.MinValue ? DateTime.Now : requestedAt;
                        txtGhiChuTra.Text = isPending
                            ? $"Phiếu đang chờ duyệt trả thiết bị. Số lượng trả đang chờ duyệt được hiển thị dưới đây."
                            : $"Phiếu đang chờ duyệt trả. Số dòng yêu cầu: {pendingItems.Count}.";
                    }
                }

                dgvData.DataSource = dt;
                if (dgvData.Columns.Contains("DeviceID"))
                    dgvData.Columns["DeviceID"].Visible = false;
                if (dgvData.Columns.Contains("InstanceID"))
                    dgvData.Columns["InstanceID"].Visible = false;
                dgvData.ReadOnly = true;
                _allowPartialReturn = false;
                txtHanTra2.Visible = false;
                btnXacNhanTra.Enabled = !isPendingApprovalView && !isPending;
                btnTraThieuLoi.Enabled = !isPendingApprovalView && !isPending;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải chi tiết: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplyPendingReturnQuantities(int ticketID, DataTable dt)
        {
            ReturnTicketRepository.ApplyPendingReturnQuantities(ticketID, dt);
        }

        private void BtnXacNhanTra_Click(object sender, EventArgs e)
        {
            if (_selectedTicketID < 0)
            {
                MessageBox.Show("Vui lòng chọn phiếu cần trả!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Gửi yêu cầu trả thiết bị cho admin phê duyệt?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            try
            {
                if (dgvData.DataSource is not DataTable dt || dt.Rows.Count == 0)
                {
                    MessageBox.Show("Phiếu này không có thiết bị để trả!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var returnItems = new List<(int DeviceID, int InstanceID, int BorrowQty, int ReturnQty, string Note)>();
                foreach (DataRow row in dt.Rows)
                {
                    int borrowedQty = Convert.ToInt32(row["SL mượn"]);
                    int returnQty = Convert.ToInt32(row["SL trả"]);

                    if (returnQty < 0 || returnQty > borrowedQty)
                    {
                        MessageBox.Show("Số lượng trả phải nằm trong khoảng từ 0 đến số lượng mượn.", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    returnItems.Add((
                        Convert.ToInt32(row["DeviceID"]),
                        row["InstanceID"] == DBNull.Value ? 0 : Convert.ToInt32(row["InstanceID"]),
                        borrowedQty,
                        returnQty,
                        GetReturnCondition(row)));
                }

                if (returnItems.All(item => item.ReturnQty == 0))
                {
                    MessageBox.Show("Cần nhập ít nhất một thiết bị có số lượng trả lớn hơn 0.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_allowPartialReturn &&
                    returnItems.Any(item => item.ReturnQty < item.BorrowQty) &&
                    string.IsNullOrWhiteSpace(txtHanTra2.Text) &&
                    string.IsNullOrWhiteSpace(txtGhiChuTra.Text))
                {
                    MessageBox.Show("Vui lòng nhập ghi chú/lý do khi trả thiếu hoặc thiết bị lỗi.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtHanTra2.Visible = true;
                    txtHanTra2.Focus();
                    return;
                }

                string requestNote = BuildReturnNote(returnItems);
                ReturnTicketRepository.SubmitReturnRequest(
                    _selectedTicketID,
                    _currentUserId,
                    _appRole,
                    dtpNgayTra.Value,
                    returnItems,
                    requestNote);

                MessageBox.Show("Đã gửi yêu cầu trả thiết bị. Vui lòng chờ admin phê duyệt.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                ClearForm();
                LoadBorrowingTickets();
                if (_appRole != AppRole.User) LoadPendingReturnTickets();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi trả thiết bị: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnTraThieuLoi_Click(object sender, EventArgs e)
        {
            if (_selectedTicketID < 0)
            {
                MessageBox.Show("Vui lòng chọn phiếu!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvData.DataSource is not DataTable dt || dt.Rows.Count == 0)
            {
                MessageBox.Show("Phiếu này không có thiết bị để xử lý trả thiếu/lỗi.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ReturnIssueDialog.ShowFor(dt, this))
                return;

            _allowPartialReturn = true;
            dgvData.ReadOnly = false;
            dgvData.AllowUserToAddRows = false;
            if (dgvData.Columns.Contains("Tên thiết bị"))
                dgvData.Columns["Tên thiết bị"].ReadOnly = true;
            if (dgvData.Columns.Contains("Mã tài sản"))
                dgvData.Columns["Mã tài sản"].ReadOnly = true;
            if (dgvData.Columns.Contains("SL mượn"))
                dgvData.Columns["SL mượn"].ReadOnly = true;
            if (dgvData.Columns.Contains("DeviceID"))
                dgvData.Columns["DeviceID"].ReadOnly = true;
            if (dgvData.Columns.Contains("InstanceID"))
                dgvData.Columns["InstanceID"].ReadOnly = true;
            if (dgvData.Columns.Contains("SL trả"))
                dgvData.Columns["SL trả"].ReadOnly = false;
            if (dgvData.Columns.Contains("Tình trạng khi trả"))
                dgvData.Columns["Tình trạng khi trả"].ReadOnly = false;
            if (dgvData.Columns.Contains("Ghi chú"))
                dgvData.Columns["Ghi chú"].ReadOnly = false;

            txtHanTra2.Visible = true;
            txtHanTra2.Text = "Có thiết bị trả thiếu/lỗi. Vui lòng kiểm tra tình trạng khi trả và ghi chú cảnh báo trước khi duyệt.";
            txtGhiChuTra.Text = BuildReturnIssueWarning(dt);
        }

        private static string BuildReturnIssueWarning(DataTable table)
        {
            List<string> warnings = new();
            foreach (DataRow row in table.Rows)
            {
                int borrowedQty = Convert.ToInt32(row["SL mượn"]);
                int returnQty = Convert.ToInt32(row["SL trả"]);
                string condition = GetCellText(row, "Tình trạng khi trả");
                string note = GetCellText(row, "Ghi chú");

                if (returnQty < borrowedQty || !IsGoodReturnCondition(condition))
                    warnings.Add($"{row["Tên thiết bị"]}: trả {returnQty}/{borrowedQty}, tình trạng {condition}" +
                                 (string.IsNullOrWhiteSpace(note) ? string.Empty : $" - {note}"));
            }

            return warnings.Count == 0
                ? string.Empty
                : "Cảnh báo trả thiếu/lỗi: " + string.Join(" | ", warnings);
        }

        private void DgvData_CellValidating(object? sender, DataGridViewCellValidatingEventArgs e)
        {
            if (!_allowPartialReturn || e.RowIndex < 0 || !dgvData.Columns.Contains("SL trả"))
                return;

            if (dgvData.Columns[e.ColumnIndex].Name != "SL trả")
                return;

            int borrowedQty = Convert.ToInt32(dgvData.Rows[e.RowIndex].Cells["SL mượn"].Value);
            if (!int.TryParse(e.FormattedValue?.ToString(), out int returnQty) || returnQty < 0 || returnQty > borrowedQty)
            {
                e.Cancel = true;
                MessageBox.Show($"Số lượng trả phải từ 0 đến {borrowedQty}.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnInBienBan_Click(object sender, EventArgs e)
        {
            if (_selectedTicketID <= 0 || dgvData.DataSource is not DataTable dt || dt.Rows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn phiếu trước khi in biên bản.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string summary = BuildReturnReportText(dt);
            using Form preview = new Form
            {
                Text = "Biên bản trả thiết bị",
                StartPosition = FormStartPosition.CenterParent,
                Size = new Size(720, 520)
            };
            TextBox txtPreview = new TextBox
            {
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                Dock = DockStyle.Fill,
                Font = new Font("Consolas", 10F),
                Text = summary
            };
            preview.Controls.Add(txtPreview);
            preview.ShowDialog(this);
        }

        private string BuildReturnReportText(DataTable dt)
        {
            List<string> lines = new List<string>
            {
                "BIEN BAN TRA THIET BI",
                $"So phieu: {txtSoPhieu2.Text}",
                $"Nguoi muon: {txtNgayMuon.Text}",
                $"Ngay muon: {txtNgayMuon2.Text}",
                $"Han tra: {txtHanTra.Text}",
                $"Ngay tra: {dtpNgayTra.Value:dd/MM/yyyy}",
                $"Nguoi nhan: {lblAdminThuKho.Text}",
                "",
                "Danh sach thiet bi:"
            };

            foreach (DataRow row in dt.Rows)
            {
                lines.Add($"- {row["Tên thiết bị"]} | Ma TS: {row["Mã tài sản"]} | Muon: {row["SL mượn"]} | Tra: {row["SL trả"]} | TT muon: {GetCellText(row, "Tình trạng khi mượn")} | TT tra: {GetCellText(row, "Tình trạng khi trả")} | Ghi chu: {row["Ghi chú"]}");
            }

            if (!string.IsNullOrWhiteSpace(txtGhiChuTra.Text))
            {
                lines.Add("");
                lines.Add("Ghi chu tra:");
                lines.Add(txtGhiChuTra.Text.Trim());
            }

            if (!string.IsNullOrWhiteSpace(txtHanTra2.Text))
            {
                lines.Add("");
                lines.Add("Ghi chu thieu/loi:");
                lines.Add(txtHanTra2.Text.Trim());
            }

            return string.Join(Environment.NewLine, lines);
        }

        private void BtnHuy_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            _selectedTicketID = -1;
            if (_appRole != AppRole.User)
                txtSoPhieu.Text = "";
            txtSoPhieu2.Text = "";
            txtNgayMuon.Text = "";
            txtNgayMuon2.Text = "";
            txtHanTra.Text = "";
            txtGhiChuTra.Text = "";
            txtHanTra2.Text = "";
            txtHanTra2.Visible = false;
            lblPhieuNayDaQua.Visible = false;
            dgvData.DataSource = null;
            _allowPartialReturn = false;
            dgvData.ReadOnly = true;
            btnXacNhanTra.Enabled = true;
            btnTraThieuLoi.Enabled = true;
        }

        private void ConfigureTicketLookupUi()
        {
            if (_appRole != AppRole.User)
                return;

            lblTraCuuPhieuMuon.Text = "📋  Phiếu mượn của bạn";
            lblNgayMuon.Text = "Chọn phiếu:";
            lblSoPhieu.Visible = false;
            txtSoPhieu.Visible = false;
            btnTim.Visible = false;

            lblNgayMuon.Location = lblSoPhieu.Location;
            cmbNgayMuon.Location = txtSoPhieu.Location;
            cmbNgayMuon.Size = new Size(btnTim.Right - txtSoPhieu.Left, txtSoPhieu.Height);
        }

        private string BuildReturnNote(IEnumerable<(int DeviceID, int InstanceID, int BorrowQty, int ReturnQty, string Note)> returnItems)
        {
            string baseNote = txtGhiChuTra.Text.Trim();
            if (!_allowPartialReturn)
                return string.IsNullOrWhiteSpace(baseNote)
                    ? $"Yêu cầu trả thiết bị ngày {dtpNgayTra.Value:dd/MM/yyyy}."
                    : baseNote;

            var issueNotes = returnItems
                .Where(x => x.ReturnQty < x.BorrowQty || !string.IsNullOrWhiteSpace(x.Note))
                .Select(x => $"Cá Thể #{x.InstanceID}: trả {x.ReturnQty}/{x.BorrowQty}" +
                             (string.IsNullOrWhiteSpace(x.Note) ? string.Empty : $" - {x.Note.Trim()}"))
                .ToList();

            if (!string.IsNullOrWhiteSpace(txtHanTra2.Text))
                issueNotes.Insert(0, txtHanTra2.Text.Trim());

            if (issueNotes.Count == 0)
                return baseNote;

            return string.IsNullOrWhiteSpace(baseNote)
                ? string.Join(" | ", issueNotes)
                : baseNote + " | " + string.Join(" | ", issueNotes);
        }

        private static string GetReturnCondition(DataRow row)
        {
            string returnCondition = GetCellText(row, "Tình trạng khi trả");
            string note = GetCellText(row, "Ghi chú");
            string condition = string.IsNullOrWhiteSpace(returnCondition) ? "Tốt" : returnCondition.Trim();

            return string.IsNullOrWhiteSpace(note)
                ? condition
                : $"{condition} - {note.Trim()}";
        }

        private static bool IsGoodReturnCondition(string condition)
        {
            return string.IsNullOrWhiteSpace(condition) ||
                   condition.Trim().Equals("Tốt", StringComparison.OrdinalIgnoreCase);
        }

        private static string GetCellText(DataRow row, string columnName)
        {
            if (!row.Table.Columns.Contains(columnName) || row[columnName] == DBNull.Value)
                return string.Empty;

            return row[columnName]?.ToString() ?? string.Empty;
        }

        // ═══════════════════════════════════════════════════════════════
        //  ADMIN: Pending Return Approval Panel
        // ═══════════════════════════════════════════════════════════════

        private void SetupPendingReturnPanel()
        {
            _pnlPendingReturns.Visible = true;
            _dgvPendingReturns.SelectionChanged += DgvPendingReturns_SelectionChanged;
            _dgvPendingReturns.CellDoubleClick += (_, _) => LoadSelectedPendingReturnDetails();
            _btnDuyetTra.Click += BtnDuyetTra_Click;
            _btnTuChoiTra.Click += BtnTuChoiTra_Click;
            LoadPendingReturnTickets();
        }

        private void LayoutPendingPanel()
        {
            // Được designer quản lý qua Dock và Anchor
        }

        private void LoadPendingReturnTickets()
        {
            if (_dgvPendingReturns == null) return;
            try
            {
                _dgvPendingReturns.DataSource = ReturnApprovalService.GetPendingReturnTickets();
                if (_dgvPendingReturns.Columns.Contains(Col.TicketID))
                    _dgvPendingReturns.Columns[Col.TicketID].Visible = false;
                SyncPendingReturnSelection();
            }
            catch (Exception ex)
            {
                AppLogger.Error($"[UcTrathietbi] Lỗi load pending", ex);
            }
        }

        private void DgvPendingReturns_SelectionChanged(object? sender, EventArgs e)
        {
            SyncPendingReturnSelection();
        }

        private void SyncPendingReturnSelection()
        {
            if (!TryGetSelectedPendingTicketId(out int tid))
            {
                _pendingSelectedTicketID = -1;
                if (_btnDuyetTra != null) _btnDuyetTra.Enabled = false;
                if (_btnTuChoiTra != null) _btnTuChoiTra.Enabled = false;
                return;
            }

            _pendingSelectedTicketID = tid;
            _btnDuyetTra.Enabled = true;
            _btnTuChoiTra.Enabled = true;
            LoadSelectedPendingReturnDetails();
        }

        private bool TryGetSelectedPendingTicketId(out int ticketId)
        {
            ticketId = -1;
            if (_dgvPendingReturns == null || !_dgvPendingReturns.Columns.Contains(Col.TicketID))
                return false;

            DataGridViewRow? row = _dgvPendingReturns.SelectedRows.Count > 0
                ? _dgvPendingReturns.SelectedRows[0]
                : _dgvPendingReturns.CurrentRow;

            if (row == null || row.Index < 0)
                return false;

            object? ticketVal = row.Cells[Col.TicketID].Value;
            return ticketVal != null && int.TryParse(ticketVal.ToString(), out ticketId);
        }

        private void LoadSelectedPendingReturnDetails()
        {
            if (_pendingSelectedTicketID <= 0)
                return;

            LoadTicketDetails(_pendingSelectedTicketID, isPendingApprovalView: true);
        }

        private List<DataGridViewRow> GetSelectedGridRows()
        {
            var rows = new List<DataGridViewRow>();
            var addedIndices = new HashSet<int>();

            foreach (DataGridViewRow row in dgvData.SelectedRows)
            {
                if (row.Index >= 0 && !addedIndices.Contains(row.Index))
                {
                    rows.Add(row);
                    addedIndices.Add(row.Index);
                }
            }

            foreach (DataGridViewCell cell in dgvData.SelectedCells)
            {
                if (cell.RowIndex >= 0 && !addedIndices.Contains(cell.RowIndex))
                {
                    DataGridViewRow row = dgvData.Rows[cell.RowIndex];
                    rows.Add(row);
                    addedIndices.Add(cell.RowIndex);
                }
            }

            return rows;
        }

        private void BtnDuyetTra_Click(object? sender, EventArgs e)
        {
            if (!TryGetSelectedPendingTicketId(out int ticketId)) return;
            _pendingSelectedTicketID = ticketId;

            if (MessageBox.Show($"Xác nhận duyệt trả toàn bộ phiếu #{ticketId}?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            try
            {
                ReturnApprovalService.ApproveReturn(ticketId, _currentUserId);
                MessageBox.Show("Đã duyệt trả thiết bị thành công.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
                LoadBorrowingTickets(selectFirst: false);
                LoadPendingReturnTickets();
            }
            catch (Exception ex)
            {
                LoadPendingReturnTickets();
                MessageBox.Show("Lỗi khi duyệt trả: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnTuChoiTra_Click(object? sender, EventArgs e)
        {
            if (!TryGetSelectedPendingTicketId(out int ticketId)) return;
            _pendingSelectedTicketID = ticketId;

            string reason = Microsoft.VisualBasic.Interaction.InputBox("Lý do từ chối yêu cầu trả?", "Từ chối trả");
            if (string.IsNullOrWhiteSpace(reason)) return;

            try
            {
                ReturnApprovalService.RejectReturn(ticketId, reason);
                MessageBox.Show("Đã từ chối yêu cầu trả. Phiếu quay lại trạng thái đang mượn.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
                LoadBorrowingTickets(selectFirst: false);
                LoadPendingReturnTickets();
            }
            catch (Exception ex)
            {
                LoadPendingReturnTickets();
                MessageBox.Show("Lỗi khi từ chối trả: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}



