using System.Data;
using FrmProject.DAL;

namespace FrmProject.BLL
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardService(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        public DashboardSnapshot Load(int pageNumber, int pageSize) =>
            _dashboardRepository.Load(pageNumber, pageSize);

        public DataTable LoadBorrowingListOnly(int pageNumber, int pageSize) =>
            _dashboardRepository.LoadBorrowingListOnly(pageNumber, pageSize);
    }
}
