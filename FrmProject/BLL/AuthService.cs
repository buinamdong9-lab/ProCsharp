using FrmProject.DAL;

namespace FrmProject.BLL
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;

        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public LoginUserRecord? GetLoginUser(string username) =>
            _authRepository.GetLoginUser(username);

        public void ResetFailedLoginState(int userId) =>
            _authRepository.ResetFailedLoginState(userId);

        public LoginAttemptResult RegisterFailedLogin(int userId, int failedCount, DateTime? lockoutUntil) =>
            _authRepository.RegisterFailedLogin(userId, failedCount, lockoutUntil);
    }
}
