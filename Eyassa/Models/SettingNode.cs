using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using Eyassa.Interfaces;
using Eyassa.Managers;
using LabApi.Features.Console;
using MEC;

namespace Eyassa.Models;

public abstract class SettingNode
{
    internal int HeaderId { get; } = IdManager.GetNextId();

    private HeaderSetting? _headerSetting;
    public abstract string GetHeaderName(Player player);
    public abstract string GetHeaderHintDescription(Player player);
    public virtual bool GetHeaderPadding(Player player) => false;
    public abstract List<IOption> Options { get; }
    public virtual int GetPriority(Player player) => 0;
    public void Register()
    {
        _headerSetting = new HeaderSetting(HeaderId, "Default");
        RegisterSettings();
        SettingsManager.Nodes.Add(this);
    }

    public void RegisterSettings()
    {
        foreach (var option in Options)
        {
            option.Register();
        }
    }
    protected virtual void OnFirstUpdate(Player? player){}

    internal void OnFirstUpdateInternal(Player? player)
    {
        OnFirstUpdate(player);
    }
    

    public void UpdateNode(Player? player)
    {
        if(player == null) return;
        UpdateLabel(player);
    }

    public void UpdateOptions(Player? player)
    {
        if(player == null)
            return;
        foreach (var option in Options)
        {
            option.UpdateOption(player);
        }
    }
    private void UpdateLabel(Player player)
    {
        _headerSetting?.UpdateLabelAndHint(GetHeaderName(player), GetHeaderHintDescription(player));
    }

    public bool CheckForUpdateRequirement(Player? player)
    {
        if (player == null)
            return false;


        var didSeeBefore = AvailableForPlayers.Contains(player);
        var isVisible = IsVisibleToPlayer(player);
        bool update = false;
        switch (isVisible)
        {
            case true when !didSeeBefore:
                AvailableForPlayers.Add(player);
                update = true;
                break;
            case false when didSeeBefore:
                AvailableForPlayers.Remove(player);
                update = true;
                break;
        }


        if (update)
        {
            Log.Info($"Node {GetType().Name} scheduling update...");
            return update;
        }
        return Options.Any(x=>x.CheckForUpdateRequirement(player));
    }


    public virtual bool IsVisibleToPlayer(Player? player) => true;

    private List<Player> AvailableForPlayers { get; } = new();

}