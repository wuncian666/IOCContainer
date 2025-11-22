using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;
using System.Windows.Forms;

namespace NLogPratice
{
    internal static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var serviceCollection = new IOCContainer.ServiceCollection();

            serviceCollection.AddTransient<Form, Form1>(); // Runner is the custom class
            serviceCollection.AddLogging(loggingBuilder =>
            {
                // configure Logging with NLog
                loggingBuilder.ClearProviders();
                loggingBuilder.SetMinimumLevel(LogLevel.Trace);
                loggingBuilder.AddNLog();
            });
            var provider = serviceCollection.BuildServiceProvider();

            var form = provider.GetService<Form>();
            Application.Run(form);
        }
    }
}