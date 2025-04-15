using System.Threading.Tasks;
using taskMasterProjet.ViewModels;
using taskMasterProjet.Services; //pour la classe de gestion de com et sous taches


namespace taskMasterProjet.Views;

[QueryProperty(nameof(TaskId), "taskId")]
public partial class EditTaskPage : ContentPage
{
    private List<CommentaireEntry> _commentEntries = new();
    private List<SousTacheEntry> _sousTacheEntries = new();


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
        LoadSousTaches(); 

    }

    protected override async void OnDisappearing()
    {
        base.OnDisappearing();

        // Passer les commentaires au ViewModel
        TransferCommentairesToViewModel();
        TransferSousTachesToViewModel();
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
        await Task.Delay(100); //pârce que sinon ça pete un max
        TransferCommentairesToViewModel();
        TransferSousTachesToViewModel();


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


    private void OnAjouterSousTacheClicked(object sender, EventArgs e)
    {
        var titreEntry = new Entry { Placeholder = "Titre de la sous-tâche" };

        var statutPicker = new Picker { Title = "Statut" };
        statutPicker.ItemsSource = _viewModel.SousTacheStatuts;
        statutPicker.SelectedIndex = 0;

        var echeancePicker = new DatePicker
        {
            MinimumDate = DateTime.Today,
            Format = "dd/MM/yyyy"
        };

        var deleteButton = new Button
        {
            Text = "Supprimer",
            BackgroundColor = Colors.Red
        };
        deleteButton.Clicked += OnDeleteSousTacheClicked;

        var sousTacheEntry = new SousTacheEntry
        {
            TitreEntry = titreEntry,
            StatutPicker = statutPicker,
            EcheancePicker = echeancePicker,
            DeleteButton = deleteButton
        };

        _sousTacheEntries.Add(sousTacheEntry);

        // Ajout à l'interface avec labels
        SousTachesLayout.Children.Add(new Label { Text = "Titre:" });
        SousTachesLayout.Children.Add(titreEntry);
        SousTachesLayout.Children.Add(new Label { Text = "Statut:" });
        SousTachesLayout.Children.Add(statutPicker);
        SousTachesLayout.Children.Add(new Label { Text = "Échéance:" });
        SousTachesLayout.Children.Add(echeancePicker);
        SousTachesLayout.Children.Add(deleteButton);
    }

    private void OnDeleteSousTacheClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var entryToDelete = _sousTacheEntries.FirstOrDefault(x => x.DeleteButton == button);

        if (entryToDelete != null)
        {
            // Si c'est une sous-tâche existante, la marquer pour suppression
            if (entryToDelete.TitreEntry.BindingContext is SousTache existingSousTache)
            {
                _viewModel.SousTachesToDelete.Add(existingSousTache);
            }

            // Supprimer tous les contrôles associés
            var index = SousTachesLayout.Children.IndexOf(entryToDelete.TitreEntry);
            // Supprimer les labels et contrôles (en ordre inverse)
            for (int i = 0; i < 7; i++) // 7 éléments par sous-tâche (3 labels + 3 contrôles + bouton)
            {
                if (index - i >= 0)
                {
                    SousTachesLayout.Children.RemoveAt(index - i);
                }
            }

            _sousTacheEntries.Remove(entryToDelete);
            TransferSousTachesToViewModel();
        }
    }

    private void LoadSousTaches()
    {
        SousTachesLayout.Children.Clear();
        _sousTacheEntries.Clear();

        if (_viewModel.Task?.SousTaches != null)
        {
            foreach (var sousTache in _viewModel.Task.SousTaches)
            {
                var titreEntry = new Entry { Text = sousTache.Titre };
                titreEntry.BindingContext = sousTache;

                var statutPicker = new Picker { ItemsSource = _viewModel.SousTacheStatuts };
                statutPicker.SelectedItem = sousTache.Statut;

                var echeancePicker = new DatePicker
                {
                    Date = sousTache.Echeance ?? DateTime.Today,
                    Format = "dd/MM/yyyy"
                };

                var deleteButton = new Button
                {
                    Text = "Supprimer",
                    BackgroundColor = Colors.Red
                };
                deleteButton.Clicked += OnDeleteSousTacheClicked;

                var entry = new SousTacheEntry
                {
                    TitreEntry = titreEntry,
                    StatutPicker = statutPicker,
                    EcheancePicker = echeancePicker,
                    DeleteButton = deleteButton
                };

                _sousTacheEntries.Add(entry);

                // Ajout à l'interface
                SousTachesLayout.Children.Add(new Label { Text = "Titre:" });
                SousTachesLayout.Children.Add(titreEntry);
                SousTachesLayout.Children.Add(new Label { Text = "Statut:" });
                SousTachesLayout.Children.Add(statutPicker);
                SousTachesLayout.Children.Add(new Label { Text = "Échéance:" });
                SousTachesLayout.Children.Add(echeancePicker);
                SousTachesLayout.Children.Add(deleteButton);
            }
        }
    }

    private void TransferSousTachesToViewModel()
    {
        if (_viewModel.Task != null)
        {
            // Mettre à jour les sous-tâches existantes
            foreach (var entry in _sousTacheEntries.Where(e => e.TitreEntry.BindingContext is SousTache))
            {
                var sousTache = (SousTache)entry.TitreEntry.BindingContext;
                sousTache.Titre = entry.TitreEntry.Text;
                sousTache.Statut = entry.StatutPicker.SelectedItem?.ToString() ?? "À faire";
                sousTache.Echeance = entry.EcheancePicker.Date;
            }

            // Ajouter les nouvelles sous-tâches
            var nouvellesSousTaches = _sousTacheEntries
                .Where(e => e.TitreEntry.BindingContext == null && !string.IsNullOrWhiteSpace(e.TitreEntry.Text))
                .Select(e => new SousTache
                {
                    Titre = e.TitreEntry.Text,
                    Statut = e.StatutPicker.SelectedItem?.ToString() ?? "À faire",
                    Echeance = e.EcheancePicker.Date,
                    TacheId = _viewModel.Task.Id
                }).ToList();

            if (_viewModel.Task.SousTaches == null)
            {
                _viewModel.Task.SousTaches = nouvellesSousTaches;
            }
            else
            {
                foreach (var nouvelle in nouvellesSousTaches)
                {
                    _viewModel.Task.SousTaches.Add(nouvelle);
                }
            }
        }
    }

}