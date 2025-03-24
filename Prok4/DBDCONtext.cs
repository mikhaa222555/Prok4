using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;  // Добавить
using System.IO;  // Добавить 
using System.Windows;
using TaskManagerApp.Data;
using static Prok4.Prok4.MainWindow;
using Prok4;
using Microsoft.EntityFrameworkCore;
//using Pomelo.EntityFrameworkCore.MySql;  // Альтернативный провайдер (по желанию)

namespace TaskManagerApp
{
    public class Program
        {

                // 1. Сначала настраиваем конфигурацию
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())  // Важно для нахождения appsettings.json
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

                // 2. Создаем ServiceCollection (аналог IHostBuilder для WPF)
                var services = new ServiceCollection();

            // 3. Регистрируем DbContext
            object value = services.AddDbContext<TaskManagerDbContext>(options =>
                {
                    // Используем строку подключения из конфигурации
                    string connectionString = configuration.GetConnectionString("TaskManagerDB");

                    options.UseMySql(configuration.GetConnectionString("TaskManagerDB"));
                    //  options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)); // Альтернативный способ с Pomelo

                });

                // 4. Регистрируем остальные сервисы (ViewModels, Views)
                services.AddSingleton<MainViewModel>();
                services.AddSingleton<MainWindow>();

                // 5. Создаем ServiceProvider
                var serviceProvider = services.BuildServiceProvider();

                // 6.
                var mainWindow = serviceProvider.GetRequiredService<MainWindow>();
                mainWindow.DataContext = serviceProvider.GetRequiredService<MainViewModel>();

                // 7. Запускаем  WPF
                Application app = new Application();
                app.Run(mainWindow);
            }

        }
    }

    