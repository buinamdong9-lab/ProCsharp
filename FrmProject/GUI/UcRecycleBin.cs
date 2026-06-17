using System.Data;

namespace FrmProject.GUI
{
    public partial class UcRecycleBin : UserControl
    {
        private IRecycleBinService RecycleBinService => AppServiceProvider.Get<IRecycleBinService>();

        public UcRecycleBin()
        {
            InitializeComponent();
            ViewHelper.ApplyBaseStyle(this);
            Load += UcRecycleBin_Load;
            cboLoai.SelectedIndexChanged += (s, e) => LoadData();
            txtTimKiem.TextChanged += (s, e) => LoadData();
            btnLamMoi.Click += (s, e) => LoadData();
            btnKhoiPhuc.Click += BtnKhoiPhuc_Click;
            btnXoaVinhVien.Click += BtnXoaVinhVien_Click;
        }

        private void UcRecycleBin_Load(object? sender, EventArgs e)
        {
            cboLoai.SelectedIndex = 0;
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                string itemType = cboLoai.Text;
                string keyword = txtTimKiem.Text.Trim();
                var items = RecycleBinService.Load(itemType, keyword);
                dgvRecycleBin.DataSource = items;
                ConfigureGrid();
                lblTong.Text = $"Tổng: {items.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải thùng rác:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigureGrid()
        {
            dgvRecycleBin.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            if (dgvRecycleBin.Columns.Contains("ID"))
            {
                dgvRecycleBin.Columns["ID"].Width = 70;
                dgvRecycleBin.Columns["ID"].FillWeight = 45;
                dgvRecycleBin.Columns["ID"].HeaderText = "Mã ID";
            }
            if (dgvRecycleBin.Columns.Contains("ItemType"))
            {
                dgvRecycleBin.Columns["ItemType"].FillWeight = 85;
                dgvRecycleBin.Columns["ItemType"].HeaderText = "Loại";
            }
            if (dgvRecycleBin.Columns.Contains("Code"))
            {
                dgvRecycleBin.Columns["Code"].FillWeight = 90;
                dgvRecycleBin.Columns["Code"].HeaderText = "Mã";
            }
            if (dgvRecycleBin.Columns.Contains("Name"))
            {
                dgvRecycleBin.Columns["Name"].FillWeight = 160;
                dgvRecycleBin.Columns["Name"].HeaderText = "Tên";
            }
            if (dgvRecycleBin.Columns.Contains("Status"))
            {
                dgvRecycleBin.Columns["Status"].FillWeight = 100;
                dgvRecycleBin.Columns["Status"].HeaderText = "Trạng thái";
            }
            if (dgvRecycleBin.Columns.Contains("Note"))
            {
                dgvRecycleBin.Columns["Note"].FillWeight = 220;
                dgvRecycleBin.Columns["Note"].HeaderText = "Ghi chú";
            }
        }

        private void BtnKhoiPhuc_Click(object? sender, EventArgs e)
        {
            if (!TryGetSelectedItem(out string itemType, out int itemId, out string itemName))
                return;

            if (MessageBox.Show($"Khôi phục {itemType.ToLower()} \"{itemName}\"?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                RecycleBinService.Restore(itemType, itemId);
                MessageBox.Show("Khôi phục thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể khôi phục:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnXoaVinhVien_Click(object? sender, EventArgs e)
        {
            if (!TryGetSelectedItem(out string itemType, out int itemId, out string itemName))
                return;

            if (MessageBox.Show(
                    $"Xóa vĩnh viễn {itemType.ToLower()} \"{itemName}\"?\n\nThao tác này không thể hoàn tác.",
                    "Xác nhận xóa vĩnh viễn",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            try
            {
                RecycleBinService.DeleteForever(itemType, itemId);
                MessageBox.Show("Đã xóa vĩnh viễn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể xóa vĩnh viễn:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool TryGetSelectedItem(out string itemType, out int itemId, out string itemName)
        {
            itemType = string.Empty;
            itemId = 0;
            itemName = string.Empty;

            if (dgvRecycleBin.CurrentRow == null || dgvRecycleBin.CurrentRow.IsNewRow)
            {
                MessageBox.Show("Vui lòng chọn một dòng trong thùng rác.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            itemType = dgvRecycleBin.CurrentRow.Cells["ItemType"].Value?.ToString() ?? string.Empty;
            itemName = dgvRecycleBin.CurrentRow.Cells["Name"].Value?.ToString() ?? string.Empty;
            object? idValue = dgvRecycleBin.CurrentRow.Cells["ID"].Value;
            if (string.IsNullOrWhiteSpace(itemType) || idValue == null || !int.TryParse(idValue.ToString(), out itemId))
            {
                MessageBox.Show("Dòng được chọn không hợp lệ.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }
    }
}



