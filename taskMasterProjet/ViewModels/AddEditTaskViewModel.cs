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
    private List<Projet> projects = new();

    [ObservableProperty]
    private Projet? selectedProject;

    //pour les etiquettes 
    [ObservableProperty]
    private string etiquetteInput = string.Empty;

    //pour le réalisateur
    [ObservableProperty]
    private List<Utilisateur> teamMembers = new();



    public List<string> Statuses { get; } = new() { "À faire", "En cours", "Terminée", "Annulée" };
    public List<string> Priorities { get; } = new() { "Basse", "Moyenne", "Haute", "Critique" };
    public List<string> Categories { get; } = new() { "Perso", "Travail", "Projet" };
    public DateTime Today { get; } = DateTime.Today;


    //pour la gestion des commentaires
    public List<string> CommentaireTextes { get; set; } = new();


    public AddEditTaskViewModel(TaskService taskService, UserSession userSession, AppDbContext context, ProjetService projetService)
    {
        _taskService = taskService;
        _userSession = userSession;
        _context = context;
        _projetService = projetService;
        _projetService = projetService;
        //InitializeDataAsync();
    }

    public async Task InitializeDataAsync()
    {
        // Valeurs par défaut
        Task.Statut = "À faire";
        Task.Priorite = "Moyenne";
        Task.Categorie = "Perso";
        Task.AuteurId = _userSession.CurrentUser.Id;

        // Charger les membres de l'équipe (hors utilisateur connecté)
        var projetService = new ProjetService(_context);
        TeamMembers = await projetService.GetAllUsers();
        SelectedAssignee = TeamMembers.FirstOrDefault(u => u.Id == Task.RealisateurId);

        // Charger tous les projets disponibles
        Projects = await _projetService.GetAllProjects();
        SelectedProject = Projects.FirstOrDefault(p => p.Id == Task.ProjetId);

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
            //if (!string.IsNullOrWhiteSpace(SelectedProjectName))
            //{
            //    var projet = await _projetService.GetProjectByName(SelectedProjectName);
            //    if (projet == null)
            //    {
            //        await Shell.Current.DisplayAlert("Erreur", $"Le projet '{SelectedProjectName}' n'existe pas.", "OK");
            //        return;
            //    }
            //    Task.ProjetId = projet.Id;
            //}
            if (SelectedProject != null)
            {
                Task.ProjetId = SelectedProject.Id;
            }



            // Traitement des étiquettes
            if (!string.IsNullOrWhiteSpace(EtiquetteInput))
            {
                var nomsEtiquettes = EtiquetteInput
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(n => n.Trim().ToLower())
                    .Where(n => !string.IsNullOrWhiteSpace(n))
                    .Distinct();

                var etiquettes = new List<Etiquette>();

                foreach (var nom in nomsEtiquettes)
                {
                    var existante = await _context.Etiquettes.FirstOrDefaultAsync(e => e.Nom.ToLower() == nom);
                    if (existante != null)
                    {
                        etiquettes.Add(existante);
                    }
                    else
                    {
                        var nouvelle = new Etiquette { Nom = nom };
                        etiquettes.Add(nouvelle);
                        _context.Etiquettes.Add(nouvelle); // pour créer l’étiquette en base
                    }
                }

                Task.Etiquettes = etiquettes;
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