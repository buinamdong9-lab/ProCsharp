namespace FrmProject.Models
{
    internal static class BorrowTicketStatus
    {
        public const string Pending = "PENDING";
        public const string Borrowing = "BORROWING";
        public const string ReturnPending = "RETURN_PENDING";
        public const string Returned = "RETURNED";
        public const string Rejected = "REJECTED";
    }

    internal static class DeviceStatus
    {
        public const string Available = "Có sẵn";
        public const string Borrowed = "Đang mượn";
        public const string Maintenance = "Bảo trì";
        public const string Retired = "Ngừng sử dụng";
    }

    internal static class DeviceCondition
    {
        public const string Good = "Tốt";
    }

    internal static class RoomStatus
    {
        public const string Active = "Hoạt động";
        public const string Retired = "Ngừng sử dụng";
    }
}
