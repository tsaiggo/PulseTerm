using DynamicData;
using Microsoft.Extensions.Logging;
using PulseTerm.Core.Models;

namespace PulseTerm.Core.Ssh;

public class SshConnectionService : ISshConnectionService
{
    private readonly ILogger<SshConnectionService>? _logger;
    private readonly Func<ISshClientWrapper> _clientFactory;
    private readonly SourceList<SshSession> _sessions = new();
    private readonly Dictionary<Guid, ISshClientWrapper> _clients = new();

    public SshConnectionService(Func<ISshClientWrapper> clientFactory, ILogger<SshConnectionService>? logger = null)
    {
        _clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
        _logger = logger;
    }

    public IObservableList<SshSession> Sessions => _sessions.AsObservableList();

    public async Task<SshSession> ConnectAsync(Models.ConnectionInfo connectionInfo, CancellationToken cancellationToken = default)
    {
        var session = new SshSession
        {
            ConnectionInfo = connectionInfo,
            Status = SessionStatus.Connecting
        };

        _sessions.Add(session);

        try
        {
            var client = _clientFactory();
            await client.ConnectAsync(cancellationToken);

            if (!client.IsConnected)
            {
                throw new InvalidOperationException("Client connection failed without exception");
            }

            _clients[session.SessionId] = client;

            session.Status = SessionStatus.Connected;
            session.ConnectedAt = DateTime.UtcNow;

            _logger?.LogInformation("SSH session {SessionId} connected to {Host}:{Port}",
                session.SessionId, connectionInfo.Host, connectionInfo.Port);

            return session;
        }
        catch (Exception ex)
        {
            session.Status = SessionStatus.Error;
            session.ErrorMessage = ex.Message;

            _logger?.LogError(ex, "Failed to connect SSH session {SessionId} to {Host}:{Port}",
                session.SessionId, connectionInfo.Host, connectionInfo.Port);

            throw;
        }
    }

    public async Task DisconnectAsync(Guid sessionId, CancellationToken cancellationToken = default)
    {
        var session = GetSession(sessionId);
        if (session == null)
        {
            throw new InvalidOperationException($"Session {sessionId} not found");
        }

        if (_clients.TryGetValue(sessionId, out var client))
        {
            await Task.Run(() =>
            {
                client.Disconnect();
                client.Dispose();
            }, cancellationToken);

            _clients.Remove(sessionId);
        }

        session.Status = SessionStatus.Disconnected;

        _logger?.LogInformation("SSH session {SessionId} disconnected", sessionId);
    }

    public SshSession? GetSession(Guid sessionId)
    {
        return _sessions.Items.FirstOrDefault(s => s.SessionId == sessionId);
    }
}
