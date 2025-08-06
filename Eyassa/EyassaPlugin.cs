using System;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Loader;
using Eyassa.Interfaces;
using Eyassa.Managers;
using Eyassa.Models;
using HarmonyLib;
using MEC;
using Player = Exiled.Events.Handlers.Player;

namespace Eyassa;

public class EyassaPlugin : Plugin<Configs, EyassaTranslations>
{
    public override string Name { get; } = "Eyassa";
    public override Version Version { get; } = new(1, 2, 0);
    public override string Author { get; } = "Tili :3";

    public override PluginPriority Priority { get; } = PluginPriority.First;

    public static EyassaPlugin Instance { get; private set; }
    private Harmony Harmony { get; } = new("com.tili.eyassa");
    public override void OnEnabled()
    {
        Instance = this;
        Player.Verified += Verified;
        Player.Joined += Joined;
        Harmony.PatchAll();
        Timing.RunCoroutine(UpdateCoroutine());
        base.OnEnabled();
    }

    private static void Joined(JoinedEventArgs ev)
    {
        JoiningPlayers.Add(ev.Player);
    }

    private static void Verified(VerifiedEventArgs ev)
    {
        foreach (var node in SettingsManager.Nodes)
        {
            node.CheckForUpdateRequirement(ev.Player);
            node.Options.ForEach(x=>x.CheckForUpdateRequirement(ev.Player));
        }

        SettingsManager.UpdatePlayer(ev.Player);
        JoiningPlayers.Remove(ev.Player);

    }

    private static List<Exiled.API.Features.Player> JoiningPlayers { get; } = new();
    private static IEnumerator<float> UpdateCoroutine()
    {
        while (true)
        {
            foreach (var player in Exiled.API.Features.Player.List)
            {
                if(JoiningPlayers.Contains(player))
                    continue;
                var sendSettings = false;
                
                foreach (var node in SettingsManager.Nodes)
                {
                    if(node.CheckForUpdateRequirement(player))
                        sendSettings = true;
                    node.UpdateNode(player);
                    node.UpdateOptions(player);
                }
                if(sendSettings)
                    SettingsManager.UpdatePlayer(player);
            }
            yield return Timing.WaitForSeconds(0.5f);

        }
    }
}

