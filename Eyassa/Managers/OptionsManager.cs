using System.Diagnostics;
using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using Exiled.Events.EventArgs.Player;
using Eyassa.Interfaces;
using Eyassa.Models;
using MEC;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Eyassa.Managers;

public class OptionsManager
{
    internal static List<OptionNode> Nodes { get; } = new();
    internal static Dictionary<Player, List<int>> SentIds { get; } = new();
    private static void SendToPlayer(Player? player)
    {
        if(player == null)
            return;
        if(!SentIds.ContainsKey(player))
            SentIds[player] = [];
        
        List<IOption> settings = [];
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
            settings.AddRange(node.Options.Where(x => x.IsVisibleToPlayer(player)));
        }
        
        Log.Debug($"Sending {settings.Count} settings to {player.Nickname}");
        SettingBase.SendToPlayer(player, settings.Select(x => GetSelector(x, player)));
        settings.ForEach(x=>x.OnSentSettingInternal(player));
    }

    private static void SendAll(Player? player)
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
            return new TextInputSetting(IdManager.GetNextId(), $"Error: {e}");
        }

    }
    public static void OnVerified(VerifiedEventArgs ev)
    {
        Timing.RunCoroutine(SettingUpdater(ev.Player));
    }
    private static IEnumerator<float> SettingUpdater(Exiled.API.Features.Player player)
    {
        yield return Timing.WaitForSeconds(1f);
        SendAll(player);
        while (player != null)
        {
            yield return Timing.WaitForSeconds(0.5f);
            try
            {
                var sendSettings = false;

                foreach (var node in Nodes)
                {
                    if (node.CheckSendRequired(player))
                        sendSettings = true;
                    node.Options.ForEach(x=>x.UpdateOption(player));
                }
                if(sendSettings)
                    SendToPlayer(player);

            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

    }

}