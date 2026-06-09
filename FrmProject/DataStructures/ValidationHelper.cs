using System.Text.RegularExpressions;

namespace FrmProject.DataStructures
{
    /// <summary>
    /// Centralized input validation — tập trung logic kiểm tra dữ liệu đầu vào.
    /// Thay vì rải rác validation trong mỗi event handler, gọi ValidationHelper từ mọi form.
    /// </summary>
    internal static class ValidationHelper
    {
        private static readonly Regex EmailRegex = new(
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex PhoneRegex = new(
            @"^[\d\s\-\+\(\)]{8,15}$",
            RegexOptions.Compiled);

        /// <summary>
        /// Kiểm tra các trường bắt buộc. Trả về tên trường đầu tiên bị trống, hoặc null nếu tất cả hợp lệ.
        /// </summary>
        public static string? GetFirstEmptyField(params (string Value, string FieldName)[] fields)
        {
            foreach (var (value, fieldName) in fields)
            {
                if (string.IsNullOrWhiteSpace(value))
                    return fieldName;
            }
            return null;
        }

        /// <summary>
        /// Kiểm tra các trường bắt buộc và hiển thị MessageBox nếu có trường trống.
        /// Trả về true nếu tất cả hợp lệ.
        /// </summary>
        public static bool ValidateRequired(params (string Value, string FieldName)[] fields)
        {
            string? emptyField = GetFirstEmptyField(fields);
            if (emptyField != null)
            {
                MessageBox.Show($"Vui lòng nhập {emptyField}!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Kiểm tra định dạng email hợp lệ.
        /// </summary>
        public static bool IsValidEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return true; // Email không bắt buộc — trống thì bỏ qua

            return EmailRegex.IsMatch(email.Trim());
        }

        /// <summary>
        /// Kiểm tra định dạng số điện thoại hợp lệ.
        /// </summary>
        public static bool IsValidPhone(string? phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return true; // SĐT không bắt buộc — trống thì bỏ qua

            return PhoneRegex.IsMatch(phone.Trim());
        }

        /// <summary>
        /// Kiểm tra mật khẩu mạnh: ≥6 ký tự, có ít nhất 1 chữ cái và 1 chữ số.
        /// </summary>
        public static bool IsStrongPassword(string? password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            if (password.Length < 6)
                return false;

            bool hasLetter = false;
            bool hasDigit = false;

            foreach (char c in password)
            {
                if (char.IsLetter(c)) hasLetter = true;
                if (char.IsDigit(c)) hasDigit = true;
                if (hasLetter && hasDigit) return true;
            }

            return hasLetter && hasDigit;
        }

        /// <summary>
        /// Kiểm tra khoảng ngày hợp lệ (từ ngày ≤ đến ngày).
        /// </summary>
        public static bool IsValidDateRange(DateTime from, DateTime to)
        {
            return from.Date <= to.Date;
        }

        /// <summary>
        /// Kiểm tra email và hiển thị MessageBox nếu không hợp lệ.
        /// </summary>
        public static bool ValidateEmail(string? email)
        {
            if (!IsValidEmail(email))
            {
                MessageBox.Show("Định dạng email không hợp lệ!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Kiểm tra SĐT và hiển thị MessageBox nếu không hợp lệ.
        /// </summary>
        public static bool ValidatePhone(string? phone)
        {
            if (!IsValidPhone(phone))
            {
                MessageBox.Show("Định dạng số điện thoại không hợp lệ!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Kiểm tra mật khẩu mạnh và hiển thị MessageBox nếu không đạt.
        /// </summary>
        public static bool ValidatePassword(string? password)
        {
            if (!IsStrongPassword(password))
            {
                MessageBox.Show("Mật khẩu phải có ít nhất 6 ký tự, bao gồm chữ cái và chữ số!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }
    }
}

