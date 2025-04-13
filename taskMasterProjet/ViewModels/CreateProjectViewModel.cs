using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using taskMasterProjet.Services;
using taskMasterProjet.Views;

namespace taskMasterProjet.ViewModels;

public partial class CreateProjectViewModel : ObservableObject
{
    private readonly ProjetService _projetService;
    private readonly UserSession _userSession;

    [ObservableProperty]
    private string _nom;

    [ObservableProperty]
    private string _description;

    public CreateProjectViewModel(ProjetService projetService, UserSession userSession)
    {
        _projetService = projetService;
        _userSession = userSession;
    }

    [RelayCommand]
    private async Task CreateProject()
    {
        if (string.IsNullOrWhiteSpace(Nom))
        {
            await Shell.Current.DisplayAlert("Erreur", "Le nom du projet est obligatoire", "OK");
            return;
        }

        var projet = new Projet
        {
            Nom = Nom,
            Description = Description,
            CreateurId = _userSession.CurrentUser.Id
        };

        await _projetService.CreateProject(projet);
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private async Task Cancel()
    {
        await Shell.Current.GoToAsync("..");
    }
}