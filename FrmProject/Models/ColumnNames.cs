namespace FrmProject.Models
{
    /// <summary>
    /// Tập trung tất cả tên cột dùng trong DataTable / DataGridView.
    /// Thay vì hardcode "Mã ND", "Họ tên"... khắp nơi, dùng Col.MaND, Col.HoTen...
    /// Khi cần đổi tên cột chỉ sửa 1 nơi duy nhất.
    /// </summary>
    internal static class Col
    {
        // ═══════ Users ═══════
        public const string MaND = "Mã ND";
        public const string HoTen = "Họ tên";
        public const string MaSo = "Mã số";
        public const string Email = "Email";
        public const string KhoaBoMon = "Khoa/Bộ Môn";
        public const string VaiTro = "Vai trò";
        public const string SoDienThoai = "Số điện thoại";
        public const string DangMuon = "Đang mượn";
        public const string TrangThai = "Trạng thái";

        // ═══════ Devices ═══════
        public const string DeviceID = "DeviceID";
        public const string DeviceCode = "DeviceCode";
        public const string TenThietBi = "Tên thiết bị";
        public const string MaThietBi = "Mã thiết bị";
        public const string LoaiThietBi = "Loại thiết bị";
        public const string ViTri = "Vị trí";
        public const string TinhTrang = "Tình trạng";
        public const string SoLuong = "Số lượng";
        public const string AvailableQuantity = "AvailableQuantity";
        public const string GhiChu = "Ghi chú";

        // ═══════ Rooms ═══════
        public const string MaPhong = "Mã phòng";
        public const string TenPhong = "Tên phòng";
        public const string Loai = "Loại";
        public const string Tang = "Tầng";
        public const string SucChua = "Sức chứa";

        // ═══════ Devices in Room ═══════
        public const string MaTB = "Mã TB";
        public const string SL = "SL";

        // ═══════ Borrow Tickets ═══════
        public const string TicketID = "TicketID";
        public const string InstanceID = "InstanceID";
        public const string MaTaiSan = "Mã tài sản";
        public const string SLMuon = "SL mượn";
        public const string SLTra = "SL trả";
        public const string HanTra = "Hạn Trả";

        // ═══════ Dashboard ═══════
        public const string NguoiMuon = "Người mượn";
        public const string NgayMuon = "Ngày Mượn";
    }
}
