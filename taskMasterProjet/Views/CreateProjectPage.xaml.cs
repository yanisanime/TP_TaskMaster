using taskMasterProjet.ViewModels;

namespace taskMasterProjet.Views;

public partial class CreateProjectPage : ContentPage
{
    public CreateProjectPage(CreateProjectViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}