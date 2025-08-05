using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using UserSettings.ServerSpecific;

namespace Eyassa.Models.Options;

public abstract class DropdownOption : OptionBase<DropdownSetting>
{
    protected abstract List<string> GetOptions(Player player);
    protected virtual int GetDefaultOptionIndex(Player player) => 0;
    protected virtual SSDropdownSetting.DropdownEntryType GetEntryType(Player player) => SSDropdownSetting.DropdownEntryType.Regular;

    protected sealed override void UpdateOption(Player? player, bool overrideValue = true)
    {
        if(player==null)
            return;
        var setting = GetSetting(player);
        var options = GetOptions(player);
        setting.Cast<DropdownSetting>().UpdateSetting(GetOptions(player).ToArray());
        LastSentOptions[player] = options;
        setting.UpdateLabelAndHint(GetLabel(player), GetHint(player));
    }

    public sealed override SettingBase BuildBase(Player? player)
    {
        if(player == null)
            return new DropdownSetting(Id, "Default", new []{"Default"},onChanged: OnChanged);
        var options = GetOptions(player);
        LastSentOptions[player] = options;
        return new DropdownSetting(Id, GetLabel(player), options, GetDefaultOptionIndex(player),
            GetEntryType(player), GetHint(player), onChanged: OnChanged);

    }

    private Dictionary<Player, List<string>> LastSentOptions { get; } = new();

    protected List<string> GetLastSentOptions(Player? player)
    {
        if(player == null)
            return new List<string>();
        if(!LastSentOptions.ContainsKey(player))
            LastSentOptions[player] = new List<string>();
        return LastSentOptions[player];
    }
    
    private void OnChanged(Player? arg1, SettingBase arg2)
    {
        if(arg2.Id != Id)
            return;
        
        LastReceivedValues[arg1] = arg2;
        OnValueChanged(arg1);
    }
}