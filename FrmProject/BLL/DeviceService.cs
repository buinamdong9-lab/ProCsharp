using System;
using System.Collections.Generic;
using FrmProject.DAL;
using FrmProject.Models;

namespace FrmProject.BLL
{
    public class DeviceService(IDeviceRepository deviceRepository) : IDeviceService
    {
        public int GetTotalDevicesCount(string keyword, string category, string status) =>
            deviceRepository.GetTotalDevicesCount(keyword, category, status);

        public List<DeviceDisplayModel> GetDevicesPaged(int pageNumber, int pageSize, string keyword, string category, string status) =>
            deviceRepository.GetDevicesPaged(pageNumber, pageSize, keyword, category, status);

        public List<CategoryModel> GetCategories() => deviceRepository.GetCategories();
        public List<RoomDeviceModel> GetDevicesByRoom(string roomCode) => deviceRepository.GetDevicesByRoom(roomCode);
        public string GenerateDeviceCode() => deviceRepository.GenerateDeviceCode();

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
            if (string.IsNullOrWhiteSpace(deviceName))
                throw new ArgumentException("Tên thiết bị không được để trống.");
            if (string.IsNullOrWhiteSpace(deviceCode))
                throw new ArgumentException("Mã thiết bị không được để trống.");
            if (totalQuantity < 0)
                throw new ArgumentException("Tổng số lượng không được nhỏ hơn 0.");
            if (selectedTotalQuantity < 0)
                throw new ArgumentException("Số lượng tổng ban đầu không hợp lệ.");
            if (selectedAvailableQuantity < 0)
                throw new ArgumentException("Số lượng khả dụng ban đầu không hợp lệ.");

            int currentBorrowed = selectedTotalQuantity - selectedAvailableQuantity;
            if (currentBorrowed < 0)
                throw new ArgumentException("Số lượng đã mượn không hợp lệ.");
            if (totalQuantity < currentBorrowed)
                throw new ArgumentException($"Không thể giảm tổng số lượng xuống {totalQuantity} vì hiện có {currentBorrowed} thiết bị đang được mượn.");

            deviceRepository.SaveDevice(deviceId, deviceCode, deviceName, categoryName, roomNameOrCode,
                totalQuantity, selectedTotalQuantity, selectedAvailableQuantity, status, note);
        }

        public void DeleteDevice(int deviceId) => deviceRepository.DeleteDevice(deviceId);

        public List<DeviceDisplayModel> GetAllDevices() => deviceRepository.GetAllDevices();
        public List<DeviceDisplayModel> GetAvailableDevices() => deviceRepository.GetAvailableDevices();
    }
}
