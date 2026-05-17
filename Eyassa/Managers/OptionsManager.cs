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
    internal static Dictionary<Player, HashSet<int>> SentIds { get; } = new();
    private static void SendToPlayer(Player? player)
    {
        if(player == null)
            return;
        if(!SentIds.TryGetValue(player, out var sentIds))
        {
            sentIds = [];
            SentIds[player] = sentIds;
        }
        
        List<IOption> settings = [];
        foreach (var node in Nodes.OrderByDescending(x=>x.Priority))
        {
            if (!node.IsVisibleToPlayer(player))
                continue;

            foreach (var option in node.Options)
            {
                if (sentIds.Add(option.Id))
                {
                    try
                    {
                        option.OnFirstUpdate(player);
                    }
                    catch (Exception e)
                    {
                        Log.Error(e);
                    }
                }

                if (option.IsVisibleToPlayer(player))
                    settings.Add(option);
            }
        }
        
        Log.Debug($"Sending {settings.Count} settings to {player.Nickname}");
        var selectors = new List<SettingBase>(settings.Count);
        foreach (var setting in settings)
            selectors.Add(GetSelector(setting, player));
        SettingBase.SendToPlayer(player, selectors);
        foreach (var setting in settings)
            setting.OnSentSettingInternal(player);
    }

    private static void SendAll(Player? player)
    {
        if(player == null)
            return;
        if(!SentIds.TryGetValue(player, out var sentIds))
        {
            sentIds = [];
            SentIds[player] = sentIds;
        }
        List<SettingBase> settings = [];
        foreach (var node in Nodes)
        {
            foreach (var option in node.Options)
            {
                if (sentIds.Add(option.Id))
                {
                    try
                    {
                        option.OnFirstUpdate(player);
                    }
                    catch (Exception e)
                    {
                        Log.Error(e);
                    }
                }

                if (option.SendOnJoin)
                    settings.Add(GetSelector(option, player));
            }
        }
        Log.Debug($"OnJoined: Sending {settings.Count} settings to {player.Nickname}");
        SettingBase.SendToPlayer(player, settings);
    }
    private static SettingBase GetSelector(IOption arg, Player player)
    {
        try
        {
            var setting = arg.BuildBase(player);
            arg.RememberBuiltSetting(player, setting);
            return setting;
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

    private static float SettingUpdateInterval => Math.Max(0.1f, EyassaPlugin.Instance?.Config.SettingUpdateInterval ?? 0.5f);
    private static float VisibilityCheckInterval => Math.Max(SettingUpdateInterval, EyassaPlugin.Instance?.Config.VisibilityCheckInterval ?? 2f);

    private static IEnumerator<float> SettingUpdater(Exiled.API.Features.Player player)
    {
        yield return Timing.WaitForSeconds(1f);
        SendAll(player);
        var visibilityCheckDelay = 0f;
        while (player.IsConnected)
        {
            var updateInterval = SettingUpdateInterval;
            yield return Timing.WaitForSeconds(updateInterval);
            try
            {
                var sendSettings = false;
                visibilityCheckDelay -= updateInterval;
                var checkVisibility = visibilityCheckDelay <= 0f;
                if (checkVisibility)
                    visibilityCheckDelay = VisibilityCheckInterval;

                foreach (var node in Nodes)
                {
                    if (checkVisibility && node.CheckSendRequired(player))
                        sendSettings = true;

                    foreach (var option in node.Options)
                        option.UpdateOption(player);
                }
                if(sendSettings)
                    SendToPlayer(player);

            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        SentIds.Remove(player);
        foreach (var node in Nodes)
            node.ForgetPlayer(player);

    }

}
