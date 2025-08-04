using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using Eyassa.Interfaces;

namespace Eyassa;

public class SettingsManager
{
    public static List<IOption> Options { get; } = new();
    
    
    public static Dictionary<Player, List<int>> SentIds { get; } = new();
    public static void SendToPlayer(Player? player)
    {
        var seenSettings = Options.Where(x => x.IsVisibleToPlayer(player));
        

        if(!SentIds.ContainsKey(player))
            SentIds[player] = new();
        var alreadySendSettings = Options.Where(x => !SentIds[player].Contains(x.Id));
        foreach (var option in alreadySendSettings)
        {
            option.OnFirstUpdate(player);
        }
        var settings = seenSettings.Select(x => x.BuildBase(player)).ToList();
        SettingBase.SendToPlayer(player, settings);
        Log.Info($"Sent {settings.Count} to {player.Nickname}");
    }
}