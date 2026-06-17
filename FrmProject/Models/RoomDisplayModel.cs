namespace FrmProject.Models
{
    public class RoomDisplayModel
    {
        public string RoomCode { get; set; } = string.Empty;
        public string RoomName { get; set; } = string.Empty;
        public string RoomType { get; set; } = string.Empty;
        public string Floor { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
