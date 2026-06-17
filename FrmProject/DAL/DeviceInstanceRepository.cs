using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using Dapper;
using FrmProject.Models;

namespace FrmProject.DAL
{
    public class DeviceInstanceRepository : IDeviceInstanceRepository
    {
        public List<DeviceInstanceDisplayModel> GetByDevice(int deviceId)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            var rows = conn.Query(
                "sp_GetInstancesByDevice",
                new { deviceId },
                commandType: CommandType.StoredProcedure);

            return rows.Select(row => {
                var dict = (IDictionary<string, object>)row;
                return new DeviceInstanceDisplayModel
                {
                    InstanceID = Convert.ToInt32(dict["InstanceID"]),
                    AssetCode = dict["Mã tài sản"]?.ToString() ?? "",
                    Status = dict["Trạng thái"]?.ToString() ?? "",
                    Condition = dict["Tình trạng"]?.ToString() ?? ""
                };
            }).ToList();
        }

        /// <summary>
        /// Lấy DeviceCode (mã gốc) của loại thiết bị.
        /// </summary>
        public string GetDeviceCode(int deviceId)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            return conn.ExecuteScalar<string>(
                "sp_GetDeviceCodeOnly",
                new { deviceId },
                commandType: CommandType.StoredProcedure) ?? deviceId.ToString();
        }

        /// <summary>
        /// Tự động sinh mã cá thể tiếp theo dạng {DeviceCode}-{STT:D3}.
        /// Ví dụ: TH001 → TH001-001, TH001-002, ...
        /// </summary>
        public string GetNextAssetCode(int deviceId)
        {
            string deviceCode = GetDeviceCode(deviceId);

            using SqlConnection conn = DbHelper.GetConnection();
            var existingCodes = new HashSet<string>(
                conn.Query<string>(
                    "sp_GetInstanceAssetCodesByDevice",
                    new { deviceId },
                    commandType: CommandType.StoredProcedure),
                StringComparer.OrdinalIgnoreCase);

            // Tìm số thứ tự nhỏ nhất chưa dùng
            int seq = 1;
            string candidate;
            do
            {
                candidate = $"{deviceCode}-{seq:D3}";
                seq++;
            } while (existingCodes.Contains(candidate));

            return candidate;
        }

        public bool AssetCodeExists(string assetCode)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            return conn.ExecuteScalar<int>(
                "sp_AssetCodeExists",
                new { assetCode },
                commandType: CommandType.StoredProcedure) > 0;
        }

        public void Insert(int deviceId, string assetCode, string status, string condition)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Execute(
                "sp_InsertDeviceInstance",
                new
                {
                    devId = deviceId,
                    code = assetCode,
                    status,
                    cond = condition,
                    retiredStatus = DeviceStatus.Retired,
                    availableStatus = DeviceStatus.Available,
                    goodCondition = DeviceCondition.Good
                },
                commandType: CommandType.StoredProcedure);
        }

        public void Delete(int deviceId, int instanceId)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Execute(
                "sp_DeleteDeviceInstance",
                new
                {
                    deviceId,
                    instanceId,
                    note = $"Xóa mềm ngày {DateTime.Now:dd/MM/yyyy HH:mm}",
                    retiredStatus = DeviceStatus.Retired,
                    availableStatus = DeviceStatus.Available,
                    goodCondition = DeviceCondition.Good
                },
                commandType: CommandType.StoredProcedure);
        }

        public void UpdateStatusAndCondition(int deviceId, int instanceId, string status, string condition)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Execute(
                "sp_UpdateDeviceInstanceStatusAndCondition",
                new
                {
                    deviceId,
                    instanceId,
                    status,
                    condition,
                    retiredStatus = DeviceStatus.Retired,
                    availableStatus = DeviceStatus.Available,
                    goodCondition = DeviceCondition.Good
                },
                commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Cập nhật số lượng thiết bị sử dụng Stored Procedure.
        /// Được gọi từ BLL (như ReturnApprovalService).
        /// </summary>
        public void UpdateDeviceQuantities(SqlConnection conn, SqlTransaction tran, int deviceId)
        {
            conn.Execute(
                "sp_UpdateDeviceQuantities",
                new
                {
                    id = deviceId,
                    retiredStatus = DeviceStatus.Retired,
                    availableStatus = DeviceStatus.Available,
                    goodCondition = DeviceCondition.Good
                },
                transaction: tran,
                commandType: CommandType.StoredProcedure);
        }
    }
}
