namespace PulseTerm.Core.Models;

public class KnownHost
{
    public string HostKey { get; set; } = string.Empty;
    
    public string Fingerprint { get; set; } = string.Empty;
    
    public string Algorithm { get; set; } = string.Empty;
    
    public DateTime FirstSeenAt { get; set; } = DateTime.UtcNow;
    
    public DateTime LastSeenAt { get; set; } = DateTime.UtcNow;
}
