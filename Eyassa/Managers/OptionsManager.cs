using System.Diagnostics;
using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using Eyassa.Interfaces;
using Eyassa.Models;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Eyassa.Managers;

public class OptionsManager
{
    internal static List<OptionNode> Nodes { get; } = new();
    internal static Dictionary<Player, List<int>> SentIds { get; } = new();
    public static void UpdatePlayer(Player? player)
    {
        if(player == null)
            return;
        if(!SentIds.ContainsKey(player))
            SentIds[player] = [];
        
        List<SettingBase> settings = [];
        foreach (var node in Nodes.Where(x=>x.IsVisibleToPlayer(player)).OrderByDescending(x=>x.Priority))
        {
            var first = node.Options.Where(x => !SentIds[player].Contains(x.Id));
            foreach (var option in first)
            {
                SentIds[player].Add(option.Id);
                try
                {
                    option.OnFirstUpdate(player);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
            var options = node.Options.Where(x => x.IsVisibleToPlayer(player));
            settings.AddRange(options.Select(x=> GetSelector(x, player)));
        }
        Log.Debug($"Sending {settings.Count} settings to {player.Nickname}");
        SettingBase.SendToPlayer(player, settings);
    }

    public static void OnJoined(Player? player)
    {
        if(player == null)
            return;
        if(!SentIds.ContainsKey(player))
            SentIds[player] = [];
        List<SettingBase> settings = [];
        foreach (var node in Nodes)
        {
            foreach (var option in node.Options)
            {
                SentIds[player].Add(option.Id);
                try
                {
                    option.OnFirstUpdate(player);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
            var options = node.Options.Where(x => x.SendOnJoin);
            settings.AddRange(options.Select(x=> GetSelector(x, player)));
        }
        Log.Debug($"OnJoined: Sending {settings.Count} settings to {player.Nickname}");
        SettingBase.SendToPlayer(player, settings);
    }
    private static SettingBase GetSelector(IOption arg, Player player)
    {
        try
        {
            return arg.BuildBase(player);
        }
        catch (Exception e)
        {
            Log.Error(e);
            return new TextInputSetting(Random.Range(0, int.MaxValue), $"Error: {e}");
        }
    }
}