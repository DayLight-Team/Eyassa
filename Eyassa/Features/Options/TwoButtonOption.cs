using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using Eyassa.Models;

namespace Eyassa.Features.Options;


public abstract class TwoButtonOption : OptionBase<TwoButtonsSetting>
{
    protected abstract string GetFirstButtonText(Player player);
    protected abstract string GetSecondButtonText(Player player);
    protected abstract bool GetIsSecondsButtonDefault(Player player);

    protected sealed override void UpdateOption(Player? player, bool overrideValue = true)
    {
        if(player==null)
            return;
        var setting = GetSetting(player);
        setting.Cast<TwoButtonsSetting>().UpdateSetting(GetFirstButtonText(player),GetSecondButtonText(player), overrideValue);
        setting.UpdateLabelAndHint(GetLabel(player), GetHint(player));
    }

    public sealed override SettingBase BuildBase(Player? player)
    {
        if(player == null)
            return new TwoButtonsSetting(Id, "Default", "Default","Default",false, "Default", null, OnChanged);
        
        return new TwoButtonsSetting(Id, GetLabel(player), GetFirstButtonText(player),GetSecondButtonText(player), GetIsSecondsButtonDefault(player) , GetHint(player), null,
            OnChanged);
    }
    
    private void OnChanged(Player? arg1, SettingBase arg2)
    {
        if(arg2.Id != Id)
            return;
        if(arg1 == null)
            return;
        
        LastReceivedValues[arg1] = arg2;
        OnValueChanged(arg1);
    }
}