using Renci.SshNet;
using Renci.SshNet.Common;

namespace PulseTerm.Core.Ssh;

/// <summary>
/// Wrapper interface for SSH.NET's SshClient.
///
/// Design Note: While SSH.NET 2025.1.0 now provides ISshClient interface (resolving issue #890),
/// this wrapper is retained for the following reasons:
/// 1. Architectural consistency - maintains uniform abstraction layer across all SSH operations
/// 2. Future flexibility - allows potential migration to alternative SSH libraries without changing consuming code
/// 3. Enhanced testability - provides additional control over mock behavior in unit tests
/// 4. Custom extensions - enables adding PulseTerm-specific functionality without modifying SSH.NET types
///
/// If you're evaluating whether to use ISshClient directly or this wrapper in new code,
/// consider the trade-offs between directness (ISshClient) and flexibility (wrapper).
/// </summary>
public interface ISshClientWrapper : IDisposable
{
    bool IsConnected { get; }
    TimeSpan ConnectionTimeout { get; set; }

    void Connect();
    Task ConnectAsync(CancellationToken cancellationToken);
    void Disconnect();

    IShellStreamWrapper CreateShellStream(
        string terminalName,
        uint columns,
        uint rows,
        uint width,
        uint height,
        int bufferSize,
        IDictionary<TerminalModes, uint>? terminalModeValues = null);

    void AddForwardedPort(ForwardedPort port);
    void RemoveForwardedPort(ForwardedPort port);
}
