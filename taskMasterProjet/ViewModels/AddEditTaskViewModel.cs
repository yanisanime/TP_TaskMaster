using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using taskMasterProjet.Services;

namespace taskMasterProjet.ViewModels;

public partial class AddEditTaskViewModel : ObservableObject
{
    private readonly TaskService _taskService;
    private readonly ProjetService _projetService;
    private readonly UserSession _userSession;
    private readonly AppDbContext _context;

    [ObservableProperty]
    private Tache _task = new();

    [ObservableProperty]
    private Utilisateur? _selectedAssignee;

    [ObservableProperty]
    private string realisateurNom = string.Empty;

    //pour la gestion de projet
    [ObservableProperty]
    private Projet _selectedProject;

    //liste de projets pour l'affichage des possibilité
    public List<Projet> Projects { get; private set; } = new();


    public List<string> Statuses { get; } = new() { "À faire", "En cours", "Terminée", "Annulée" };
    public List<string> Priorities { get; } = new() { "Basse", "Moyenne", "Haute", "Critique" };
    public List<string> Categories { get; } = new() { "Perso", "Travail", "Projet" };
    public List<Utilisateur> TeamMembers { get; private set; } = new();
    public DateTime Today { get; } = DateTime.Today;


    //pour la gestion des commentaires
    public List<string> CommentaireTextes { get; set; } = new();


    public AddEditTaskViewModel(TaskService taskService, UserSession userSession, AppDbContext context, ProjetService projetService)
    {
        _taskService = taskService;
        _userSession = userSession;
        _context = context;
        _projetService = projetService;
        InitializeData();
        _projetService = projetService;
    }

    private async void InitializeData()
    {
        // Charger les membres de l'équipe (exemple simple)
        TeamMembers = await _context.Utilisateurs.ToListAsync();

        Projects = await _projetService.GetUserProjects(_userSession.CurrentUser.Id);


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


            // Ajouter les commentaires s'il y en a
            if (CommentaireTextes.Any())
            {
                //pour debug on affiche le nombre de commentaire dans une boite de diloge 
                //await Shell.Current.DisplayAlert("Debug", $"Nombre de commentaires : {CommentaireTextes.Count}", "OK");
                Task.Commentaires = CommentaireTextes.Select(text => new Commentaire
                {
                    Contenu = text,
                    AuteurId = _userSession.CurrentUser.Id,
                    Date = DateTime.Now
                }).ToList();
            }


            // Associer le projet sélectionné
            if (SelectedProject != null)
            {
                Task.ProjetId = SelectedProject.Id;
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