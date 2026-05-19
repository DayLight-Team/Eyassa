using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using Eyassa.Models;

namespace Eyassa.Features.Options;


public abstract class ButtonOption : OptionBase<ButtonSetting>
{
    private Dictionary<Player, (string Text, float HoldTime)> LastSentSettings { get; } = new();

    protected abstract string GetButtonText(Player player);

    protected virtual float GetHoldTime(Player player) => 0f;

    public override bool IsIdCached => false;

    public sealed override string CustomId { get; } = "";

    internal override void OnRegisteredInternal()
    {
        OriginalDefinition = new ButtonSetting(Id, "Default", "Default", 0, "Default", null, OnChanged);
    }
    public sealed override void UpdateOption(Player? player, bool overrideValue = true)
    {
        if(player==null)
            return;
        var setting = GetSetting(player);
        var text = GetButtonText(player);
        var holdTime = GetHoldTime(player);
        if (!LastSentSettings.TryGetValue(player, out var previous) ||
            previous.Text != text ||
            previous.HoldTime != holdTime)
        {
            setting?.Cast<ButtonSetting>().UpdateSetting(text, holdTime, overrideValue, filter: player1 => player1 == player);
            LastSentSettings[player] = (text, holdTime);
        }

        UpdateLabelAndHintIfChanged(setting, player, overrideValue);

    }

    public sealed override SettingBase BuildBase(Player player)
    {
        var label = GetLabel(player);
        var hint = GetHint(player);
        var text = GetButtonText(player);
        var holdTime = GetHoldTime(player);
        CacheLabelAndHint(player, label, hint);
        LastSentSettings[player] = (text, holdTime);
        return new ButtonSetting(Id, label, text, holdTime, hint, null, OnChanged);
    }
    protected abstract void OnPressed(Player player);
    private void OnChanged(Player? player, SettingBase setting)
    {
        if(!IsRegistered)
            return;
        if(player == null)
            return;
        if(Id != setting.Id)
            return;
        LastReceivedValues[player] = setting;
        try
        {
            OnPressed(player);
        }
        catch (Exception e)
        {
            Log.Error(e);
        }
    }
}
