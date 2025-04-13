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

    /// Propriétés pour la gestion des commentaires
    public List<string> CommentaireTextes { get; set; } = new();


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

        /*GESTION DES COMMENTAIRE*/



        ////On supprime les ancien commentaire supprimer s'il y en as 
        //var anciensCommentaires = await _context.Commentaires
        //    .Where(c => c.TacheId == Task.Id)
        //    .ToListAsync();

        ////On affiche le nombre de commentaire dans une boite de dialogue pour debug
        //await Shell.Current.DisplayAlert("Debug", $"Nombre d'ancien commentaires 1 : {anciensCommentaires.Count}", "OK");

        //_context.Commentaires.RemoveRange(anciensCommentaires);

        ////On affiche le nombre de commentaire dans une boite de dialogue pour debug
        //await Shell.Current.DisplayAlert("Debug", $"Nombre d'ancien commentaires 2 : {anciensCommentaires.Count}", "OK");

        //On affiche le nombre de commentaire dans une boite de dialogue pour debug
        await Shell.Current.DisplayAlert("Debug", $"Nombre de commentaires : {CommentaireTextes.Count}", "OK");


        // ensuite on ajoute les nouveaux commentaires
        if (CommentaireTextes.Any())
        {
            Task.Commentaires = CommentaireTextes
                .Select(text => new Commentaire
                {
                    Contenu = text,
                    AuteurId = _userSession.CurrentUser.Id,
                    Date = DateTime.Now,
                    TacheId = Task.Id // important pour la persistance sinon ça plante mochement !!!!
                }).ToList();
        }
        else
        {
            Task.Commentaires = new List<Commentaire>();
        }



        await _taskService.UpdateTask(task);

        //On met a joure la liste de commentaire dans la base en la netoyant 
        await _taskService.DeleteAllCommentaireByTacheIdNull();

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