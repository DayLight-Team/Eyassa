using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using Eyassa.Interfaces;

namespace Eyassa.Models;


public abstract class ButtonOption : OptionBase<ButtonSetting>, IValueReceiver
{
    public abstract string GetLabel(Player player);
    public abstract string GetText(Player player);
    public abstract string GetHint(Player player);
    public abstract float GetHoldTime(Player player);
    public abstract void OnValueChanged(Player player);
    public override void UpdateOption(Player player, bool overrideValue = true)
    {
        
    }

    protected sealed override ButtonSetting BuildBase(Player player)
    {
        return new ButtonSetting(Id,GetLabel(player), GetText(player), GetHoldTime(player), GetHint(player), null, OnChanged)
    }

    private void OnChanged(Player arg1, SettingBase arg2)
    {
        OnValueChanged(arg1);
    }
}