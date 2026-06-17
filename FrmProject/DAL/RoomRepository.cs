using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using Dapper;
using FrmProject.Models;

namespace FrmProject.DAL
{
    public class RoomRepository : IRoomRepository
    {
        public List<RoomDisplayModel> GetAllRooms()
        {
            using SqlConnection conn = DbHelper.GetConnection();
            var rows = conn.Query("sp_GetAllRooms", commandType: CommandType.StoredProcedure);
            return rows.Select(row => {
                var dict = (IDictionary<string, object>)row;
                return new RoomDisplayModel
                {
                    RoomCode = dict["Mã phòng"]?.ToString() ?? "",
                    RoomName = dict["Tên phòng"]?.ToString() ?? "",
                    RoomType = dict["Loại"]?.ToString() ?? "",
                    Floor = dict["Tầng"]?.ToString() ?? "",
                    Capacity = Convert.ToInt32(dict["Sức chứa"]),
                    Status = dict["Trạng thái"]?.ToString() ?? ""
                };
            }).ToList();
        }

        public List<RoomDisplayModel> SearchRooms(string keyword, string type, string status)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            var rows = conn.Query(
                "sp_SearchRooms",
                new
                {
                    keyword = string.IsNullOrWhiteSpace(keyword) ? null : keyword,
                    type = string.IsNullOrWhiteSpace(type) ? null : type,
                    status = string.IsNullOrWhiteSpace(status) ? null : status
                },
                commandType: CommandType.StoredProcedure);

            return rows.Select(row => {
                var dict = (IDictionary<string, object>)row;
                return new RoomDisplayModel
                {
                    RoomCode = dict["Mã phòng"]?.ToString() ?? "",
                    RoomName = dict["Tên phòng"]?.ToString() ?? "",
                    RoomType = dict["Loại"]?.ToString() ?? "",
                    Floor = dict["Tầng"]?.ToString() ?? "",
                    Capacity = Convert.ToInt32(dict["Sức chứa"]),
                    Status = dict["Trạng thái"]?.ToString() ?? ""
                };
            }).ToList();
        }

        /// <summary>Returns (RoomID, Floor, Capacity, Note) or null if not found.</summary>
        public (int RoomID, string Floor, string Capacity, string Note)? GetRoomByCode(string roomCode)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            var row = conn.QueryFirstOrDefault(
                "sp_GetRoomByCode",
                new { code = roomCode },
                commandType: CommandType.StoredProcedure);

            if (row == null)
                return null;

            return (
                (int)row.RoomID,
                row.Floor?.ToString() ?? "",
                row.Capacity?.ToString() ?? "0",
                row.Note?.ToString() ?? ""
            );
        }

        public void InsertRoom(string code, string name, string type, string floor, int capacity, string status, string note)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Execute(
                "sp_InsertRoom",
                new
                {
                    code,
                    name,
                    type,
                    floor,
                    cap = capacity,
                    status,
                    note
                },
                commandType: CommandType.StoredProcedure);
        }

        public void UpdateRoom(int roomId, string code, string name, string type, string floor, int capacity, string status, string note)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Execute(
                "sp_UpdateRoom",
                new
                {
                    roomId,
                    code,
                    name,
                    type,
                    floor,
                    cap = capacity,
                    status,
                    note
                },
                commandType: CommandType.StoredProcedure);
        }

        public void DeleteRoom(int roomId)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Execute(
                "sp_DeleteRoom",
                new
                {
                    id = roomId,
                    note = $"Xóa mềm ngày {DateTime.Now:dd/MM/yyyy HH:mm}",
                    retiredStatus = RoomStatus.Retired
                },
                commandType: CommandType.StoredProcedure);
        }
    }
}
