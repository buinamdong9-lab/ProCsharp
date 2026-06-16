using System.Data;
using FrmProject.DAL;

namespace FrmProject.BLL
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;

        public ReportService(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public DataTable GetMonthlyStats(DateTime from, DateTime to) =>
            _reportRepository.GetMonthlyStats(from, to);

        public DataTable GetTopDevices(DateTime from, DateTime to) =>
            _reportRepository.GetTopDevices(from, to);

        public DataTable GetOverdueTickets(DateTime from, DateTime to) =>
            _reportRepository.GetOverdueTickets(from, to);
    }
}
