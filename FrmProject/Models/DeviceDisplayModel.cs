namespace FrmProject.Models
{
    public class DeviceDisplayModel
    {
        public int DeviceID { get; set; }
        public string DeviceCode { get; set; } = string.Empty;
        public string DeviceName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string RoomName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int TotalQuantity { get; set; }
        public int AvailableQuantity { get; set; }
        public string Note { get; set; } = string.Empty;
    }
}