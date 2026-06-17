using FrmProject.DAL;

namespace FrmProject.BLL
{
    public class AuthService(IAuthRepository authRepository) : IAuthService
    {
        public LoginUserRecord? GetLoginUser(string username) =>
            authRepository.GetLoginUser(username);

        public void ResetFailedLoginState(int userId) =>
            authRepository.ResetFailedLoginState(userId);

        public LoginAttemptResult RegisterFailedLogin(int userId, int failedCount, DateTime? lockoutUntil) =>
            authRepository.RegisterFailedLogin(userId, failedCount, lockoutUntil);
    }
}
