using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace FrmProject.GUI
{
    public partial class UcQuanLyCaThe : Form
    {
        private IDeviceInstanceService DeviceInstanceService => AppServiceProvider.Get<IDeviceInstanceService>();

        private int _deviceID;
        private string _deviceCode = string.Empty;
        private string _deviceName = string.Empty;
        private int _selectedInstanceId = -1;

        // Default constructor required for Visual Studio Designer
        public UcQuanLyCaThe()
        {
            InitializeComponent();
            ViewHelper.ApplyBaseStyle(this);
            WireEvents();
        }

        public UcQuanLyCaThe(int deviceID, string deviceName)
        {
            _deviceID = deviceID;
            _deviceName = deviceName;

            InitializeComponent();
            ViewHelper.ApplyBaseStyle(this);
            WireEvents();

            this.Text = $"Quản lý mã cá thể: {_deviceName}";
            cmbStatus.SelectedIndex = 0;
            this.Load += (s, e) => OnFormLoad();
        }

        private void WireEvents()
        {
            dgvInstances.CellClick += DgvInstances_CellClick;
            dgvInstances.SelectionChanged += DgvInstances_SelectionChanged;
        }

        private void OnFormLoad()
        {
            // Lấy DeviceCode (mã gốc) để làm tiền tố cho ID cá thể
            try
            {
                _deviceCode = DeviceInstanceService.GetDeviceCode(_deviceID);
                lblDeviceInfo.Text = $"Loại thiết bị: {_deviceName}   |   Mã gốc: {_deviceCode}";
                LoadData();
                EnterCreateMode();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khởi tạo form: " + ex.Message);
            }
        }

        /// <summary>
        /// Sinh và hiển thị mã cá thể tiếp theo lên txtAssetCode (readonly).
        /// </summary>
        private void RefreshNextCode()
        {
            try
            {
                txtAssetCode.Text = DeviceInstanceService.GetNextAssetCode(_deviceID);
                lblAssetCode.Text = "Mã cá thể tiếp theo:";
                lblNextCode.Text = "← Mã được sinh tự động từ mã loại thiết bị";
            }
            catch (Exception ex)
            {
                AppLogger.Error("[UcQuanLyCaThe] RefreshNextCode", ex);
                txtAssetCode.Text = $"{_deviceCode}-001";
            }
        }

        private void LoadData()
        {
            try
            {
                dgvInstances.DataSource = DeviceInstanceService.GetByDevice(_deviceID);
                if (dgvInstances.Columns["InstanceID"] != null)
                    dgvInstances.Columns["InstanceID"].Visible = false;
                if (dgvInstances.Columns["AssetCode"] != null)
                    dgvInstances.Columns["AssetCode"].HeaderText = "Mã cá thể";
                if (dgvInstances.Columns["Status"] != null)
                    dgvInstances.Columns["Status"].HeaderText = "Trạng thái";
                if (dgvInstances.Columns["Condition"] != null)
                    dgvInstances.Columns["Condition"].HeaderText = "Tình trạng";

                // Làm mới mã tiếp theo sau khi tải lại danh sách
                RefreshNextCode();
                dgvInstances.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải cá thể: " + ex.Message);
            }
        }

        private void BtnThem_Click(object? sender, EventArgs e)
        {
            if (_selectedInstanceId > 0)
            {
                var confirmNew = MessageBox.Show(
                    "Bạn đang chọn một mã cá thể để sửa. Nút Thêm mới sẽ sinh mã cá thể mới, không cập nhật mã đang chọn.\n\nTiếp tục thêm mới?",
                    "Thêm mã cá thể mới",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirmNew != DialogResult.Yes)
                    return;

                EnterCreateMode(keepStatusAndCondition: true);
            }

            string assetCode = txtAssetCode.Text.Trim();
            if (string.IsNullOrWhiteSpace(assetCode))
            {
                MessageBox.Show("Không thể sinh mã cá thể. Vui lòng kiểm tra lại mã thiết bị gốc!");
                return;
            }

            // Xác nhận trước khi thêm
            var confirm = MessageBox.Show(
                $"Xác nhận thêm cá thể mới?\n\nMã cá thể: {assetCode}\nTrạng thái: {cmbStatus.SelectedItem}\nTình trạng: {txtCondition.Text.Trim()}",
                "Xác nhận thêm",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            try
            {
                // Kiểm tra trùng lặp (phòng trường hợp race condition)
                if (DeviceInstanceService.AssetCodeExists(assetCode))
                {
                    MessageBox.Show("Mã này đã tồn tại (có thể vừa được thêm). Đang làm mới...");
                    LoadData();
                    return;
                }

                DeviceInstanceService.Insert(
                    _deviceID,
                    assetCode,
                    cmbStatus.SelectedItem?.ToString() ?? DeviceStatus.Available,
                    txtCondition.Text.Trim());

                txtCondition.Text = "";
                cmbStatus.SelectedIndex = 0;
                LoadData(); // LoadData sẽ gọi RefreshNextCode tự động
                EnterCreateMode();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm cá thể: " + ex.Message);
            }
        }

        private void BtnCapNhat_Click(object? sender, EventArgs e)
        {
            if (_selectedInstanceId <= 0)
            {
                MessageBox.Show("Vui lòng chọn một mã tài sản trong danh sách để cập nhật.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string assetCode = txtAssetCode.Text.Trim();
            string newStatus = cmbStatus.SelectedItem?.ToString() ?? DeviceStatus.Available;
            string newCondition = txtCondition.Text.Trim();

            var confirm = MessageBox.Show(
                $"Cập nhật tình trạng cho [{assetCode}]?\n\nTrạng thái: {newStatus}\nTình trạng: {newCondition}",
                "Xác nhận cập nhật",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                DeviceInstanceService.UpdateStatusAndCondition(_deviceID, _selectedInstanceId, newStatus, newCondition);
                MessageBox.Show("Đã cập nhật tình trạng mã cá thể.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                EnterCreateMode();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật tình trạng: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnXoa_Click(object? sender, EventArgs e)
        {
            if (dgvInstances.CurrentRow == null) return;

            int instanceId = (int)dgvInstances.CurrentRow.Cells["InstanceID"].Value;
            string? maTS = dgvInstances.CurrentRow.Cells["AssetCode"].Value?.ToString();

            if (MessageBox.Show($"Bạn có chắc chắn xóa mềm cá thể [{maTS}]?\n\nMã tài sản sẽ chuyển sang trạng thái Ngừng sử dụng.", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    DeviceInstanceService.Delete(_deviceID, instanceId);
                    LoadData();
                    EnterCreateMode();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Không thể xóa mềm cá thể này. Lỗi: " + ex.Message);
                }
            }
        }

        private void DgvInstances_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            LoadSelectedInstance(dgvInstances.Rows[e.RowIndex]);
        }

        private void DgvInstances_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvInstances.CurrentRow == null || dgvInstances.CurrentRow.Index < 0)
                return;

            LoadSelectedInstance(dgvInstances.CurrentRow);
        }

        private void LoadSelectedInstance(DataGridViewRow row)
        {
            if (row.Cells["InstanceID"].Value == null ||
                !int.TryParse(row.Cells["InstanceID"].Value.ToString(), out int instanceId))
                return;

            _selectedInstanceId = instanceId;
            txtAssetCode.Text = row.Cells["AssetCode"].Value?.ToString() ?? string.Empty;
            txtCondition.Text = row.Cells["Condition"].Value?.ToString() ?? string.Empty;

            string status = row.Cells["Status"].Value?.ToString() ?? DeviceStatus.Available;
            int statusIndex = cmbStatus.FindStringExact(status);
            cmbStatus.SelectedIndex = statusIndex >= 0 ? statusIndex : 0;

            lblAssetCode.Text = "Mã cá thể đang chọn:";
            lblNextCode.Text = "← Chỉnh trạng thái/tình trạng rồi bấm Cập nhật";
            btnCapNhat.Enabled = true;
        }

        private void EnterCreateMode(bool keepStatusAndCondition = false)
        {
            _selectedInstanceId = -1;
            RefreshNextCode();

            if (!keepStatusAndCondition)
            {
                txtCondition.Text = "";
                if (cmbStatus.Items.Count > 0)
                    cmbStatus.SelectedIndex = 0;
            }

            btnCapNhat.Enabled = false;
            dgvInstances.ClearSelection();
        }
    }
}


