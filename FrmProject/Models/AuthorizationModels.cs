namespace FrmProject.Models
{
    public enum AppRole
    {
        User,
        Staff,
        Admin
    }

    public sealed class PermissionSet
    {
        public bool Dashboard { get; init; }
        public bool Devices { get; init; }
        public bool Rooms { get; init; }
        public bool Users { get; init; }
        public bool BorrowTicket { get; init; }
        public bool ReturnDevice { get; init; }
        public bool TicketList { get; init; }
        public bool Reports { get; init; }
        public bool Settings { get; init; }
    }
}
