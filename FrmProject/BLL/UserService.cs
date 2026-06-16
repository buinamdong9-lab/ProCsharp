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
        public void SaveUser(UserEditModel user, bool isAdding) => _userRepository.SaveUser(user, isAdding);
        public void ResetPassword(int userId, string passwordHash) => _userRepository.ResetPassword(userId, passwordHash);

        public DataTable GetAllUsers() => _userRepository.GetAllUsers();
        public DataTable SearchUsers(string keyword, string role, string status) =>
            _userRepository.SearchUsers(keyword, role, status);
    }
}
