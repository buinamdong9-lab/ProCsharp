using System.Data;

namespace FrmProject.Models
{
    internal sealed class ReturnTicketDetails
    {
        public string TicketDisplay { get; set; } = string.Empty;
        public string BorrowerName { get; set; } = string.Empty;
        public DateTime BorrowDate { get; set; }
        public DateTime ExpectedReturnDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string ReturnNote { get; set; } = string.Empty;
        public DataTable Items { get; set; } = new();
    }
}
