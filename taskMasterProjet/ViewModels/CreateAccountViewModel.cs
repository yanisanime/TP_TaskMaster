﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;


using taskMasterProjet.Services;
using taskMasterProjet.Views;

namespace taskMasterProjet.ViewModels;

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


        // Vérifie si un utilisateur existe déjà avec cet email
        if (await _authService.UserExistsAsync(Email!))
        {
            await Shell.Current.DisplayAlert("Erreur", "Cet email est déjà utilisé", "OK");
            return;
        }

        if (Password != ConfirmPassword || Password.Length <5)
        {
            await Shell.Current.DisplayAlert("Erreur", "Les mots de passe ne correspondent pas ou trop court", "OK");
            return;
        }

        if(!Email.Contains("@") || !Email.Contains(".")) // Vérifie si l'email est valide
        {
            await Shell.Current.DisplayAlert("Erreur", "Email invalide", "OK");
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
                // On redirige l'utilisateur vers la page d'accueil si la création a marché
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
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

    [RelayCommand]
    private async Task GoBack()
    {
        await Shell.Current.GoToAsync($"//{nameof(HomePage)}"); // Retour à la page précédente
    }
}