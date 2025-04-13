using System.Threading.Tasks;
using taskMasterProjet.ViewModels;
using taskMasterProjet.Services; //pour la classe de gestion de com


namespace taskMasterProjet.Views;

[QueryProperty(nameof(TaskId), "taskId")]
public partial class EditTaskPage : ContentPage
{
    private List<CommentaireEntry> _commentEntries = new();

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
        LoadCommentaires();
    }

    // Ajouter un commentaire
    private void OnAjouterCommentaireClicked(object sender, EventArgs e)
    {
        var entry = new Entry { Placeholder = "Écrire un commentaire..." };
        var deleteButton = new Button
        {
            Text = "Supprimer",
            BackgroundColor = Colors.Red, // Couleur rouge
        };
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

        TransferCommentairesToViewModel();

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


    public void TransferCommentairesToViewModel()
    {
        if (BindingContext is EditTaskViewModel vm)
        {
            vm.CommentaireTextes = _commentEntries
                .Where(e => !string.IsNullOrWhiteSpace(e.Commentaire.Text))
                .Select(e => e.Commentaire.Text!)
                .ToList();
          //  await Shell.Current.DisplayAlert("Debug", $"Commentaires transférés : {vm.CommentaireTextes.Count}", "OK");

        }

    }

    /// Appelée lorsque le bouton "Enregistrer" est cliqué pour pouvoir faire des truc avec les commentaire avant de push vers le model
    private async void OnSaveClicked(object sender, EventArgs e)
    {
        TransferCommentairesToViewModel();
        await Task.Delay(100); //pour laisser le temps à la mise à jour de se faire

        if (BindingContext is EditTaskViewModel vm)
        {
            await vm.SaveChangesCommand.ExecuteAsync(null);
        }

        TransferCommentairesToViewModel();
    }

    //méthode pour charger les commentaires 
    private void LoadCommentaires()
    {
        CommentairesLayout.Children.Clear();
        _commentEntries.Clear();

        if (BindingContext is EditTaskViewModel vm && vm.Task.Commentaires != null)
        {
            foreach (var commentaire in vm.Task.Commentaires)
            {
                var entry = new Entry { Text = commentaire.Contenu };
                var deleteButton = new Button
                {
                    Text = "Supprimer",
                    BackgroundColor = Colors.Red,
                };
                deleteButton.Clicked += OnDeleteCommentaireClicked;

                var commentaireEntry = new CommentaireEntry
                {
                    Commentaire = entry,
                    DeleteButton = deleteButton
                };

                _commentEntries.Add(commentaireEntry);
                CommentairesLayout.Children.Add(entry);
                CommentairesLayout.Children.Add(deleteButton);
            }
        }

        TransferCommentairesToViewModel();
    }

}