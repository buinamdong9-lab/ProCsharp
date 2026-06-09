namespace FrmProject.Models
{
    public class DeviceInstanceModel
    {
        public int InstanceID { get; set; }
        public int DeviceID { get; set; }
        public string AssetCode { get; set; } = string.Empty;
        public string Status { get; set; } = "Có sẵn";
        public string Condition { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public string DeviceName { get; set; } = string.Empty;
    }
}
