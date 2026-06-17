using System;
using System.Collections.Generic;
using FrmProject.DAL;
using FrmProject.Models;

namespace FrmProject.BLL
{
    public class DeviceInstanceService(IDeviceInstanceRepository deviceInstanceRepository) : IDeviceInstanceService
    {
        public string GetDeviceCode(int deviceId) => deviceInstanceRepository.GetDeviceCode(deviceId);
        public string GetNextAssetCode(int deviceId) => deviceInstanceRepository.GetNextAssetCode(deviceId);
        public List<DeviceInstanceDisplayModel> GetByDevice(int deviceId) => deviceInstanceRepository.GetByDevice(deviceId);
        public bool AssetCodeExists(string assetCode) => deviceInstanceRepository.AssetCodeExists(assetCode);

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

            if (deviceInstanceRepository.AssetCodeExists(assetCode))
                throw new InvalidOperationException($"Mã tài sản '{assetCode}' đã tồn tại trong hệ thống.");

            deviceInstanceRepository.Insert(deviceId, assetCode, status, condition);
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

            deviceInstanceRepository.UpdateStatusAndCondition(deviceId, instanceId, status, condition);
        }

        public void Delete(int deviceId, int instanceId)
        {
            if (deviceId <= 0)
                throw new ArgumentException("Mã thiết bị không hợp lệ.");
            if (instanceId <= 0)
                throw new ArgumentException("Mã cá thể thiết bị không hợp lệ.");

            deviceInstanceRepository.Delete(deviceId, instanceId);
        }
    }
}
