using System;
using System.Collections.Generic;
using FrmProject.DAL;
using FrmProject.Models;

namespace FrmProject.BLL
{
    public class UserService(IUserRepository userRepository) : IUserService
    {
        public int GetTotalUsersCount(string keyword, string role, string status) =>
            userRepository.GetTotalUsersCount(keyword, role, status);

        public List<UserDisplayModel> GetUsersPaged(int pageNumber, int pageSize, string keyword, string role, string status) =>
            userRepository.GetUsersPaged(pageNumber, pageSize, keyword, role, status);

        public UserIdentity? FindUserIdentity(string code, string username) =>
            userRepository.FindUserIdentity(code, username);

        public string GenerateUserCode() => userRepository.GenerateUserCode();
        public void DeleteUser(int userId) => userRepository.DeleteUser(userId);

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
                var existing = userRepository.FindUserIdentity("", user.Username);
                if (existing != null)
                    throw new InvalidOperationException($"Tên đăng nhập '{user.Username}' đã tồn tại trong hệ thống.");
            }

            userRepository.SaveUser(user, isAdding);
        }

        public void ResetPassword(int userId, string passwordHash) => userRepository.ResetPassword(userId, passwordHash);

        public List<UserDisplayModel> GetAllUsers() => userRepository.GetAllUsers();

        public List<UserDisplayModel> SearchUsers(string keyword, string role, string status) =>
            userRepository.SearchUsers(keyword, role, status);
    }
}
