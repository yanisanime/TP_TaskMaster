using Microsoft.Maui.Controls;
using taskMasterProjet.ViewModels;

namespace taskMasterProjet.Views;

public partial class AddEditTaskPage : ContentPage
{
    private List<Entry> _commentEntries = new();
    private List<Button> _deleteButtons = new();


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

        // Ajouter un bouton pour supprimer ce commentaire
        var deleteButton = new Button { Text = "Supprimer" };
        deleteButton.Clicked += OnDeleteCommentaireClicked;
        _deleteButtons.Add(deleteButton);
        CommentairesLayout.Children.Add(deleteButton);
    }

    private void OnDeleteCommentaireClicked(object sender, EventArgs e)
    {
        // Trouver le bouton cliqué et son champ de texte correspondant
        var button = sender as Button;
        var entryToDelete = _commentEntries.FirstOrDefault(entry => CommentairesLayout.Children.Contains(entry));
        if (entryToDelete != null)
        {
            // Supprimer l'Entry et le bouton
            _commentEntries.Remove(entryToDelete);
            CommentairesLayout.Children.Remove(entryToDelete);
            CommentairesLayout.Children.Remove(button); // Supprimer aussi le bouton associé

            // Mettre à jour la liste des commentaires dans le ViewModel
            TransferCommentairesToViewModel();
        }
    }

    protected override async void OnDisappearing()
    {
        base.OnDisappearing();

        // Passer les commentaires au ViewModel
        //if (BindingContext is AddEditTaskViewModel vm)
        //{
        //    vm.CommentaireTextes = _commentEntries
        //        .Where(e => !string.IsNullOrWhiteSpace(e.Text))
        //        .Select(e => e.Text!)
        //        .ToList();
        //}
        TransferCommentairesToViewModel();

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
