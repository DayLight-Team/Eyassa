using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using Eyassa.Models;
using UserSettings.ServerSpecific;
using HarmonyLib;
using UnityEngine;

namespace Eyassa.Features.Options;

public abstract class DropdownOption : OptionBase<DropdownSetting>
{
    private Dictionary<Player, List<string>> LastSentValues { get; } = new();

    protected abstract List<string> GetOptions(Player player);
    protected virtual int GetDefaultOptionIndex(Player player) => 0;
    protected virtual SSDropdownSetting.DropdownEntryType GetEntryType(Player player) => SSDropdownSetting.DropdownEntryType.Regular;

    internal override void OnRegisteredInternal()
    {
        OriginalDefinition = new DropdownSetting(Id, "Default", ["Default"],onChanged: OnChanged);
    }
    public sealed override void UpdateOption(Player? player, bool overrideValue = true)
    {
        if(player==null)
            return;
        var setting = GetSetting(player);
        var options = GetOptions(player);
        if (!LastSentValues.TryGetValue(player, out var previous) || !previous.SequenceEqual(options))
        {
            setting?.Cast<DropdownSetting>().UpdateSetting(options.ToArray(),overrideValue, filter: player1 => player1 == player);
            LastSentValues[player] = options.ToList();
        }

        UpdateLabelAndHintIfChanged(setting, player, overrideValue);
    }

    public sealed override SettingBase BuildBase(Player player)
    {
        var label = GetLabel(player);
        var hint = GetHint(player);
        var options = GetOptions(player);
        var setting = new DropdownSetting(Id, label, options, GetDefaultOptionIndex(player),
            GetEntryType(player), hint, onChanged: OnChanged);
        CacheLabelAndHint(player, label, hint);
        LastSentValues[player] = options.ToList();
        return setting;

    }

    protected abstract void OnValueChanged(Player player, string selectedOption);
    private void OnChanged(Player? player, SettingBase setting)
    {

        if(!IsRegistered)
            return;
        if(player == null)
            return;
        if(Id != setting.Id)
            return;
        var dropdownSetting = setting.Cast<DropdownSetting>();
        if(!LastSentValues.TryGetValue(player, out var last))
            return;
        
        var index = Mathf.Clamp(dropdownSetting.SelectedIndex, 0, last.Count - 1);
        
 
        
        try
        {
            OnValueChanged(player, LastSentValues[player][index]);
        }
        catch (Exception e)
        {
            Log.Error(e);
        }
    }
}
