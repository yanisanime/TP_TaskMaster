using taskMasterProjet.ViewModels;


namespace taskMasterProjet.Views.Components;

public partial class TaskItemView : ContentView
{
    public static readonly BindableProperty DashboardViewModelProperty =
    BindableProperty.Create(nameof(DashboardViewModel), typeof(DashboardViewModel), typeof(TaskItemView));

    public TaskItemView()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty TaskProperty =
           BindableProperty.Create(nameof(Task), typeof(Tache), typeof(TaskItemView), null);

    public Tache Task
    {
        get => (Tache)GetValue(TaskProperty);
        set
        {
            SetValue(TaskProperty, value);
            BindingContext = value; // pour afficher les données dans le XAML
        }
    }
    public DashboardViewModel DashboardViewModel
    {
        get => (DashboardViewModel)GetValue(DashboardViewModelProperty);
        set => SetValue(DashboardViewModelProperty, value);
    }


}
