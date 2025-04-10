using taskMasterProjet.ViewModels;

namespace taskMasterProjet.Views;

public partial class HomePage : ContentPage
{

    public HomePage(HomeViewModel viewModel)  // Modifié pour injecter le ViewModel
    {
        try
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"HomePage ERROR: {ex}");
            throw;
        }
    }

    //public HomePage()
    //{
    //    InitializeComponent();
    //    BindingContext = new HomeViewModel();
    //}
}
