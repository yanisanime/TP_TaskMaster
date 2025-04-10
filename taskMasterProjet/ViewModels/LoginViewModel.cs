using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel.DataAnnotations;
using taskMasterProjet.Services;
using taskMasterProjet.Views;

namespace taskMasterProjet.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    [ObservableProperty]
    private string email;

    [ObservableProperty]
    private string password;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    private bool isBusy;

    //Pour empéché l'utilisateur de cliquer plusieurs fois sur le bouton de connexion
    public bool IsNotBusy => !IsBusy;

    //Service d'authentification
    private readonly AuthService _authService;
    private readonly UserSession _userSession;


    public LoginViewModel(AuthService authService, UserSession userSession)
    {
        _authService = authService;
        _userSession = userSession;
    }

    [RelayCommand]
    private async Task Login()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;

            // Simulation de connexion sans DB pour tester
            var user = new Utilisateur { Email = Email, Nom = "Test", Prenom = "User" };
            _userSession.Login(user);
            await Shell.Current.GoToAsync("//DashboardPage");

            // Version réelle avec DB (à décommenter plus tard)
            // var user = await _authService.LoginAsync(Email, Password);
            // if (user != null) {
            //     _userSession.Login(user);
            //     await Shell.Current.GoToAsync("//DashboardPage");
            // }

            //var user = await _authService.LoginAsync(Email, Password);

            //if (user != null)
            //{
            //    //await Shell.Current.GoToAsync($"//{nameof(DashboardPage)}"); //marche pas
            //    _userSession.Login(user);
            //    //await Shell.Current.GoToAsync(nameof(DashboardPage));
            //    await Shell.Current.GoToAsync("//DashboardPage");

            //}
            //else
            //{
            //    await Shell.Current.DisplayAlert("Erreur", "Email ou mot de passe incorrect", "OK");
            //}
        }
        finally
        {
            IsBusy = false;
        }
    }
}