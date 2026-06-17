using System.Collections.Generic;
using FrmProject.DAL;
using FrmProject.Models;

namespace FrmProject.BLL
{
    public class DashboardService(IDashboardRepository dashboardRepository) : IDashboardService
    {
        public DashboardSnapshot Load(int pageNumber, int pageSize) =>
            dashboardRepository.Load(pageNumber, pageSize);

        public List<DashboardBorrowingItemModel> LoadBorrowingListOnly(int pageNumber, int pageSize) =>
            dashboardRepository.LoadBorrowingListOnly(pageNumber, pageSize);
    }
}
