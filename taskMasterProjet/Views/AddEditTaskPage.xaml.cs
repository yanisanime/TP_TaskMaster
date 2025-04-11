using Microsoft.Maui.Controls;
using taskMasterProjet.ViewModels;

namespace taskMasterProjet.Views;

public partial class AddEditTaskPage : ContentPage
{
    private List<Entry> _commentEntries = new();

    public AddEditTaskPage(AddEditTaskViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private void OnAjouterCommentaireClicked(object sender, EventArgs e)
    {
        var entry = new Entry { Placeholder = "Écrire un commentaire..." };
        _commentEntries.Add(entry);
        CommentairesLayout.Children.Add(entry);
    }

    protected override async void OnDisappearing()
    {
        base.OnDisappearing();

        // Passer les commentaires au ViewModel
        if (BindingContext is AddEditTaskViewModel vm)
        {
            vm.CommentaireTextes = _commentEntries
                .Where(e => !string.IsNullOrWhiteSpace(e.Text))
                .Select(e => e.Text!)
                .ToList();
        }
    }

    public void TransferCommentairesToViewModel()
    {
        if (BindingContext is AddEditTaskViewModel vm)
        {
            vm.CommentaireTextes = _commentEntries
                .Where(e => !string.IsNullOrWhiteSpace(e.Text))
                .Select(e => e.Text!)
                .ToList();
        }
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        TransferCommentairesToViewModel();

        if (BindingContext is AddEditTaskViewModel vm)
        {
            await vm.SaveCommand.ExecuteAsync(null);
        }
    }

}
