using System.Collections.Generic;
using System.Data;
using OfficeOpenXml;
using FrmProject.Models;

namespace FrmProject.GUI
{
    public partial class UcBaocao : UserControl
    {
        private IReportService ReportService => AppServiceProvider.Get<IReportService>();

        public UcBaocao()
        {
            InitializeComponent();
            ViewHelper.ApplyBaseStyle(this);
            this.Load += UcBaocao_Load;

            btnXemBaoCao.Click += BtnXemBaoCao_Click;  // 📊 Xem báo cáo
            btnXuatExcel.Click += BtnXuatExcel_Click;  // 📅 Xuất Excel
            cmbLoaiBaoCao.SelectionChangeCommitted += (s, e) => LoadSelectedReport();
        }

        private void UcBaocao_Load(object sender, EventArgs e)
        {
            // Report types
            cmbLoaiBaoCao.Items.Clear();
            cmbLoaiBaoCao.Items.Add("Tất cả báo cáo");
            cmbLoaiBaoCao.Items.Add("Tổng hợp mượn trả");
            cmbLoaiBaoCao.Items.Add("Thiết bị sử dụng nhiều nhất");
            cmbLoaiBaoCao.Items.Add("Thiết bị quá hạn");
            cmbLoaiBaoCao.SelectedIndex = 0;

            dtpTuNgay.Value = new DateTime(DateTime.Now.Year, 1, 1);
            dtpDenNgay.Value = DateTime.Now;

            LoadSelectedReport();
        }

        private void BtnXemBaoCao_Click(object sender, EventArgs e)
        {
            LoadSelectedReport();
        }

        private void LoadSelectedReport()
        {
            try
            {
                if (dtpTuNgay.Value.Date > dtpDenNgay.Value.Date)
                {
                    MessageBox.Show("Từ ngày không được lớn hơn Đến ngày.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DateTime fromDate = dtpTuNgay.Value.Date;
                DateTime toDate = dtpDenNgay.Value.Date.AddDays(1);
                string selectedReport = cmbLoaiBaoCao.Text;
                bool showAll = string.IsNullOrWhiteSpace(selectedReport) || selectedReport == "Tất cả báo cáo";

                pnlGridData3.Visible = showAll || selectedReport == "Tổng hợp mượn trả";
                pnlGridData2.Visible = showAll || selectedReport == "Thiết bị sử dụng nhiều nhất";
                pnlGridData4.Visible = showAll || selectedReport == "Thiết bị quá hạn";

                if (pnlGridData3.Visible)
                    LoadMonthlyStats(fromDate, toDate);
                else
                    dgvData.DataSource = null;

                if (pnlGridData2.Visible)
                    LoadTopDevices(fromDate, toDate);
                else
                    dgvData2.DataSource = null;

                if (pnlGridData4.Visible)
                    LoadOverdueTickets(fromDate, toDate);
                else
                    dgvData4.DataSource = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải báo cáo: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadMonthlyStats(DateTime from, DateTime to)
        {
            List<MonthlyStatsModel> list = ReportService.GetMonthlyStats(from, to);
            dgvData.DataSource = list;
            dgvData.AutoGenerateColumns = true;

            if (dgvData.Columns.Count > 0)
            {
                dgvData.Columns["Month"].HeaderText = "Tháng";
                dgvData.Columns["TicketCount"].HeaderText = "Số phiếu mượn";
                dgvData.Columns["TotalBorrowedQty"].HeaderText = "Tổng SL mượn";
                dgvData.Columns["ReturnedQty"].HeaderText = "Đã trả";
                dgvData.Columns["OverdueQty"].HeaderText = "Quá hạn";
            }

            // Summary row
            lblThongKeMuonTra.Text = $"📊  Thống kê mượn trả theo tháng ({from.Year})";
        }

        private void LoadTopDevices(DateTime from, DateTime to)
        {
            List<TopDeviceModel> list = ReportService.GetTopDevices(from, to);

            dgvData2.AutoGenerateColumns = false;
            colHang.DataPropertyName = "Rank";
            colThietBi.DataPropertyName = "DeviceName";
            colLuotMuon.DataPropertyName = "BorrowCount";
            colTiLe.DataPropertyName = "Ratio";
            dgvData2.DataSource = list;
        }

        private void LoadOverdueTickets(DateTime from, DateTime to)
        {
            List<OverdueTicketModel> list = ReportService.GetOverdueTickets(from, to);

            dgvData4.AutoGenerateColumns = false;
            colSoPhieu.DataPropertyName = "TicketCode";
            colNguoiMuon.DataPropertyName = "BorrowerName";
            colPhong.DataPropertyName = "RoomName";
            colNgayMuon.DataPropertyName = "BorrowDate";
            colHanTra.DataPropertyName = "ExpectedReturnDate";
            colSoNgayQua.DataPropertyName = "OverdueDays";
            colTinhTrang.DataPropertyName = "Status";
            dgvData4.DataSource = list;
        }

        private void BtnXuatExcel_Click(object sender, EventArgs e)
        {
            ExcelPackage.License.SetNonCommercialPersonal("BaoCaoUser");
            using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "Excel Workbook|*.xlsx", Title = "Lưu Báo Cáo Thống Kê" })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var fileInfo = new System.IO.FileInfo(sfd.FileName);
                        if (fileInfo.Exists)
                        {
                            fileInfo.Delete();
                        }
                        using (ExcelPackage package = new ExcelPackage(fileInfo))
                        {
                            // Hàm phụ giúp xuất DataGridView ra sheet
                            void ExportGridToSheet(DataGridView dgv, string sheetName)
                            {
                                if (dgv.Rows.Count == 0) return; // Không tạo sheet nếu grid trống
                                ExcelWorksheet ws = package.Workbook.Worksheets.Add(sheetName);

                                // Ghi tiêu đề cột
                                int colIndex = 1;
                                for (int i = 0; i < dgv.Columns.Count; i++)
                                {
                                    if (dgv.Columns[i].Visible)
                                    {
                                        ws.Cells[1, colIndex].Value = dgv.Columns[i].HeaderText;
                                        ws.Cells[1, colIndex].Style.Font.Bold = true;
                                        colIndex++;
                                    }
                                }

                                // Ghi dữ liệu từng dòng
                                for (int i = 0; i < dgv.Rows.Count; i++)
                                {
                                    colIndex = 1;
                                    for (int j = 0; j < dgv.Columns.Count; j++)
                                    {
                                        if (dgv.Columns[j].Visible)
                                        {
                                            ws.Cells[i + 2, colIndex].Value = dgv.Rows[i].Cells[j].Value?.ToString();
                                            colIndex++;
                                        }
                                    }
                                }
                                ws.Cells.AutoFitColumns();
                            }

                            // Xuất 3 bảng
                            if (pnlGridData3.Visible)
                                ExportGridToSheet(dgvData, "ThongKeTheoThang");
                            if (pnlGridData2.Visible)
                                ExportGridToSheet(dgvData2, "ThietBiSuDungNhieu");
                            if (pnlGridData4.Visible)
                                ExportGridToSheet(dgvData4, "ThietBiQuaHan");

                            package.Save();
                        }
                        MessageBox.Show("Đã xuất báo cáo ra Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi xuất Excel: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}



