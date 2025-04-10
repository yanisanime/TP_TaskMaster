using taskMasterProjet.ViewModels;

namespace taskMasterProjet.Views;

public partial class CreateAccountPage : ContentPage
{
    public CreateAccountPage(CreateAccountViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}