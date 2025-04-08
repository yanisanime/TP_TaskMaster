using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using taskmaster.Services;
using taskmaster.Views;

namespace taskmaster.ViewModels;

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


    public LoginViewModel()
    {
        _authService = new AuthService();
    }

    [RelayCommand]
    private async Task Login()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;

            var isLoggedIn = await _authService.LoginAsync(Email, Password);

            if (isLoggedIn)
            {
                //await Shell.Current.GoToAsync($"//{nameof(DashboardPage)}"); //marche pas
                await Shell.Current.GoToAsync(nameof(DashboardPage));

            }
            else
            {
                await Shell.Current.DisplayAlert("Erreur", "Email ou mot de passe incorrect", "OK");
            }
        }
        finally
        {
            IsBusy = false;
        }
    }
}