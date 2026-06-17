using System;
using System.Collections.Generic;
using FrmProject.DAL;
using FrmProject.Models;

namespace FrmProject.BLL
{
    public class RoomService(IRoomRepository roomRepository, IDeviceRepository deviceRepository) : IRoomService
    {
        public List<RoomDisplayModel> GetAllRooms() => roomRepository.GetAllRooms();

        public (int RoomID, string Floor, string Capacity, string Note)? GetRoomByCode(string roomCode) =>
            roomRepository.GetRoomByCode(roomCode);

        public List<RoomDeviceModel> GetDevicesByRoom(string roomCode) => deviceRepository.GetDevicesByRoom(roomCode);

        public void DeleteRoom(int roomId) => roomRepository.DeleteRoom(roomId);

        public void InsertRoom(string code, string name, string type, string floor, int capacity, string status, string note)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Mã phòng không được để trống.");
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Tên phòng không được để trống.");
            if (capacity <= 0)
                throw new ArgumentException("Sức chứa phải lớn hơn 0.");

            // Check if room code already exists on insert
            var existing = roomRepository.GetRoomByCode(code);
            if (existing != null)
                throw new InvalidOperationException($"Mã phòng '{code}' đã tồn tại trong hệ thống.");

            roomRepository.InsertRoom(code, name, type, floor, capacity, status, note);
        }

        public void UpdateRoom(int id, string code, string name, string type, string floor, int capacity, string status, string note)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Mã phòng không được để trống.");
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Tên phòng không được để trống.");
            if (capacity <= 0)
                throw new ArgumentException("Sức chứa phải lớn hơn 0.");

            roomRepository.UpdateRoom(id, code, name, type, floor, capacity, status, note);
        }

        public List<RoomDisplayModel> SearchRooms(string keyword, string type, string status) =>
            roomRepository.SearchRooms(keyword, type, status);
    }
}
