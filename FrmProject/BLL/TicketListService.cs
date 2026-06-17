using System;
using System.Collections.Generic;
using FrmProject.DAL;
using FrmProject.Models;

namespace FrmProject.BLL
{
    public class TicketListService(ITicketListRepository ticketListRepository) : ITicketListService
    {
        public List<TicketHistoryModel> SearchTickets(DateTime from, DateTime to, string keyword, string statusFilter, int currentUserId, AppRole appRole) =>
            ticketListRepository.SearchTickets(from, to, keyword, statusFilter, currentUserId, appRole);

        public TicketListStats GetStats(int currentUserId, AppRole appRole) =>
            ticketListRepository.GetStats(currentUserId, appRole);

        public TicketDetailView? GetTicketDetail(int ticketId, int currentUserId, AppRole appRole) =>
            ticketListRepository.GetTicketDetail(ticketId, currentUserId, appRole);
    }
}
