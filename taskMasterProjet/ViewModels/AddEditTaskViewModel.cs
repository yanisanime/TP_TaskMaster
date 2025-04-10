using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using taskMasterProjet.Services;

namespace taskMasterProjet.ViewModels;

public partial class AddEditTaskViewModel : ObservableObject
{
    private readonly TaskService _taskService;
    private readonly UserSession _userSession;
    private readonly AppDbContext _context;

    [ObservableProperty]
    private Tache _task = new();

    [ObservableProperty]
    private Utilisateur? _selectedAssignee;

    [ObservableProperty]
    private string realisateurNom = string.Empty;

    public List<string> Statuses { get; } = new() { "À faire", "En cours", "Terminée", "Annulée" };
    public List<string> Priorities { get; } = new() { "Basse", "Moyenne", "Haute", "Critique" };
    public List<string> Categories { get; } = new() { "Perso", "Travail", "Projet" };
    public List<Utilisateur> TeamMembers { get; private set; } = new();
    public DateTime Today { get; } = DateTime.Today;

    public AddEditTaskViewModel(TaskService taskService, UserSession userSession, AppDbContext context)
    {
        _taskService = taskService;
        _userSession = userSession;
        _context = context;
        InitializeData();
    }

    private async void InitializeData()
    {
        // Charger les membres de l'équipe (exemple simple)
        TeamMembers = await _context.Utilisateurs.ToListAsync();

        // Valeurs par défaut
        Task.Statut = "À faire";
        Task.Priorite = "Moyenne";
        Task.Categorie = "Perso";
        Task.AuteurId = _userSession.CurrentUser.Id;
    }

    [RelayCommand]
    private async Task Save()
    {
        try
        {

            // Validation titre
            if (string.IsNullOrWhiteSpace(Task.Titre))
            {
                await Shell.Current.DisplayAlert("Erreur", "Le titre est obligatoire", "OK");
                return;
            }

            // Validation date d'échéance
            if (SelectedAssignee != null)
                Task.RealisateurId = SelectedAssignee.Id;

            // Vérification du réalisateur si un nom est saisi
            if (!string.IsNullOrWhiteSpace(realisateurNom))
            {
                var realisateur = await _context.Utilisateurs
                    .FirstOrDefaultAsync(u => u.Nom == realisateurNom);

                if (realisateur == null)
                {
                    await Shell.Current.DisplayAlert("Erreur", $"Aucun utilisateur nommé '{realisateurNom}' n'existe. Veuillez corriger.", "OK");
                    return;
                }

                Task.RealisateurId = realisateur.Id;
            }



            // Sauvegarde
            await _taskService.CreateTask(Task);

            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Erreur", ex.Message, "OK");
        }
    }

    [RelayCommand]
    private async Task Cancel()
    {
        await Shell.Current.GoToAsync("..");
    }
}