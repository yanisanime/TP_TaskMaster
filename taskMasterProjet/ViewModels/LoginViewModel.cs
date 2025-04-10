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

            var user = await _authService.LoginAsync(Email, Password);

            if (user != null)
            {
                _userSession.Login(user);
                await Shell.Current.GoToAsync($"//{nameof(DashboardPage)}");
            }
            else
            {
                await Shell.Current.DisplayAlert("Erreur", "Email ou mot de passe incorrect, ou compte inexistant.", "OK");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur pendant la connexion : {ex.Message}");
            await Shell.Current.DisplayAlert("Erreur", "Une erreur est survenue pendant la connexion.", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task GoBack()
    {
        await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
    }
}