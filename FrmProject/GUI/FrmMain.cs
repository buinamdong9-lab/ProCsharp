using System.Data;
using FrmProject.DAL;

namespace FrmProject.GUI
{
    public partial class FrmMain : Form
    {
        private IAuthorizationService AuthorizationService => AppServiceProvider.Get<IAuthorizationService>();
        private IDashboardService DashboardService => AppServiceProvider.Get<IDashboardService>();

        private readonly int _userID;
        private readonly string _fullName;
        private readonly string _roleName;
        private readonly AppRole _appRole;
        private readonly PermissionSet _permissions;
        
        private readonly System.Collections.Generic.Dictionary<string, Control> _viewCache = new();
        private Label _lblLoadingOverlay;

        private int _currentDashboardPage = 1;
        private const int DashboardPageSize = 10;
        private int _totalDashboardPages = 1;
        private int _totalDashboardRecords = 0;


        public FrmMain(int userID, string fullName, string roleName)
        {
            InitializeComponent();
            _userID = userID;
            _fullName = fullName;
            _roleName = roleName;
            _appRole = AuthorizationService.ResolveRole(roleName);
            _permissions = AuthorizationService.BuildPermissions(_appRole);

            pnlContainerelContent.BackColor = Color.FromArgb(245, 246, 248);
            pnlContainerelContent.AutoScroll = true;
            panelContent.BackColor = Color.White;
            pnlContainerelContent.SendToBack();
            EnsureDashboardPanelsAreTopLevel();

            _lblLoadingOverlay = new Label
            {
                Text = "⏳ Đang tải dữ liệu, vui lòng chờ...",
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.FromArgb(220, 255, 255, 255),
                ForeColor = Color.FromArgb(0, 123, 255),
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                Visible = false
            };
            this.Controls.Add(_lblLoadingOverlay);

            this.Resize += FrmMain_Resize;
            this.Load += FrmMain_Load;
            this.FormClosed += (s, e) =>
            {
                // Nếu không còn form nào mở (đóng bằng nút X), thoát app
                if (Application.OpenForms.Count == 0)
                    Application.ExitThread();
            };

            // Sidebar nạp UserControl vào vùng nội dung chính.
            btnTongQuan.Click += (s, e) => LoadDashboard();
            btnThietBi.Click += (s, e) => LoadViewToMain("ThietBi", () => new UcThietbi());
            btnPhongHoc.Click += (s, e) => LoadViewToMain("PhongHoc", () => new UcPhonghoc());
            btnNguoiDung.Click += (s, e) => LoadViewToMain("NguoiDung", () => new UcNguoidung());
            btnLapPhieuMuon.Click += (s, e) => LoadViewToMain("LapPhieu", () => new UcLapthietbi(_userID, _fullName, _appRole));
            btnTraThietBi.Click += (s, e) => LoadViewToMain("TraThietBi", () => new UcTrathietbi(_userID, _appRole));
            btnDanhSachPhieu.Click += (s, e) => LoadViewToMain("DanhSachPhieu", () => new UcDanhsachphieu(_userID, _appRole));
            btnBaoCao.Click += (s, e) => LoadViewToMain("BaoCao", () => new UcBaocao());
            btnCauHinh.Click += (s, e) => LoadViewToMain("CauHinh", () => new UcCauhinh());
            btnThungRac.Click += (s, e) => LoadViewToMain("ThungRac", () => new UcRecycleBin());
            btnDangXuat.Click += (s, e) => DangXuat();
            ConfigureDashboardPaging();
        }

        private void EnsureDashboardPanelsAreTopLevel()
        {
            MoveDashboardPanelToContainer(pnlContainerel12);
            MoveDashboardPanelToContainer(pnlContainerMuonTheoThang);
        }

        private void MoveDashboardPanelToContainer(Panel panel)
        {
            if (panel.Parent == pnlContainerelContent)
                return;

            panel.Parent?.Controls.Remove(panel);
            pnlContainerelContent.Controls.Add(panel);
            panel.BringToFront();
        }

        private void AddNotification(string message, Color bgColor, string iconText)
        {
            if (flpNotifications == null) return;

            Panel pnl = new Panel
            {
                BackColor = bgColor,
                Width = flpNotifications.ClientSize.Width - 15,
                Height = 42,
                Margin = new Padding(3, 3, 3, 5)
            };

            Label lbl = new Label
            {
                Text = $"{iconText} {message}",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 9.75F, FontStyle.Regular),
                ForeColor = Color.FromArgb(50, 50, 50),
                Padding = new Padding(8, 0, 0, 0)
            };

            pnl.Controls.Add(lbl);
            flpNotifications.Controls.Add(pnl);
        }

        private async void LoadViewToMain(string viewKey, Func<Control> viewFactory)
        {
            pnlGridData.Visible = false;
            pnlContainerel12.Visible = false;
            pnlContainerMuonTheoThang.Visible = false;
            panelContent.Visible = true;

            if (!_viewCache.TryGetValue(viewKey, out Control? targetView) || targetView == null)
            {
                // Hiển thị Loading Overlay
                _lblLoadingOverlay.Bounds = panelContent.Bounds;
                _lblLoadingOverlay.BringToFront();
                _lblLoadingOverlay.Visible = true;

                // Khởi tạo Form trên background thread rồi quay lại UI thread
                // (thay Application.DoEvents() anti-pattern)
                await Task.Yield();

                targetView = viewFactory();
                targetView.BackColor = Color.White;
                targetView.Dock = DockStyle.Fill;
                
                _viewCache[viewKey] = targetView;
                panelContent.Controls.Add(targetView);
            }

            // Ẩn tất cả màn hình khác trong panelContent.
            foreach (Control ctrl in panelContent.Controls)
            {
                if (ctrl != targetView)
                    ctrl.Hide();
            }

            targetView.Show();
            targetView.BringToFront();

            // Ẩn Loading Overlay
            _lblLoadingOverlay.Visible = false;
        }

        private async void LoadDashboard()
        {
            // Ẩn panelContent, hiện dashboard
            panelContent.Visible = false;
            // Ẩn tất cả các màn hình con thay vì hủy.
            foreach (Control ctrl in panelContent.Controls)
                ctrl.Hide();

            pnlGridData.Visible = true;
            pnlContainerel12.Visible = true;
            pnlContainerMuonTheoThang.Visible = true;
            pnlGridData.BringToFront();
            pnlContainerel12.BringToFront();
            pnlContainerMuonTheoThang.BringToFront();

            try
            {
                // Xóa thông báo cũ
                flpNotifications?.Controls.Clear();

                // Reset page to 1 on initial load
                _currentDashboardPage = 1;

                // Async: load dashboard data trên background thread, tránh UI freeze
                DashboardSnapshot dashboard = await Task.Run(() => DashboardService.Load(_currentDashboardPage, DashboardPageSize));
                lbl04.Text = dashboard.TotalDevices.ToString();
                lbl03.Text = dashboard.BorrowedDevices.ToString();

                // Phiếu mượn quá hạn
                lbl02.Text = dashboard.OverdueTickets.ToString();
                if (dashboard.OverdueTickets > 0)
                    AddNotification($"{dashboard.OverdueTickets} phiếu mượn quá hạn cần xử lý ngay", Color.FromArgb(255, 243, 205), "⚠");

                // Thiết bị hỏng / bảo trì
                lbl0.Text = dashboard.IssueDevices.ToString();
                if (dashboard.IssueDevices > 0)
                    AddNotification($"{dashboard.IssueDevices} thiết bị đang gặp sự cố hoặc cần bảo trì", Color.FromArgb(204, 229, 255), "🔧");

                // Thiết bị vừa được trả hôm nay
                if (dashboard.ReturnedToday > 0)
                    AddNotification($"{dashboard.ReturnedToday} phiếu mượn vừa được trả hôm nay", Color.FromArgb(212, 237, 218), "✅");

                // Nếu không có thông báo nào
                if (flpNotifications != null && flpNotifications.Controls.Count == 0)
                    AddNotification("Không có cảnh báo nào", Color.FromArgb(240, 240, 240), "✅");

                _totalDashboardRecords = dashboard.TotalBorrowingCount;
                _totalDashboardPages = (int)Math.Ceiling((double)_totalDashboardRecords / DashboardPageSize);
                if (_totalDashboardPages < 1) _totalDashboardPages = 1;

                UpdateDashboardPagingLabel();
                SetDashboardPagingButtonsEnabled();

                LoadBorrowingList(dashboard.BorrowingList);
                LoadMonthlyBorrowStats(dashboard.MonthlyBorrowStats);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load dashboard:\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadBorrowingList(DataTable dt)
        {
            dgvData.DataSource = dt;

            foreach (DataGridViewRow row in dgvData.Rows)
            {
                if (row.Cells[Col.HanTra].Value != DBNull.Value)
                    if (Convert.ToDateTime(row.Cells[Col.HanTra].Value) < DateTime.Now)
                        row.DefaultCellStyle.BackColor = Color.FromArgb(255, 220, 220);
            }
        }

        private void LoadMonthlyBorrowStats(DataTable dt)
        {
            if (pnlContainerMuonTheoThang == null)
                return;

            for (int i = pnlContainerMuonTheoThang.Controls.Count - 1; i >= 0; i--)
            {
                Control control = pnlContainerMuonTheoThang.Controls[i];
                if (control != lblMuonTheoThang)
                {
                    pnlContainerMuonTheoThang.Controls.RemoveAt(i);
                    control.Dispose();
                }
            }

            if (dt.Rows.Count == 0)
            {
                Label empty = new Label
                {
                    Text = "Chưa có dữ liệu mượn trong 6 tháng gần đây.",
                    AutoSize = false,
                    Location = new Point(12, 42),
                    Size = new Size(Math.Max(120, pnlContainerMuonTheoThang.ClientSize.Width - 24), 32),
                    ForeColor = Color.FromArgb(90, 100, 115),
                    Font = new Font("Segoe UI", 9.5F)
                };
                pnlContainerMuonTheoThang.Controls.Add(empty);
                return;
            }

            int maxQuantity = dt.Rows.Cast<DataRow>()
                .Select(row => Convert.ToInt32(row["Số thiết bị"]))
                .DefaultIfEmpty(1)
                .Max();
            maxQuantity = Math.Max(1, maxQuantity);

            int top = 40;
            int rowHeight = 30;
            int labelWidth = 76;
            int valueWidth = 88;
            int availableBarWidth = Math.Max(80, pnlContainerMuonTheoThang.ClientSize.Width - labelWidth - valueWidth - 36);

            foreach (DataRow row in dt.Rows)
            {
                string month = row["Tháng"]?.ToString() ?? string.Empty;
                int ticketCount = Convert.ToInt32(row["Số phiếu"]);
                int quantity = Convert.ToInt32(row["Số thiết bị"]);
                int barWidth = Math.Max(6, (int)Math.Round(availableBarWidth * (quantity / (double)maxQuantity)));

                Label monthLabel = new Label
                {
                    Text = month,
                    AutoSize = false,
                    Location = new Point(12, top + 3),
                    Size = new Size(labelWidth, 22),
                    TextAlign = ContentAlignment.MiddleLeft,
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(45, 55, 72)
                };

                Panel barBack = new Panel
                {
                    Location = new Point(12 + labelWidth, top + 7),
                    Size = new Size(availableBarWidth, 14),
                    BackColor = Color.FromArgb(232, 238, 245)
                };

                Panel barFill = new Panel
                {
                    Location = new Point(0, 0),
                    Size = new Size(barWidth, 14),
                    BackColor = Color.FromArgb(58, 123, 213)
                };
                barBack.Controls.Add(barFill);

                Label valueLabel = new Label
                {
                    Text = $"{quantity} TB / {ticketCount} phiếu",
                    AutoSize = false,
                    Location = new Point(18 + labelWidth + availableBarWidth, top + 3),
                    Size = new Size(valueWidth, 22),
                    TextAlign = ContentAlignment.MiddleLeft,
                    Font = new Font("Segoe UI", 8.5F),
                    ForeColor = Color.FromArgb(65, 75, 90)
                };

                pnlContainerMuonTheoThang.Controls.Add(monthLabel);
                pnlContainerMuonTheoThang.Controls.Add(barBack);
                pnlContainerMuonTheoThang.Controls.Add(valueLabel);

                top += rowHeight;
                if (top + rowHeight > pnlContainerMuonTheoThang.ClientSize.Height)
                    break;
            }
        }

        private void DangXuat()
        {
            if (MessageBox.Show("Bạn có chắc muốn đăng xuất?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DialogResult = DialogResult.Retry;
                Close();
            }
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            lblHeThongQuanLy.Text = $"🏫 Hệ Thống Quản Lý Mượn Trả Thiết Bị  |  Xin chào: {_fullName} ({_roleName})";
            lblNgayGio.Text = DateTime.Now.ToString("dddd, dd/MM/yyyy");
            AdjustLayout();
            PhanQuyen();
        }

        private void PhanQuyen()
        {
            btnTongQuan.Visible = _permissions.Dashboard;
            btnThietBi.Visible = _permissions.Devices;
            btnPhongHoc.Visible = _permissions.Rooms;
            btnNguoiDung.Visible = _permissions.Users;
            btnLapPhieuMuon.Visible = _permissions.BorrowTicket;
            btnTraThietBi.Visible = _permissions.ReturnDevice;
            btnDanhSachPhieu.Visible = _permissions.TicketList;
            btnBaoCao.Visible = _permissions.Reports;
            btnCauHinh.Visible = _permissions.Settings;
            btnThungRac.Visible = _permissions.Settings;

            ReflowSidebarButtons();

            if (_permissions.Dashboard)
                LoadDashboard();
            else if (_permissions.BorrowTicket)
                LoadViewToMain("LapPhieu", () => new UcLapthietbi(_userID, _fullName, _appRole));
            else if (_permissions.ReturnDevice)
                LoadViewToMain("TraThietBi", () => new UcTrathietbi(_userID, _appRole));
        }

        private void FrmMain_Resize(object sender, EventArgs e) => AdjustLayout();

        private void AdjustLayout()
        {
            int w = this.ClientSize.Width;
            int h = this.ClientSize.Height;
            int layoutW = Math.Max(w, 1120);
            int contentTop = 188;
            int minContentH = 560;

            // Lấy độ rộng thực tế của Sidebar (có thể đã tự co giãn theo giao diện/Font)
            int sidebarW = pn1Sidebar.Width;
            int marginX = sidebarW + 10;

            int contentH = Math.Max(minContentH, h - contentTop);
            int layoutBottom = contentTop + contentH + 20;
            pnlContainerelContent.AutoScrollMinSize = new Size(layoutW, layoutBottom);

            pn1Sidebar.Size = new Size(sidebarW, Math.Max(h, layoutBottom));
            pnlContainerel3.Location = new Point(sidebarW, 0);
            pnlContainerel3.Size = new Size(layoutW - sidebarW, 55);
            lblNgayGio.Location = new Point(layoutW - 370, 15);

            int statW = Math.Max(180, (layoutW - marginX) / 4);
            pnlContainer04.Location = new Point(marginX, 60); pnlContainer04.Size = new Size(statW - 5, 90);
            pnlContainer03.Location = new Point(marginX + statW, 60); pnlContainer03.Size = new Size(statW - 5, 90);
            pnlContainer02.Location = new Point(marginX + statW * 2, 60); pnlContainer02.Size = new Size(statW - 5, 90);
            pnlContainer0.Location = new Point(marginX + statW * 3, 60); pnlContainer0.Size = new Size(statW - 5, 90);

            int availableContentW = Math.Max(900, layoutW - marginX);
            int tableW = Math.Max(520, availableContentW * 6 / 10);
            int rightW = Math.Max(340, availableContentW - tableW - 15);

            // panelContent chiếm toàn bộ vùng nội dung
            panelContent.Location = new Point(marginX, contentTop);
            panelContent.Size = new Size(availableContentW, contentH);

            // Dashboard panels
            int pagingH = 40;
            pnlGridData.Location = new Point(marginX, contentTop); pnlGridData.Size = new Size(tableW, contentH);
            dgvData.Location = new Point(10, 35);
            dgvData.Size = new Size(Math.Max(120, tableW - 20), Math.Max(160, contentH - 35 - pagingH - 10));

            if (pnlPagingDashboard != null)
            {
                pnlPagingDashboard.Location = new Point(10, contentH - pagingH - 5);
                pnlPagingDashboard.Size = new Size(tableW - 20, pagingH);
                LayoutDashboardPagingControls();
            }

            pnlContainerel12.Location = new Point(marginX + tableW + 10, contentTop); pnlContainerel12.Size = new Size(rightW, contentH / 2 - 5);
            pnlContainerMuonTheoThang.Location = new Point(marginX + tableW + 10, contentTop + contentH / 2 + 5); pnlContainerMuonTheoThang.Size = new Size(rightW, contentH / 2 - 5);
        }

        private void ConfigureDashboardPaging()
        {
            StylePagingButton(btnFirstDashboard, "⏮ Đầu", (s, e) => ChangeDashboardPage(1));
            StylePagingButton(btnPrevDashboard, "◀ Trước", (s, e) => ChangeDashboardPage(_currentDashboardPage - 1));
            StylePagingButton(btnNextDashboard, "Tiếp ▶", (s, e) => ChangeDashboardPage(_currentDashboardPage + 1));
            StylePagingButton(btnLastDashboard, "Cuối ⏭", (s, e) => ChangeDashboardPage(_totalDashboardPages));

            lblPageInfoDashboard.Text = "Trang 1 / 1";
            lblPageInfoDashboard.TextAlign = ContentAlignment.MiddleCenter;
            lblPageInfoDashboard.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblPageInfoDashboard.ForeColor = Color.FromArgb(33, 37, 41);
            lblPageInfoDashboard.BackColor = Color.Transparent;
        }

        private void StylePagingButton(Button btn, string text, EventHandler onClick)
        {
            btn.Text = text;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0; // Flat style (no border)
            btn.BackColor = Color.FromArgb(13, 110, 253); // Bootstrap Primary Blue
            btn.ForeColor = Color.White;
            btn.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
            btn.Click += onClick;

            // Hover effects
            btn.MouseEnter += (s, e) =>
            {
                if (btn.Enabled)
                {
                    btn.BackColor = Color.FromArgb(11, 94, 215); // Darker blue on hover
                }
            };
            btn.MouseLeave += (s, e) =>
            {
                if (btn.Enabled)
                {
                    btn.BackColor = Color.FromArgb(13, 110, 253);
                }
            };
        }

        private void LayoutDashboardPagingControls()
        {
            if (pnlPagingDashboard == null) return;

            int panelW = pnlPagingDashboard.Width;
            int panelH = pnlPagingDashboard.Height;

            int firstW = 75;
            int prevW = 85;
            int nextW = 80;
            int lastW = 85;
            int lblW = 100;
            int btnH = 30;
            int spacing = 5;

            // Total width
            int totalW = firstW + prevW + nextW + lastW + lblW + (4 * spacing);
            int startX = (panelW - totalW) / 2;
            int startY = (panelH - btnH) / 2;

            btnFirstDashboard.Bounds = new Rectangle(startX, startY, firstW, btnH);
            btnPrevDashboard.Bounds = new Rectangle(startX + firstW + spacing, startY, prevW, btnH);
            lblPageInfoDashboard.Bounds = new Rectangle(startX + firstW + prevW + 2 * spacing, startY, lblW, btnH);
            btnNextDashboard.Bounds = new Rectangle(startX + firstW + prevW + lblW + 3 * spacing, startY, nextW, btnH);
            btnLastDashboard.Bounds = new Rectangle(startX + firstW + prevW + lblW + nextW + 4 * spacing, startY, lastW, btnH);
        }

        private async void ChangeDashboardPage(int newPage)
        {
            if (newPage < 1 || newPage > _totalDashboardPages) return;

            _currentDashboardPage = newPage;
            UpdateDashboardPagingLabel();
            SetDashboardPagingButtonsEnabled();

            try
            {
                // Disable controls during loading
                btnFirstDashboard.Enabled = false;
                btnPrevDashboard.Enabled = false;
                btnNextDashboard.Enabled = false;
                btnLastDashboard.Enabled = false;

                DataTable dt = await Task.Run(() => DashboardService.LoadBorrowingListOnly(_currentDashboardPage, DashboardPageSize));
                LoadBorrowingList(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải trang dữ liệu:\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetDashboardPagingButtonsEnabled();
            }
        }

        private void SetDashboardPagingButtonsEnabled()
        {
            btnFirstDashboard.Enabled = _currentDashboardPage > 1;
            btnPrevDashboard.Enabled = _currentDashboardPage > 1;
            btnNextDashboard.Enabled = _currentDashboardPage < _totalDashboardPages;
            btnLastDashboard.Enabled = _currentDashboardPage < _totalDashboardPages;

            ResetButtonColors(btnFirstDashboard);
            ResetButtonColors(btnPrevDashboard);
            ResetButtonColors(btnNextDashboard);
            ResetButtonColors(btnLastDashboard);
        }

        private void ResetButtonColors(Button btn)
        {
            if (btn.Enabled)
            {
                btn.BackColor = Color.FromArgb(13, 110, 253);
                btn.ForeColor = Color.White;
            }
            else
            {
                btn.BackColor = Color.FromArgb(230, 235, 240);
                btn.ForeColor = Color.DarkGray;
            }
        }

        private void UpdateDashboardPagingLabel()
        {
            lblPageInfoDashboard.Text = $"Trang {_currentDashboardPage} / {_totalDashboardPages}";
        }

        private void ReflowSidebarButtons()
        {
            int currentTop = btnTongQuan.Top;
            int spacing = 6;

            foreach (Button button in GetSidebarButtonsInOrder())
            {
                if (!button.Visible)
                    continue;

                button.Top = currentTop;
                currentTop += button.Height + spacing;
            }

            btnDangXuat.Top = Math.Max(currentTop + 12, pn1Sidebar.Height - btnDangXuat.Height - 20);
        }

        private IEnumerable<Button> GetSidebarButtonsInOrder()
        {
            yield return btnTongQuan;
            yield return btnThietBi;
            yield return btnPhongHoc;
            yield return btnNguoiDung;
            yield return btnLapPhieuMuon;
            yield return btnTraThietBi;
            yield return btnDanhSachPhieu;
            yield return btnBaoCao;
            yield return btnCauHinh;
            yield return btnThungRac;
        }
    }
}
