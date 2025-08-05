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
    public override string Name { get; } = "Even Yet another server-specific settings API wrapper for EXILED";
    public override Version Version { get; } = new(1, 2, 0);
    public override string Author { get; } = "Tili :3";

    public override PluginPriority Priority { get; } = PluginPriority.First;

    public static EyassaPlugin Instance { get; private set; }
    
    public IdManager IdManager { get; } = new IdManager();

    private Harmony Harmony { get; } = new("com.tili.eyassa");
    public override void OnEnabled()
    {
        Instance = this;
        Harmony.PatchAll();
        Player.Verified += OnVerified;
        base.OnEnabled();
    }

    private static void OnVerified(VerifiedEventArgs ev)
    {
        SettingsManager.SendToPlayer(ev.Player);
    }
    
}

