using System.Data;

namespace FrmProject.Models
{
    public sealed class DashboardSnapshot
    {
        public int TotalDevices { get; init; }
        public int BorrowedDevices { get; init; }
        public int OverdueTickets { get; init; }
        public int IssueDevices { get; init; }
        public int ReturnedToday { get; init; }
        public int TotalBorrowingCount { get; init; }
        public DataTable BorrowingList { get; init; } = new();
        public DataTable MonthlyBorrowStats { get; init; } = new();
    }
}
