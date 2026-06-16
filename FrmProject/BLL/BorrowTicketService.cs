using System;
using System.Collections.Generic;
using System.Linq;
using FrmProject.DAL;
using FrmProject.Models;

namespace FrmProject.BLL
{
    public class BorrowTicketService : IBorrowTicketService
    {
        private readonly IBorrowTicketRepository _borrowTicketRepository;

        public BorrowTicketService(IBorrowTicketRepository borrowTicketRepository)
        {
            _borrowTicketRepository = borrowTicketRepository;
        }

        public List<LookupItem> GetEnabledUsers() => _borrowTicketRepository.GetEnabledUsers();
        public List<LookupItem> GetRooms() => _borrowTicketRepository.GetRooms();
        public List<LookupItem> GetBorrowableDevices() => _borrowTicketRepository.GetBorrowableDevices();
        public List<LookupItem> GetAvailableInstances(int deviceId) => _borrowTicketRepository.GetAvailableInstances(deviceId);

        public int CreatePendingTicket(BorrowTicketDraft draft)
        {
            if (draft == null)
                throw new ArgumentNullException(nameof(draft));

            if (draft.BorrowerId <= 0)
                throw new ArgumentException("Yêu cầu bắt buộc phải chọn người mượn.");

            if (draft.Items == null || draft.Items.Count == 0)
                throw new ArgumentException("Danh sách thiết bị đăng ký mượn không được trống.");

            if (draft.Items.Any(item => item.InstanceId <= 0 || item.Quantity != 1))
                throw new ArgumentException("Mỗi dòng phiếu mượn phải gắn với một cá thể thiết bị hợp lệ và có số lượng bằng 1.");

            if (draft.Items.Select(item => item.InstanceId).Distinct().Count() != draft.Items.Count)
                throw new ArgumentException("Một cá thể thiết bị không được xuất hiện nhiều lần trong cùng phiếu.");

            if (draft.ExpectedReturnDate < draft.BorrowDate)
                throw new ArgumentException("Ngày dự kiến trả phải lớn hơn hoặc bằng ngày mượn.");

            return _borrowTicketRepository.CreatePendingTicket(draft);
        }
    }
}
