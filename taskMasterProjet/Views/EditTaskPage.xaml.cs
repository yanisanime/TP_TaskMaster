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

    protected override async void OnDisappearing()
    {
        base.OnDisappearing();

        // Passer les commentaires au ViewModel
        TransferCommentairesToViewModel();
    }

    // Ajouter un commentaire
    private void OnAjouterCommentaireClicked(object sender, EventArgs e)
    {
        var entry = new Entry { Placeholder = "Écrire un commentaire..." };
        entry.TextChanged += OnCommentaireTextChanged; //Important, il faut lié sinon ça plante mochement !!!!

        var deleteButton = new Button
        {
            Text = "Supprimer",
            BackgroundColor = Colors.Red, // Couleur rouge pour le style joli
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
        var button = sender as Button;
        var commentaireEntryToDelete = _commentEntries.FirstOrDefault(entry => entry.DeleteButton == button);

        if (commentaireEntryToDelete != null)
        {
            // Si c'est un commentaire existant (a un Id), on le marque pour suppression
            if (commentaireEntryToDelete.Commentaire.BindingContext is Commentaire existingComment)
            {
                if (BindingContext is EditTaskViewModel vm)
                {
                    vm.CommentairesToDelete ??= new List<Commentaire>();
                    vm.CommentairesToDelete.Add(existingComment);
                }
            }

            _commentEntries.Remove(commentaireEntryToDelete);
            CommentairesLayout.Children.Remove(commentaireEntryToDelete.Commentaire);
            CommentairesLayout.Children.Remove(commentaireEntryToDelete.DeleteButton);

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
                .Distinct() // Évite les doublons
                .ToList();

            //ICI pour debug, je veux afficher dans une boite de dialogue le contenu de chaque commentaire
            foreach (var commentaire in vm.CommentaireTextes)
            {
                System.Diagnostics.Debug.WriteLine($"Commentaire Yanis : '{commentaire}'");
            }
        }
    }

    /// Appelée lorsque le bouton "Enregistrer" est cliqué pour pouvoir faire des truc avec les commentaire avant de push vers le model
    private async void OnSaveClicked(object sender, EventArgs e)
    {
        await Task.Delay(100);
        TransferCommentairesToViewModel();

        if (BindingContext is EditTaskViewModel vm)
        {
            await vm.SaveChangesCommand.ExecuteAsync(null);
        }
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
                entry.BindingContext = commentaire; // pour lier le commentaire à l'entrée
                entry.TextChanged += OnCommentaireTextChanged; // Écoute les changements de texte

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

    private void OnCommentaireTextChanged(object sender, TextChangedEventArgs e)
    {
        var entry = sender as Entry;
        if (entry != null)
        {
            var commentaireEntry = _commentEntries.FirstOrDefault(c => c.Commentaire == entry);
            if (commentaireEntry != null)
            {
                // Mettre à jour le texte dans l'Entry
                commentaireEntry.Commentaire.Text = e.NewTextValue;

                // MAJ de l'objet Commentaire lié si c’est un existant
                if (entry.BindingContext is Commentaire existingComment)
                {
                    existingComment.Contenu = e.NewTextValue;
                }

                // Mettre à jour le ViewModel sans ajouter de doublons
                TransferCommentairesToViewModel();
            }
        }
    }
}