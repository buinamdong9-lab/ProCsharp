namespace FrmProject.Models
{
    public class MonthlyStatsModel
    {
        public string Month { get; set; } = string.Empty;
        public int TicketCount { get; set; }
        public int TotalBorrowedQty { get; set; }
        public int ReturnedQty { get; set; }
        public int OverdueQty { get; set; }
    }

    public class TopDeviceModel
    {
        public int Rank { get; set; }
        public string DeviceName { get; set; } = string.Empty;
        public int BorrowCount { get; set; }
        public string Ratio { get; set; } = string.Empty;
    }

    public class OverdueTicketModel
    {
        public string TicketCode { get; set; } = string.Empty;
        public string BorrowerName { get; set; } = string.Empty;
        public string RoomName { get; set; } = string.Empty;
        public string BorrowDate { get; set; } = string.Empty;
        public string ExpectedReturnDate { get; set; } = string.Empty;
        public int OverdueDays { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
