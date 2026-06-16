using System.Data;
using FrmProject.DAL;

namespace FrmProject.BLL
{
    public class DeviceService : IDeviceService
    {
        private readonly IDeviceRepository _deviceRepository;

        public DeviceService(IDeviceRepository deviceRepository)
        {
            _deviceRepository = deviceRepository;
        }

        public int GetTotalDevicesCount(string keyword, string category, string status) =>
            _deviceRepository.GetTotalDevicesCount(keyword, category, status);

        public DataTable GetDevicesPaged(int pageNumber, int pageSize, string keyword, string category, string status) =>
            _deviceRepository.GetDevicesPaged(pageNumber, pageSize, keyword, category, status);

        public DataTable GetCategories() => _deviceRepository.GetCategories();
        public DataTable GetDevicesByRoom(string roomCode) => _deviceRepository.GetDevicesByRoom(roomCode);
        public string GenerateDeviceCode() => _deviceRepository.GenerateDeviceCode();

        public void SaveDevice(
            int deviceId,
            string deviceCode,
            string deviceName,
            string categoryName,
            string roomNameOrCode,
            int totalQuantity,
            int borrowedQuantity,
            int availableQuantity,
            string status,
            string note) =>
            _deviceRepository.SaveDevice(deviceId, deviceCode, deviceName, categoryName, roomNameOrCode,
                totalQuantity, borrowedQuantity, availableQuantity, status, note);

        public void DeleteDevice(int deviceId) => _deviceRepository.DeleteDevice(deviceId);

        public DataTable GetAllDevices() => _deviceRepository.GetAllDevices();
        public List<DeviceDisplayModel> GetAvailableDevices() => _deviceRepository.GetAvailableDevices();
    }
}
