using System.Data;

namespace FrmProject.Models
{
    public sealed class TicketListStats
    {
        public int Total { get; init; }
        public int Pending { get; init; }
        public int Active { get; init; }
        public int Overdue { get; init; }
    }

    public sealed class TicketDetailView
    {
        public string TicketDisplay { get; init; } = string.Empty;
        public string BorrowerName { get; init; } = string.Empty;
        public string RoomName { get; init; } = string.Empty;
        public string Purpose { get; init; } = string.Empty;
        public DateTime BorrowDate { get; init; }
        public DateTime ExpectedReturnDate { get; init; }
        public string ApprovedByName { get; init; } = string.Empty;
        public DateTime? ReturnDate { get; init; }
        public string StatusText { get; init; } = string.Empty;
        public int OverdueDays { get; init; }
        public DataTable Items { get; init; } = new();
    }
}
