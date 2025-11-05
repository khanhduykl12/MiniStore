namespace ProductManagement
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                if (Environment.OSVersion.Version.Major >= 6) SetProcessDPIAware();
                Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);

                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FormLogin());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "L?i");
            }
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();

    }
}