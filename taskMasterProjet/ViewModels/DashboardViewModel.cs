using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using taskMasterProjet.Services;
using taskMasterProjet.Views;

namespace taskMasterProjet.ViewModels;

public partial class DashboardViewModel : ObservableObject
{
    private readonly UserSession _userSession;
    private readonly TaskService _taskService;

    [ObservableProperty]
    private List<Tache> _tasks;

    [ObservableProperty]
    private List<Tache> _filteredTasks;

    [ObservableProperty]
    private string _selectedStatus;

    [ObservableProperty]
    private string _selectedPriority;

    //pour afficher le nom et prénom de l'utilisateur connecté
    public string CurrentUserDisplayName =>
        $"{_userSession.CurrentUser?.Prenom} {_userSession.CurrentUser?.Nom}";

    public List<string> Statuses { get; } = new() { "Tous", "À faire", "En cours", "Terminée", "Annulée" };
    public List<string> Priorities { get; } = new() { "Toutes", "Basse", "Moyenne", "Haute", "Critique" };

    public DashboardViewModel(UserSession userSession, TaskService taskService)
    {
        _userSession = userSession;
        _taskService = taskService;

        SelectedStatus = "Tous";
        SelectedPriority = "Toutes";
        LoadTasks();
    }

    [RelayCommand]
    private async Task LoadTasks()
    {
        Tasks = await _taskService.GetUserTasks(_userSession.CurrentUser.Id);


        ApplyFilters();

        // await Shell.Current.DisplayAlert("Debug", $"Nombre de tache après filtre :  '{Tasks.lenth}'.", "OK");

    }

    [RelayCommand]
    private void ApplyFilters()
    {
        FilteredTasks = Tasks
            .Where(t => SelectedStatus == "Tous" || t.Statut == SelectedStatus)
            .Where(t => SelectedPriority == "Toutes" || t.Priorite == SelectedPriority)
            .ToList();


    }

    [RelayCommand]
    private async Task AddTask()
    {
        //await Shell.Current.GoToAsync(nameof(AddTaskPage));
        await Shell.Current.GoToAsync(nameof(AddEditTaskPage));
    }

    [RelayCommand]
    private async Task Logout()
    {
        _userSession.Logout();
        await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
    }

    [RelayCommand]
    private async Task ShowTaskDetail(Tache task)
    {
        var parameters = new Dictionary<string, object> { ["Task"] = task };
        //await Shell.Current.GoToAsync(nameof(TaskDetailPage), parameters);
        await Shell.Current.GoToAsync(nameof(DashboardPage), parameters);
    }

    [RelayCommand]
    private async Task DeleteTask(Tache task)
    {
        await Shell.Current.DisplayAlert("Debug", $"Suppression de la tâche :  '{task.Titre}'.", "OK");

        bool confirm = await Shell.Current.DisplayAlert(
            "Confirmation",
            $"Voulez-vous vraiment supprimer la tâche '{task.Titre}' ?",
            "Oui", "Non");

        if (confirm)
        {
            await _taskService.DeleteTask(task.Id);
            await LoadTasks(); // Recharger la liste
        }
    }

    [RelayCommand]
    private async Task EditTask(Tache task)
    {
        await Shell.Current.DisplayAlert("Debug", $"Modification de la tâche :  '{task.Titre}'.", "OK");

        var parameters = new Dictionary<string, object> { ["Task"] = task };
        await Shell.Current.GoToAsync(nameof(AddEditTaskPage), parameters);
    }
}