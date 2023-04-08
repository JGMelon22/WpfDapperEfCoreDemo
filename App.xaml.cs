using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Data;
using System.IO;
using System.Windows;
using WpfDapperEfCoreDemo.Interfaces;
using WpfDapperEfCoreDemo.Repositories;

namespace WpfDapperEfCoreDemo;


public partial class App : Application
{
	public static IHost? AppHost { get; private set; }
	public App() // Service registration
	{
		AppHost = Host.CreateDefaultBuilder()
			.ConfigureAppConfiguration((hostContext, config) =>
			{
				config.SetBasePath(Directory.GetCurrentDirectory());
				config.AddJsonFile(@"C:\Users\joao\Documents\Desenvolvimento\TempCode\WpfDapperEfCoreDemo\appsettings.json", optional: false, reloadOnChange: true);
			})
			.ConfigureServices((hostContext, services) =>
			{
				services.AddSingleton<MainWindow>();
				services.AddSingleton<IPessoaRepository, PessoaRepository>();
				services.AddScoped<IDbConnection>((x) => new SqlConnection(hostContext.Configuration.GetConnectionString("Default")));
			})
			.Build();
	}

	// Start services 
	protected override async void OnStartup(StartupEventArgs e)
	{
		await AppHost!.StartAsync(); // Start application

		var startUpForm = AppHost.Services.GetRequiredService<MainWindow>();
		startUpForm.Show();

		base.OnStartup(e);
	}


	protected override async void OnExit(ExitEventArgs e)
	{
		await AppHost!.StopAsync();
		base.OnExit(e);
	}
}