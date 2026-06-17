using System;

namespace FrmProject.Models
{
    public class TicketHistoryModel
    {
        public int TicketID { get; set; }
        public string TicketCode { get; set; } = string.Empty;
        public string CreatedDate { get; set; } = string.Empty;
        public string BorrowerName { get; set; } = string.Empty;
        public string RoomName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string BorrowDate { get; set; } = string.Empty;
        public string ExpectedReturnDate { get; set; } = string.Empty;
        public string ReturnDate { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
    }

    public class PendingReturnTicketModel
    {
        public int TicketID { get; set; }
        public string TicketCode { get; set; } = string.Empty;
        public string BorrowerName { get; set; } = string.Empty;
        public string BorrowDate { get; set; } = string.Empty;
        public string ExpectedReturnDate { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
    }

    public class DashboardBorrowingItemModel
    {
        public string DeviceCode { get; set; } = string.Empty;
        public string DeviceName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string BorrowerName { get; set; } = string.Empty;
        public DateTime BorrowDate { get; set; }
        public DateTime ExpectedReturnDate { get; set; }
    }

    public class RoleStatsModel
    {
        public int RoleID { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public int UserCount { get; set; }
    }

    public class ReturnTicketItemModel
    {
        public int DeviceID { get; set; }
        public int InstanceID { get; set; }
        public string AssetCode { get; set; } = string.Empty;
        public string DeviceName { get; set; } = string.Empty;
        public int BorrowQty { get; set; }
        public int ReturnQty { get; set; }
        public string BorrowCondition { get; set; } = string.Empty;
        public string ReturnCondition { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
    }

    public class DashboardMonthlyStatsModel
    {
        public string Month { get; set; } = string.Empty;
        public int TicketCount { get; set; }
        public int DeviceCount { get; set; }
    }
}
