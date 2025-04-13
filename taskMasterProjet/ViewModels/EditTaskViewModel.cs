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
    private Utilisateur? _selectedAssignee; // Utilisateur assigné à la tâche

    public int UserId => _userSession.CurrentUser.Id;




    /// Propriétés pour la gestion des commentaires
    public List<string> CommentaireTextes { get; set; } = new();

    public List<Commentaire> CommentairesToDelete { get; set; }


    [ObservableProperty]
    private string realisateurNom = string.Empty;

    public List<string> Statuses { get; } = new() { "À faire", "En cours", "Terminée", "Annulée" };
    public List<string> Priorities { get; } = new() { "Basse", "Moyenne", "Haute", "Critique" };
    public List<string> Categories { get; } = new() { "Perso", "Travail", "Projet" };

    [ObservableProperty]
    private Tache task;

    public EditTaskViewModel(TaskService taskService, UserSession userSession, AppDbContext context)
    {
        _taskService = taskService;
        _userSession = userSession;
        _context = context;


    }

    public void SetTask(Tache task)
    {
        Task = task;
    }

    [RelayCommand]
    private async Task SaveChanges()
    {
        if (string.IsNullOrWhiteSpace(Task.Titre))
        {
            await Shell.Current.DisplayAlert("Erreur", "Le titre est obligatoire", "OK");
            return;
        }

        // Charger tous les commentaires liés à la tâche
        var commentairesExistants = await _context.Commentaires
            .Where(c => c.TacheId == Task.Id)
            .ToListAsync();


        //poru chaque commentaire on affiche une fenetre de dialogue avec le contenu pour debug
        foreach (var commentaire in commentairesExistants)
        {
            await Shell.Current.DisplayAlert("Debug", $"Commentaire : {commentaire.Contenu}", "OK");
        }

        // Supprimer les commentaires marqués pour suppression
        if (CommentairesToDelete != null && CommentairesToDelete.Any())
        {
            foreach (var commentaire in CommentairesToDelete)
            {
                var commentaireSuivi = commentairesExistants.FirstOrDefault(c => c.Id == commentaire.Id);
                if (commentaireSuivi != null)
                {
                    _context.Commentaires.Remove(commentaireSuivi);
                }
            }
        }

        // Mettre à jour ou ajouter les commentaires
        foreach (var commentaire in Task.Commentaires)
        {
            // On vérif d'abord si le commentaire existe déjà dans la base de données
            var commentaireDansDb = commentairesExistants.FirstOrDefault(c => c.Id == commentaire.Id);

            if (commentaireDansDb != null)
            {
                await Shell.Current.DisplayAlert("Erreur", "Le commentaire exite déjà", "OK");
                //mise à jour des commentaires existants
                commentaireDansDb.Contenu = commentaire.Contenu;
                commentaireDansDb.Date = DateTime.Now;
            }
            else
            {
                await Shell.Current.DisplayAlert("Erreur", "Ajout d'un commentaire nouveau", "OK");
                // Ajout des nouveaux commentaires
                commentaire.TacheId = Task.Id;
                commentaire.AuteurId = _userSession.CurrentUser.Id;
                commentaire.Date = DateTime.Now;
                _context.Commentaires.Add(commentaire);
            }
        }

        await _taskService.UpdateTask(Task);
        await _context.SaveChangesAsync(); // Important ici car tu manipules directement le contexte

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