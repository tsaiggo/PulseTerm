using Renci.SshNet;
using Renci.SshNet.Sftp;

namespace PulseTerm.Core.Ssh;

public class SftpClientWrapper : ISftpClientWrapper
{
    private readonly SftpClient _client;

    public SftpClientWrapper(SftpClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public bool IsConnected => _client.IsConnected;

    public TimeSpan ConnectionTimeout
    {
        get => _client.ConnectionInfo.Timeout;
        set => _client.ConnectionInfo.Timeout = value;
    }

    public string WorkingDirectory => _client.WorkingDirectory;

    public void Connect()
    {
        _client.Connect();
    }

    public Task ConnectAsync(CancellationToken cancellationToken)
    {
        return _client.ConnectAsync(cancellationToken);
    }

    public void Disconnect()
    {
        _client.Disconnect();
    }

    public IEnumerable<ISftpFile> ListDirectory(string path)
    {
        return _client.ListDirectory(path);
    }

    public Task<IEnumerable<ISftpFile>> ListDirectoryAsync(string path, CancellationToken cancellationToken)
    {
        return Task.Run(() => _client.ListDirectory(path), cancellationToken);
    }

    public void UploadFile(Stream input, string path, bool canOverride = true)
    {
        _client.UploadFile(input, path, canOverride);
    }

    public Task UploadAsync(Stream input, string path, Action<ulong>? uploadCallback = null, CancellationToken cancellationToken = default)
    {
        return Task.Run(() => _client.UploadFile(input, path, true, uploadCallback), cancellationToken);
    }

    public void DownloadFile(string path, Stream output)
    {
        _client.DownloadFile(path, output);
    }

    public Task DownloadAsync(string path, Stream output, Action<ulong>? downloadCallback = null, CancellationToken cancellationToken = default)
    {
        return Task.Run(() => _client.DownloadFile(path, output, downloadCallback), cancellationToken);
    }

    public void Dispose()
    {
        _client?.Dispose();
    }
}
