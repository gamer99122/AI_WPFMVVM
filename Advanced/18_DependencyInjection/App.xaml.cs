using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DependencyInjectionExample.Services;
using DependencyInjectionExample.ViewModels;
using DependencyInjectionExample.Views;
using DependencyInjectionExample.Data;
using DependencyInjectionExample.Data.Repositories;

namespace DependencyInjectionExample
{
    /// <summary>
    /// App.xaml 的互動邏輯
    /// 配置依賴注入容器
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _host;

        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    // 註冊服務
                    ConfigureServices(services);
                })
                .Build();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // 資料庫上下文（Singleton）
            services.AddSingleton<AppDbContext>(provider =>
            {
                var dbContext = new AppDbContext("Data Source=app.db");
                dbContext.Initialize();
                return dbContext;
            });

            // 資料存取層（Scoped）
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();

            // 服務層（Scoped）
            services.AddScoped<IDialogService, DialogService>();
            services.AddScoped<INavigationService, NavigationService>();

            // ViewModels（Transient）
            services.AddTransient<MainViewModel>();
            services.AddTransient<CustomerViewModel>();
            services.AddTransient<ProductViewModel>();

            // Views（Transient）
            services.AddTransient<MainWindow>();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await _host.StartAsync();

            // 從 DI 容器取得主視窗
            var mainWindow = _host.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            using (_host)
            {
                await _host.StopAsync();
            }

            base.OnExit(e);
        }

        /// <summary>
        /// 取得服務的靜態方法（Service Locator 模式）
        /// 注意：僅在無法使用建構子注入時使用
        /// </summary>
        public static T GetService<T>() where T : class
        {
            return ((App)Current)._host.Services.GetRequiredService<T>();
        }
    }
}
