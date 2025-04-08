using taskmaster.Views;

namespace taskmaster;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        // Enregistrez vos routes
        Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
        Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        Routing.RegisterRoute(nameof(DashboardPage), typeof(DashboardPage));

        MainPage = new AppShell();
    }
}