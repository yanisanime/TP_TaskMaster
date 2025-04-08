using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using taskmaster.Services;
using taskmaster.Views;

namespace taskmaster.ViewModels;

public partial class CreateAccountViewModel : ObservableObject
{
    [ObservableProperty]
    private string? nom;

    [ObservableProperty]
    private string? prenom;

    [ObservableProperty]
    private string? email;

    [ObservableProperty]
    private string? password;

    [ObservableProperty]
    private string? confirmPassword;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    private bool isBusy;
    //Pour empéché l'utilisateur de cliquer plusieurs fois sur le bouton de connexion
    public bool IsNotBusy => !IsBusy;


    private readonly AuthService _authService;

    public CreateAccountViewModel(AuthService authService)
    {
        _authService = authService;
    }

    [RelayCommand]
    private async Task CreateAccount()
    {
        if (IsBusy || string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            return;

        if (Password != ConfirmPassword)
        {
            await Shell.Current.DisplayAlert("Erreur", "Les mots de passe ne correspondent pas", "OK");
            return;
        }

        try
        {
            IsBusy = true;

            var success = await _authService.RegisterAsync(
                Nom ?? string.Empty,
                Prenom ?? string.Empty,
                Email!,
                Password!);

            if (success)
            {
                await Shell.Current.DisplayAlert("Succès", "Compte créé avec succès", "OK");
                await Shell.Current.GoToAsync(".."); // Retour à la page précédente
            }
            else
            {
                await Shell.Current.DisplayAlert("Erreur", "Cet email est déjà utilisé", "OK");
            }
        }
        finally
        {
            IsBusy = false;
        }
    }
}