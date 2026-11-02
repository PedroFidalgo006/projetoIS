using CommunityToolkit.Mvvm.ComponentModel;

namespace Solid.MoistureSensor.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    public partial string Greeting { get; set; } = "Welcome to Avalonia!";
}
