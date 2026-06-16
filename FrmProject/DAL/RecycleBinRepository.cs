using Microsoft.Data.SqlClient;
using System.Data;
using System;
using System.Linq;
using System.Collections.Generic;
using Dapper;

namespace FrmProject.DAL
{
    public class RecycleBinRepository : IRecycleBinRepository
    {
        public DataTable Load(string itemType, string keyword)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();

            DataTable result = CreateResultTable();
            if (itemType == "Tất cả" || itemType == "Thiết bị")
                AppendDeletedDevices(conn, result, keyword);
            if (itemType == "Tất cả" || itemType == "Mã cá thể")
                AppendDeletedInstances(conn, result, keyword);
            if (itemType == "Tất cả" || itemType == "Phòng học")
                AppendDeletedRooms(conn, result, keyword);
            if (itemType == "Tất cả" || itemType == "Người dùng")
                AppendDeletedUsers(conn, result, keyword);

            return result;
        }

        public void Restore(string itemType, int id)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();

            switch (itemType)
            {
                case "Thiết bị":
                    conn.Execute(
                        "sp_RestoreDevice",
                        new { id, goodStatus = DeviceCondition.Good, availableStatus = DeviceStatus.Available, retiredStatus = DeviceStatus.Retired },
                        commandType: CommandType.StoredProcedure);
                    break;
                case "Mã cá thể":
                    conn.Execute(
                        "sp_RestoreInstance",
                        new { id, availableStatus = DeviceStatus.Available, retiredStatus = DeviceStatus.Retired },
                        commandType: CommandType.StoredProcedure);
                    break;
                case "Phòng học":
                    conn.Execute(
                        "sp_RestoreRoom",
                        new { id, activeStatus = RoomStatus.Active },
                        commandType: CommandType.StoredProcedure);
                    break;
                case "Người dùng":
                    conn.Execute(
                        "sp_RestoreUser",
                        new { id },
                        commandType: CommandType.StoredProcedure);
                    break;
                default:
                    throw new InvalidOperationException("Loại dữ liệu không hợp lệ.");
            }
        }

        public void DeleteForever(string itemType, int id)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();

            switch (itemType)
            {
                case "Thiết bị":
                    conn.Execute(
                        "sp_DeleteDeviceForever",
                        new { id, retiredStatus = DeviceStatus.Retired },
                        commandType: CommandType.StoredProcedure);
                    break;
                case "Mã cá thể":
                    conn.Execute(
                        "sp_DeleteInstanceForever",
                        new { id, retiredStatus = DeviceStatus.Retired, availableStatus = DeviceStatus.Available },
                        commandType: CommandType.StoredProcedure);
                    break;
                case "Phòng học":
                    conn.Execute(
                        "sp_DeleteRoomForever",
                        new { id, retiredStatus = RoomStatus.Retired },
                        commandType: CommandType.StoredProcedure);
                    break;
                case "Người dùng":
                    conn.Execute(
                        "sp_DeleteUserForever",
                        new { id },
                        commandType: CommandType.StoredProcedure);
                    break;
                default:
                    throw new InvalidOperationException("Loại dữ liệu không hợp lệ.");
            }
        }

        private static DataTable CreateResultTable()
        {
            DataTable table = new();
            table.Columns.Add("Loại", typeof(string));
            table.Columns.Add("ID", typeof(int));
            table.Columns.Add("Mã", typeof(string));
            table.Columns.Add("Tên", typeof(string));
            table.Columns.Add("Trạng thái", typeof(string));
            table.Columns.Add("Ghi chú", typeof(string));
            return table;
        }

        private static void AppendDeletedDevices(SqlConnection conn, DataTable result, string keyword)
        {
            AppendRows(conn, result, "Thiết bị", "sp_GetDeletedDevices", new { status = DeviceStatus.Retired, kw = keyword.Trim() });
        }

        private static void AppendDeletedInstances(SqlConnection conn, DataTable result, string keyword)
        {
            AppendRows(conn, result, "Mã cá thể", "sp_GetDeletedInstances", new { status = DeviceStatus.Retired, kw = keyword.Trim() });
        }

        private static void AppendDeletedRooms(SqlConnection conn, DataTable result, string keyword)
        {
            AppendRows(conn, result, "Phòng học", "sp_GetDeletedRooms", new { status = RoomStatus.Retired, kw = keyword.Trim() });
        }

        private static void AppendDeletedUsers(SqlConnection conn, DataTable result, string keyword)
        {
            AppendRows(conn, result, "Người dùng", "sp_GetDeletedUsers", new { kw = keyword.Trim() });
        }

        private static void AppendRows(SqlConnection conn, DataTable result, string type, string spName, object param)
        {
            var rows = conn.Query(spName, param, commandType: CommandType.StoredProcedure);
            foreach (var row in rows)
            {
                var dict = (IDictionary<string, object>)row;
                var keys = dict.Keys.ToList();
                result.Rows.Add(
                    type,
                    dict[keys[0]] != null ? Convert.ToInt32(dict[keys[0]]) : 0,
                    dict[keys[1]]?.ToString() ?? string.Empty,
                    dict[keys[2]]?.ToString() ?? string.Empty,
                    dict[keys[3]]?.ToString() ?? string.Empty,
                    dict[keys[4]]?.ToString() ?? string.Empty
                );
            }
        }
    }
}
