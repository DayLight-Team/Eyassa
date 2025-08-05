using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using Eyassa.Models;

namespace Eyassa.Managers;

internal class SettingsManager
{
    internal static List<SettingNode> Nodes { get; } = new();
    
    
    internal static Dictionary<Player, List<int>> SentIds { get; } = new();
    internal static Dictionary<Player, List<int>> SentNodes { get; } = new();
    internal static void SendToPlayer(Player? player)
    {
        
        if(player == null)
            return;
        if(!SentIds.ContainsKey(player))
            SentIds[player] = new();
        if(!SentNodes.ContainsKey(player))
            SentNodes[player] = new();
        
        List<SettingBase> settings = [];
        foreach (var node in Nodes.OrderByDescending(x=>x.GetPriority(player)))
        {
            
            
            settings.Add(new HeaderSetting(node.HeaderId, node.GetHeaderName(player), node.GetHeaderHintDescription(player), node.GetHeaderPadding(player)));
            var first = node.Options.Where(x => !SentIds[player].Contains(x.Id));
            foreach (var option in first)
            {
                option.OnFirstUpdate(player);
            }
            var options = node.Options.Where(x => x.IsVisibleToPlayer(player));
            settings.AddRange(options.Select(x=>x.BuildBase(player)));
            if (!SentNodes[player].Contains(node.HeaderId))
            {
                node.OnFirstUpdate(player);
                SentNodes[player].Add(node.HeaderId); 
            }
        }
        Log.Debug($"Sending {settings.Count} settings to {player.Nickname}");
        SettingBase.SendToPlayer(player, settings);
    }
}