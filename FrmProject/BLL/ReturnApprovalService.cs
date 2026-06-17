using System.Data;
using FrmProject.DAL;

namespace FrmProject.BLL
{
    /// <summary>
    /// Shared service for approving/rejecting RETURN_PENDING tickets.
    /// Used by both UcDanhsachphieu and UcTrathietbi.
    /// </summary>
    public class ReturnApprovalService(IReturnTicketRepository returnTicketRepository) : IReturnApprovalService
    {
        /// <summary>
        /// Approves a RETURN_PENDING ticket: restores inventory and marks RETURNED.
        /// </summary>
        public void ApproveReturn(int ticketId, int approvedByUserId)
        {
            returnTicketRepository.ApproveReturn(ticketId, approvedByUserId);
        }

        /// <summary>
        /// Rejects a RETURN_PENDING ticket: reverts to BORROWING and clears ReturnNote.
        /// </summary>
        public void RejectReturn(int ticketId, string reason)
        {
            returnTicketRepository.RejectReturn(ticketId, reason);
        }

        /// <summary>
        /// Loads pending return tickets for admin review.
        /// </summary>
        public List<PendingReturnTicketModel> GetPendingReturnTickets()
        {
            return returnTicketRepository.GetPendingReturnTickets();
        }
    }
}
