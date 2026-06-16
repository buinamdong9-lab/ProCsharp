namespace FrmProject.Models
{
    public sealed class BorrowTicketDraft
    {
        public int BorrowerId { get; set; }
        public string TicketCode { get; set; } = string.Empty;
        public DateTime BorrowDate { get; set; }
        public DateTime ExpectedReturnDate { get; set; }
        public string Purpose { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public List<BorrowTicketDraftItem> Items { get; } = new();
    }

    public sealed class BorrowTicketDraftItem
    {
        public int DeviceId { get; set; }
        public int InstanceId { get; set; }
        public int Quantity { get; set; }
    }
}
