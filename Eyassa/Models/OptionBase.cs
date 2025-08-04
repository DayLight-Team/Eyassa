using System.Collections.Generic;
using System.Reflection;
using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using Eyassa.Interfaces;
using MEC;
using UserSettings.ServerSpecific;

namespace Eyassa.Models;

public abstract class OptionBase<T> : IOption where T : SettingBase
{
    public virtual int Id { get; } = IdManager.GetNextId();
    public abstract string GetLabel(Player player);
    public abstract string GetHint(Player player);
    public abstract void OnValueChanged(Player? player);
    public float TextUpdateTime { get; set; } = 0.5f;
    public Dictionary<Player?, SettingBase> LastReceivedValues { get; } = new();

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
    
    public IEnumerator<float> UpdateCoroutine(Player? player)
    {
        while (player != null)
        {
            UpdateOption(player);
            UpdateVisibility(player);
            yield return Timing.WaitForSeconds(TextUpdateTime);
        }
    }

    public void OnFirstUpdate(Player? player)
    {
        Timing.RunCoroutine(UpdateCoroutine(player));
    }

    private bool _isInitialized = false;
    public void Init()
    {
        if(_isInitialized)
            return;
        _isInitialized = true;
        SettingBase.Register(new List<SettingBase>() { BuildBase(null) }, _=> false);
    }
    public T GetSetting(Player? player) => LastReceivedValues.ContainsKey(player) ? LastReceivedValues[player].Cast<T>() : (T)BuildBase(player);
    public abstract void UpdateOption(Player? player, bool overrideValue = true);
    public abstract SettingBase BuildBase(Player? player);
}