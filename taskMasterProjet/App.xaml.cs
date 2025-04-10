using taskMasterProjet.Views;

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
    }

    protected override Window CreateWindow(IActivationState activationState)
    {
        // Définir la page principale via CreateWindow
        return new Window(new AppShell());
    }
}