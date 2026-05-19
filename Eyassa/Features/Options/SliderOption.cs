using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using Eyassa.Models;

namespace Eyassa.Features.Options;

public abstract class SliderOption : OptionBase<SliderSetting>
{
    private Dictionary<Player, (float Min, float Max, bool IsInteger, string StringFormat, string DisplayFormat)> LastSentSettings { get; } = new();

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
        var min = GetMin(player);
        var max = GetMax(player);
        var isInteger = GetIsInteger(player);
        var stringFormat = GetStringFormat(player);
        var displayFormat = GetDisplayFormat(player);
        if (!LastSentSettings.TryGetValue(player, out var previous) ||
            previous.Min != min ||
            previous.Max != max ||
            previous.IsInteger != isInteger ||
            previous.StringFormat != stringFormat ||
            previous.DisplayFormat != displayFormat)
        {
            setting?.UpdateSetting(min, max, isInteger, stringFormat, displayFormat, overrideValue,filter: player1 => player1 == player);
            LastSentSettings[player] = (min, max, isInteger, stringFormat, displayFormat);
        }

        UpdateLabelAndHintIfChanged(setting, player, overrideValue);

    }
    public sealed override SettingBase BuildBase(Player player)
    {
        var label = GetLabel(player);
        var hint = GetHint(player);
        var min = GetMin(player);
        var max = GetMax(player);
        var isInteger = GetIsInteger(player);
        var stringFormat = GetStringFormat(player);
        var displayFormat = GetDisplayFormat(player);
        CacheLabelAndHint(player, label, hint);
        LastSentSettings[player] = (min, max, isInteger, stringFormat, displayFormat);
        return new SliderSetting(Id, label, min, max, GetDefaultValue(player), isInteger, stringFormat, displayFormat);
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
