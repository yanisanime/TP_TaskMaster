using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using taskMasterProjet.Services;
using taskMasterProjet.ViewModels;
using taskMasterProjet.Views;

namespace taskMasterProjet;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts => {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        // Configuration de la base de données
        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseMySql(
                "server=localhost;port=8080;database=taskmaster;user=root;",
                new MySqlServerVersion(new Version(10, 4, 28))
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