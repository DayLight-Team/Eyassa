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

    protected T? OriginalDefinition { get; set; }
    public virtual bool SendOnJoin { get; } = true;

    public abstract string CustomId { get; }
    protected abstract string GetLabel(Player player);
    protected virtual string? GetHint(Player player) => null;
    internal Dictionary<string, SettingBase> LastReceivedValues { get; } = new();
    public virtual bool IsVisibleToPlayer(Player player) => true;
    public virtual bool IsIdCached => true;
    private HashSet<string> AvailableForPlayers { get; } = [];
    private Dictionary<string, (string Label, string? Hint)> LastSentLabelHints { get; } = new();
    internal abstract void OnRegisteredInternal();

    protected static string GetPlayerCacheKey(Player player)
    {
        return !string.IsNullOrWhiteSpace(player.UserId) ? player.UserId : player.Id.ToString();
    }

    protected void CacheReceivedValue(Player player, SettingBase setting)
    {
        LastReceivedValues[GetPlayerCacheKey(player)] = setting;
    }

    protected void ClearPlayerState(Player player)
    {
        var key = GetPlayerCacheKey(player);
        AvailableForPlayers.Remove(key);
        LastSentLabelHints.Remove(key);
        LastReceivedValues.Remove(key);
    }
    
    bool IOption.CheckForUpdate(Player? player)
    {
        try
        {
            if (player == null)
                return false;
            var key = GetPlayerCacheKey(player);
            var didSeeBefore = AvailableForPlayers.Contains(key);
            var isVisible = IsVisibleToPlayer(player);
            bool update = false;
            switch (isVisible)
            {
                case true when !didSeeBefore:
                    AvailableForPlayers.Add(key);
                    update = true;
                    break;
                case false when didSeeBefore:
                    ClearPlayerState(player);
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
        return AvailableForPlayers.Contains(GetPlayerCacheKey(player));
    }

    protected bool UpdateLabelAndHintIfChanged(SettingBase? setting, Player player, bool overrideValue = true)
    {
        if (setting == null)
            return false;

        var label = GetLabel(player);
        var hint = GetHint(player);
        var key = GetPlayerCacheKey(player);

        if (LastSentLabelHints.TryGetValue(key, out var previous) &&
            previous.Label == label &&
            previous.Hint == hint)
            return false;
        setting.UpdateLabelAndHint(label, hint, overrideValue, filter: player1 => player1.UserId == player.UserId);
        LastSentLabelHints[key] = (label, hint);
        return true;
    }

    protected void CacheLabelAndHint(Player player, string label, string? hint)
    {
        LastSentLabelHints[GetPlayerCacheKey(player)] = (label, hint);
    }

    public virtual void OnFirstUpdate(Player? player)
    {
    }

    public void Register()
    {
        if(IsRegistered)
            return;
        if (EyassaPlugin.Instance == null || !EyassaPlugin.Instance.IsLoaded)
        {
            Log.Error("Trying to register an option before Eyassa is loaded");
            return;
        }
        Id = IsIdCached ? IdManager.Instance.GetId(CustomId) : IdManager.GetNextId();
        OnRegisteredInternal();
        SettingBase.Register(new List<SettingBase?>() { OriginalDefinition }, _=> false);
        IsRegistered = true;
    }
    protected virtual void OnSentSetting(Player player)
    {
        
    }
    void IOption.Send(Player player)
    {
        OnSentSetting(player);
        UpdateOption(player);
    }
    protected bool IsRegistered;
    protected T GetSetting(Player player)
    {
        try
        {
            var value = LastReceivedValues.TryGetValue(GetPlayerCacheKey(player), out var receivedValue) ? receivedValue.Cast<T>() : (T)BuildBase(player);
            return value;
        }
        catch (Exception e)
        {
            Log.Error(e);
            return OriginalDefinition ?? throw new Exception("OriginalDefinition is null");
        }
    }
    public abstract void UpdateOption(Player? player, bool overrideValue = true);
    
    public abstract SettingBase BuildBase(Player player);
}
