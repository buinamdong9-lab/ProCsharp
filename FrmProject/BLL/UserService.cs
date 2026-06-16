using System.Data;
using FrmProject.DAL;

namespace FrmProject.BLL
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public int GetTotalUsersCount(string keyword, string role, string status) =>
            _userRepository.GetTotalUsersCount(keyword, role, status);

        public DataTable GetUsersPaged(int pageNumber, int pageSize, string keyword, string role, string status) =>
            _userRepository.GetUsersPaged(pageNumber, pageSize, keyword, role, status);

        public UserIdentity? FindUserIdentity(string code, string username) =>
            _userRepository.FindUserIdentity(code, username);

        public string GenerateUserCode() => _userRepository.GenerateUserCode();
        public void DeleteUser(int userId) => _userRepository.DeleteUser(userId);
        public void SaveUser(UserEditModel user, bool isAdding)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrWhiteSpace(user.FullName))
                throw new ArgumentException("Họ và tên không được để trống.");
            if (string.IsNullOrWhiteSpace(user.Username))
                throw new ArgumentException("Tên đăng nhập không được để trống.");
            if (user.Username.Length < 3)
                throw new ArgumentException("Tên đăng nhập phải có ít nhất 3 ký tự.");
            if (!System.Text.RegularExpressions.Regex.IsMatch(user.Username, "^[a-zA-Z0-9_]+$"))
                throw new ArgumentException("Tên đăng nhập chỉ được chứa chữ cái, chữ số và dấu gạch dưới.");

            if (!string.IsNullOrWhiteSpace(user.Email) && !System.Text.RegularExpressions.Regex.IsMatch(user.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new ArgumentException("Địa chỉ Email không đúng định dạng.");

            if (!string.IsNullOrWhiteSpace(user.Phone) && !System.Text.RegularExpressions.Regex.IsMatch(user.Phone, @"^[0-9+\-\s()]{8,15}$"))
                throw new ArgumentException("Số điện thoại không hợp lệ.");

            if (isAdding)
            {
                var existing = _userRepository.FindUserIdentity("", user.Username);
                if (existing != null)
                    throw new InvalidOperationException($"Tên đăng nhập '{user.Username}' đã tồn tại trong hệ thống.");
            }

            _userRepository.SaveUser(user, isAdding);
        }
        public void ResetPassword(int userId, string passwordHash) => _userRepository.ResetPassword(userId, passwordHash);

        public DataTable GetAllUsers() => _userRepository.GetAllUsers();
        public DataTable SearchUsers(string keyword, string role, string status) =>
            _userRepository.SearchUsers(keyword, role, status);
    }
}
