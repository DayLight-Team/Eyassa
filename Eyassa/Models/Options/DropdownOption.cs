using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using UserSettings.ServerSpecific;

namespace Eyassa.Models.Options;

public abstract class DropdownOption : OptionBase<DropdownSetting>
{
    public abstract List<string> GetOptions(Player player);
    public virtual int GetDefaultOptionIndex(Player player) => 0;
    public virtual SSDropdownSetting.DropdownEntryType GetEntryType(Player player) => SSDropdownSetting.DropdownEntryType.Regular;
    public override void UpdateOption(Player? player, bool overrideValue = true)
    {
        if(player==null)
            return;
        var setting = GetSetting(player);
        setting.Cast<DropdownSetting>().UpdateSetting(GetOptions(player).ToArray());
        setting.UpdateLabelAndHint(GetLabel(player), GetHint(player));
    }

    public sealed override SettingBase BuildBase(Player? player)
    {
        if(player == null)
            return new DropdownSetting(Id, "Default", new []{"Default"},onChanged: OnChanged);

        return new DropdownSetting(Id, GetLabel(player), GetOptions(player), GetDefaultOptionIndex(player),
            GetEntryType(player), GetHint(player), onChanged: OnChanged);
    }
    
    private void OnChanged(Player? arg1, SettingBase arg2)
    {
        if(arg2.Id != Id)
            return;
        
        LastReceivedValues[arg1] = arg2;
        OnValueChanged(arg1);
    }
}