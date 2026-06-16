using System.Data;
using FrmProject.DAL;

namespace FrmProject.BLL
{
    public class TicketListService : ITicketListService
    {
        private readonly ITicketListRepository _ticketListRepository;

        public TicketListService(ITicketListRepository ticketListRepository)
        {
            _ticketListRepository = ticketListRepository;
        }

        public DataTable SearchTickets(DateTime from, DateTime to, string keyword, string statusFilter, int currentUserId, AppRole appRole) =>
            _ticketListRepository.SearchTickets(from, to, keyword, statusFilter, currentUserId, appRole);

        public TicketListStats GetStats(int currentUserId, AppRole appRole) =>
            _ticketListRepository.GetStats(currentUserId, appRole);

        public TicketDetailView? GetTicketDetail(int ticketId, int currentUserId, AppRole appRole) =>
            _ticketListRepository.GetTicketDetail(ticketId, currentUserId, appRole);
    }
}
