using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using FrmProject.Models;

namespace FrmProject.DAL
{
    public interface IAuthRepository
    {
        LoginUserRecord? GetLoginUser(string username);
        void ResetFailedLoginState(int userId);
        LoginAttemptResult RegisterFailedLogin(int userId, int failedCount, DateTime? lockoutUntil);
    }

    public interface IUserRepository
    {
        List<UserDisplayModel> GetAllUsers();
        UserIdentity? FindUserIdentity(string userCode, string username);
        void DeleteUser(int userId);
        string GenerateUserCode();
        void SaveUser(UserEditModel user, bool isAdding);
        void ResetPassword(int userId, string passwordHash);
        List<UserDisplayModel> SearchUsers(string keyword, string role, string status);
        List<UserDisplayModel> GetUsersPaged(int pageNumber, int pageSize, string keyword = "", string role = "", string status = "");
        int GetTotalUsersCount(string keyword = "", string role = "", string status = "");
    }

    public interface IDeviceRepository
    {
        List<DeviceDisplayModel> GetAllDevices();
        int GetTotalDevicesCount(string keyword = "", string categoryName = "", string status = "");
        List<DeviceDisplayModel> GetDevicesPaged(int pageNumber, int pageSize, string keyword = "", string categoryName = "", string status = "");
        List<CategoryModel> GetCategories();
        void SaveDevice(int deviceId, string deviceCode, string deviceName, string categoryName, string roomNameOrCode, int totalQuantity, int selectedTotalQuantity, int selectedAvailableQuantity, string status, string note);
        void DeleteDevice(int deviceId);
        string GenerateDeviceCode();
        List<DeviceDisplayModel> GetAvailableDevices();
        List<RoomDeviceModel> GetDevicesByRoom(string roomCode);
    }

    public interface IDeviceInstanceRepository
    {
        List<DeviceInstanceDisplayModel> GetByDevice(int deviceId);
        string GetDeviceCode(int deviceId);
        string GetNextAssetCode(int deviceId);
        bool AssetCodeExists(string assetCode);
        void Insert(int deviceId, string assetCode, string status, string condition);
        void Delete(int deviceId, int instanceId);
        void UpdateStatusAndCondition(int deviceId, int instanceId, string status, string condition);
        void UpdateDeviceQuantities(SqlConnection conn, SqlTransaction tran, int deviceId);
    }

    public interface IRoomRepository
    {
        List<RoomDisplayModel> GetAllRooms();
        List<RoomDisplayModel> SearchRooms(string keyword, string type, string status);
        (int RoomID, string Floor, string Capacity, string Note)? GetRoomByCode(string roomCode);
        void InsertRoom(string code, string name, string type, string floor, int capacity, string status, string note);
        void UpdateRoom(int roomId, string code, string name, string type, string floor, int capacity, string status, string note);
        void DeleteRoom(int roomId);
    }

    public interface IBorrowTicketRepository
    {
        List<LookupItem> GetEnabledUsers();
        List<LookupItem> GetRooms();
        List<LookupItem> GetBorrowableDevices();
        List<LookupItem> GetAvailableInstances(int deviceId);
        int CreatePendingTicket(BorrowTicketDraft draft);
        void ApproveBorrow(int ticketId);
        void RejectBorrow(int ticketId, string reason);
    }

    public interface IReturnTicketRepository
    {
        List<LookupItem> SearchBorrowingTickets(int currentUserId, AppRole appRole, string keyword = "");
        ReturnTicketDetails? GetTicketDetails(int ticketId);
        void ApplyPendingReturnQuantities(int ticketId, List<ReturnTicketItemModel> items);
        bool TryLoadPendingReturnRequest(int ticketId, out DateTime requestedAt, out List<ReturnRequestItem> pendingItems);
        void SubmitReturnRequest(int ticketId, int currentUserId, AppRole appRole, DateTime requestedAt, List<(int DeviceID, int InstanceID, int BorrowQty, int ReturnQty, string Note)> returnItems, string requestNote);
        void ApproveReturn(int ticketId, int approvedByUserId);
        void RejectReturn(int ticketId, string reason);
        List<PendingReturnTicketModel> GetPendingReturnTickets();
        string GetTicketStatus(SqlConnection conn, SqlTransaction? tran, int ticketId);
        void VerifyTicketOwnership(SqlConnection conn, SqlTransaction tran, int ticketId, int userId);
    }

    public interface ISettingsRepository
    {
        void EnsureAppSettingsTable();
        string GetValue(string key, string defaultValue = "");
        int GetIntValue(string key, int defaultValue);
        bool GetYesNoValue(string key, bool defaultValue);
        void SaveValues(IReadOnlyDictionary<string, string> settings);
        List<DeviceCategoryStatsModel> GetDeviceCategories();
        List<RoleStatsModel> GetRoles();
        void AddDeviceCategory(string categoryName);
    }

    public interface IDashboardRepository
    {
        DashboardSnapshot Load(int pageNumber = 1, int pageSize = 10);
        List<DashboardBorrowingItemModel> LoadBorrowingListOnly(int pageNumber, int pageSize);
    }

    public interface ITicketListRepository
    {
        List<TicketHistoryModel> SearchTickets(DateTime from, DateTime to, string keyword, string statusFilter, int currentUserId, AppRole appRole);
        TicketListStats GetStats(int currentUserId, AppRole appRole);
        TicketDetailView? GetTicketDetail(int ticketId, int currentUserId, AppRole appRole);
    }

    public interface IRecycleBinRepository
    {
        List<RecycleBinItemModel> Load(string itemType, string keyword);
        void Restore(string itemType, int id);
        void DeleteForever(string itemType, int id);
    }

    public interface IReturnRequestRepository
    {
        void SaveRequest(SqlConnection conn, SqlTransaction tran, int ticketId, DateTime requestedAt, IEnumerable<(int DeviceID, int InstanceID, int BorrowQty, int ReturnQty, string Note)> items, string note);
        bool TryLoadRequest(SqlConnection conn, SqlTransaction? tran, int ticketId, out DateTime requestedAt, out List<ReturnRequestItem> items);
        void ClearRequest(SqlConnection conn, SqlTransaction tran, int ticketId);
        void ValidateSchema(SqlConnection conn, SqlTransaction? tran = null);
    }

    public interface IReportRepository
    {
        List<MonthlyStatsModel> GetMonthlyStats(DateTime from, DateTime to);
        List<TopDeviceModel> GetTopDevices(DateTime from, DateTime to);
        List<OverdueTicketModel> GetOverdueTickets(DateTime from, DateTime to);
    }
}
