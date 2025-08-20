using Exiled.API.Features;
using LabApi.Features.Console;
using Newtonsoft.Json;
using Random = UnityEngine.Random;

namespace Eyassa.Managers;

public class IdManager
{
    internal static IdManager Instance { get; } = new IdManager();

    internal readonly static HashSet<int> UsedNumbers = new();
    private static string FolderPath => Path.Combine(Paths.Configs, "EyassaCache");
    private static string FilePath => Path.Combine(FolderPath, $"{Server.Port}.json");

    private Dictionary<string, int>? _idMap;
    private IdManager()
    {
        Load();
    }
    private int GetSettingId(string customId)
    {
        if (_idMap == null)
        {
            Log.Error("IdMap is null (how tf??)");
            return -1;
        }
        if (_idMap.TryGetValue(customId, out int id))
        {
            return id;
        }

        var nextRandom = GetNextId();
        SetSettingId(customId, nextRandom);
        return nextRandom;
    }

    private void SetSettingId(string customId, int id)
    {
        if (_idMap == null)
        {
            Log.Error("IdMap is null");
            return;
        }
        _idMap[customId] = id;
        Save();
    }

    private void Load()
    {
        if(!Directory.Exists(FolderPath))
            Directory.CreateDirectory(FolderPath);

        if (File.Exists(FilePath))
        {
            var json = File.ReadAllText(FilePath);
            _idMap = JsonConvert.DeserializeObject<Dictionary<string, int>>(json)
                     ?? new Dictionary<string, int>();
        }
        else
        {
            _idMap = new Dictionary<string, int>();
        }
    }

    private void Save()
    {
        var json = JsonConvert.SerializeObject(_idMap, Formatting.Indented);
        File.WriteAllText(FilePath, json);
    }

    internal int GetId(string customId)
    {
        if (customId == "")
            throw new Exception("CustomId is empty");
        var id = GetSettingId(customId);
        return id;
    }
    internal static int GetNextId()
    {
        var randomNumber = Random.Range(0, int.MaxValue);

        while (UsedNumbers.Contains(randomNumber))
            randomNumber = Random.Range(0, int.MaxValue);
        UsedNumbers.Add(randomNumber);
        return randomNumber;
    }
    
}