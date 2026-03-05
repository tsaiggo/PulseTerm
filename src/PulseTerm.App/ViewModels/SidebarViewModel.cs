using System.Reactive;
using ReactiveUI;

namespace PulseTerm.App.ViewModels;

public class SidebarViewModel : ReactiveObject
{
    private string _quickConnectText;

    public SidebarViewModel()
    {
        _quickConnectText = string.Empty;

        QuickConnectCommand = ReactiveCommand.Create(() => { });
        SettingsCommand = ReactiveCommand.Create(() => { });
        NotificationsCommand = ReactiveCommand.Create(() => { });
    }

    public string QuickConnectText
    {
        get => _quickConnectText;
        set => this.RaiseAndSetIfChanged(ref _quickConnectText, value);
    }

    public ReactiveCommand<Unit, Unit> QuickConnectCommand { get; }
    public ReactiveCommand<Unit, Unit> SettingsCommand { get; }
    public ReactiveCommand<Unit, Unit> NotificationsCommand { get; }
}
