using Microsoft.Data.SqlClient;
using System.Data;

namespace FrmProject.DAL
{
    /// <summary>
    /// Renamed from RoomDao → RoomRepository for naming consistency.
    /// </summary>
    public static class RoomRepository
    {
        public static DataTable GetAllRooms()
        {
            DataTable dt = new DataTable();
            string query = @"
                SELECT 
                    RoomCode AS [Mã phòng],
                    RoomName AS [Tên phòng],
                    RoomType AS [Loại],
                    Floor AS [Tầng],
                    Capacity AS [Sức chứa],
                    Status AS [Trạng thái]
                FROM Rooms";

            using SqlConnection conn = DbHelper.GetConnection();
            using SqlDataAdapter da = new SqlDataAdapter(query, conn);
            da.Fill(dt);
            return dt;
        }

        public static DataTable SearchRooms(string keyword, string type, string status)
        {
            string query = @"
                SELECT RoomCode AS [Mã phòng], RoomName AS [Tên phòng],
                       RoomType AS [Loại], Floor AS [Tầng],
                       Capacity AS [Sức chứa], Status AS [Trạng thái]
                FROM Rooms WHERE 1=1";

            if (!string.IsNullOrEmpty(keyword))
                query += " AND (RoomCode LIKE @kw OR RoomName LIKE @kw)";
            if (!string.IsNullOrEmpty(type) && !type.Contains("Tất cả"))
                query += " AND RoomType = @type";
            if (!string.IsNullOrEmpty(status) && !status.Contains("Tất cả"))
                query += " AND Status = @status";

            using SqlConnection conn = DbHelper.GetConnection();
            using SqlDataAdapter da = new SqlDataAdapter(query, conn);
            if (!string.IsNullOrEmpty(keyword))
                da.SelectCommand.Parameters.AddWithValue("@kw", $"%{keyword}%");
            if (!string.IsNullOrEmpty(type) && !type.Contains("Tất cả"))
                da.SelectCommand.Parameters.AddWithValue("@type", type);
            if (!string.IsNullOrEmpty(status) && !status.Contains("Tất cả"))
                da.SelectCommand.Parameters.AddWithValue("@status", status);

            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        /// <summary>Returns (RoomID, Floor, Capacity, Note) or null if not found.</summary>
        public static (int RoomID, string Floor, string Capacity, string Note)? GetRoomByCode(string roomCode)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();
            using var cmd = new SqlCommand("SELECT RoomID, Floor, Capacity, Note FROM Rooms WHERE RoomCode = @code", conn);
            cmd.Parameters.AddWithValue("@code", roomCode);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return (
                    reader.GetInt32(0),
                    reader.IsDBNull(1) ? "" : reader.GetValue(1).ToString()!,
                    reader.IsDBNull(2) ? "0" : reader.GetValue(2).ToString()!,
                    reader.IsDBNull(3) ? "" : reader.GetString(3)
                );
            }
            return null;
        }

        public static void InsertRoom(string code, string name, string type, string floor, int capacity, string status, string note)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();
            string sql = @"INSERT INTO Rooms 
                (RoomCode, RoomName, RoomType, Floor, Capacity, Status, Note)
                VALUES (@code, @name, @type, @floor, @cap, @status, @note)";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@code", code);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@type", type);
            cmd.Parameters.AddWithValue("@floor", floor);
            cmd.Parameters.AddWithValue("@cap", capacity);
            cmd.Parameters.AddWithValue("@status", status);
            cmd.Parameters.AddWithValue("@note", note);
            cmd.ExecuteNonQuery();
        }

        public static void UpdateRoom(int roomId, string code, string name, string type, string floor, int capacity, string status, string note)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();
            string sql = @"UPDATE Rooms SET
                RoomCode = @code, RoomName = @name, RoomType = @type,
                Floor = @floor, Capacity = @cap, Status = @status, Note = @note
                WHERE RoomID = @id";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@code", code);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@type", type);
            cmd.Parameters.AddWithValue("@floor", floor);
            cmd.Parameters.AddWithValue("@cap", capacity);
            cmd.Parameters.AddWithValue("@status", status);
            cmd.Parameters.AddWithValue("@note", note);
            cmd.Parameters.AddWithValue("@id", roomId);
            cmd.ExecuteNonQuery();
        }

        public static void DeleteRoom(int roomId)
        {
            using SqlConnection conn = DbHelper.GetConnection();
            conn.Open();
            using var cmd = new SqlCommand(@"
                UPDATE Rooms
                SET Status = @retiredStatus,
                    Note = LTRIM(RTRIM(CONCAT(ISNULL(NULLIF(Note, ''), ''), CASE WHEN ISNULL(NULLIF(Note, ''), '') = '' THEN '' ELSE ' | ' END, @note)))
                WHERE RoomID = @id", conn);
            cmd.Parameters.AddWithValue("@retiredStatus", RoomStatus.Retired);
            cmd.Parameters.AddWithValue("@note", $"Xóa mềm ngày {DateTime.Now:dd/MM/yyyy HH:mm}");
            cmd.Parameters.AddWithValue("@id", roomId);
            cmd.ExecuteNonQuery();
        }
    }
}

