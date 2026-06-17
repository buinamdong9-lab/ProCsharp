using System.Collections.Generic;
using System.Data;
using System.IO;
using FrmProject.Models;

namespace FrmProject.GUI
{
    public partial class UcCauhinh : UserControl
    {
        private ISettingsService SettingsService => AppServiceProvider.Get<ISettingsService>();

        public UcCauhinh()
        {
            InitializeComponent();
            ViewHelper.ApplyBaseStyle(this);
            AutoScroll = true;
            AutoScrollMinSize = new Size(0, GetContentBottom() + 24);
            Resize += (_, _) => UpdateScrollAndHeaderLayout();
            this.Load += UcCauhinh_Load;

            btnLuuThongTin.Click += BtnLuuThongTin_Click;   // 💾 Lưu thông tin
            btnLuuCaiDat.Click += BtnLuuCaiDat_Click;     // 💾 Lưu cài đặt
            btnThem.Click += BtnThemLoaiTB_Click;    // ➕ Thêm loại TB

            btnMoLog.Click += (s, e) =>
            {
                string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
                if (Directory.Exists(logPath))
                {
                    System.Diagnostics.Process.Start("explorer.exe", logPath);
                }
                else
                {
                    MessageBox.Show("Thư mục logs chưa được tạo (chưa có sự kiện nào được ghi).", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };
        }

        private int GetContentBottom()
        {
            return Controls.Cast<Control>()
                .Where(control => control.Visible)
                .Select(control => control.Bottom)
                .DefaultIfEmpty(0)
                .Max();
        }

        private void UpdateScrollAndHeaderLayout()
        {
            AutoScrollMinSize = new Size(0, GetContentBottom() + 24);
            pnlContainerCauHinhHeThong.Width = Math.Max(ClientSize.Width, 1373);
        }

        private void UcCauhinh_Load(object sender, EventArgs e)
        {
            cmbCanPheDuyet.Items.Clear();
            cmbCanPheDuyet.Items.Add("Có");
            cmbCanPheDuyet.Items.Add("Không");

            cmbGuiGmailNhacNho.Items.Clear();
            cmbGuiGmailNhacNho.Items.Add("Có");
            cmbGuiGmailNhacNho.Items.Add("Không");

            EnsureAppSettingsTable();
            LoadSettings();
            MarkNotificationSettingsUnavailable();
            LoadDeviceCategories();
            LoadRoles();
            UpdateScrollAndHeaderLayout();
        }

        // ───────── AppSettings helpers ─────────

        private void EnsureAppSettingsTable()
        {
            try
            {
                SettingsService.EnsureAppSettingsTable();
            }
            catch (Exception ex)
            {
                AppLogger.Error("[UcCauhinh] Không thể đảm bảo bảng AppSettings.", ex);
            }
        }

        // ───────── Load / Save ─────────

        private void LoadSettings()
        {
            try
            {
                // Thông tin đơn vị
                txtTenTruong.Text = SettingsService.GetValue("DonVi_TenTruong");
                txtTenPhongBan.Text = SettingsService.GetValue("DonVi_TenPhongBan");
                txtDiaChi.Text = SettingsService.GetValue("DonVi_DiaChi");
                txtDienThoai.Text = SettingsService.GetValue("DonVi_DienThoai");
                txtEmail.Text = SettingsService.GetValue("DonVi_Email");
                txtWebsite.Text = SettingsService.GetValue("DonVi_Website");
                 
                // Cài đặt mượn trả
                if (decimal.TryParse(SettingsService.GetValue("CaiDat_ThoiHanMuon", "7"), out decimal thoiHan))
                    nudThoiHanMuonMacDinh.Value = Math.Clamp(thoiHan, nudThoiHanMuonMacDinh.Minimum, nudThoiHanMuonMacDinh.Maximum);
                else nudThoiHanMuonMacDinh.Value = 7;

                if (decimal.TryParse(SettingsService.GetValue("CaiDat_ToiDaThietBi", "10"), out decimal toiDa))
                    nudToiDaThietBiphieu.Value = Math.Clamp(toiDa, nudToiDaThietBiphieu.Minimum, nudToiDaThietBiphieu.Maximum);
                else nudToiDaThietBiphieu.Value = 10;

                if (decimal.TryParse(SettingsService.GetValue("CaiDat_NhacTruocHanTra", "2"), out decimal nhac))
                    nudNhacTruocHanTra.Value = Math.Clamp(nhac, nudNhacTruocHanTra.Minimum, nudNhacTruocHanTra.Maximum);
                else nudNhacTruocHanTra.Value = 2;

                string pheDuyet = SettingsService.GetValue("CaiDat_CanPheDuyet", "Có");
                cmbCanPheDuyet.SelectedIndex = pheDuyet == "Không" ? 1 : 0;

                string gmail = SettingsService.GetValue("CaiDat_GuiGmail", "Không");
                cmbGuiGmailNhacNho.SelectedIndex = gmail == "Có" ? 0 : 1;
            }
            catch (Exception ex)
            {
                AppLogger.Error("[UcCauhinh] Lỗi LoadSettings", ex);
                nudThoiHanMuonMacDinh.Value = 8;
                nudToiDaThietBiphieu.Value = 10;
                nudNhacTruocHanTra.Value = 2;
                cmbCanPheDuyet.SelectedIndex = 0;
                cmbGuiGmailNhacNho.SelectedIndex = 1;
            }
        }

        private void MarkNotificationSettingsUnavailable()
        {
            cmbGuiGmailNhacNho.SelectedIndex = 1;
            cmbGuiGmailNhacNho.Enabled = false;
            nudNhacTruocHanTra.Value = 0;
            nudNhacTruocHanTra.Enabled = false;
            lblGuiGmailNhacNho.Text = "Email nhắc (chưa có):";
            lblNhacTruocHanTra.Text = "Nhắc hạn (chưa có):";
            cmbGuiGmailNhacNho.Text = "Không";
            cmbGuiGmailNhacNho.BackColor = Color.FromArgb(235, 241, 245);
            nudNhacTruocHanTra.BackColor = Color.FromArgb(235, 241, 245);
            cmbGuiGmailNhacNho.Tag = "Chưa triển khai chức năng gửi email tự động.";
            nudNhacTruocHanTra.Tag = "Chỉ bật lại khi có job gửi email/nhắc hạn tự động.";
        }

        private void LoadDeviceCategories()
        {
            try
            {
                List<DeviceCategoryStatsModel> list = SettingsService.GetDeviceCategories();

                dgvData.AutoGenerateColumns = false;
                colMaLoai.DataPropertyName = "CategoryID";
                colTenLoai.DataPropertyName = "CategoryName";
                colSoThietBi.DataPropertyName = "DeviceCount";
                colThaoTac.DataPropertyName = "Action";
                dgvData.DataSource = list;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải loại thiết bị: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadRoles()
        {
            try
            {
                List<RoleStatsModel> list = SettingsService.GetRoles();
                dgvData2.DataSource = list;
                dgvData2.AutoGenerateColumns = true;
                if (dgvData2.Columns.Count > 0)
                {
                    dgvData2.Columns["RoleID"].HeaderText = "Mã vai trò";
                    dgvData2.Columns["RoleName"].HeaderText = "Tên vai trò";
                    dgvData2.Columns["UserCount"].HeaderText = "Số người dùng";
                }
            }
            catch (Exception ex) { AppLogger.Error($"[UcCauhinh] Lỗi LoadRoles", ex); }
        }

        private void BtnLuuThongTin_Click(object sender, EventArgs e)
        {
            try
            {
                SettingsService.SaveValues(new Dictionary<string, string>
                {
                    ["DonVi_TenTruong"] = txtTenTruong.Text.Trim(),
                    ["DonVi_TenPhongBan"] = txtTenPhongBan.Text.Trim(),
                    ["DonVi_DiaChi"] = txtDiaChi.Text.Trim(),
                    ["DonVi_DienThoai"] = txtDienThoai.Text.Trim(),
                    ["DonVi_Email"] = txtEmail.Text.Trim(),
                    ["DonVi_Website"] = txtWebsite.Text.Trim()
                });

                MessageBox.Show("Đã lưu thông tin đơn vị thành công!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lưu thông tin: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnLuuCaiDat_Click(object sender, EventArgs e)
        {
            try
            {
                SettingsService.SaveValues(new Dictionary<string, string>
                {
                    ["CaiDat_ThoiHanMuon"] = nudThoiHanMuonMacDinh.Value.ToString(),
                    ["CaiDat_ToiDaThietBi"] = nudToiDaThietBiphieu.Value.ToString(),
                    ["CaiDat_NhacTruocHanTra"] = "0",
                    ["CaiDat_CanPheDuyet"] = cmbCanPheDuyet.Text,
                    ["CaiDat_GuiGmail"] = "Không"
                });

                MessageBox.Show(
                    $"Đã lưu cài đặt thành công!\n" +
                    $"- Thời hạn mượn: {nudThoiHanMuonMacDinh.Value} ngày\n" +
                    $"- Tối đa TB/phiếu: {nudToiDaThietBiphieu.Value} thiết bị\n" +
                    $"- Phê duyệt: {cmbCanPheDuyet.Text}\n" +
                    $"- Email nhắc hạn: Chưa triển khai",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lưu cài đặt: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnThemLoaiTB_Click(object sender, EventArgs e)
        {
            string categoryName = txtCauHinhHeThong.Text.Trim();
            if (string.IsNullOrEmpty(categoryName))
            {
                MessageBox.Show("Vui lòng nhập tên loại thiết bị!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                SettingsService.AddDeviceCategory(categoryName);

                MessageBox.Show("Thêm loại thiết bị thành công!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtCauHinhHeThong.Text = "";
                LoadDeviceCategories();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}




