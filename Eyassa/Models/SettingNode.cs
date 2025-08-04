using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using Eyassa.Interfaces;
using LabApi.Features.Console;
using MEC;

namespace Eyassa.Models;

public abstract class SettingNode
{
    internal int HeaderId { get; } = IdManager.GetNextId();

    private HeaderSetting HeaderSetting;
    public abstract string GetHeaderName(Player player);
    public abstract string GetHeaderHintDescription(Player player);
    public virtual bool GetHeaderPadding(Player player) => false;
    public abstract IOption[] Options { get; }

    public virtual int GetPriority(Player player) => 0;
    
    public virtual float NodeUpdateTime { get; } = 0.5f;
    public void Register()
    {
        HeaderSetting = new HeaderSetting(HeaderId, "Default");
        foreach (var option in Options)
        {
            option.Init();
        }
        SettingsManager.Nodes.Add(this);
    }

    public void OnFirstUpdate(Player? player)
    {
        Timing.RunCoroutine(UpdateCoroutine(player));
    }
    
    public IEnumerator<float> UpdateCoroutine(Player? player)
    {
        while (player != null)
        {
            UpdateOptions(player);
            UpdateVisibility(player);
            yield return Timing.WaitForSeconds(NodeUpdateTime);
        }
    }

    private void UpdateOptions(Player player)
    {
        HeaderSetting.UpdateLabelAndHint(GetHeaderName(player), GetHeaderHintDescription(player));
    }

    public void UpdateVisibility(Player? player)
    {
        var didSeeBefore = AvailableForPlayers.Contains(player);
        var isVisible = IsVisibleToPlayer(player);
        if (isVisible && !didSeeBefore)
        {
            AvailableForPlayers.Add(player);
            SettingsManager.SendToPlayer(player);
        }
        if(!isVisible && didSeeBefore)
        {
            AvailableForPlayers.Remove(player);
            SettingsManager.SendToPlayer(player);
        }
    }

    
    public virtual bool IsVisibleToPlayer(Player? player) => true;

    private List<Player> AvailableForPlayers { get; } = new();

}