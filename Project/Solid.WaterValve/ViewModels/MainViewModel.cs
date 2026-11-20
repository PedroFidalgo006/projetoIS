using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;

namespace Solid.WaterValve.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    public partial int Humidity { get; set; }

    [ObservableProperty]
    public partial int Threshold { get; set; } = 40;

    [ObservableProperty]
    public partial bool ValveIsOn { get; set; }

    [ObservableProperty]
    public partial string Status { get; set; } = "Waiting for notifications";

    [ObservableProperty]
    public partial string LastNotification { get; set; } = "--";

    [ObservableProperty]
    public partial string ValveState { get; set; } = "OFF";

    [ObservableProperty]
    public partial string ValveColor { get; set; } = "#78909C";

    public ObservableCollection<string> Notifications { get; } = [];

    [RelayCommand]
    private void ClearNotifications()
    {
        Notifications.Clear();
        LastNotification = "--";
    }

    partial void OnThresholdChanged(int value)
    {
        ApplyValveLogic();
    }

    partial void OnHumidityChanged(int value)
    {
        ApplyValveLogic();
    }

    public void AddNotification(int humidity)
    {
        Humidity = humidity;
        var message = $"{DateTime.Now:HH:mm:ss} - Humidity: {humidity}%";
        Notifications.Insert(0, message);
        LastNotification = message;
        Status = "Notification received";
    }

    private void ApplyValveLogic()
    {
        ValveIsOn = Humidity < Threshold;
        ValveState = ValveIsOn ? "ON" : "OFF";
        ValveColor = ValveIsOn ? "#2A9D8F" : "#78909C";
    }
}
