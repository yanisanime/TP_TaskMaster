﻿using Microsoft.EntityFrameworkCore;
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
        // ConfigureDatabase(builder);
        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseMySql(
                "server=localhost;port=3306;database=taskmaster;user=root;password=;",
                new MySqlServerVersion(new Version(8, 0, 29))
            );
        });


        //// Configuration de la base de données avec gestion des erreurs
        //// var connectionString = "server=localhost;port=8080;database=taskmaster;user=root;";
        //var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        //var serverVersion = new MySqlServerVersion(new Version(8, 0, 29));
        //try
        //{
        //    builder.Services.AddDbContext<AppDbContext>(
        //        dbContextOptions => dbContextOptions
        //            .UseMySql(connectionString, serverVersion) // Utilisez UseMySql avec Pomelo
        //            .LogTo(Console.WriteLine, LogLevel.Information) // Log des requêtes pour le débogage
        //            .EnableSensitiveDataLogging()
        //            .EnableDetailedErrors()
        //    );
        //    // Test de connexion à la base de données
        //    using (var scope = builder.Services.BuildServiceProvider().CreateScope())
        //    {
        //        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        //        dbContext.Database.OpenConnection(); // Ouvre une connexion pour tester
        //        dbContext.Database.CloseConnection(); // Ferme la connexion après le test
        //        Debug.WriteLine("Connexion à la base de données réussie.");
        //    }
        //}
        //catch (Exception ex)
        //{
        //    Debug.WriteLine($"Erreur lors de la connexion à la base de données : {ex.Message}");
        //    //// Affiche un message d'erreur à l'utilisateur
        //    //Application.Current?.Dispatcher.Dispatch(async () =>
        //    //{
        //    //    await Application.Current.MainPage.DisplayAlert(
        //    //        "Erreur",
        //    //        "Impossible de se connecter à la base de données. Veuillez vérifier la configuration.",
        //    //        "OK"
        //    //    );
        //    //});
        //}






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


    //private static void ConfigureDatabase(MauiAppBuilder builder)
    //{

    //    // Configuration de la DB
    //    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    //    var serverVersion = new MySqlServerVersion(new Version(8, 0, 29));

    //    builder.Services.AddDbContext<AppDbContext>(options =>
    //        options.UseMySql(connectionString, serverVersion)
    //               .EnableDetailedErrors()
    //               .EnableSensitiveDataLogging()
    //               .LogTo(message => Debug.WriteLine(message)));

    //    //var connectionString = "server=localhost;port=3306;database=taskmaster;user=root;";
    //    //var serverVersion = new MySqlServerVersion(new Version(8, 0, 29));

    //    //builder.Services.AddDbContext<AppDbContext>(options =>
    //    //{
    //    //    options.UseMySql(connectionString, serverVersion, mysqlOptions =>
    //    //    {
    //    //        mysqlOptions.EnableRetryOnFailure(
    //    //            maxRetryCount: 5,
    //    //            maxRetryDelay: TimeSpan.FromSeconds(5),
    //    //            errorNumbersToAdd: null);
    //    //    })
    //    //    .EnableSensitiveDataLogging()
    //    //    .EnableDetailedErrors();
    //    //});


    //}
}