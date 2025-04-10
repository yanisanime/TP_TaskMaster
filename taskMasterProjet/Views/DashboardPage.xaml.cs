using taskMasterProjet.ViewModels;

namespace taskMasterProjet.Views;

public partial class DashboardPage : ContentPage
{
    public DashboardPage( DashboardViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}