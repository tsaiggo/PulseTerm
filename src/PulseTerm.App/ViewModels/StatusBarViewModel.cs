using PulseTerm.Core.Resources;
using ReactiveUI;

namespace PulseTerm.App.ViewModels;

public class StatusBarViewModel : ReactiveObject
{
    private string _statusText;
    private string _connectionInfo;

    public StatusBarViewModel()
    {
        _statusText = Strings.Ready;
        _connectionInfo = string.Empty;
    }

    public string StatusText
    {
        get => _statusText;
        set => this.RaiseAndSetIfChanged(ref _statusText, value);
    }

    public string ConnectionInfo
    {
        get => _connectionInfo;
        set => this.RaiseAndSetIfChanged(ref _connectionInfo, value);
    }
}
