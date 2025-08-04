using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using Eyassa.Interfaces;

namespace Eyassa.Models.Options;

public abstract class SliderOption : OptionBase<SliderSetting>
{
    
    public abstract float GetMin(Player player);
    public abstract float GetMax(Player player);
    public abstract float GetDefaultValue(Player player);
    public virtual bool GetIsInteger(Player player) => false;
    public virtual string GetStringFormat(Player player) => "0.##";
    public virtual string GetDisplayFormat(Player player) => "{0}";

    public override void UpdateOption(Player? player, bool overrideValue = true)
    {
        if(player==null)
            return;
        var setting = GetSetting(player);
        setting.UpdateSetting(GetMin(player), GetMax(player), GetIsInteger(player), GetStringFormat(player), GetDisplayFormat(player), overrideValue);
        setting.UpdateLabelAndHint(GetLabel(player), GetHint(player));
    }
    public sealed override SettingBase BuildBase(Player? player)
    {
        if (player == null)
            return new SliderSetting(Id, "Default", 0, 0, 0, onChanged: OnChanged);
        return new SliderSetting(Id, GetLabel(player), GetMin(player), GetMax(player), GetDefaultValue(player), GetIsInteger(player), GetStringFormat(player), GetDisplayFormat(player));
    }

    private void OnChanged(Player arg1, SettingBase arg2)
    {
        if(Id != arg2.Id)
            return;
        LastReceivedValues[arg1] = arg2;
        OnValueChanged(arg1);
    }
}