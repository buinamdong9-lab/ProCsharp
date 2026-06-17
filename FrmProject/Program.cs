using Microsoft.Extensions.DependencyInjection;

namespace FrmProject
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            // Initialize DI Container
            var services = new Microsoft.Extensions.DependencyInjection.ServiceCollection();

            // Register Repositories
            services.AddSingleton<DAL.IAuthRepository, DAL.AuthRepository>();
            services.AddSingleton<DAL.IUserRepository, DAL.UserRepository>();
            services.AddSingleton<DAL.IDeviceRepository, DAL.DeviceRepository>();
            services.AddSingleton<DAL.IDeviceInstanceRepository, DAL.DeviceInstanceRepository>();
            services.AddSingleton<DAL.IRoomRepository, DAL.RoomRepository>();
            services.AddSingleton<DAL.IBorrowTicketRepository, DAL.BorrowTicketRepository>();
            services.AddSingleton<DAL.IReturnTicketRepository, DAL.ReturnTicketRepository>();
            services.AddSingleton<DAL.ISettingsRepository, DAL.SettingsRepository>();
            services.AddSingleton<DAL.IDashboardRepository, DAL.DashboardRepository>();
            services.AddSingleton<DAL.ITicketListRepository, DAL.TicketListRepository>();
            services.AddSingleton<DAL.IRecycleBinRepository, DAL.RecycleBinRepository>();
            services.AddSingleton<DAL.IReturnRequestRepository, DAL.ReturnRequestRepository>();
            services.AddSingleton<DAL.IReportRepository, DAL.ReportRepository>();

            // Register Services
            services.AddSingleton<BLL.IAuthService, BLL.AuthService>();
            services.AddSingleton<BLL.IUserService, BLL.UserService>();
            services.AddSingleton<BLL.IDeviceService, BLL.DeviceService>();
            services.AddSingleton<BLL.IDeviceInstanceService, BLL.DeviceInstanceService>();
            services.AddSingleton<BLL.IRoomService, BLL.RoomService>();
            services.AddSingleton<BLL.ISettingsService, BLL.SettingsService>();
            services.AddSingleton<BLL.IDashboardService, BLL.DashboardService>();
            services.AddSingleton<BLL.ITicketListService, BLL.TicketListService>();
            services.AddSingleton<BLL.IRecycleBinService, BLL.RecycleBinService>();
            services.AddSingleton<BLL.IBorrowTicketService, BLL.BorrowTicketService>();
            services.AddSingleton<BLL.IReturnTicketService, BLL.ReturnTicketService>();
            services.AddSingleton<BLL.IBorrowApprovalService, BLL.BorrowApprovalService>();
            services.AddSingleton<BLL.IReturnApprovalService, BLL.ReturnApprovalService>();
            services.AddSingleton<BLL.IReportService, BLL.ReportService>();
            services.AddSingleton<BLL.IAuthorizationService, BLL.AuthorizationService>();

            AppServiceProvider.Provider = services.BuildServiceProvider();

            // Task 9: Initialize logger
            AppLogger.Initialize();
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            // Database migrations with DbUp
            try
            {
                var connectionString = DAL.DbHelper.GetConnectionString();

                // Ensure the database exists
                DbUp.EnsureDatabase.For.SqlDatabase(connectionString);

                var upgrader = DbUp.DeployChanges.To
                    .SqlDatabase(connectionString)
                    .WithScriptsEmbeddedInAssembly(
                        System.Reflection.Assembly.GetExecutingAssembly(),
                        name => name.Contains(".Data.Migrations."))
                    .LogToConsole()
                    .Build();

                var result = upgrader.PerformUpgrade();
                if (!result.Successful)
                {
                    AppLogger.Error("Database migration failed on startup", result.Error);
                    MessageBox.Show(
                        "Lỗi nâng cấp cơ sở dữ liệu:\n" + result.Error.Message + "\nChi tiết đã được ghi trong file log.",
                        "Lỗi khởi động",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    AppLogger.CloseAndFlush();
                    return;
                }

                var spUpgrader = DbUp.DeployChanges.To
                    .SqlDatabase(connectionString)
                    .WithScriptsEmbeddedInAssembly(
                        System.Reflection.Assembly.GetExecutingAssembly(),
                        name => name.Contains(".StoredProcedures."))
                    .JournalTo(new DbUp.Helpers.NullJournal())
                    .LogToConsole()
                    .Build();

                var spResult = spUpgrader.PerformUpgrade();
                if (!spResult.Successful)
                {
                    AppLogger.Error("Database stored procedures deployment failed on startup", spResult.Error);
                    MessageBox.Show(
                        "Lỗi triển khai Stored Procedures:\n" + spResult.Error.Message + "\nChi tiết đã được ghi trong file log.",
                        "Lỗi khởi động",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    AppLogger.CloseAndFlush();
                    return;
                }
            }
            catch (Exception ex)
            {
                AppLogger.Error("Database migration initialization failed on startup", ex);
                MessageBox.Show(
                    "Không thể kết nối đến cơ sở dữ liệu để chạy migration.\nChi tiết đã được ghi trong file log.",
                    "Lỗi khởi động",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                AppLogger.CloseAndFlush();
                return;
            }

            try
            {
                // Dùng ApplicationContext để app không thoát khi FrmLogin đóng
                // App chỉ thoát khi không còn form nào mở
                var loginForm = new FrmLogin();
                loginForm.Show();
                Application.Run();
            }
            catch (Exception ex)
            {
                AppLogger.Error("Fatal application error", ex);
                MessageBox.Show("Đã xảy ra lỗi nghiêm trọng. Ứng dụng sẽ đóng lại.\nChi tiết xem trong file log.", "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Ensure logs are flushed
                AppLogger.CloseAndFlush();
            }
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            ShowHandledError("Lỗi thao tác", e.Exception);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception exception = e.ExceptionObject as Exception
                ?? new Exception(e.ExceptionObject?.ToString() ?? "Unknown error");

            ShowHandledError("Lỗi hệ thống", exception);
        }

        private static void ShowHandledError(string title, Exception ex)
        {
            AppLogger.Error(title, ex);
            MessageBox.Show(
                "Đã xảy ra lỗi trong quá trình xử lý.\n" +
                "Ứng dụng đã ghi log để kiểm tra, bạn có thể tiếp tục thao tác hoặc mở Log Hệ Thống trong phần Cấu Hình.\n\n" +
                ex.Message,
                title,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}
