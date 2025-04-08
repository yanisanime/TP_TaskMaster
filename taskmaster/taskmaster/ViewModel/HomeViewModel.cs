using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using taskmaster.Views;

namespace taskmaster.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    [RelayCommand]
    private async Task GoToLogin()
    {
        await Shell.Current.GoToAsync(nameof(LoginPage));
    }

    [RelayCommand]
    private async Task GoToCreateAccount()
    {
        await Shell.Current.GoToAsync(nameof(CreateAccountPage));
    }
}