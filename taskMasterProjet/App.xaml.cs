using taskMasterProjet.Views;
using taskMasterProjet.Services;
using System.Diagnostics;


namespace taskMasterProjet;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        // ICI on enregistre toutes nos routes si j'ai bien comprits
        Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
        Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        Routing.RegisterRoute(nameof(CreateAccountPage), typeof(CreateAccountPage));
        Routing.RegisterRoute(nameof(DashboardPage), typeof(DashboardPage));

        // MainPage = new AppShell();
        //OnStart();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }


    protected override async void OnStart()
    {
        base.OnStart();

        try
        {
            using var scope = MauiProgram.CreateMauiApp().Services.CreateScope();
            var authService = scope.ServiceProvider.GetRequiredService<AuthService>();
            await authService.TestConnection();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERREUR DB: {ex}");
            // Ici tu pourrais afficher une alerte à l'utilisateur
            // Pour afficher une alerte, il faut attendre que la MainPage soit prête
            //if (Current?.MainPage != null)
            //{
            //    await Current.MainPage.DisplayAlert("Erreur", "Problème de connexion à la base de données", "OK");
            //}

        }
    }

    private async Task TestDatabaseConnection()
    {
        using var scope = MauiProgram.CreateMauiApp().Services.CreateScope();
        var authService = scope.ServiceProvider.GetRequiredService<AuthService>();

        if (!await authService.CanConnectToDatabase())
        {
            // Mode dégradé sans DB
            Debug.WriteLine("Using fallback mode without database");
        }
        //try
        //{
        //    using var scope = MauiProgram.CreateMauiApp().Services.CreateScope();
        //    var authService = scope.ServiceProvider.GetRequiredService<AuthService>();
        //    await authService.TestConnection();
        //}
        //catch (Exception ex)
        //{
        //    Debug.WriteLine($"ERREUR DB: {ex}");
        //    // Afficher l'alerte sur le thread UI
        //    Application.Current.Dispatcher.Dispatch(async () =>
        //    {
        //        await Application.Current.MainPage.DisplayAlert(
        //            "Erreur",
        //            "Problème de connexion à la base de données",
        //            "OK");
        //    });
        //}
    }

}