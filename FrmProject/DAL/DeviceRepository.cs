using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using Dapper;
using FrmProject.Models;

namespace FrmProject.DAL
{
    public class DeviceRepository : IDeviceRepository
    {
        public List<DeviceDisplayModel> GetAllDevices()
        {
            using SqlConnection conn = DbHelper.GetConnection();
            var rows = conn.Query("sp_GetAllDevices", commandType: CommandType.StoredProcedure);
            return rows.Select(row => {
                var dict = (IDictionary<string, object>)row;
                return new DeviceDisplayModel
                {
                    DeviceID = Convert.ToInt32(dict["DeviceID"]),
                    DeviceCode = dict["DeviceCode"]?.ToString() ?? "",
                    DeviceName = dict["Tên thiết bị"]?.ToString() ?? "",
                    CategoryName = dict["Loại thiết bị"]?.ToString() ?? "",
                    RoomName = dict["Vị trí"]?.ToString() ?? "",
                    Status = dict["Tình trạng"]?.ToString() ?? "",
                    TotalQuantity = Convert.ToInt32(dict["Số lượng"]),
                    AvailableQuantity = Convert.ToInt32(dict["AvailableQuantity"]),
                    Note = dict["Ghi chú"]?.ToString() ?? ""
                };
            }).ToList();
        }

        public int GetTotalDevicesCount(string keyword = "", string categoryName = "", string status = "")
        {
            using SqlConnection conn = DbHelper.GetConnection();
            return conn.ExecuteScalar<int>(
                "sp_GetTotalDevicesCount",
                new
                {
                    keyword = string.IsNullOrWhiteSpace(keyword) ? null : keyword,
                    categoryName = string.IsNullOrWhiteSpace(categoryName) ? null : categoryName,
                    status = string.IsNullOrWhiteSpace(status) ? null : status
                },
                commandType: CommandType.StoredProcedure);
        }

        public List<DeviceDisplayModel> GetDevicesPaged(int pageNumber, int pageSize, string keyword = "", string categoryName = "", string status = "")
        {
            using SqlConnection conn = DbHelper.GetConnection();
            var rows = conn.Query(
                "sp_GetDevicesPaged",
                new
                {
                    pageNumber,
                    pageSize,
                    keyword = string.IsNullOrWhiteSpace(keyword) ? null : keyword,
                    categoryName = string.IsNullOrWhiteSpace(categoryName) ? null : categoryName,
                    status = string.IsNullOrWhiteSpace(status) ? null : status
                },
                commandType: CommandType.StoredProcedure);

            return rows.Select(row => {
                var dict = (IDictionary<string, object>)row;
                return new DeviceDisplayModel
                {
                    DeviceID = Convert.ToInt32(dict["DeviceID"]),
                    DeviceCode = dict["DeviceCode"]?.ToString() ?? "",
                    DeviceName = dict["Tên thiết bị"]?.ToString() ?? "",
                    CategoryName = dict["Loại thiết bị"]?.ToString() ?? "",
                    RoomName = dict["Vị trí"]?.ToString() ?? "",
                    Status = dict["Tình trạng"]?.ToString() ?? "",
                    TotalQuantity = Convert.ToInt32(dict["Số lượng"]),
                    AvailableQuantity = Convert.ToInt32(dict["AvailableQuantity"]),
                    Note = dict["Ghi chú"]?.ToString() ?? ""
                };
            }).ToList();
        }

        public List<CategoryModel> GetCategories()
        {
            using SqlConnection conn = DbHelper.GetConnection();
            return conn.Query<CategoryModel>("sp_GetCategories", commandType: CommandType.StoredProcedure).ToList();
        }

        public void SaveDevice(
            int deviceId,
            string deviceCode,
            string deviceName,
            string categoryName,
            string roomNameOrCode,
            int totalQuantity,
            int selectedTotalQuantity,
            int selectedAvailableQuantity,
            string status,
            string note)
        {
            int borrowedQuantity = selectedTotalQuantity - selectedAvailableQuantity;

            using SqlConnection conn = DbHelper.GetConnection();
            conn.Execute(
                "sp_SaveDevice",
                new
                {
                    deviceId,
                    deviceCode,
                    deviceName,
                    categoryName,
                    roomNameOrCode = string.IsNullOrWhiteSpace(roomNameOrCode) ? null : roomNameOrCode,
                    totalQuantity,
                    borrowedQuantity,
                    status = string.IsNullOrWhiteSpace(status) ? "Tốt" : status,
                    note
                },
                commandType: CommandType.StoredProcedure);
        }

        public void DeleteDevice(int deviceId)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Execute(
                "sp_DeleteDevice",
                new
                {
                    id = deviceId,
                    note = $"Xóa mềm ngày {DateTime.Now:dd/MM/yyyy HH:mm}",
                    retiredStatus = DeviceStatus.Retired,
                    availableStatus = DeviceStatus.Available
                },
                commandType: CommandType.StoredProcedure);
        }

        public string GenerateDeviceCode()
        {
            using SqlConnection conn = DbHelper.GetConnection();
            object? result = conn.ExecuteScalar("sp_GenerateDeviceCode", commandType: CommandType.StoredProcedure);
            int nextId = (result == DBNull.Value || result == null) ? 1 : Convert.ToInt32(result) + 1;
            return $"TB{nextId:D2}";
        }

        public List<DeviceDisplayModel> GetAvailableDevices()
        {
            using SqlConnection conn = DbHelper.GetConnection();
            return conn.Query<DeviceDisplayModel>(
                "sp_GetAvailableDevices",
                new
                {
                    maintenanceStatus = DeviceStatus.Maintenance,
                    retiredStatus = DeviceStatus.Retired,
                    retiredRoomStatus = RoomStatus.Retired
                },
                commandType: CommandType.StoredProcedure).ToList();
        }

        public List<RoomDeviceModel> GetDevicesByRoom(string roomCode)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            var rows = conn.Query(
                "sp_GetDevicesByRoom",
                new
                {
                    roomCode,
                    retiredStatus = DeviceStatus.Retired
                },
                commandType: CommandType.StoredProcedure);

            return rows.Select(row => {
                var dict = (IDictionary<string, object>)row;
                return new RoomDeviceModel
                {
                    DeviceCode = dict["Mã TB"]?.ToString() ?? "",
                    DeviceName = dict["Tên thiết bị"]?.ToString() ?? "",
                    Quantity = Convert.ToInt32(dict["SL"]),
                    Status = dict["Tình trạng"]?.ToString() ?? ""
                };
            }).ToList();
        }
    }
}
