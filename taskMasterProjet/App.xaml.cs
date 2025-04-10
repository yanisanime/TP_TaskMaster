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
        Routing.RegisterRoute(nameof(AddEditTaskPage), typeof(AddEditTaskPage));

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
        }
    }

}