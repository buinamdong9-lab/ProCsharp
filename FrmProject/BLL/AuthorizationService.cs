namespace FrmProject.BLL
{
    public class AuthorizationService : IAuthorizationService
    {
        public AppRole ResolveRole(string roleName)
        {
            string normalizedRole = roleName.Trim().ToLowerInvariant();

            if (normalizedRole.Contains("admin") || normalizedRole.Contains("quản trị"))
                return AppRole.Admin;

            if (normalizedRole.Contains("thủ kho") || normalizedRole.Contains("quan ly") ||
                normalizedRole.Contains("quản lý") || normalizedRole.Contains("staff"))
                return AppRole.Staff;

            return AppRole.User;
        }

        public PermissionSet BuildPermissions(AppRole role) =>
            role switch
            {
                AppRole.Admin => new PermissionSet
                {
                    Dashboard = true,
                    Devices = true,
                    Rooms = true,
                    Users = true,
                    BorrowTicket = true,
                    ReturnDevice = true,
                    TicketList = true,
                    Reports = true,
                    Settings = true
                },
                AppRole.Staff => new PermissionSet
                {
                    Dashboard = true,
                    Devices = true,
                    Rooms = true,
                    Users = false,
                    BorrowTicket = true,
                    ReturnDevice = true,
                    TicketList = true,
                    Reports = true,
                    Settings = false
                },
                _ => new PermissionSet
                {
                    Dashboard = false,
                    Devices = false,
                    Rooms = false,
                    Users = false,
                    BorrowTicket = true,
                    ReturnDevice = true,
                    TicketList = true,
                    Reports = false,
                    Settings = false
                }
            };
    }
}
