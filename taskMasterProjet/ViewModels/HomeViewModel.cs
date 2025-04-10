using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using taskMasterProjet.Views;

namespace taskMasterProjet.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    [RelayCommand]
    private async Task GoToLogin()
    {
        await Shell.Current.GoToAsync("//LoginPage");
    }

    [RelayCommand]
    private async Task GoToCreateAccount()
    {
        await Shell.Current.GoToAsync("//CreateAccountPage");
    }
}