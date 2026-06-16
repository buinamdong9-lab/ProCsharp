using System;
using System.Data;
using FrmProject.DAL;

namespace FrmProject.BLL
{
    public class DeviceInstanceService : IDeviceInstanceService
    {
        private readonly IDeviceInstanceRepository _deviceInstanceRepository;

        public DeviceInstanceService(IDeviceInstanceRepository deviceInstanceRepository)
        {
            _deviceInstanceRepository = deviceInstanceRepository;
        }

        public string GetDeviceCode(int deviceId) => _deviceInstanceRepository.GetDeviceCode(deviceId);
        public string GetNextAssetCode(int deviceId) => _deviceInstanceRepository.GetNextAssetCode(deviceId);
        public DataTable GetByDevice(int deviceId) => _deviceInstanceRepository.GetByDevice(deviceId);
        public bool AssetCodeExists(string assetCode) => _deviceInstanceRepository.AssetCodeExists(assetCode);

        public void Insert(int deviceId, string assetCode, string status, string condition)
        {
            if (deviceId <= 0)
                throw new ArgumentException("Mã thiết bị không hợp lệ.");
            if (string.IsNullOrWhiteSpace(assetCode))
                throw new ArgumentException("Mã tài sản không được để trống.");
            if (string.IsNullOrWhiteSpace(status))
                throw new ArgumentException("Trạng thái thiết bị không được để trống.");
            if (string.IsNullOrWhiteSpace(condition))
                throw new ArgumentException("Tình trạng thiết bị không được để trống.");

            if (_deviceInstanceRepository.AssetCodeExists(assetCode))
                throw new InvalidOperationException($"Mã tài sản '{assetCode}' đã tồn tại trong hệ thống.");

            _deviceInstanceRepository.Insert(deviceId, assetCode, status, condition);
        }

        public void UpdateStatusAndCondition(int deviceId, int instanceId, string status, string condition)
        {
            if (deviceId <= 0)
                throw new ArgumentException("Mã thiết bị không hợp lệ.");
            if (instanceId <= 0)
                throw new ArgumentException("Mã cá thể thiết bị không hợp lệ.");
            if (string.IsNullOrWhiteSpace(status))
                throw new ArgumentException("Trạng thái thiết bị không được để trống.");
            if (string.IsNullOrWhiteSpace(condition))
                throw new ArgumentException("Tình trạng thiết bị không được để trống.");

            _deviceInstanceRepository.UpdateStatusAndCondition(deviceId, instanceId, status, condition);
        }

        public void Delete(int deviceId, int instanceId)
        {
            if (deviceId <= 0)
                throw new ArgumentException("Mã thiết bị không hợp lệ.");
            if (instanceId <= 0)
                throw new ArgumentException("Mã cá thể thiết bị không hợp lệ.");

            _deviceInstanceRepository.Delete(deviceId, instanceId);
        }
    }
}
