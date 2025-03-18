using Prok4;
using System;
using static Prok4.Prok4.MainWindow;
using Microsoft.Extensions.Hosting;
using TaskManagerApp.Data;
using Microsoft.Extensions.DependencyInjection;  // Убедитесь, что эта строка есть

namespace TaskManagerApp
{
    public class Program
    {
        public static object? Host { get; private set; }

        [STAThread]
        public static void Main()
        {
            var builder = Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    string connectionString = hostContext.Configuration.GetConnectionString("TaskManagerDB");
                    services.AddDbContext<TaskManagerDbContext>(options =>
                    {
                        options.UseNpgsql(connectionString); // PostgreSQL
                    });

                 
                    services.AddSingleton<MainViewModel>();
                    services.AddSingleton<MainWindow>(); 

                });

            var host = builder.Build();

            var mainWindow = host.Services.GetRequiredService<MainWindow>();
            mainWindow.DataContext = host.Services.GetRequiredService<MainViewModel>(); 
            mainWindow.Show();

            System.Windows.Application app = new System.Windows.Application();
            app.Run(mainWindow);
        }
    }
}