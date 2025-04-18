﻿using taskMasterProjet.Views;

namespace taskMasterProjet;

public partial class AppShell : Shell
{
    public AppShell()
    {
        try
        {
            InitializeComponent();

            // Enregistrement des routes (au cas où)
            Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(CreateAccountPage), typeof(CreateAccountPage));
            Routing.RegisterRoute(nameof(DashboardPage), typeof(DashboardPage));
            Routing.RegisterRoute(nameof(EditTaskPage), typeof(EditTaskPage));
            Routing.RegisterRoute(nameof(CreateProjectPage), typeof(CreateProjectPage));

        }
        catch (Exception ex)
        {
            Console.WriteLine($"AppShell Error: {ex}");
            throw;
        }
    }
}
