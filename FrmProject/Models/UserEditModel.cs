namespace FrmProject.Models
{
    public sealed class UserIdentity
    {
        public int UserId { get; init; }
        public string Username { get; init; } = string.Empty;
    }

    public sealed class UserEditModel
    {
        public int UserId { get; set; }
        public string UserCode { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public bool IsLocked { get; set; }
        public string? PasswordHash { get; set; }
    }
}
