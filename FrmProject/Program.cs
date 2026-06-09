namespace FrmProject
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            // Task 9: Initialize logger
            AppLogger.Initialize();
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            // Run database migrations on startup
            try
            {
                using var conn = DAL.DbHelper.GetConnection();
                conn.Open();
                DAL.DbSchemaHelper.EnsureReturnedQuantitySchemaAndRestore(conn);
                DAL.DbSchemaHelper.EnsureBorrowDetailInstanceIntegrity(conn);
            }
            catch (Exception ex)
            {
                AppLogger.Error("Failed to run database migrations on startup", ex);
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
