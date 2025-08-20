using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using Eyassa.Models;

namespace Eyassa.Features.Options;

public abstract class SliderOption : OptionBase<SliderSetting>
{

    protected abstract float GetMin(Player player);
    protected abstract float GetMax(Player player);
    protected abstract float GetDefaultValue(Player player);
    protected virtual bool GetIsInteger(Player player) => false;
    protected virtual string GetStringFormat(Player player) => "0.##";
    protected virtual string GetDisplayFormat(Player player) => "{0}";

    internal override void OnRegisteredInternal()
    {
        OriginalDefinition = new SliderSetting(Id, "Default", 0, 0, 0, onChanged: OnChanged);
    }
    public sealed override void UpdateOption(Player? player, bool overrideValue = true)
    {
        if(player==null)
            return;
        var setting = GetSetting(player);
        setting?.UpdateSetting(GetMin(player), GetMax(player), GetIsInteger(player), GetStringFormat(player), GetDisplayFormat(player), overrideValue,filter: player1 => player1 == player);
        setting?.UpdateLabelAndHint(GetLabel(player), GetHint(player), filter: player1 => player1 == player);


    }
    public sealed override SettingBase BuildBase(Player player)
    {
        return new SliderSetting(Id, GetLabel(player), GetMin(player), GetMax(player), GetDefaultValue(player), GetIsInteger(player), GetStringFormat(player), GetDisplayFormat(player));
    }
    protected abstract void OnValueChanged(Player player, float value);
    private void OnChanged(Player? player, SettingBase setting)
    {
        if(player == null)
            return;
        if(Id != setting.Id)
            return;
        LastReceivedValues[player] = setting;
        try
        {
            OnValueChanged(player, setting.Cast<SliderSetting>().SliderValue);
        }
        catch (Exception e)
        {
            Log.Error(e);
        }
    }
}