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
    public abstract IOption[] Options { get; }
    public virtual int GetPriority(Player player) => 0;
    protected virtual float NodeUpdateTime { get; } = 0.5f;
    public void Register()
    {
        _headerSetting = new HeaderSetting(HeaderId, "Default");
        foreach (var option in Options)
        {
            option.Init();
        }
        SettingsManager.Nodes.Add(this);
    }

    internal void OnFirstUpdate(Player? player)
    {
        Timing.RunCoroutine(UpdateCoroutine(player));
    }

    private IEnumerator<float> UpdateCoroutine(Player? player)
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
        _headerSetting?.UpdateLabelAndHint(GetHeaderName(player), GetHeaderHintDescription(player));
    }

    private void UpdateVisibility(Player? player)
    {
        var didSeeBefore = AvailableForPlayers.Contains(player);
        var isVisible = IsVisibleToPlayer(player);
        switch (isVisible)
        {
            case true when !didSeeBefore:
                AvailableForPlayers.Add(player);
                SettingsManager.SendToPlayer(player);
                break;
            case false when didSeeBefore:
                AvailableForPlayers.Remove(player);
                SettingsManager.SendToPlayer(player);
                break;
        }
    }


    protected virtual bool IsVisibleToPlayer(Player? player) => true;

    private List<Player> AvailableForPlayers { get; } = new();

}