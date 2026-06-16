using System.Data;
using FrmProject.DAL;

namespace FrmProject.BLL
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IDeviceRepository _deviceRepository;

        public RoomService(IRoomRepository roomRepository, IDeviceRepository deviceRepository)
        {
            _roomRepository = roomRepository;
            _deviceRepository = deviceRepository;
        }

        public DataTable GetAllRooms() => _roomRepository.GetAllRooms();

        public (int RoomID, string Floor, string Capacity, string Note)? GetRoomByCode(string roomCode) =>
            _roomRepository.GetRoomByCode(roomCode);

        public DataTable GetDevicesByRoom(string roomCode) => _deviceRepository.GetDevicesByRoom(roomCode);

        public void DeleteRoom(int roomId) => _roomRepository.DeleteRoom(roomId);

        public void InsertRoom(string code, string name, string type, string floor, int capacity, string status, string note)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Mã phòng không được để trống.");
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Tên phòng không được để trống.");
            if (capacity <= 0)
                throw new ArgumentException("Sức chứa phải lớn hơn 0.");

            // Check if room code already exists on insert
            var existing = _roomRepository.GetRoomByCode(code);
            if (existing != null)
                throw new InvalidOperationException($"Mã phòng '{code}' đã tồn tại trong hệ thống.");

            _roomRepository.InsertRoom(code, name, type, floor, capacity, status, note);
        }

        public void UpdateRoom(int id, string code, string name, string type, string floor, int capacity, string status, string note)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Mã phòng không được để trống.");
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Tên phòng không được để trống.");
            if (capacity <= 0)
                throw new ArgumentException("Sức chứa phải lớn hơn 0.");

            _roomRepository.UpdateRoom(id, code, name, type, floor, capacity, status, note);
        }

        public DataTable SearchRooms(string keyword, string type, string status) =>
            _roomRepository.SearchRooms(keyword, type, status);
    }
}
