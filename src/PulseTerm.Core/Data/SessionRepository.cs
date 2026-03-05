using PulseTerm.Core.Models;

namespace PulseTerm.Core.Data;

public class SessionRepository : ISessionRepository
{
    private readonly JsonDataStore _dataStore;
    private readonly string _dataPath;

    public SessionRepository(JsonDataStore dataStore, string? dataPath = null)
    {
        _dataStore = dataStore;
        
        if (string.IsNullOrEmpty(dataPath))
        {
            var userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            _dataPath = Path.Combine(userProfile, ".pulseterm", "sessions.json");
        }
        else
        {
            _dataPath = dataPath;
        }
    }

    public async Task<List<ServerGroup>> GetAllGroupsAsync()
    {
        var data = await LoadDataAsync();
        return data.Groups;
    }

    public async Task<SessionProfile?> GetSessionAsync(Guid id)
    {
        var data = await LoadDataAsync();
        return data.Sessions.FirstOrDefault(s => s.Id == id);
    }

    public async Task SaveSessionAsync(SessionProfile session)
    {
        var data = await LoadDataAsync();
        var existingIndex = data.Sessions.FindIndex(s => s.Id == session.Id);
        
        if (existingIndex >= 0)
        {
            data.Sessions[existingIndex] = session;
        }
        else
        {
            data.Sessions.Add(session);
        }
        
        await _dataStore.SaveAsync(_dataPath, data);
    }

    public async Task DeleteSessionAsync(Guid id)
    {
        var data = await LoadDataAsync();
        data.Sessions.RemoveAll(s => s.Id == id);
        
        foreach (var group in data.Groups)
        {
            group.Sessions.Remove(id);
        }
        
        await _dataStore.SaveAsync(_dataPath, data);
    }

    public async Task SaveGroupAsync(ServerGroup group)
    {
        var data = await LoadDataAsync();
        var existingIndex = data.Groups.FindIndex(g => g.Id == group.Id);
        
        if (existingIndex >= 0)
        {
            data.Groups[existingIndex] = group;
        }
        else
        {
            data.Groups.Add(group);
        }
        
        await _dataStore.SaveAsync(_dataPath, data);
    }

    public async Task DeleteGroupAsync(Guid id)
    {
        var data = await LoadDataAsync();
        data.Groups.RemoveAll(g => g.Id == id);
        
        foreach (var session in data.Sessions.Where(s => s.GroupId == id))
        {
            session.GroupId = null;
        }
        
        await _dataStore.SaveAsync(_dataPath, data);
    }

    private async Task<SessionData> LoadDataAsync()
    {
        return await _dataStore.LoadAsync<SessionData>(_dataPath) ?? new SessionData();
    }
}

internal class SessionData
{
    public List<ServerGroup> Groups { get; set; } = new();
    public List<SessionProfile> Sessions { get; set; } = new();
}
