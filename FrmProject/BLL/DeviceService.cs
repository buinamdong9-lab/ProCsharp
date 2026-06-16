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
            string note)
        {
            if (string.IsNullOrWhiteSpace(deviceName))
                throw new ArgumentException("Tên thiết bị không được để trống.");
            if (string.IsNullOrWhiteSpace(deviceCode))
                throw new ArgumentException("Mã thiết bị không được để trống.");
            if (totalQuantity < 0)
                throw new ArgumentException("Tổng số lượng không được nhỏ hơn 0.");
            if (borrowedQuantity < 0)
                throw new ArgumentException("Số lượng đang mượn không được nhỏ hơn 0.");
            if (availableQuantity < 0)
                throw new ArgumentException("Số lượng có sẵn không được nhỏ hơn 0.");
            if (availableQuantity + borrowedQuantity > totalQuantity)
                throw new ArgumentException("Tổng số lượng có sẵn và đang mượn không được vượt quá tổng số lượng thực tế.");

            _deviceRepository.SaveDevice(deviceId, deviceCode, deviceName, categoryName, roomNameOrCode,
                totalQuantity, borrowedQuantity, availableQuantity, status, note);
        }

        public void DeleteDevice(int deviceId) => _deviceRepository.DeleteDevice(deviceId);

        public DataTable GetAllDevices() => _deviceRepository.GetAllDevices();
        public List<DeviceDisplayModel> GetAvailableDevices() => _deviceRepository.GetAvailableDevices();
    }
}
