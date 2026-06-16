namespace FrmProject.Models
{
    public sealed class LoginUserRecord
    {
        public int UserID { get; init; }
        public string FullName { get; init; } = string.Empty;
        public string RoleName { get; init; } = "User";
        public string PasswordHash { get; init; } = string.Empty;
        public int FailedLoginCount { get; init; }
        public DateTime? LockoutUntil { get; init; }
        public bool IsActive { get; init; } = true;
        public bool IsLocked { get; init; }
    }

    public sealed class LoginAttemptResult
    {
        public int AttemptsLeft { get; init; }
        public DateTime? LockoutUntil { get; init; }
        public bool IsLockedOut => LockoutUntil.HasValue && LockoutUntil.Value > DateTime.Now;
    }
}
