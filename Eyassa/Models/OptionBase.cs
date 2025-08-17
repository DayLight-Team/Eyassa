using System.Collections.Generic;
using System.Reflection;
using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using Eyassa.Interfaces;
using Eyassa.Managers;
using MEC;
using UnityEngine;
using UserSettings.ServerSpecific;

namespace Eyassa.Models;

public abstract class OptionBase<T> : IOption where T : SettingBase
{
    public int Id { get; private set; } = -1;

    public virtual bool SendOnJoin { get; } = true;

    public abstract string CustomId { get; }
    protected abstract string GetLabel(Player player);
    protected virtual string? GetHint(Player player) => null;
    protected abstract void OnValueChanged(Player player);
    internal Dictionary<Player?, SettingBase> LastReceivedValues { get; } = new();
    public virtual bool IsVisibleToPlayer(Player player) => true;
    public virtual bool IsIdCached => true;
    private List<Player> AvailableForPlayers { get; } = [];
    bool IOption.CheckForUpdate(Player? player)
    {
        try
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
            return update;

        }
        catch (Exception e)
        {
            Log.Error(e);
            return false;
        }
    }
    
    public bool IsCurrentlyVisible(Player player)
    {
        return AvailableForPlayers.Contains(player);
    }

    public virtual void OnFirstUpdate(Player? player)
    {
    }

    public void Register()
    {
        if(IsRegistered)
            return;
        Id = IsIdCached ? IdManager.Instance.GetId(CustomId) : IdManager.GetNextId();
        SettingBase.Register(new List<SettingBase>() { BuildBase(null) }, _=> false);
        IsRegistered = true;
    }

    protected bool IsRegistered;
    protected T GetSetting(Player? player)
    {
        try
        {
            var value = player != null && LastReceivedValues.TryGetValue(player, out var receivedValue) ? receivedValue.Cast<T>() : (T)BuildBase(player);
            return value;
        }
        catch (Exception e)
        {
            Log.Error(e);
            return (T)BuildBase(null);
        }
    }
    public abstract void UpdateOption(Player? player, bool overrideValue = true);
    
    public abstract SettingBase BuildBase(Player? player);
}