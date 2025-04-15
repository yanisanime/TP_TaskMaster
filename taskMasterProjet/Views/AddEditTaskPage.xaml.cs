using Microsoft.Maui.Controls;
using taskMasterProjet.ViewModels;
using taskMasterProjet.Services; //pour la classe de gestion de com

namespace taskMasterProjet.Views;



public partial class AddEditTaskPage : ContentPage
{
    private List<CommentaireEntry> _commentEntries = new();

    private List<SousTacheEntry> _sousTacheEntries = new();

    public AddEditTaskPage(AddEditTaskViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private void OnAjouterCommentaireClicked(object sender, EventArgs e)
    {
        var entry = new Entry { Placeholder = "Écrire un commentaire..." };
        var deleteButton = new Button { Text = "Supprimer",
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
        TransferSousTachesToViewModel();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is AddEditTaskViewModel viewModel)
        {
            try
            {
                await viewModel.InitializeDataAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erreur d'initialisation", ex.Message, "OK");
            }
        }
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

    /// Appelée lorsque le bouton "Enregistrer" est cliqué pour pouvoir faire des truc avec les commentaire avant de push vers le model
    private async void OnSaveClicked(object sender, EventArgs e)
    {
        TransferCommentairesToViewModel();
        TransferSousTachesToViewModel();

        if (BindingContext is AddEditTaskViewModel vm)
        {
            await vm.SaveCommand.ExecuteAsync(null);
        }
    }






    private void OnAjouterSousTacheClicked(object sender, EventArgs e)
    {
        // Créer les contrôles pour une sous-tâche
        var titreEntry = new Entry { Placeholder = "Titre de la sous-tâche" };
        var statutPicker = new Picker { Title = "Statut" };
        statutPicker.ItemsSource = (BindingContext as AddEditTaskViewModel)?.SousTacheStatuts;
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

        // Créer un objet SousTacheEntry
        var sousTacheEntry = new SousTacheEntry
        {
            TitreEntry = titreEntry,
            StatutPicker = statutPicker,
            EcheancePicker = echeancePicker,
            DeleteButton = deleteButton
        };

        // Ajouter à la liste
        _sousTacheEntries.Add(sousTacheEntry);

        // Ajouter à l'interface
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
        var sousTacheEntryToDelete = _sousTacheEntries.FirstOrDefault(entry => entry.DeleteButton == button);

        if (sousTacheEntryToDelete != null)
        {
            // Supprimer tous les contrôles associés
            SousTachesLayout.Children.Remove(sousTacheEntryToDelete.TitreEntry);
            SousTachesLayout.Children.Remove(sousTacheEntryToDelete.StatutPicker);
            SousTachesLayout.Children.Remove(sousTacheEntryToDelete.EcheancePicker);
            SousTachesLayout.Children.Remove(sousTacheEntryToDelete.DeleteButton);

            // Supprimer les labels associés (on suppose qu'ils sont dans l'ordre)
            var index = SousTachesLayout.Children.IndexOf(sousTacheEntryToDelete.TitreEntry);
            for (int i = 0; i < 3; i++) // 3 labels avant les contrôles
            {
                if (index - i - 1 >= 0)
                {
                    SousTachesLayout.Children.RemoveAt(index - i - 1);
                }
            }

            _sousTacheEntries.Remove(sousTacheEntryToDelete);
            TransferSousTachesToViewModel();
        }
    }

    public void TransferSousTachesToViewModel()
    {
        if (BindingContext is AddEditTaskViewModel vm)
        {
            vm.SousTachesTemporaires = _sousTacheEntries
                .Where(e => !string.IsNullOrWhiteSpace(e.TitreEntry.Text))
                .Select(e => new SousTache
                {
                    Titre = e.TitreEntry.Text,
                    Statut = e.StatutPicker.SelectedItem?.ToString() ?? "À faire",
                    Echeance = e.EcheancePicker.Date
                })
                .ToList();
        }
    }

}
