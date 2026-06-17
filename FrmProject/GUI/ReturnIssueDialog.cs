using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FrmProject.Models;

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

        private readonly List<ReturnTicketItemModel> _source;
        private readonly List<ReturnTicketItemModel> _editList;

        private ReturnIssueDialog(List<ReturnTicketItemModel> source, IWin32Window owner)
        {
            _source = source;
            _editList = source.Select(item => new ReturnTicketItemModel
            {
                DeviceID = item.DeviceID,
                InstanceID = item.InstanceID,
                AssetCode = item.AssetCode,
                DeviceName = item.DeviceName,
                BorrowQty = item.BorrowQty,
                ReturnQty = item.ReturnQty,
                BorrowCondition = string.IsNullOrWhiteSpace(item.BorrowCondition) ? "Tốt" : item.BorrowCondition,
                ReturnCondition = string.IsNullOrWhiteSpace(item.ReturnCondition) ? "Tốt" : item.ReturnCondition,
                Note = item.Note
            }).ToList();

            InitializeComponent();
            ViewHelper.ApplyBaseStyle(this);
            ApplyOwnerSize(owner);

            dgvReturnIssues.AutoGenerateColumns = false;
            dgvReturnIssues.DataSource = _editList;

            dgvReturnIssues.BackgroundColor = Color.White;
            dgvReturnIssues.GridColor = Color.FromArgb(220, 220, 220);
            dgvReturnIssues.EnableHeadersVisualStyles = false;
            dgvReturnIssues.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(235, 240, 248);
            dgvReturnIssues.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgvReturnIssues.DefaultCellStyle.SelectionBackColor = Color.FromArgb(225, 239, 252);
            dgvReturnIssues.DefaultCellStyle.SelectionForeColor = Color.Black;
        }

        public static bool ShowFor(List<ReturnTicketItemModel> source, IWin32Window owner)
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
            if (!ValidateReturnIssueRows(_editList))
                return;

            ApplyReturnIssueRows(_source, _editList);
            DialogResult = DialogResult.OK;
            Close();
        }

        private static bool ValidateReturnIssueRows(List<ReturnTicketItemModel> list)
        {
            foreach (var item in list)
            {
                int borrowedQty = item.BorrowQty;
                int returnQty = item.ReturnQty;
                if (returnQty < 0 || returnQty > borrowedQty)
                {
                    MessageBox.Show($"Số lượng trả của thiết bị '{item.DeviceName}' phải từ 0 đến {borrowedQty}.",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                string returnCondition = item.ReturnCondition;
                bool isGoodCondition = IsGoodReturnCondition(returnCondition);
                bool isPartialReturnOfReturnedItems = returnQty > 0 && returnQty < borrowedQty;
                bool isBadCondition = !isGoodCondition;

                if ((isPartialReturnOfReturnedItems || isBadCondition) && string.IsNullOrWhiteSpace(item.Note))
                {
                    MessageBox.Show($"Vui lòng nhập cảnh báo/ghi chú cho thiết bị '{item.DeviceName}'.",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }

            return true;
        }

        private static void ApplyReturnIssueRows(List<ReturnTicketItemModel> target, List<ReturnTicketItemModel> source)
        {
            for (int i = 0; i < target.Count && i < source.Count; i++)
            {
                target[i].ReturnQty = source[i].ReturnQty;
                target[i].ReturnCondition = source[i].ReturnCondition;
                target[i].Note = source[i].Note;
            }
        }

        private static bool IsGoodReturnCondition(string condition)
        {
            return string.IsNullOrWhiteSpace(condition) ||
                   condition.Trim().Equals("Tốt", StringComparison.OrdinalIgnoreCase);
        }
    }
}
