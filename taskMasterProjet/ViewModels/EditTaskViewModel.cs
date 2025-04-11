using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using taskMasterProjet.Services;
using taskMasterProjet.Views;

namespace taskMasterProjet.ViewModels;

public partial class EditTaskViewModel : ObservableObject
{
    private readonly TaskService _taskService;
    private readonly UserSession _userSession;
    private readonly AppDbContext _context;


    [ObservableProperty]
    private string realisateurNom = string.Empty;

    public List<string> Statuses { get; } = new() { "À faire", "En cours", "Terminée", "Annulée" };
    public List<string> Priorities { get; } = new() { "Basse", "Moyenne", "Haute", "Critique" };
    public List<string> Categories { get; } = new() { "Perso", "Travail", "Projet" };

    [ObservableProperty]
    private Tache task;

    public EditTaskViewModel(TaskService taskService)
    {
        _taskService = taskService;
    }

    public void SetTask(Tache task)
    {
        task = task;
    }

    [RelayCommand]
    private async Task SaveChanges()
    {
        // Validation titre
        if (string.IsNullOrWhiteSpace(Task.Titre))
        {
            await Shell.Current.DisplayAlert("Erreur", "Le titre est obligatoire", "OK");
            return;
        }

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



        await _taskService.UpdateTask(task);
        await Shell.Current.GoToAsync($"//{nameof(DashboardPage)}");
    }

    [RelayCommand]
    private async Task Cancel()
    {
        await Shell.Current.GoToAsync($"//{nameof(DashboardPage)}");
    }

    public async Task LoadTaskAsync(int taskId)
    {
        var loadedTask = await _taskService.GetTaskById(taskId);
        if (loadedTask != null)
        {
            Task = loadedTask;

            // ICI on assosi explicitement les valeurs au bon objet de la liste parce que sinon ça ne marche pas 
            Task.Statut = Statuses.FirstOrDefault(s => s == Task.Statut);
            Task.Priorite = Priorities.FirstOrDefault(p => p == Task.Priorite);
            Task.Categorie = Categories.FirstOrDefault(c => c == Task.Categorie);
        }
    }
}