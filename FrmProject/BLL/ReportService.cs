using System;
using System.Collections.Generic;
using FrmProject.DAL;
using FrmProject.Models;

namespace FrmProject.BLL
{
    public class ReportService(IReportRepository reportRepository) : IReportService
    {
        public List<MonthlyStatsModel> GetMonthlyStats(DateTime from, DateTime to) =>
            reportRepository.GetMonthlyStats(from, to);

        public List<TopDeviceModel> GetTopDevices(DateTime from, DateTime to) =>
            reportRepository.GetTopDevices(from, to);

        public List<OverdueTicketModel> GetOverdueTickets(DateTime from, DateTime to) =>
            reportRepository.GetOverdueTickets(from, to);
    }
}
