using System;
using System.Collections.Generic;
using FrmProject.DAL;
using FrmProject.Models;

namespace FrmProject.BLL
{
    public class ReturnTicketService(IReturnTicketRepository returnTicketRepository) : IReturnTicketService
    {
        public List<LookupItem> SearchBorrowingTickets(int currentUserId, AppRole appRole, string keyword = "") =>
            returnTicketRepository.SearchBorrowingTickets(currentUserId, appRole, keyword);

        public ReturnTicketDetails? GetTicketDetails(int ticketId) =>
            returnTicketRepository.GetTicketDetails(ticketId);

        public void ApplyPendingReturnQuantities(int ticketId, List<ReturnTicketItemModel> items) =>
            returnTicketRepository.ApplyPendingReturnQuantities(ticketId, items);

        public bool TryLoadPendingReturnRequest(int ticketId, out DateTime requestedAt, out List<ReturnRequestItem> pendingItems) =>
            returnTicketRepository.TryLoadPendingReturnRequest(ticketId, out requestedAt, out pendingItems);

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
            bool hasValidItem = false;
            foreach (var item in returnItems)
            {
                if (item.ReturnQty < 0)
                    throw new ArgumentException("Số lượng trả không được nhỏ hơn 0.");
                if (item.ReturnQty > item.BorrowQty)
                    throw new ArgumentException("Số lượng trả không được lớn hơn số lượng đang mượn.");

                bool isReturned = item.ReturnQty > 0;
                // Bug fix: isReportedIssue chỉ cần tình trạng xấu (bao gồm cả mất hoàn toàn ReturnQty=0)
                bool isGoodCondition = string.IsNullOrWhiteSpace(item.Note) ||
                                       item.Note.StartsWith("Tốt", StringComparison.OrdinalIgnoreCase);
                bool isReportedIssue = !isGoodCondition;

                if (isReturned || isReportedIssue)
                {
                    hasValidItem = true;
                }
            }

            if (!hasValidItem)
                throw new ArgumentException("Cần nhập ít nhất một thiết bị được trả hoặc báo lỗi/mất.");

            returnTicketRepository.SubmitReturnRequest(ticketId, currentUserId, appRole, requestedAt, returnItems, requestNote);
        }
    }
}
