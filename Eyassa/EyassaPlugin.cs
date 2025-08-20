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
using UnityEngine;
using Player = Exiled.Events.Handlers.Player;

namespace Eyassa;

public class EyassaPlugin : Plugin<Configs, EyassaTranslations>
{
    public override string Name { get; } = "Eyassa";
    public override Version Version { get; } = new(1, 0, 0);
    public override string Author { get; } = "Tili :3";

    public override PluginPriority Priority { get; } = PluginPriority.First;

    public static EyassaPlugin? Instance { get; private set; }
    private Harmony Harmony { get; } = new("com.tili.eyassa");

    public bool IsLoaded { get; private set; } = false;
    public override void OnEnabled()
    {
        Instance = this;
        Player.Verified += OptionsManager.OnVerified;
        IsLoaded = true;
        Harmony.PatchAll();
        base.OnEnabled();
    }
    


}

