using System.Collections.Generic;
using System.Reflection;
using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using Eyassa.Interfaces;
using Eyassa.Managers;
using MEC;
using UserSettings.ServerSpecific;

namespace Eyassa.Models;

public abstract class OptionBase<T> : IOption where T : SettingBase
{
    public int Id => EyassaPlugin.Instance.IdManager.GetId(CustomId);
    public abstract string CustomId { get; }
    protected abstract string GetLabel(Player player);
    protected virtual string? GetHint(Player player) => null;
    protected abstract void OnValueChanged(Player player);
    protected virtual float UpdateDelaySeconds { get; set; } = 0.5f;
    internal Dictionary<Player?, SettingBase> LastReceivedValues { get; } = new();
    
    public virtual bool IsVisibleToPlayer(Player player) => true;

    private List<Player> AvailableForPlayers { get; } = [];

    private IEnumerator<float> UpdateCoroutine(Player? player)
    {
        // ReSharper disable once LoopVariableIsNeverChangedInsideLoop
        while (player != null)
        {
            UpdateOption(player);
            yield return Timing.WaitForSeconds(UpdateDelaySeconds);
        }
    }

    public void UpdateOption(Player? player)
    {
        UpdateVisibility(player);
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

    void IOption.OnFirstUpdateInternal(Player? player)
    {
        Timing.RunCoroutine(UpdateCoroutine(player));
        OnFirstUpdate(player);
    }
    public virtual void OnFirstUpdate(Player? player)
    {
    }

    private bool _isInitialized = false;
    public void Register()
    {
        if(_isInitialized)
            return;
        _isInitialized = true;
        SettingBase.Register(new List<SettingBase>() { BuildBase(null) }, _=> false);
    }

    protected T GetSetting(Player? player) => LastReceivedValues.ContainsKey(player) ? LastReceivedValues[player].Cast<T>() : (T)BuildBase(player);
    protected abstract void UpdateOption(Player? player, bool overrideValue = true);
    public abstract SettingBase BuildBase(Player? player);
}