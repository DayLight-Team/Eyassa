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
    protected virtual float NodeUpdateTime { get; } = 0.5f;
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
        Timing.RunCoroutine(UpdateCoroutine(player));
        OnFirstUpdate(player);
    }

    private IEnumerator<float> UpdateCoroutine(Player? player)
    {
        while (true)
        {
            if (player == null)
            {
                yield break;
            }
            try
            {
                UpdateNode(player);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
            yield return Timing.WaitForSeconds(NodeUpdateTime);
        }
    }

    public void UpdateNode(Player? player)
    {
        if(player == null)
            return;
        UpdateLabel(player);
        UpdateVisibility(player);
    }

    private void UpdateLabel(Player player)
    {
        _headerSetting?.UpdateLabelAndHint(GetHeaderName(player), GetHeaderHintDescription(player));
    }

    private void UpdateVisibility(Player? player)
    {
        if(player == null)
            return;
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