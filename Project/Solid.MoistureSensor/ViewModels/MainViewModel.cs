using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Avalonia.Threading;
using System;

namespace Solid.MoistureSensor.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    public partial int Humidity { get; set; }

    [ObservableProperty]
    public partial bool IsRunning { get; set; }

    [ObservableProperty]
    public partial string Status { get; set; } = "Sensor stopped";

    [ObservableProperty]
    public partial string LastPublished { get; set; } = "--";

    [ObservableProperty]
    public partial string SensorButtonText { get; set; } = "Start sensor";

    [ObservableProperty]
    public partial string ConnectionText { get; set; } = "Offline";

    private readonly Random _random = new();
    private readonly DispatcherTimer _timer = new()
    {
        Interval = TimeSpan.FromSeconds(1)
    };

    public MainViewModel()
    {
        _timer.Tick += OnTimerTick;
    }

    [RelayCommand]
    private void ToggleSensor()
    {
        IsRunning = !IsRunning;

        if (IsRunning)
        {
            Status = "Publishing to SOLID";
            SensorButtonText = "Stop sensor";
            ConnectionText = "Online";
            _timer.Start();
        }
        else
        {
            Status = "Sensor stopped";
            SensorButtonText = "Start sensor";
            ConnectionText = "Offline";
            _timer.Stop();
        }
    }

    private void OnTimerTick(object? sender, EventArgs e)
    {
        Humidity = _random.Next(0, 101);
        LastPublished = DateTime.Now.ToString("HH:mm:ss");
    }
}
