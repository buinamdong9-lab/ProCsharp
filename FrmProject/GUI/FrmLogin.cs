using FrmProject.GUI;
using System.Collections.Concurrent;

namespace FrmProject
{
    public partial class FrmLogin : Form
    {
        private IAuthService AuthService => AppServiceProvider.Get<IAuthService>();
        private sealed class UnknownUserAttemptState
        {
            public int FailedAttempts { get; init; }
            public DateTime LockoutUntil { get; init; }
        }

        private static readonly ConcurrentDictionary<string, UnknownUserAttemptState> _unknownUserAttempts =
            new(StringComparer.OrdinalIgnoreCase);

        private const int MaxAttempts = 5;
        private static readonly TimeSpan LockoutDuration = TimeSpan.FromMinutes(5);

        public FrmLogin()
        {
            InitializeComponent();

            btnShowPass.Click += (s, e) =>
            {
                txtControl.PasswordChar = txtControl.PasswordChar == '*' ? '\0' : '*';
            };

            txtControl.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter) btnLogin_Click(this, EventArgs.Empty);
            };

            txtUsername.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter) txtControl.Focus();
            };
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            Application.Exit();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtControl.Text.Trim();

            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtControl.Focus();
                return;
            }

            try
            {
                LoginUserRecord? user = AuthService.GetLoginUser(username);
                if (user != null)
                {
                    if (IsLockoutActive(user.LockoutUntil, out TimeSpan remainingLockout))
                    {
                        ShowLockoutMessage(remainingLockout);
                        return;
                    }

                    bool passwordOk = PasswordHelper.VerifyPassword(user.PasswordHash, password);
                    bool canAccess = user.IsActive && !user.IsLocked;

                    if (passwordOk && canAccess)
                    {
                        AuthService.ResetFailedLoginState(user.UserID);
                        _unknownUserAttempts.TryRemove(username, out _);

                        // Ghi lịch sử đăng nhập thành công ra file log
                        AppLogger.Info($"[LỊCH SỬ] Người dùng '{user.FullName}' (Role: {user.RoleName}) đã đăng nhập vào hệ thống thành công.");

                        txtControl.Text = string.Empty;
                        Hide();

                        using FrmMain frm = new FrmMain(user.UserID, user.FullName, user.RoleName);
                        DialogResult result = frm.ShowDialog(this);

                        if (result == DialogResult.Retry)
                        {
                            Show();
                            Activate();
                            txtUsername.Focus();
                        }
                        else
                        {
                            Close();
                        }
                    }
                    else
                    {
                        LoginAttemptResult result = AuthService.RegisterFailedLogin(user.UserID, user.FailedLoginCount, user.LockoutUntil);
                        AppLogger.Warn($"[CẢNH BÁO] Tài khoản '{username}' đăng nhập thất bại.");
                        ShowFailedLoginMessage(result, "Tên đăng nhập hoặc mật khẩu không đúng, hoặc tài khoản hiện không khả dụng.");
                        txtControl.Text = string.Empty;
                        txtControl.Focus();
                    }
                }
                else
                {
                    if (IsUnknownUserLockedOut(username, out TimeSpan remaining))
                    {
                        ShowLockoutMessage(remaining);
                        return;
                    }

                    LoginAttemptResult result = RegisterUnknownUserFailure(username);
                    AppLogger.Warn($"[CẢNH BÁO] Tài khoản không tồn tại '{username}' cố gắng đăng nhập thất bại.");
                    ShowFailedLoginMessage(result, "Tên đăng nhập hoặc mật khẩu không đúng, hoặc tài khoản hiện không khả dụng.");
                    txtControl.Text = string.Empty;
                    txtControl.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối cơ sở dữ liệu:\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static bool IsLockoutActive(DateTime? lockoutUntil, out TimeSpan remaining)
        {
            if (lockoutUntil.HasValue && lockoutUntil.Value > DateTime.Now)
            {
                remaining = lockoutUntil.Value - DateTime.Now;
                return true;
            }

            remaining = TimeSpan.Zero;
            return false;
        }

        private static bool IsUnknownUserLockedOut(string username, out TimeSpan remaining)
        {
            remaining = TimeSpan.Zero;
            if (!_unknownUserAttempts.TryGetValue(username, out UnknownUserAttemptState? state))
                return false;

            if (state.LockoutUntil > DateTime.Now)
            {
                remaining = state.LockoutUntil - DateTime.Now;
                return true;
            }

            if (state.LockoutUntil != DateTime.MinValue)
                _unknownUserAttempts.TryRemove(username, out _);

            return false;
        }

        private static LoginAttemptResult RegisterUnknownUserFailure(string username)
        {
            DateTime now = DateTime.Now;
            UnknownUserAttemptState newState = _unknownUserAttempts.AddOrUpdate(
                username,
                _ => BuildUnknownUserAttemptState(new UnknownUserAttemptState { FailedAttempts = 0, LockoutUntil = DateTime.MinValue }, now),
                (_, current) => BuildUnknownUserAttemptState(current, now));

            return new LoginAttemptResult
            {
                AttemptsLeft = newState.LockoutUntil > now ? 0 : Math.Max(0, MaxAttempts - newState.FailedAttempts),
                LockoutUntil = newState.LockoutUntil > now ? newState.LockoutUntil : null
            };
        }

        private static UnknownUserAttemptState BuildUnknownUserAttemptState(UnknownUserAttemptState current, DateTime now)
        {
            if (current.LockoutUntil > now)
                return current;

            int nextFailedCount = current.FailedAttempts;
            if (current.LockoutUntil != DateTime.MinValue && current.LockoutUntil <= now)
                nextFailedCount = 0;

            nextFailedCount++;
            if (nextFailedCount >= MaxAttempts)
            {
                return new UnknownUserAttemptState
                {
                    FailedAttempts = 0,
                    LockoutUntil = now.Add(LockoutDuration)
                };
            }

            return new UnknownUserAttemptState
            {
                FailedAttempts = nextFailedCount,
                LockoutUntil = DateTime.MinValue
            };
        }

        private static void ShowFailedLoginMessage(LoginAttemptResult result, string baseMessage)
        {
            if (result.IsLockedOut && result.LockoutUntil.HasValue)
            {
                ShowLockoutMessage(result.LockoutUntil.Value - DateTime.Now);
                return;
            }

            MessageBox.Show(
                $"{baseMessage}\nCòn {result.AttemptsLeft} lần thử.",
                "Đăng nhập thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private static void ShowLockoutMessage(TimeSpan remaining)
        {
            int totalMinutes = Math.Max(0, (int)remaining.TotalMinutes);
            int seconds = Math.Max(0, remaining.Seconds);
            MessageBox.Show(
                $"Tài khoản tạm khóa do đăng nhập sai quá nhiều lần.\nVui lòng thử lại sau khoảng {totalMinutes} phút {seconds} giây.",
                "Tạm khóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}

