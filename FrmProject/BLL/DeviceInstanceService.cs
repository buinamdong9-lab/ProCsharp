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
        public void Insert(int deviceId, string assetCode, string status, string condition) =>
            _deviceInstanceRepository.Insert(deviceId, assetCode, status, condition);
        public void UpdateStatusAndCondition(int deviceId, int instanceId, string status, string condition) =>
            _deviceInstanceRepository.UpdateStatusAndCondition(deviceId, instanceId, status, condition);
        public void Delete(int deviceId, int instanceId) => _deviceInstanceRepository.Delete(deviceId, instanceId);
    }
}
