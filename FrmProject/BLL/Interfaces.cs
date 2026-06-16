using System;
using System.Data;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using FrmProject.Models;

namespace FrmProject.BLL
{
    public interface IAuthService
    {
        LoginUserRecord? GetLoginUser(string username);
        void ResetFailedLoginState(int userId);
        LoginAttemptResult RegisterFailedLogin(int userId, int failedCount, DateTime? lockoutUntil);
    }

    public interface IAuthorizationService
    {
        AppRole ResolveRole(string roleName);
        PermissionSet BuildPermissions(AppRole role);
    }

    public interface IBorrowApprovalService
    {
        void ApproveBorrow(int ticketId);
        void RejectBorrow(int ticketId, string reason);
    }

    public interface IBorrowTicketService
    {
        List<LookupItem> GetEnabledUsers();
        List<LookupItem> GetRooms();
        List<LookupItem> GetBorrowableDevices();
        List<LookupItem> GetAvailableInstances(int deviceId);
        int CreatePendingTicket(BorrowTicketDraft draft);
    }

    public interface IDashboardService
    {
        DashboardSnapshot Load(int pageNumber = 1, int pageSize = 10);
        DataTable LoadBorrowingListOnly(int pageNumber, int pageSize);
    }

    public interface IDeviceInstanceService
    {
        DataTable GetByDevice(int deviceId);
        string GetDeviceCode(int deviceId);
        string GetNextAssetCode(int deviceId);
        bool AssetCodeExists(string assetCode);
        void Insert(int deviceId, string assetCode, string status, string condition);
        void Delete(int deviceId, int instanceId);
        void UpdateStatusAndCondition(int deviceId, int instanceId, string status, string condition);
    }

    public interface IDeviceService
    {
        DataTable GetAllDevices();
        int GetTotalDevicesCount(string keyword = "", string categoryName = "", string status = "");
        DataTable GetDevicesPaged(int pageNumber, int pageSize, string keyword = "", string categoryName = "", string status = "");
        DataTable GetCategories();
        void SaveDevice(int deviceId, string deviceCode, string deviceName, string categoryName, string roomNameOrCode, int totalQuantity, int selectedTotalQuantity, int selectedAvailableQuantity, string status, string note);
        void DeleteDevice(int deviceId);
        string GenerateDeviceCode();
        List<DeviceDisplayModel> GetAvailableDevices();
        DataTable GetDevicesByRoom(string roomCode);
    }

    public interface IRecycleBinService
    {
        DataTable Load(string itemType, string keyword);
        void Restore(string itemType, int id);
        void DeleteForever(string itemType, int id);
    }

    public interface IReportService
    {
        DataTable GetMonthlyStats(DateTime from, DateTime to);
        DataTable GetTopDevices(DateTime from, DateTime to);
        DataTable GetOverdueTickets(DateTime from, DateTime to);
    }

    public interface IReturnApprovalService
    {
        void ApproveReturn(int ticketId, int approvedByUserId);
        void RejectReturn(int ticketId, string reason);
        void VerifyTicketOwnership(SqlConnection conn, SqlTransaction tran, int ticketId, int userId);
        DataTable GetPendingReturnTickets();
    }

    public interface IReturnTicketService
    {
        List<LookupItem> SearchBorrowingTickets(int currentUserId, AppRole appRole, string keyword = "");
        ReturnTicketDetails? GetTicketDetails(int ticketId);
        void ApplyPendingReturnQuantities(int ticketId, DataTable dt);
        bool TryLoadPendingReturnRequest(int ticketId, out DateTime requestedAt, out List<ReturnRequestItem> pendingItems);
        void SubmitReturnRequest(int ticketId, int currentUserId, AppRole appRole, DateTime requestedAt, List<(int DeviceID, int InstanceID, int BorrowQty, int ReturnQty, string Note)> returnItems, string requestNote);
    }

    public interface IRoomService
    {
        DataTable GetAllRooms();
        (int RoomID, string Floor, string Capacity, string Note)? GetRoomByCode(string roomCode);
        DataTable GetDevicesByRoom(string roomCode);
        void DeleteRoom(int roomId);
        void InsertRoom(string code, string name, string type, string floor, int capacity, string status, string note);
        void UpdateRoom(int id, string code, string name, string type, string floor, int capacity, string status, string note);
        DataTable SearchRooms(string keyword, string type, string status);
    }

    public interface ISettingsService
    {
        void EnsureAppSettingsTable();
        string GetValue(string key, string defaultValue = "");
        int GetIntValue(string key, int defaultValue);
        bool GetYesNoValue(string key, bool defaultValue);
        void SaveValues(IReadOnlyDictionary<string, string> settings);
        DataTable GetDeviceCategories();
        DataTable GetRoles();
        void AddDeviceCategory(string categoryName);
    }

    public interface ITicketListService
    {
        DataTable SearchTickets(DateTime from, DateTime to, string keyword, string statusFilter, int currentUserId, AppRole appRole);
        TicketListStats GetStats(int currentUserId, AppRole appRole);
        TicketDetailView? GetTicketDetail(int ticketId, int currentUserId, AppRole appRole);
    }

    public interface IUserService
    {
        DataTable GetAllUsers();
        UserIdentity? FindUserIdentity(string code, string username);
        void DeleteUser(int userId);
        string GenerateUserCode();
        void SaveUser(UserEditModel user, bool isAdding);
        void ResetPassword(int userId, string password);
        DataTable SearchUsers(string keyword, string role, string status);
        DataTable GetUsersPaged(int pageNumber, int pageSize, string keyword = "", string role = "", string status = "");
        int GetTotalUsersCount(string keyword = "", string role = "", string status = "");
    }
}
