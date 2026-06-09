using FrmProject.GUI;
using System.Data;

namespace FrmProject.GUI
{
    public partial class UcDanhsachphieu : UserControl
    {
        private readonly int _currentUserId;
        private readonly AppRole _appRole;

        public UcDanhsachphieu(int currentUserId = 0, AppRole appRole = AppRole.Staff)
        {
            _currentUserId = currentUserId;
            _appRole = appRole;
            InitializeComponent();
            ViewHelper.ApplyBaseStyle(this);
            this.Load += UcDanhsachphieu_Load;

            ConfigureActionPanel(); // Initialize dynamic action panel
            pnlActions.Resize += (_, _) => LayoutActionPanel();
            LayoutActionPanel();
            ConfigureTicketInfoGrid();

            btnLoc.Click += BtnLoc_Click;      // ⚡ Lọc
            btnReset.Click += BtnReset_Click;    // ↺ Reset
            txtTatCa128.TextChanged += (s, e) => LoadData();
            cmbDangMuon.SelectedIndexChanged += (s, e) => LoadData();

            dgvData.CellClick += DgvData_CellClick;
            dgvData.CellMouseClick += DgvData_CellMouseClick;
            dgvData.CellPainting += DgvData_CellPainting;
            dgvData.SelectionChanged += DgvData_SelectionChanged;
            dgvData.DataBindingComplete += DgvData_DataBindingComplete;
            dgvData.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvData.MultiSelect = false;
        }

        private int _selectedTicketID = -1;
        private string _selectedStatus = "";
        private bool _isClearingGridSelection;
        private bool _hasLoaded;

        private void ConfigureTicketInfoGrid()
        {
            dgvThongTinPhieu.AllowUserToAddRows = false;
            dgvThongTinPhieu.AllowUserToDeleteRows = false;
            dgvThongTinPhieu.AllowUserToResizeRows = false;
            dgvThongTinPhieu.AutoGenerateColumns = false;
            dgvThongTinPhieu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvThongTinPhieu.BackgroundColor = Color.White;
            dgvThongTinPhieu.BorderStyle = BorderStyle.None;
            dgvThongTinPhieu.ColumnHeadersHeight = 34;
            dgvThongTinPhieu.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvThongTinPhieu.EnableHeadersVisualStyles = false;
            dgvThongTinPhieu.GridColor = Color.FromArgb(235, 239, 245);
            dgvThongTinPhieu.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgvThongTinPhieu.Location = new Point(15, 76);
            dgvThongTinPhieu.Margin = new Padding(3, 4, 3, 4);
            dgvThongTinPhieu.MultiSelect = false;
            dgvThongTinPhieu.Name = "dgvThongTinPhieu";
            dgvThongTinPhieu.ReadOnly = true;
            dgvThongTinPhieu.RowHeadersVisible = false;
            dgvThongTinPhieu.RowTemplate.Height = 24;
            dgvThongTinPhieu.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvThongTinPhieu.Size = new Size(1148, 151);

            if (dgvThongTinPhieu.Columns.Count == 0)
            {
                dgvThongTinPhieu.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Trường 1",
                    HeaderText = "Thông tin",
                    Name = "colInfoField1",
                    FillWeight = 90F,
                    ReadOnly = true
                });
                dgvThongTinPhieu.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Giá trị 1",
                    HeaderText = "Giá trị",
                    Name = "colInfoValue1",
                    FillWeight = 160F,
                    ReadOnly = true
                });
                dgvThongTinPhieu.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Trường 2",
                    HeaderText = "Thông tin",
                    Name = "colInfoField2",
                    FillWeight = 90F,
                    ReadOnly = true
                });
                dgvThongTinPhieu.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Giá trị 2",
                    HeaderText = "Giá trị",
                    Name = "colInfoValue2",
                    FillWeight = 160F,
                    ReadOnly = true
                });
            }

            HideOldTicketInfoLabels();
            dgvChiTietThietBi.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgvChiTietThietBi.Location = new Point(15, 241);
            dgvChiTietThietBi.Size = new Size(1148, 96);
            pnlChiTietLine.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlChiTietLine.Size = new Size(1148, 1);

            if (!pnlChiTietPhieu.Controls.Contains(dgvThongTinPhieu))
                pnlChiTietPhieu.Controls.Add(dgvThongTinPhieu);
            dgvThongTinPhieu.BringToFront();
            ApplyTicketInfoGridTheme();
        }

        private void HideOldTicketInfoLabels()
        {
            lblCTSoPhieu.Visible = false;
            lblCTSoPhieuTitle.Visible = false;
            lblCTNguoiMuon.Visible = false;
            lblCTNguoiMuonTitle.Visible = false;
            lblCTPhong.Visible = false;
            lblCTPhongTitle.Visible = false;
            lblCTMucDich.Visible = false;
            lblCTMucDichTitle.Visible = false;
            lblCTNgayMuon.Visible = false;
            lblCTNgayMuonTitle.Visible = false;
            lblCTHanTra.Visible = false;
            lblCTHanTraTitle.Visible = false;
            lblCTNguoiDuyet.Visible = false;
            lblCTNguoiDuyetTitle.Visible = false;
            lblCTTrangThai.Visible = false;
            lblCTTrangThaiTitle.Visible = false;
        }

        private void UcDanhsachphieu_Load(object sender, EventArgs e)
        {
            EnsureStatusFilterItems();
            ApplyGridTheme();
            ApplyDetailGridTheme();
            ClearTicketDetail();
            if (_appRole == AppRole.User)
                lblDanhSachPhieuMuon.Text = "🗒  Phiếu Mượn Của Tôi";
            cmbDangMuon.SelectedIndex = 0;
            dtpTuNgay.Value = DateTime.Now.AddMonths(-3);
            dtpDenNgay.Value = DateTime.Now;
            LoadData();
            LoadStats();
            _hasLoaded = true;
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);

            if (!Visible || !_hasLoaded || IsDisposed)
                return;

            LoadData();
            LoadStats();
        }

        private void LoadData()
        {
            try
            {
                string keyword = txtTatCa128.Text.Trim();
                string statusFilter = cmbDangMuon.Text;

                DataTable dt = TicketListRepository.SearchTickets(
                    dtpTuNgay.Value.Date,
                    dtpDenNgay.Value.Date.AddDays(1),
                    keyword,
                    statusFilter,
                    _currentUserId,
                    _appRole);

                dgvData.AutoGenerateColumns = false;
                colTicketID.DataPropertyName = "ID phiếu";
                colSoPhieu.DataPropertyName = "Số phiếu";
                colNgayLap.DataPropertyName = "Ngày lập";
                ColNguoimuon.DataPropertyName = "Người mượn";
                colPhong.DataPropertyName = "Phòng";
                colSoluong.DataPropertyName = "Số lượng";
                colNgayMuon.DataPropertyName = "Ngày mượn";
                colHanTra.DataPropertyName = "Hạn trả";
                colNgaytra.DataPropertyName = "Ngày trả";
                colTrangThai.DataPropertyName = "Trạng thái";
                colThaoTac.DataPropertyName = "";
                colThaoTac.HeaderText = "Thao tác";
                colThaoTac.ReadOnly = true;

                dgvData.DataSource = dt;
                ApplyGridTheme();
                ForceGridTextColors();
                ApplyActionColumn();
                ResetSelectionState();
                ClearGridSelection();

                // Highlight overdue rows
                foreach (DataGridViewRow row in dgvData.Rows)
                {
                    row.DefaultCellStyle.ForeColor = Color.Black;
                    row.DefaultCellStyle.SelectionForeColor = Color.Black;

                    if (row.Cells["colTrangThai"].Value?.ToString() == "Quá hạn")
                    {
                        row.DefaultCellStyle.BackColor = Color.FromArgb(255, 220, 220);
                        row.DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 220, 220);
                    }
                    else if (row.Cells["colTrangThai"].Value?.ToString() == "Đã trả")
                    {
                        row.DefaultCellStyle.BackColor = Color.FromArgb(220, 255, 220);
                        row.DefaultCellStyle.SelectionBackColor = Color.FromArgb(220, 255, 220);
                    }
                    else
                    {
                        row.DefaultCellStyle.BackColor = Color.White;
                        row.DefaultCellStyle.SelectionBackColor = Color.FromArgb(225, 239, 252);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadStats()
        {
            try
            {
                TicketListStats stats = TicketListRepository.GetStats(_currentUserId, _appRole);
                lblTatCa128.Text = "Tất cả: " + stats.Total;
                lblChoDuyet3.Text = "Chờ duyệt: " + stats.Pending;
                lblDangMuon.Text = _appRole == AppRole.User ? "Đang giữ: " + stats.Active : "Đang mượn: " + stats.Active;
                lblQuaHan5.Text = "Quá hạn: " + stats.Overdue;
            }
            catch (Exception ex) { AppLogger.Error($"[UcDanhsachphieu] Lỗi LoadStats", ex); }
        }

        private void BtnLoc_Click(object sender, EventArgs e)
        {
            LoadData();
            LoadStats();
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            txtTatCa128.Text = "";
            cmbDangMuon.SelectedIndex = 0;
            dtpTuNgay.Value = DateTime.Now.AddMonths(-3);
            dtpDenNgay.Value = DateTime.Now;
            LoadData();
            LoadStats();
        }

        private void DgvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex >= 0 && dgvData.Columns[e.ColumnIndex].Name == "colThaoTac")
                return;

            ApplySelectedRowState(dgvData.Rows[e.RowIndex]);
        }

        private void DgvData_CellMouseClick(object? sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return;
            TryHandleGridAction(e.RowIndex, e.ColumnIndex, e.X);
        }

        private void DgvData_CellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 ||
                e.ColumnIndex < 0 ||
                dgvData.Columns[e.ColumnIndex].Name != "colThaoTac")
                return;

            string actionText = e.Value?.ToString() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(actionText) || e.Graphics == null)
                return;

            e.Handled = true;
            e.PaintBackground(e.CellBounds, true);
            e.Paint(e.CellBounds, DataGridViewPaintParts.Border);

            Rectangle inner = Rectangle.Inflate(e.CellBounds, -6, -6);
            int gap = 6;
            int buttonWidth = (inner.Width - gap) / 2;
            Rectangle approveRect = new(inner.Left, inner.Top, buttonWidth, inner.Height);
            Rectangle rejectRect = new(approveRect.Right + gap, inner.Top, buttonWidth, inner.Height);

            using SolidBrush approveBrush = new(Color.FromArgb(22, 163, 74));
            using SolidBrush rejectBrush = new(Color.FromArgb(220, 38, 38));
            using SolidBrush textBrush = new(Color.White);
            using Font iconFont = new("Segoe UI", 11F, FontStyle.Bold);
            using StringFormat centerFormat = new()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            e.Graphics.FillRectangle(approveBrush, approveRect);
            e.Graphics.FillRectangle(rejectBrush, rejectRect);
            e.Graphics.DrawString("✓", iconFont, textBrush, approveRect, centerFormat);
            e.Graphics.DrawString("X", iconFont, textBrush, rejectRect, centerFormat);
        }

        private bool TryHandleGridAction(int rowIndex, int columnIndex, int clickX)
        {
            if (rowIndex < 0 ||
                columnIndex < 0 ||
                dgvData.Columns[columnIndex].Name != "colThaoTac" ||
                _appRole == AppRole.User)
                return false;

            DataGridViewRow row = dgvData.Rows[rowIndex];
            string actionText = row.Cells["colThaoTac"].Value?.ToString() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(actionText))
                return false;

            ApplySelectedRowState(row);
            if (_selectedTicketID <= 0)
                return true;

            string action = ResolveGridAction(rowIndex, clickX, actionText);

            if (action == "Duyệt mượn")
                ApproveBorrowFromGrid();
            else if (action == "Duyệt trả")
                ApproveReturnRequest();
            else if (action == "Từ chối mượn")
                RejectBorrowFromGrid();
            else if (action == "Từ chối trả")
                RejectReturnRequest();

            return true;
        }

        private string ResolveGridAction(int rowIndex, int clickX, string actionText)
        {
            string status = dgvData.Rows[rowIndex].Cells["colTrangThai"].Value?.ToString() ?? string.Empty;
            bool clickedRejectSide = clickX > (dgvData.Columns["colThaoTac"].Width / 2);

            if (status == "Chờ duyệt")
                return clickedRejectSide ? "Từ chối mượn" : "Duyệt mượn";
            if (status == "Chờ duyệt trả")
                return clickedRejectSide ? "Từ chối trả" : "Duyệt trả";

            return actionText;
        }

        private void DgvData_SelectionChanged(object? sender, EventArgs e)
        {
            if (_isClearingGridSelection)
                return;

            if (dgvData.CurrentRow == null || dgvData.CurrentRow.Index < 0)
            {
                ResetSelectionState();
                return;
            }

            ApplySelectedRowState(dgvData.CurrentRow);
        }

        private void ApplySelectedRowState(DataGridViewRow row)
        {
            if (row.Cells["colTicketID"].Value == null || !int.TryParse(row.Cells["colTicketID"].Value.ToString(), out int ticketId))
            {
                ResetSelectionState();
                return;
            }

            _selectedTicketID = ticketId;
            _selectedStatus = row.Cells["colTrangThai"].Value?.ToString() ?? string.Empty;
            string ticketCode = GetCellText(row, "colSoPhieu");
            string ticketLabel = string.IsNullOrWhiteSpace(ticketCode)
                ? $"#{_selectedTicketID}"
                : $"#{_selectedTicketID} - {ticketCode}";

            bool canApproveBorrow = _selectedStatus == "Chờ duyệt";
            bool canApproveReturn = _selectedStatus == "Chờ duyệt trả";
            bool canProcess = _appRole != AppRole.User && (canApproveBorrow || canApproveReturn);

            if (canApproveBorrow)
            {
                btnPheDuyet.Text = "✓";
                btnTuChoi.Text = "X";
            }
            else if (canApproveReturn)
            {
                btnPheDuyet.Text = "✓";
                btnTuChoi.Text = "X";
            }

            lblChonPhieu.Text = canProcess
                ? $"Phiếu đang chọn: {ticketLabel} ({_selectedStatus})"
                : $"Phiếu đang chọn: {ticketLabel}";
            pnlActions.Visible = canProcess;
            btnPheDuyet.Visible = canProcess;

            ShowBasicTicketDetail(row);
            LoadTicketDetail(_selectedTicketID);
        }

        private void LayoutActionPanel()
        {
            if (pnlActions == null || btnPheDuyet == null || btnTuChoi == null || lblChonPhieu == null)
                return;

            int margin = 12;
            int gap = 10;
            btnTuChoi.Location = new Point(pnlActions.ClientSize.Width - margin - btnTuChoi.Width, 12);
            btnPheDuyet.Location = new Point(btnTuChoi.Left - gap - btnPheDuyet.Width, 12);
            lblChonPhieu.Location = new Point(margin, 18);
            lblChonPhieu.Size = new Size(Math.Max(120, btnPheDuyet.Left - margin - gap), 24);
        }

        private void ApproveBorrowFromGrid()
        {
            if (MessageBox.Show($"Xác nhận phê duyệt phiếu mượn #{_selectedTicketID}?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            try
            {
                BorrowApprovalService.ApproveBorrow(_selectedTicketID);
                MessageBox.Show("Đã phê duyệt phiếu thành công! Thiết bị đã được xuất kho.",
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ResetSelectionState();
                LoadData();
                LoadStats();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi phê duyệt: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowBasicTicketDetail(DataGridViewRow row)
        {
            string soPhieu = GetCellText(row, "colSoPhieu");
            string nguoiMuon = GetCellText(row, "ColNguoimuon");
            string phong = GetCellText(row, "colPhong");
            string ngayMuon = GetCellText(row, "colNgayMuon");
            string hanTra = GetCellText(row, "colHanTra");
            string ngayTra = GetCellText(row, "colNgaytra");
            string trangThai = GetCellText(row, "colTrangThai");
            string soLuong = GetCellText(row, "colSoluong");
            string ghiChu = GetBoundFieldText(row, "Ghi chú");

            lblChiTietTitle.Text = $"📋 Chi tiết phiếu {soPhieu}";
            LoadTicketInfoGrid(
                soPhieu,
                string.IsNullOrWhiteSpace(nguoiMuon) ? "Chưa có" : nguoiMuon,
                string.IsNullOrWhiteSpace(phong) ? "Chưa có" : phong,
                string.IsNullOrWhiteSpace(ngayMuon) ? "-" : ngayMuon,
                string.IsNullOrWhiteSpace(hanTra) ? "-" : hanTra,
                string.IsNullOrWhiteSpace(ngayTra) ? "Chưa trả" : ngayTra,
                "Chưa có",
                string.IsNullOrWhiteSpace(ghiChu) ? "Chưa có" : ghiChu,
                string.IsNullOrWhiteSpace(trangThai) ? "-" : trangThai,
                string.IsNullOrWhiteSpace(soLuong) ? "0" : soLuong);

            dgvChiTietThietBi.DataSource = null;
            pnlChiTietPhieu.Visible = true;
            pnlChiTietPhieu.BringToFront();
        }

        private static string GetCellText(DataGridViewRow row, string columnName)
        {
            return row.DataGridView != null && row.DataGridView.Columns.Contains(columnName)
                ? row.Cells[columnName].Value?.ToString() ?? string.Empty
                : string.Empty;
        }

        private static string GetBoundFieldText(DataGridViewRow row, string fieldName)
        {
            return row.DataBoundItem is DataRowView view && view.Row.Table.Columns.Contains(fieldName)
                ? view.Row[fieldName]?.ToString() ?? string.Empty
                : string.Empty;
        }

        private void BtnPheDuyet_Click(object sender, EventArgs e)
        {
            if (_selectedTicketID <= 0) return;
            if (_selectedStatus == "Chờ duyệt trả")
            {
                ApproveReturnRequest();
                return;
            }

            if (MessageBox.Show($"Xác nhận phê duyệt phiếu mượn #{_selectedTicketID}?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            try
            {
                BorrowApprovalService.ApproveBorrow(_selectedTicketID);
                MessageBox.Show("Đã phê duyệt phiếu thành công! Thiết bị đã được xuất kho.",
                    "Thành công", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);

                ResetSelectionState();
                LoadData();
                LoadStats();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi phê duyệt: " + ex.Message, "Lỗi", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        private void BtnTuChoi_Click(object sender, EventArgs e)
        {
            if (_selectedTicketID <= 0) return;
            if (_selectedStatus == "Chờ duyệt trả")
            {
                RejectReturnRequest();
                return;
            }

            string reason = Microsoft.VisualBasic.Interaction.InputBox("Lý do từ chối phiếu mượn này?", "Từ chối phiếu");
            if (string.IsNullOrWhiteSpace(reason)) return;

            try
            {
                BorrowApprovalService.RejectBorrow(_selectedTicketID, reason);
                MessageBox.Show("Đã từ chối phiếu mượn.", "Thông báo", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                ResetSelectionState();
                LoadData();
                LoadStats();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi từ chối: " + ex.Message, "Lỗi", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        private void RejectBorrowFromGrid()
        {
            if (_selectedTicketID <= 0) return;

            string reason = Microsoft.VisualBasic.Interaction.InputBox("Lý do từ chối phiếu mượn này?", "Từ chối phiếu");
            if (string.IsNullOrWhiteSpace(reason)) return;

            try
            {
                BorrowApprovalService.RejectBorrow(_selectedTicketID, reason);
                MessageBox.Show("Đã từ chối phiếu mượn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ResetSelectionState();
                LoadData();
                LoadStats();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi từ chối: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetSelectionState()
        {
            _selectedTicketID = -1;
            _selectedStatus = string.Empty;
            lblChonPhieu.Text = "Phiếu đã chọn:";
            pnlActions.Visible = false;
            ClearTicketDetail();
            ClearGridSelection();
        }

        private void LoadTicketDetail(int ticketId)
        {
            try
            {
                TicketDetailView? detail = TicketListRepository.GetTicketDetail(ticketId, _currentUserId, _appRole);
                if (detail == null)
                {
                    ClearTicketDetail();
                    return;
                }

                lblChiTietTitle.Text = $"📋 Chi tiết phiếu {detail.TicketDisplay}";
                lblCTSoPhieu.Text = detail.TicketDisplay;
                lblCTNguoiMuon.Text = detail.BorrowerName;
                lblCTPhong.Text = string.IsNullOrWhiteSpace(detail.RoomName) ? "Chưa có" : detail.RoomName;
                lblCTMucDich.Text = string.IsNullOrWhiteSpace(detail.Purpose) ? "Chưa có" : detail.Purpose;
                lblCTNgayMuon.Text = detail.BorrowDate.ToString("dd/MM/yyyy");
                lblCTHanTra.Text = detail.ExpectedReturnDate.ToString("dd/MM/yyyy");
                lblCTNguoiDuyet.Text = string.IsNullOrWhiteSpace(detail.ApprovedByName) ? "Chưa có" : detail.ApprovedByName;

                string statusText = detail.OverdueDays > 0
                    ? $"Quá hạn {detail.OverdueDays} ngày"
                    : detail.StatusText;
                SetDetailStatus(statusText);
                LoadTicketInfoGrid(detail, statusText);

                dgvChiTietThietBi.AutoGenerateColumns = false;
                dgvChiTietThietBi.DataSource = detail.Items;
                ApplyDetailGridTheme();
                SyncSelectedGridRow(detail, statusText);
                pnlChiTietPhieu.Visible = true;
            }
            catch (Exception ex)
            {
                AppLogger.Error($"[UcDanhsachphieu] Lỗi load chi tiết phiếu #{ticketId}", ex);
            }
        }

        private void SyncSelectedGridRow(TicketDetailView detail, string statusText)
        {
            if (dgvData.CurrentRow == null || dgvData.CurrentRow.Index < 0)
                return;

            if (dgvData.CurrentRow.Cells["colTicketID"].Value == null ||
                !int.TryParse(dgvData.CurrentRow.Cells["colTicketID"].Value.ToString(), out int rowTicketId) ||
                rowTicketId != _selectedTicketID)
                return;

            dgvData.CurrentRow.Cells["colTrangThai"].Value = detail.StatusText;
            if (detail.ReturnDate.HasValue)
                dgvData.CurrentRow.Cells["colNgaytra"].Value = detail.ReturnDate.Value.ToString("dd/MM/yyyy");

            _selectedStatus = detail.StatusText;

            if (statusText.StartsWith("Quá hạn", StringComparison.OrdinalIgnoreCase))
            {
                dgvData.CurrentRow.DefaultCellStyle.BackColor = Color.FromArgb(255, 220, 220);
                dgvData.CurrentRow.DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 220, 220);
            }
            else if (detail.StatusText == "Đã trả")
            {
                dgvData.CurrentRow.DefaultCellStyle.BackColor = Color.FromArgb(220, 255, 220);
                dgvData.CurrentRow.DefaultCellStyle.SelectionBackColor = Color.FromArgb(220, 255, 220);
            }
        }

        private void ClearTicketDetail()
        {
            lblChiTietTitle.Text = "📋 Chi tiết phiếu";
            lblCTSoPhieu.Text = "-";
            lblCTNguoiMuon.Text = "-";
            lblCTPhong.Text = "-";
            lblCTMucDich.Text = "-";
            lblCTNgayMuon.Text = "-";
            lblCTHanTra.Text = "-";
            lblCTNguoiDuyet.Text = "-";
            SetDetailStatus("-");
            dgvThongTinPhieu.DataSource = null;
            dgvChiTietThietBi.DataSource = null;
            pnlChiTietPhieu.Visible = false;
        }

        private void LoadTicketInfoGrid(TicketDetailView detail, string statusText)
        {
            LoadTicketInfoGrid(
                detail.TicketDisplay,
                detail.BorrowerName,
                string.IsNullOrWhiteSpace(detail.RoomName) ? "Chưa có" : detail.RoomName,
                detail.BorrowDate.ToString("dd/MM/yyyy"),
                detail.ExpectedReturnDate.ToString("dd/MM/yyyy"),
                detail.ReturnDate.HasValue ? detail.ReturnDate.Value.ToString("dd/MM/yyyy") : "Chưa trả",
                string.IsNullOrWhiteSpace(detail.ApprovedByName) ? "Chưa có" : detail.ApprovedByName,
                string.IsNullOrWhiteSpace(detail.Purpose) ? "Chưa có" : detail.Purpose,
                statusText,
                SumBorrowedQuantity(detail.Items).ToString());
        }

        private void LoadTicketInfoGrid(
            string ticketDisplay,
            string borrowerName,
            string roomName,
            string borrowDate,
            string expectedReturnDate,
            string returnDate,
            string approvedByName,
            string purpose,
            string statusText,
            string totalQuantity)
        {
            DataTable info = new DataTable();
            info.Columns.Add("Trường 1");
            info.Columns.Add("Giá trị 1");
            info.Columns.Add("Trường 2");
            info.Columns.Add("Giá trị 2");

            info.Rows.Add(
                "Số phiếu", ticketDisplay,
                "Trạng thái", statusText);
            info.Rows.Add(
                "Người mượn", borrowerName,
                "Phòng", roomName);
            info.Rows.Add(
                "Ngày mượn", borrowDate,
                "Hạn trả", expectedReturnDate);
            info.Rows.Add(
                "Ngày trả", returnDate,
                "Người duyệt", approvedByName);
            info.Rows.Add(
                "Mục đích", purpose,
                "Số thiết bị", totalQuantity);

            dgvThongTinPhieu.DataSource = info;
            ApplyTicketInfoGridTheme();
        }

        private static int SumBorrowedQuantity(DataTable items)
        {
            string colName = items.Columns.Contains("OriginalQuantity") ? "OriginalQuantity" : "Số lượng";
            return items.Rows.Cast<DataRow>()
                .Sum(row => int.TryParse(Convert.ToString(row[colName]), out int quantity) ? quantity : 0);
        }

        private void SetDetailStatus(string statusText)
        {
            lblCTTrangThai.Text = statusText;

            if (statusText.StartsWith("Quá hạn", StringComparison.OrdinalIgnoreCase))
            {
                lblCTTrangThai.BackColor = Color.FromArgb(255, 224, 224);
                lblCTTrangThai.ForeColor = Color.FromArgb(190, 18, 18);
                lblCTHanTra.ForeColor = Color.FromArgb(220, 38, 38);
                return;
            }

            lblCTHanTra.ForeColor = Color.FromArgb(10, 24, 50);
            switch (statusText)
            {
                case "Đã trả":
                    lblCTTrangThai.BackColor = Color.FromArgb(220, 252, 231);
                    lblCTTrangThai.ForeColor = Color.FromArgb(22, 101, 52);
                    break;
                case "Đang mượn":
                    lblCTTrangThai.BackColor = Color.FromArgb(219, 234, 254);
                    lblCTTrangThai.ForeColor = Color.FromArgb(30, 64, 175);
                    break;
                case "Chờ duyệt":
                case "Chờ duyệt trả":
                    lblCTTrangThai.BackColor = Color.FromArgb(254, 243, 199);
                    lblCTTrangThai.ForeColor = Color.FromArgb(146, 64, 14);
                    break;
                case "Từ chối":
                    lblCTTrangThai.BackColor = Color.FromArgb(243, 244, 246);
                    lblCTTrangThai.ForeColor = Color.FromArgb(75, 85, 99);
                    break;
                default:
                    lblCTTrangThai.BackColor = Color.FromArgb(225, 245, 234);
                    lblCTTrangThai.ForeColor = Color.FromArgb(25, 135, 84);
                    break;
            }
        }

        private void ApproveReturnRequest()
        {
            if (MessageBox.Show($"Xác nhận duyệt trả phiếu #{_selectedTicketID}?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            try
            {
                ReturnApprovalService.ApproveReturn(_selectedTicketID, _currentUserId);
                MessageBox.Show("Đã duyệt trả thiết bị thành công.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                ResetSelectionState();
                LoadData();
                LoadStats();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi duyệt trả: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RejectReturnRequest()
        {
            string reason = Microsoft.VisualBasic.Interaction.InputBox("Lý do từ chối yêu cầu trả?", "Từ chối trả");
            if (string.IsNullOrWhiteSpace(reason)) return;

            try
            {
                ReturnApprovalService.RejectReturn(_selectedTicketID, reason);
                MessageBox.Show("Đã từ chối yêu cầu trả. Phiếu quay lại trạng thái đang mượn.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                ResetSelectionState();
                LoadData();
                LoadStats();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi từ chối trả: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EnsureStatusFilterItems()
        {
            if (!cmbDangMuon.Items.Contains("Chờ duyệt trả"))
                cmbDangMuon.Items.Add("Chờ duyệt trả");
        }

        private void ApplyGridTheme()
        {
            dgvData.BackgroundColor = Color.White;
            dgvData.GridColor = Color.FromArgb(220, 220, 220);
            dgvData.EnableHeadersVisualStyles = false;
            dgvData.DefaultCellStyle.BackColor = Color.White;
            dgvData.DefaultCellStyle.ForeColor = Color.Black;
            dgvData.DefaultCellStyle.SelectionBackColor = Color.FromArgb(225, 239, 252);
            dgvData.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvData.RowsDefaultCellStyle.BackColor = Color.White;
            dgvData.RowsDefaultCellStyle.ForeColor = Color.Black;
            dgvData.RowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(225, 239, 252);
            dgvData.RowsDefaultCellStyle.SelectionForeColor = Color.Black;
            dgvData.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
            dgvData.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;
            dgvData.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(225, 239, 252);
            dgvData.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.Black;
            dgvData.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(235, 240, 248);
            dgvData.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgvData.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(235, 240, 248);
            dgvData.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.Black;
            dgvData.RowTemplate.DefaultCellStyle.BackColor = Color.White;
            dgvData.RowTemplate.DefaultCellStyle.ForeColor = Color.Black;
            dgvData.RowTemplate.DefaultCellStyle.SelectionBackColor = Color.FromArgb(225, 239, 252);
            dgvData.RowTemplate.DefaultCellStyle.SelectionForeColor = Color.Black;
        }

        private void ApplyActionColumn()
        {
            if (!dgvData.Columns.Contains("colThaoTac") || !dgvData.Columns.Contains("colTrangThai"))
                return;

            colThaoTac.HeaderText = "Thao tác";
            colThaoTac.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colThaoTac.DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            foreach (DataGridViewRow row in dgvData.Rows)
            {
                string status = row.Cells["colTrangThai"].Value?.ToString() ?? string.Empty;
                DataGridViewCell actionCell = row.Cells["colThaoTac"];

                actionCell.Value = status switch
                {
                    "Chờ duyệt" => "✓|X",
                    "Chờ duyệt trả" => "✓|X",
                    _ => string.Empty
                };

                if (string.IsNullOrWhiteSpace(actionCell.Value?.ToString()))
                {
                    actionCell.Style.BackColor = row.DefaultCellStyle.BackColor.IsEmpty ? Color.White : row.DefaultCellStyle.BackColor;
                    actionCell.Style.ForeColor = Color.Black;
                    actionCell.Style.SelectionBackColor = row.DefaultCellStyle.SelectionBackColor.IsEmpty
                        ? Color.FromArgb(225, 239, 252)
                        : row.DefaultCellStyle.SelectionBackColor;
                    actionCell.Style.SelectionForeColor = Color.Black;
                    continue;
                }

                actionCell.Style.BackColor = Color.White;
                actionCell.Style.ForeColor = Color.Black;
                actionCell.Style.SelectionBackColor = Color.FromArgb(245, 248, 252);
                actionCell.Style.SelectionForeColor = Color.Black;
            }
        }

        private void ApplyDetailGridTheme()
        {
            dgvChiTietThietBi.BackgroundColor = Color.White;
            dgvChiTietThietBi.GridColor = Color.FromArgb(235, 239, 245);
            dgvChiTietThietBi.EnableHeadersVisualStyles = false;
            dgvChiTietThietBi.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(240, 244, 249);
            dgvChiTietThietBi.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(85, 105, 135);
            dgvChiTietThietBi.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dgvChiTietThietBi.DefaultCellStyle.BackColor = Color.White;
            dgvChiTietThietBi.DefaultCellStyle.ForeColor = Color.FromArgb(10, 24, 50);
            dgvChiTietThietBi.DefaultCellStyle.SelectionBackColor = Color.FromArgb(245, 248, 252);
            dgvChiTietThietBi.DefaultCellStyle.SelectionForeColor = Color.FromArgb(10, 24, 50);

            foreach (DataGridViewRow row in dgvChiTietThietBi.Rows)
            {
                row.DefaultCellStyle.BackColor = Color.White;
                row.DefaultCellStyle.ForeColor = Color.FromArgb(10, 24, 50);
                StyleConditionCell(row, "colCTTinhTrang");
                StyleConditionCell(row, "colCTTinhTrangTra");
            }
        }

        private static void StyleConditionCell(DataGridViewRow row, string columnName)
        {
            if (row.DataGridView == null || !row.DataGridView.Columns.Contains(columnName))
                return;

            string value = row.Cells[columnName].Value?.ToString() ?? string.Empty;
            if (value != "Tốt")
                return;

            row.Cells[columnName].Style.BackColor = Color.FromArgb(220, 252, 231);
            row.Cells[columnName].Style.ForeColor = Color.FromArgb(22, 101, 52);
            row.Cells[columnName].Style.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        }

        private void ApplyTicketInfoGridTheme()
        {
            dgvThongTinPhieu.BackgroundColor = Color.White;
            dgvThongTinPhieu.GridColor = Color.FromArgb(235, 239, 245);
            dgvThongTinPhieu.EnableHeadersVisualStyles = false;
            dgvThongTinPhieu.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(240, 244, 249);
            dgvThongTinPhieu.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(85, 105, 135);
            dgvThongTinPhieu.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dgvThongTinPhieu.DefaultCellStyle.BackColor = Color.White;
            dgvThongTinPhieu.DefaultCellStyle.ForeColor = Color.FromArgb(10, 24, 50);
            dgvThongTinPhieu.DefaultCellStyle.SelectionBackColor = Color.FromArgb(245, 248, 252);
            dgvThongTinPhieu.DefaultCellStyle.SelectionForeColor = Color.FromArgb(10, 24, 50);

            foreach (DataGridViewRow row in dgvThongTinPhieu.Rows)
            {
                row.DefaultCellStyle.BackColor = Color.White;
                row.DefaultCellStyle.ForeColor = Color.FromArgb(10, 24, 50);

                StyleInfoGridPair(row, "colInfoField1", "colInfoValue1");
                StyleInfoGridPair(row, "colInfoField2", "colInfoValue2");
            }
        }

        private static void StyleInfoGridPair(DataGridViewRow row, string fieldColumnName, string valueColumnName)
        {
            DataGridView? grid = row.DataGridView;
            if (grid == null ||
                !grid.Columns.Contains(fieldColumnName) ||
                !grid.Columns.Contains(valueColumnName))
                return;

            string fieldName = row.Cells[fieldColumnName].Value?.ToString() ?? string.Empty;
            if (fieldName == "Trạng thái")
                StyleStatusCell(row.Cells[valueColumnName]);
            else if (fieldName == "Hạn trả")
                StyleDueDateCell(row.Cells[valueColumnName]);
        }

        private static void StyleStatusCell(DataGridViewCell cell)
        {
            string statusText = cell.Value?.ToString() ?? string.Empty;
            cell.Style.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            if (statusText.StartsWith("Quá hạn", StringComparison.OrdinalIgnoreCase))
            {
                cell.Style.BackColor = Color.FromArgb(255, 224, 224);
                cell.Style.ForeColor = Color.FromArgb(190, 18, 18);
            }
            else if (statusText == "Đã trả")
            {
                cell.Style.BackColor = Color.FromArgb(220, 252, 231);
                cell.Style.ForeColor = Color.FromArgb(22, 101, 52);
            }
            else if (statusText == "Đang mượn")
            {
                cell.Style.BackColor = Color.FromArgb(219, 234, 254);
                cell.Style.ForeColor = Color.FromArgb(30, 64, 175);
            }
            else if (statusText == "Chờ duyệt" || statusText == "Chờ duyệt trả")
            {
                cell.Style.BackColor = Color.FromArgb(254, 243, 199);
                cell.Style.ForeColor = Color.FromArgb(146, 64, 14);
            }
        }

        private static void StyleDueDateCell(DataGridViewCell cell)
        {
            cell.Style.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            cell.Style.ForeColor = Color.FromArgb(10, 24, 50);
        }

        private void DgvData_DataBindingComplete(object? sender, DataGridViewBindingCompleteEventArgs e)
        {
            ApplyGridTheme();
            ForceGridTextColors();
            ApplyActionColumn();
            ClearGridSelection();
        }

        private void ForceGridTextColors()
        {
            foreach (DataGridViewColumn column in dgvData.Columns)
            {
                column.DefaultCellStyle.ForeColor = Color.Black;
                column.DefaultCellStyle.BackColor = Color.White;
                column.DefaultCellStyle.SelectionForeColor = Color.Black;
                column.DefaultCellStyle.SelectionBackColor = Color.FromArgb(245, 248, 252);
            }

            foreach (DataGridViewRow row in dgvData.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    cell.Style.ForeColor = Color.Black;
                    cell.Style.BackColor = row.DefaultCellStyle.BackColor.IsEmpty ? Color.White : row.DefaultCellStyle.BackColor;
                    cell.Style.SelectionForeColor = Color.Black;
                    cell.Style.SelectionBackColor = row.DefaultCellStyle.SelectionBackColor.IsEmpty
                        ? Color.FromArgb(245, 248, 252)
                        : row.DefaultCellStyle.SelectionBackColor;
                }
            }
        }

        private void ClearGridSelection()
        {
            if (_isClearingGridSelection || dgvData.IsDisposed || !dgvData.IsHandleCreated)
                return;

            _isClearingGridSelection = true;
            try
            {
                dgvData.BeginInvoke(new Action(() =>
                {
                    try
                    {
                        if (dgvData.IsDisposed)
                            return;

                        dgvData.ClearSelection();
                        if (dgvData.CurrentCell != null)
                            dgvData.CurrentCell = null;
                    }
                    finally
                    {
                        _isClearingGridSelection = false;
                    }
                }));
            }
            catch (InvalidOperationException)
            {
                _isClearingGridSelection = false;
            }
        }
    }
}



