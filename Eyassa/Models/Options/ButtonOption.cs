using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using Eyassa.Interfaces;

namespace Eyassa.Models.Options;


public abstract class ButtonOption : OptionBase<ButtonSetting>
{
    protected abstract string GetButtonText(Player player);

    protected virtual float GetHoldTime(Player player) => 0f;

    protected sealed override void UpdateOption(Player? player, bool overrideValue = true)
    {
        if(player==null)
            return;
        var setting = GetSetting(player);
        setting.Cast<ButtonSetting>().UpdateSetting(GetButtonText(player), GetHoldTime(player), overrideValue);
        setting.UpdateLabelAndHint(GetLabel(player), GetHint(player));
    }

    public sealed override SettingBase BuildBase(Player? player)
    {
        if(player == null)
            return new ButtonSetting(Id, "Default", "Default", 0, "Default", null, OnChanged);
        
        return new ButtonSetting(Id, GetLabel(player), GetButtonText(player), GetHoldTime(player), GetHint(player), null,
            OnChanged);
    }
    
    private void OnChanged(Player? arg1, SettingBase arg2)
    {
        if(arg2.Id != Id)
            return;
        
        LastReceivedValues[arg1] = arg2;
        OnValueChanged(arg1);
    }
}