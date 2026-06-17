namespace FrmProject.Models
{
    public class UserDisplayModel
    {
        public string UserCode { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public int BorrowingCount { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
