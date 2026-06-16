using FrmProject.DAL;

namespace FrmProject.BLL
{
    /// <summary>
    /// Shared service for approving/rejecting PENDING borrow tickets.
    /// </summary>
    public class BorrowApprovalService : IBorrowApprovalService
    {
        private readonly IBorrowTicketRepository _borrowTicketRepository;

        public BorrowApprovalService(IBorrowTicketRepository borrowTicketRepository)
        {
            _borrowTicketRepository = borrowTicketRepository;
        }

        public void ApproveBorrow(int ticketId) => _borrowTicketRepository.ApproveBorrow(ticketId);

        public void RejectBorrow(int ticketId, string reason) => _borrowTicketRepository.RejectBorrow(ticketId, reason);
    }
}
