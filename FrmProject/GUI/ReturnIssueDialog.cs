using System.Data;

namespace FrmProject.GUI
{
    internal sealed partial class ReturnIssueDialog : Form
    {
        private static readonly string[] ReturnConditions =
        {
            "Tốt",
            "Hỏng hóc",
            "Bảo trì",
            "Mất",
            "Trầy xước",
            "Thiếu phụ kiện",
            "Khác"
        };

        private readonly DataTable _source;
        private readonly DataTable _editTable;

        private ReturnIssueDialog(DataTable source, IWin32Window owner)
        {
            _source = source;
            _editTable = source.Copy();
            EnsureReturnIssueColumns(_editTable);

            InitializeComponent();
            ViewHelper.ApplyBaseStyle(this);
            ApplyOwnerSize(owner);

            dgvReturnIssues.DataSource = _editTable;

            dgvReturnIssues.BackgroundColor = Color.White;
            dgvReturnIssues.GridColor = Color.FromArgb(220, 220, 220);
            dgvReturnIssues.EnableHeadersVisualStyles = false;
            dgvReturnIssues.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(235, 240, 248);
            dgvReturnIssues.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgvReturnIssues.DefaultCellStyle.SelectionBackColor = Color.FromArgb(225, 239, 252);
            dgvReturnIssues.DefaultCellStyle.SelectionForeColor = Color.Black;
        }

        public static bool ShowFor(DataTable source, IWin32Window owner)
        {
            using ReturnIssueDialog dialog = new ReturnIssueDialog(source, owner);
            return dialog.ShowDialog(owner) == DialogResult.OK;
        }

        private void ApplyOwnerSize(IWin32Window owner)
        {
            Size ownerSize = owner is Control ownerControl ? ownerControl.ClientSize : Size.Empty;
            int ownerWidth = ownerSize.Width > 0 ? ownerSize.Width : 1100;
            int ownerHeight = ownerSize.Height > 0 ? ownerSize.Height : 760;
            int dialogWidth = Math.Clamp(ownerWidth - 80, 980, 1240);
            int dialogHeight = Math.Clamp(ownerHeight - 120, 620, 760);
            ClientSize = new Size(dialogWidth, dialogHeight);
        }

        private void ConfirmClick(object? sender, EventArgs e)
        {
            dgvReturnIssues.EndEdit();
            if (!ValidateReturnIssueRows(_editTable))
                return;

            ApplyReturnIssueRows(_source, _editTable);
            DialogResult = DialogResult.OK;
            Close();
        }

        private static void EnsureReturnIssueColumns(DataTable table)
        {
            if (!table.Columns.Contains("Tình trạng khi mượn"))
                table.Columns.Add("Tình trạng khi mượn", typeof(string));
            if (!table.Columns.Contains("Tình trạng khi trả"))
                table.Columns.Add("Tình trạng khi trả", typeof(string));
            if (!table.Columns.Contains("Ghi chú"))
                table.Columns.Add("Ghi chú", typeof(string));

            if (table.Columns.Contains("SL trả"))
                table.Columns["SL trả"].ReadOnly = false;
            if (table.Columns.Contains("Tình trạng khi trả"))
                table.Columns["Tình trạng khi trả"].ReadOnly = false;
            if (table.Columns.Contains("Ghi chú"))
                table.Columns["Ghi chú"].ReadOnly = false;

            foreach (DataRow row in table.Rows)
            {
                if (string.IsNullOrWhiteSpace(row["Tình trạng khi mượn"]?.ToString()))
                    row["Tình trạng khi mượn"] = "Tốt";
                if (string.IsNullOrWhiteSpace(row["Tình trạng khi trả"]?.ToString()))
                    row["Tình trạng khi trả"] = "Tốt";
            }
        }

        private static bool ValidateReturnIssueRows(DataTable table)
        {
            foreach (DataRow row in table.Rows)
            {
                int borrowedQty = Convert.ToInt32(row["SL mượn"]);
                if (!int.TryParse(row["SL trả"]?.ToString(), out int returnQty) || returnQty < 0 || returnQty > borrowedQty)
                {
                    MessageBox.Show($"Số lượng trả của thiết bị '{row["Tên thiết bị"]}' phải từ 0 đến {borrowedQty}.",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                string returnCondition = row["Tình trạng khi trả"]?.ToString() ?? string.Empty;
                if ((returnQty < borrowedQty || !IsGoodReturnCondition(returnCondition)) &&
                    string.IsNullOrWhiteSpace(row["Ghi chú"]?.ToString()))
                {
                    MessageBox.Show($"Vui lòng nhập cảnh báo/ghi chú cho thiết bị '{row["Tên thiết bị"]}'.",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }

            return true;
        }

        private static void ApplyReturnIssueRows(DataTable target, DataTable source)
        {
            EnsureReturnIssueColumns(target);

            for (int i = 0; i < target.Rows.Count && i < source.Rows.Count; i++)
            {
                target.Rows[i]["SL trả"] = source.Rows[i]["SL trả"];
                target.Rows[i]["Tình trạng khi trả"] = source.Rows[i]["Tình trạng khi trả"];
                target.Rows[i]["Ghi chú"] = source.Rows[i]["Ghi chú"];
            }
        }

        private static bool IsGoodReturnCondition(string condition)
        {
            return string.IsNullOrWhiteSpace(condition) ||
                   condition.Trim().Equals("Tốt", StringComparison.OrdinalIgnoreCase);
        }
    }
}

