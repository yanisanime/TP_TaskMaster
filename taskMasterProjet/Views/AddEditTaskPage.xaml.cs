using taskMasterProjet.ViewModels;


namespace taskMasterProjet.Views;


public partial class AddEditTaskPage :ContentPage
{

    public AddEditTaskPage(AddEditTaskViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

}



