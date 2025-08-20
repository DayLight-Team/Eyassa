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
        setting?.UpdateLabelAndHint(GetLabel(player), GetHint(player), filter: player1 => player1 == player);


    }
    public sealed override SettingBase BuildBase(Player player)
    {
        return new HeaderSetting(Id, GetLabel(player), GetHint(player), GetApplyPadding(player));
    }
}