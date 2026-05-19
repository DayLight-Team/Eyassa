using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using Eyassa.Models;

namespace Eyassa.Features.Options;


public abstract class HeaderOption : OptionBase<HeaderSetting>
{
    
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
        var setting = GetSetting(player);
        UpdateLabelAndHintIfChanged(setting, player, overrideValue);


    }
    public sealed override SettingBase BuildBase(Player player)
    {
        var label = GetLabel(player);
        var hint = GetHint(player);
        CacheLabelAndHint(player, label, hint);
        return new HeaderSetting(Id, label, hint, GetApplyPadding(player));
    }
}
