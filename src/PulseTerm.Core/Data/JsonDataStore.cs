using System.Text.Json;

namespace PulseTerm.Core.Data;

public class JsonDataStore
{
    private readonly Dictionary<string, SemaphoreSlim> _fileLocks = new();
    private readonly SemaphoreSlim _dictionaryLock = new(1, 1);
    
    private readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public async Task<T?> LoadAsync<T>(string filePath) where T : class, new()
    {
        if (!File.Exists(filePath))
        {
            return new T();
        }

        var fileLock = await GetFileLockAsync(filePath);
        await fileLock.WaitAsync();
        
        try
        {
            await using var stream = new FileStream(
                filePath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read);
                
            return await JsonSerializer.DeserializeAsync<T>(stream, _options);
        }
        catch (JsonException)
        {
            throw;
        }
        finally
        {
            fileLock.Release();
        }
    }

    public async Task SaveAsync<T>(string filePath, T data)
    {
        var directory = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var fileLock = await GetFileLockAsync(filePath);
        await fileLock.WaitAsync();
        
        try
        {
            for (int attempt = 0; attempt < 3; attempt++)
            {
                try
                {
                    await using var stream = new FileStream(
                        filePath,
                        FileMode.Create,
                        FileAccess.Write,
                        FileShare.None);
                        
                    await JsonSerializer.SerializeAsync(stream, data, _options);
                    return;
                }
                catch (IOException) when (attempt < 2)
                {
                    await Task.Delay((int)Math.Pow(2, attempt) * 100);
                }
            }
        }
        finally
        {
            fileLock.Release();
        }
    }

    private async Task<SemaphoreSlim> GetFileLockAsync(string filePath)
    {
        await _dictionaryLock.WaitAsync();
        try
        {
            if (!_fileLocks.TryGetValue(filePath, out var fileLock))
            {
                fileLock = new SemaphoreSlim(1, 1);
                _fileLocks[filePath] = fileLock;
            }
            return fileLock;
        }
        finally
        {
            _dictionaryLock.Release();
        }
    }
}
