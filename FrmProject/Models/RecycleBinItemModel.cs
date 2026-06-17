namespace FrmProject.Models
{
    public class RecycleBinItemModel
    {
        public string ItemType { get; set; } = string.Empty;
        public int ID { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
    }
}
