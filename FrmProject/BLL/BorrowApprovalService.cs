using FrmProject.DAL;

namespace FrmProject.BLL
{
    /// <summary>
    /// Shared service for approving/rejecting PENDING borrow tickets.
    /// </summary>
    public class BorrowApprovalService(IBorrowTicketRepository borrowTicketRepository) : IBorrowApprovalService
    {
        public void ApproveBorrow(int ticketId) => borrowTicketRepository.ApproveBorrow(ticketId);

        public void RejectBorrow(int ticketId, string reason) => borrowTicketRepository.RejectBorrow(ticketId, reason);
    }
}
