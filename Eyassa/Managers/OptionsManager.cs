using System.Diagnostics;
using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using Exiled.Events.EventArgs.Player;
using Eyassa.Interfaces;
using Eyassa.Models;
using MEC;
using UnityEngine;
using UserSettings.ServerSpecific;
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
        if(!SentIds.ContainsKey(player))
            SentIds[player] = [];
        
        List<IOption> settings = [];
        List<OptionNode> visibleNodes = [];
        foreach (var node in Nodes)
        {
            if (node.IsCurrentlyVisible(player))
                visibleNodes.Add(node);
        }

        visibleNodes.Sort((left, right) => right.Priority.CompareTo(left.Priority));

        foreach (var node in visibleNodes)
        {
            foreach (var option in node.Options)
            {
                if (!SentIds[player].Add(option.Id))
                    continue;

                try
                {
                    option.OnFirstUpdate(player);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }

            foreach (var option in node.Options)
            {
                if (option.IsCurrentlyVisible(player))
                    settings.Add(option);
            }
        }
        
        Log.Debug($"Sending {settings.Count} settings to {player.Nickname}");
        List<SettingBase> settingBases = [];
        foreach (var setting in settings)
        {
            settingBases.Add(GetSelector(setting, player));
        }

        SettingBase.SendToPlayer(player, settingBases);
        foreach (var setting in settings)
        {
            setting.Send(player);
        }
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
        //Scatter the update between random Frames
        yield return Timing.WaitForSeconds(Random.Range(1f, 1.5f));
        SendAll(player);
        while (player.IsConnected)
        {
            yield return Timing.WaitForSeconds(0.5f);
            try
            {

                if(!ServerSpecificSettingsSync.IsTabOpenForUser(player.ReferenceHub))
                    continue;
                
                Log.Debug("Sending settings");
                var sendSettings = false;

                foreach (var node in Nodes)
                {
                    if (node.CheckSendRequired(player))
                        sendSettings = true;

                    if (!node.IsCurrentlyVisible(player))
                        continue;

                    foreach (var option in node.Options)
                    {
                        if (option.IsCurrentlyVisible(player))
                            option.UpdateOption(player);
                    }
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
