using System;
using System.Collections.Generic;
using System.Data;
using FrmProject.DAL;

namespace FrmProject.BLL
{
    public class ReturnTicketService : IReturnTicketService
    {
        private readonly IReturnTicketRepository _returnTicketRepository;

        public ReturnTicketService(IReturnTicketRepository returnTicketRepository)
        {
            _returnTicketRepository = returnTicketRepository;
        }

        public List<LookupItem> SearchBorrowingTickets(int currentUserId, AppRole appRole, string keyword = "") =>
            _returnTicketRepository.SearchBorrowingTickets(currentUserId, appRole, keyword);

        public ReturnTicketDetails? GetTicketDetails(int ticketId) =>
            _returnTicketRepository.GetTicketDetails(ticketId);

        public void ApplyPendingReturnQuantities(int ticketId, DataTable dt) =>
            _returnTicketRepository.ApplyPendingReturnQuantities(ticketId, dt);

        public bool TryLoadPendingReturnRequest(int ticketId, out DateTime requestedAt, out List<ReturnRequestItem> pendingItems) =>
            _returnTicketRepository.TryLoadPendingReturnRequest(ticketId, out requestedAt, out pendingItems);

        public void SubmitReturnRequest(
            int ticketId,
            int currentUserId,
            AppRole appRole,
            DateTime requestedAt,
            List<(int DeviceID, int InstanceID, int BorrowQty, int ReturnQty, string Note)> returnItems,
            string requestNote)
        {
            if (ticketId <= 0)
                throw new ArgumentException("Mã phiếu mượn không hợp lệ.");
            if (currentUserId <= 0)
                throw new ArgumentException("Mã người dùng không hợp lệ.");
            if (returnItems == null || returnItems.Count == 0)
                throw new ArgumentException("Danh sách thiết bị trả không được trống.");

            foreach (var item in returnItems)
            {
                if (item.ReturnQty <= 0)
                    throw new ArgumentException("Số lượng trả phải lớn hơn 0.");
                if (item.ReturnQty > item.BorrowQty)
                    throw new ArgumentException("Số lượng trả không được lớn hơn số lượng đang mượn.");
            }

            _returnTicketRepository.SubmitReturnRequest(ticketId, currentUserId, appRole, requestedAt, returnItems, requestNote);
        }
    }
}
