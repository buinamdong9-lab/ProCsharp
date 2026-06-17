namespace FrmProject.Models
{
    public class CategoryModel
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; } = string.Empty;
    }

    public class RoomDeviceModel
    {
        public string DeviceCode { get; set; } = string.Empty;
        public string DeviceName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class DeviceInstanceDisplayModel
    {
        public int InstanceID { get; set; }
        public string AssetCode { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Condition { get; set; } = string.Empty;
    }

    public class DeviceCategoryStatsModel
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public int DeviceCount { get; set; }
        public string Action { get; set; } = string.Empty;
    }
}
