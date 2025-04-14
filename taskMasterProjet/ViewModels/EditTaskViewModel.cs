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


    [ObservableProperty]
    private List<Etiquette> etiquettes = new();

    [ObservableProperty]
    private string etiquetteInput = string.Empty;

    [ObservableProperty]
    private List<Projet> projets = new();

    [ObservableProperty]
    private Projet selectedProjet;





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


        try
        {
            // Charger la tâche depuis la base pour éviter les problèmes de suivi
            var taskToUpdate = await _context.Taches
                .Include(t => t.Commentaires)
                .Include(t => t.Etiquettes)
                .FirstOrDefaultAsync(t => t.Id == Task.Id);

            if (taskToUpdate == null) return;

            // Mettre à jour les propriétés de base
            taskToUpdate.Titre = Task.Titre;
            taskToUpdate.Description = Task.Description;
            taskToUpdate.Echeance = Task.Echeance;
            taskToUpdate.Statut = Task.Statut;
            taskToUpdate.Priorite = Task.Priorite;
            taskToUpdate.Categorie = Task.Categorie;
            taskToUpdate.RealisateurId = Task.RealisateurId;
            taskToUpdate.ProjetId = Task.ProjetId;


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


            // Projet
            if (SelectedProjet != null)
            {
                Task.ProjetId = SelectedProjet.Id;
            }

            // Gestion des étiquettes
            if (!string.IsNullOrWhiteSpace(EtiquetteInput))
            {
                // Détacher toutes les anciennes étiquettes
                taskToUpdate.Etiquettes.Clear();

                var nomsEtiquettes = EtiquetteInput
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(n => n.Trim().ToLower())
                    .Where(n => !string.IsNullOrWhiteSpace(n))
                    .Distinct();

                foreach (var nom in nomsEtiquettes)
                {
                    // Vérifier si l'étiquette existe déjà en base
                    var etiquetteExistante = await _context.Etiquettes
                        .FirstOrDefaultAsync(e => e.Nom.ToLower() == nom);

                    if (etiquetteExistante != null)
                    {
                        // Si elle existe, l'ajouter via son ID
                        taskToUpdate.Etiquettes.Add(etiquetteExistante);
                    }
                    else
                    {
                        // Si elle n'existe pas, créer une nouvelle étiquette
                        var nouvelleEtiquette = new Etiquette { Nom = nom };
                        _context.Etiquettes.Add(nouvelleEtiquette);
                        taskToUpdate.Etiquettes.Add(nouvelleEtiquette);
                    }
                }
            }

            await _context.SaveChangesAsync();
            await Shell.Current.GoToAsync($"//{nameof(DashboardPage)}");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Erreur", ex.Message, "OK");
        }
    }
    [RelayCommand]
    private async Task Cancel()
    {
        await Shell.Current.GoToAsync($"//{nameof(DashboardPage)}");
    }

    public async Task LoadTaskAsync(int taskId)
    {
        // Charger la tâche avec toutes les relations nécessaires
        var loadedTask = await _context.Taches
            .Include(t => t.Auteur)
            .Include(t => t.Realisateur)
            .Include(t => t.SousTaches)
            .Include(t => t.Commentaires)
            .Include(t => t.Etiquettes)
            .Include(t => t.Projet)
            .FirstOrDefaultAsync(t => t.Id == taskId);

        if (loadedTask != null)
        {
            Task = loadedTask;

            // Initialiser les étiquettes
            EtiquetteInput = string.Join(", ", Task.Etiquettes?.Select(e => e.Nom) ?? new List<string>());

            // Initialiser les autres propriétés
            Task.Statut = Statuses.FirstOrDefault(s => s == Task.Statut);
            Task.Priorite = Priorities.FirstOrDefault(p => p == Task.Priorite);
            Task.Categorie = Categories.FirstOrDefault(c => c == Task.Categorie);

            // Initialiser le projet sélectionné

                //SelectedProjet = Projets.FirstOrDefault(p => p.Id == Task.Projet.Id);
                // Charger les projets
            var projetService = new ProjetService(_context); 
            Projets = await projetService.GetAllProjects();

            SelectedProjet = Projets.FirstOrDefault(p => p.Id == Task.ProjetId);
            
        }
    }
}