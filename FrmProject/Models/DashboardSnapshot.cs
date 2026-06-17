using System.Collections.Generic;

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
        public List<DashboardBorrowingItemModel> BorrowingList { get; init; } = new();
        public List<DashboardMonthlyStatsModel> MonthlyBorrowStats { get; init; } = new();
    }
}
