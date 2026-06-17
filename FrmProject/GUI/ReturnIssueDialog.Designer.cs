namespace FrmProject.GUI
{
    internal sealed partial class ReturnIssueDialog
    {
        private Label lblTitle;
        private Panel pnlGridHost;
        private Panel pnlButtons;
        private DataGridView dgvReturnIssues;
        private Button btnConfirm;
        private Button btnCancel;
        private DataGridViewTextBoxColumn colAssetCode;
        private DataGridViewTextBoxColumn colDeviceName;
        private DataGridViewTextBoxColumn colBorrowQty;
        private DataGridViewTextBoxColumn colReturnQty;
        private DataGridViewTextBoxColumn colBorrowCondition;
        private DataGridViewComboBoxColumn colReturnCondition;
        private DataGridViewTextBoxColumn colNote;

        private void InitializeComponent()
        {
            lblTitle = new Label();
            pnlGridHost = new Panel();
            dgvReturnIssues = new DataGridView();
            pnlButtons = new Panel();
            btnConfirm = new Button();
            btnCancel = new Button();
            colAssetCode = new DataGridViewTextBoxColumn();
            colDeviceName = new DataGridViewTextBoxColumn();
            colBorrowQty = new DataGridViewTextBoxColumn();
            colReturnQty = new DataGridViewTextBoxColumn();
            colBorrowCondition = new DataGridViewTextBoxColumn();
            colReturnCondition = new DataGridViewComboBoxColumn();
            colNote = new DataGridViewTextBoxColumn();
            pnlGridHost.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvReturnIssues).BeginInit();
            pnlButtons.SuspendLayout();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.Dock = DockStyle.Top;
            lblTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(133, 77, 14);
            lblTitle.Location = new Point(0, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Padding = new Padding(12, 10, 12, 0);
            lblTitle.Size = new Size(980, 54);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Chọn tình trạng khi trả cho từng thiết bị. Các thiết bị lỗi sẽ được cảnh báo cho admin và chuyển sang bảo trì khi";
            // 
            // pnlGridHost
            // 
            pnlGridHost.BackColor = Color.White;
            pnlGridHost.Controls.Add(dgvReturnIssues);
            pnlGridHost.Dock = DockStyle.Fill;
            pnlGridHost.Location = new Point(0, 54);
            pnlGridHost.Name = "pnlGridHost";
            pnlGridHost.Padding = new Padding(12, 0, 12, 0);
            pnlGridHost.Size = new Size(980, 500);
            pnlGridHost.TabIndex = 1;
            // 
            // dgvReturnIssues
            // 
            dgvReturnIssues.AllowUserToAddRows = false;
            dgvReturnIssues.AllowUserToDeleteRows = false;
            dgvReturnIssues.AllowUserToResizeRows = false;
            dgvReturnIssues.AutoGenerateColumns = false;
            dgvReturnIssues.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvReturnIssues.BackgroundColor = Color.White;
            dgvReturnIssues.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvReturnIssues.Columns.AddRange(new DataGridViewColumn[] {
                colAssetCode,
                colDeviceName,
                colBorrowQty,
                colReturnQty,
                colBorrowCondition,
                colReturnCondition,
                colNote
            });
            dgvReturnIssues.Dock = DockStyle.Fill;
            dgvReturnIssues.Location = new Point(12, 0);
            dgvReturnIssues.MultiSelect = false;
            dgvReturnIssues.Name = "dgvReturnIssues";
            dgvReturnIssues.RowHeadersWidth = 28;
            dgvReturnIssues.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvReturnIssues.Size = new Size(956, 500);
            dgvReturnIssues.TabIndex = 0;
            // 
            // colAssetCode
            // 
            colAssetCode.DataPropertyName = "AssetCode";
            colAssetCode.HeaderText = "Mã tài sản";
            colAssetCode.Name = "colAssetCode";
            colAssetCode.ReadOnly = true;
            colAssetCode.FillWeight = 100;
            // 
            // colDeviceName
            // 
            colDeviceName.DataPropertyName = "DeviceName";
            colDeviceName.HeaderText = "Tên thiết bị";
            colDeviceName.Name = "colDeviceName";
            colDeviceName.ReadOnly = true;
            colDeviceName.FillWeight = 160;
            // 
            // colBorrowQty
            // 
            colBorrowQty.DataPropertyName = "BorrowQty";
            colBorrowQty.HeaderText = "SL mượn";
            colBorrowQty.Name = "colBorrowQty";
            colBorrowQty.ReadOnly = true;
            colBorrowQty.FillWeight = 80;
            // 
            // colReturnQty
            // 
            colReturnQty.DataPropertyName = "ReturnQty";
            colReturnQty.HeaderText = "SL trả";
            colReturnQty.Name = "colReturnQty";
            colReturnQty.ReadOnly = false;
            colReturnQty.FillWeight = 80;
            // 
            // colBorrowCondition
            // 
            colBorrowCondition.DataPropertyName = "BorrowCondition";
            colBorrowCondition.HeaderText = "Tình trạng khi mượn";
            colBorrowCondition.Name = "colBorrowCondition";
            colBorrowCondition.ReadOnly = true;
            colBorrowCondition.FillWeight = 120;
            // 
            // colReturnCondition
            // 
            colReturnCondition.DataPropertyName = "ReturnCondition";
            colReturnCondition.HeaderText = "Tình trạng khi trả";
            colReturnCondition.Name = "colReturnCondition";
            colReturnCondition.ReadOnly = false;
            colReturnCondition.FillWeight = 140;
            colReturnCondition.Items.AddRange(new object[] {
                "Tốt",
                "Hỏng hóc",
                "Bảo trì",
                "Mất",
                "Trầy xước",
                "Thiếu phụ kiện",
                "Khác"
            });
            // 
            // colNote
            // 
            colNote.DataPropertyName = "Note";
            colNote.HeaderText = "Ghi chú";
            colNote.Name = "colNote";
            colNote.ReadOnly = false;
            colNote.FillWeight = 180;
            // 
            // pnlButtons
            // 
            pnlButtons.BackColor = Color.FromArgb(245, 248, 245);
            pnlButtons.Controls.Add(btnConfirm);
            pnlButtons.Controls.Add(btnCancel);
            pnlButtons.Dock = DockStyle.Bottom;
            pnlButtons.Location = new Point(0, 554);
            pnlButtons.Name = "pnlButtons";
            pnlButtons.Padding = new Padding(12);
            pnlButtons.Size = new Size(980, 66);
            pnlButtons.TabIndex = 2;
            // 
            // btnConfirm
            // 
            btnConfirm.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnConfirm.BackColor = Color.FromArgb(27, 94, 60);
            btnConfirm.FlatAppearance.BorderSize = 0;
            btnConfirm.FlatStyle = FlatStyle.Flat;
            btnConfirm.ForeColor = Color.White;
            btnConfirm.Location = new Point(660, 13);
            btnConfirm.Name = "btnConfirm";
            btnConfirm.Size = new Size(190, 40);
            btnConfirm.TabIndex = 0;
            btnConfirm.Text = "✓ Xác nhận tình trạng";
            btnConfirm.UseVisualStyleBackColor = false;
            btnConfirm.Click += ConfirmClick;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCancel.BackColor = Color.FromArgb(108, 117, 125);
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.ForeColor = Color.White;
            btnCancel.Location = new Point(860, 13);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(105, 40);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "Hủy";
            btnCancel.UseVisualStyleBackColor = false;
            // 
            // ReturnIssueDialog
            // 
            AcceptButton = btnConfirm;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            CancelButton = btnCancel;
            ClientSize = new Size(980, 620);
            Controls.Add(pnlGridHost);
            Controls.Add(pnlButtons);
            Controls.Add(lblTitle);
            MinimizeBox = false;
            MinimumSize = new Size(980, 620);
            Name = "ReturnIssueDialog";
            StartPosition = FormStartPosition.CenterParent;
            pnlGridHost.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvReturnIssues).EndInit();
            pnlButtons.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}
