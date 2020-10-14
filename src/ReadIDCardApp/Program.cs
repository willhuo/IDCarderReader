using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace ReadIDCardApp
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File("logs" + Path.DirectorySeparatorChar + "log.txt", restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Warning)
                .CreateLogger();
            Log.Warning("程序启动完成");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new IDCardReaderForm());
        }
    }
}