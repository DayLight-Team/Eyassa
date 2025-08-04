using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using Eyassa.Interfaces;
using LabApi.Features.Console;

namespace Eyassa.Models;


public abstract class ButtonOption : OptionBase<ButtonSetting>, IValueReceiver
{
    public abstract string GetButtonText(Player player);

    public abstract float GetHoldTime(Player player);
    public abstract void OnValueChanged(Player?player);
    public override void UpdateOption(Player? player, bool overrideValue = true)
    {
        BuildBase(player).Cast<ButtonSetting>().UpdateSetting(GetButtonText(player), GetHoldTime(player));
        GetSetting(player).UpdateLabelAndHint(GetLabel(player), GetHint(player));
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