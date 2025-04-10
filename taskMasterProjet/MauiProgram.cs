using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using taskMasterProjet.Services;
using taskMasterProjet.ViewModels;
using taskMasterProjet.Views;
using Pomelo.EntityFrameworkCore.MySql;

namespace taskMasterProjet;


//comande ultra importante 
// dotnet ef migrations add MigrationInitialCreation2 --framework net9.0-windows10.0.19041.0
//dotnet ef database update --framework net9.0-windows10.0.19041.0

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        // Configuration DB
        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseMySql(
                "server=localhost;port=3306;database=taskmaster;user=root;password=;",
                new MySqlServerVersion(new Version(8, 0, 29))
            );
        });


        // Configuration simplifiée sans DB pour tester
        builder.Services.AddSingleton<AuthService>();
        builder.Services.AddSingleton<UserSession>();

        // ViewModels
        builder.Services.AddTransient<HomeViewModel>();
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<CreateAccountViewModel>();

        // Views
        builder.Services.AddTransient<HomePage>();
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<CreateAccountPage>();
        builder.Services.AddTransient<DashboardPage>();

        return builder.Build();
    }
}