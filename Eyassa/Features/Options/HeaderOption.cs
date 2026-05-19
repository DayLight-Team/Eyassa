using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using Eyassa.Models;

namespace Eyassa.Features.Options;


public abstract class HeaderOption : OptionBase<HeaderSetting>
{
    private Dictionary<Player, (string Label, string? Hint, bool ApplyPadding)> LastSentSettings { get; } = new();
    
    public virtual bool GetApplyPadding(Player player) => false;

    public override bool IsIdCached => false;
    public sealed override string CustomId { get; } = "";
    internal override void OnRegisteredInternal()
    {
        OriginalDefinition = new HeaderSetting(Id, "Default", "Default", false);
    }
    public sealed override void UpdateOption(Player? player, bool overrideValue = true)
    {
        if(player==null)
            return;

        var label = GetLabel(player);
        var hint = GetHint(player);
        var applyPadding = GetApplyPadding(player);

        if (LastSentSettings.TryGetValue(player, out var previous) &&
            previous.Label == label &&
            previous.Hint == hint &&
            previous.ApplyPadding == applyPadding)
            return;

        var setting = new HeaderSetting(Id, label, hint, applyPadding);
        SettingBase.Unregister(player, [setting]);
        SettingBase.Register(player, [setting]);
        LastSentSettings[player] = (label, hint, applyPadding);


    }
    public sealed override SettingBase BuildBase(Player player)
    {
        var label = GetLabel(player);
        var hint = GetHint(player);
        var applyPadding = GetApplyPadding(player);
        return new HeaderSetting(Id, label, hint, applyPadding);
    }
}
