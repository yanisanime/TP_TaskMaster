using System.Threading.Tasks;
using taskMasterProjet.ViewModels;

namespace taskMasterProjet.Views;

[QueryProperty(nameof(TaskId), "taskId")]
public partial class EditTaskPage : ContentPage
{
    public int TaskId { get; set; }

    private readonly EditTaskViewModel _viewModel;

    public EditTaskPage(EditTaskViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Chargez la tâche à partir de l'ID
        await _viewModel.LoadTaskAsync(TaskId);
    }
}