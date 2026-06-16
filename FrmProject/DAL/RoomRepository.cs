using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;
using FrmProject.Models;

namespace FrmProject.DAL
{
    /// <summary>
    /// Renamed from RoomDao → RoomRepository for naming consistency.
    /// </summary>
    public class RoomRepository : IRoomRepository
    {
        public DataTable GetAllRooms()
        {
            using SqlConnection conn = DbHelper.GetConnection();
            DataTable dt = new DataTable();
            dt.Load(conn.ExecuteReader("sp_GetAllRooms", commandType: CommandType.StoredProcedure));
            return dt;
        }

        public DataTable SearchRooms(string keyword, string type, string status)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            DataTable dt = new DataTable();
            dt.Load(conn.ExecuteReader(
                "sp_SearchRooms",
                new
                {
                    keyword = string.IsNullOrWhiteSpace(keyword) ? null : keyword,
                    type = string.IsNullOrWhiteSpace(type) ? null : type,
                    status = string.IsNullOrWhiteSpace(status) ? null : status
                },
                commandType: CommandType.StoredProcedure));
            return dt;
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
