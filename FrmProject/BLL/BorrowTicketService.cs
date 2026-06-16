using FrmProject.DAL;

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
        public int CreatePendingTicket(BorrowTicketDraft draft) => _borrowTicketRepository.CreatePendingTicket(draft);
    }
}
