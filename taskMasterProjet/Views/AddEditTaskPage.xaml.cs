using Microsoft.Maui.Controls;
using taskMasterProjet.ViewModels;

namespace taskMasterProjet.Views;

//pour la gestion des commentaires et des bouton de supression de commentaire
public class CommentaireEntry
{
    public Entry Commentaire { get; set; }
    public Button DeleteButton { get; set; }
}


public partial class AddEditTaskPage : ContentPage
{
    private List<CommentaireEntry> _commentEntries = new();

    public AddEditTaskPage(AddEditTaskViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private void OnAjouterCommentaireClicked(object sender, EventArgs e)
    {
        var entry = new Entry { Placeholder = "Écrire un commentaire..." };
        var deleteButton = new Button { Text = "Supprimer" };
        deleteButton.Clicked += OnDeleteCommentaireClicked;

        // Créer un objet CommentaireEntry pour lier le Entry et le bouton Delete
        var commentaireEntry = new CommentaireEntry
        {
            Commentaire = entry,
            DeleteButton = deleteButton
        };

        // Ajouter à la liste des commentaires
        _commentEntries.Add(commentaireEntry);

        // Ajouter le champ de texte et le bouton dans l'interface
        CommentairesLayout.Children.Add(entry);
        CommentairesLayout.Children.Add(deleteButton);
    }

    private void OnDeleteCommentaireClicked(object sender, EventArgs e)
    {
        // Trouver le bouton cliqué
        var button = sender as Button;

        // Chercher le CommentaireEntry qui contient ce bouton
        var commentaireEntryToDelete = _commentEntries.FirstOrDefault(entry => entry.DeleteButton == button);

        if (commentaireEntryToDelete != null)
        {
            // Supprimer le commentaire et le bouton associé de l'interface
            _commentEntries.Remove(commentaireEntryToDelete);
            CommentairesLayout.Children.Remove(commentaireEntryToDelete.Commentaire);
            CommentairesLayout.Children.Remove(commentaireEntryToDelete.DeleteButton);

            // Mettre à jour la liste des commentaires dans le ViewModel
            TransferCommentairesToViewModel();
        }
    }

    protected override async void OnDisappearing()
    {
        base.OnDisappearing();

        // Passer les commentaires au ViewModel
        TransferCommentairesToViewModel();
    }

    public void TransferCommentairesToViewModel()
    {
        if (BindingContext is AddEditTaskViewModel vm)
        {
            vm.CommentaireTextes = _commentEntries
                .Where(e => !string.IsNullOrWhiteSpace(e.Commentaire.Text))
                .Select(e => e.Commentaire.Text!)
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
