using FrmProject.DAL;
using System.Data;

namespace FrmProject.GUI
{
    public partial class UcRecycleBin : UserControl
    {
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
                DataTable table = RecycleBinRepository.Load(itemType, keyword);
                dgvRecycleBin.DataSource = table;
                ConfigureGrid();
                lblTong.Text = $"Tổng: {table.Rows.Count}";
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
            }
            if (dgvRecycleBin.Columns.Contains("Loại"))
                dgvRecycleBin.Columns["Loại"].FillWeight = 85;
            if (dgvRecycleBin.Columns.Contains("Mã"))
                dgvRecycleBin.Columns["Mã"].FillWeight = 90;
            if (dgvRecycleBin.Columns.Contains("Tên"))
                dgvRecycleBin.Columns["Tên"].FillWeight = 160;
            if (dgvRecycleBin.Columns.Contains("Ghi chú"))
                dgvRecycleBin.Columns["Ghi chú"].FillWeight = 220;
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
                RecycleBinRepository.Restore(itemType, itemId);
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
                RecycleBinRepository.DeleteForever(itemType, itemId);
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

            itemType = dgvRecycleBin.CurrentRow.Cells["Loại"].Value?.ToString() ?? string.Empty;
            itemName = dgvRecycleBin.CurrentRow.Cells["Tên"].Value?.ToString() ?? string.Empty;
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



