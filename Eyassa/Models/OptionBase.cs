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
    public int Id => IdManager.Instance.GetId(CustomId);
    public abstract string CustomId { get; }
    protected abstract string GetLabel(Player player);
    protected virtual string? GetHint(Player player) => null;
    protected abstract void OnValueChanged(Player player);
    internal Dictionary<Player?, SettingBase> LastReceivedValues { get; } = new();
    public virtual bool IsVisibleToPlayer(Player player) => true;
    private List<Player> AvailableForPlayers { get; } = [];
    public bool CheckForUpdateRequirement(Player? player)
    {
        if (player == null)
            return false;


        var didSeeBefore = AvailableForPlayers.Contains(player);
        var isVisible = IsVisibleToPlayer(player);
        switch (isVisible)
        {
            case true when !didSeeBefore:
                AvailableForPlayers.Add(player);
                return true;
            case false when didSeeBefore:
                AvailableForPlayers.Remove(player);
                return true;
        }
        return false;
    }

    void IOption.OnFirstUpdateInternal(Player? player)
    {
        CheckForUpdateRequirement(player);
        UpdateOption(player);
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

    protected T GetSetting(Player? player) => player != null && LastReceivedValues.ContainsKey(player) ? LastReceivedValues[player].Cast<T>() : (T)BuildBase(player);
    public abstract void UpdateOption(Player? player, bool overrideValue = true);
    public abstract SettingBase BuildBase(Player? player);
}