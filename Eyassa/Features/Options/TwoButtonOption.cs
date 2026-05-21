using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using Eyassa.Models;

namespace Eyassa.Features.Options;


public abstract class TwoButtonOption : OptionBase<TwoButtonsSetting>
{
    private Dictionary<string, (string FirstText, string SecondText)> LastSentSettings { get; } = new();

    protected abstract string GetFirstButtonText(Player player);
    protected abstract string GetSecondButtonText(Player player);
    protected abstract bool GetIsSecondsButtonDefault(Player player);

    internal override void OnRegisteredInternal()
    {
        OriginalDefinition = new TwoButtonsSetting(Id, "Default", "Default","Default",false, "Default", 255, onChanged: OnChanged);
    }
    public sealed override void UpdateOption(Player? player, bool overrideValue = true)
    {
        if(player==null)
            return;
        var setting = GetSetting(player);
        var key = GetPlayerCacheKey(player);
        var firstText = GetFirstButtonText(player);
        var secondText = GetSecondButtonText(player);
        if (!LastSentSettings.TryGetValue(key, out var previous) ||
            previous.FirstText != firstText ||
            previous.SecondText != secondText)
        {
            setting?.Cast<TwoButtonsSetting>().UpdateSetting(firstText, secondText, overrideValue, filter: player1 => player1.UserId == player.UserId);
            LastSentSettings[key] = (firstText, secondText);
        }

        UpdateLabelAndHintIfChanged(setting, player, overrideValue);
    }

    public sealed override SettingBase BuildBase(Player player)
    {
        var label = GetLabel(player);
        var hint = GetHint(player);
        var firstText = GetFirstButtonText(player);
        var secondText = GetSecondButtonText(player);
        var key = GetPlayerCacheKey(player);
        CacheLabelAndHint(player, label, hint);
        LastSentSettings[key] = (firstText, secondText);
        return new TwoButtonsSetting(Id, label, firstText, secondText, GetIsSecondsButtonDefault(player) , hint, 255, onChanged: OnChanged);
    }
    protected abstract void OnPressed(Player player, bool isFirst);
    private void OnChanged(Player? player, SettingBase setting)
    {
        if(!IsRegistered)
            return;
        if(player == null)
            return;
        if(Id != setting.Id)
            return;
        CacheReceivedValue(player, setting);
        try
        {
            OnPressed(player, setting.Cast<TwoButtonsSetting>().IsFirst);
        }
        catch (Exception e)
        {
            Log.Error(e);
        }
    }
}
