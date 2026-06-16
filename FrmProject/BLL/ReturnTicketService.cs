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
            string requestNote) =>
            _returnTicketRepository.SubmitReturnRequest(ticketId, currentUserId, appRole, requestedAt, returnItems, requestNote);
    }
}
