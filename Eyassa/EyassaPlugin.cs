using System;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Loader;
using Eyassa.Interfaces;
using Eyassa.Managers;
using Eyassa.Models;
using MEC;
using Player = Exiled.Events.Handlers.Player;

namespace Eyassa;

public class EyassaPlugin : Plugin<Configs, EyassaTranslations>
{
        public override string Name { get; } = "Even Yet another server-specific settings API wrapper for EXILED";

    public override string Author { get; } = "Tili :3";

    public override PluginPriority Priority { get; } = PluginPriority.First;

    public override Version Version { get; } = new(1, 0, 0);
    public EyassaPlugin Instance { get; private set; }
    
    public override void OnEnabled()
    {
        Player.Verified += Verified;
        Instance = this;
        base.OnEnabled();
    }

    private void Verified(VerifiedEventArgs ev)
    {
        SettingsManager.SendToPlayer(ev.Player);
    }
    
}

