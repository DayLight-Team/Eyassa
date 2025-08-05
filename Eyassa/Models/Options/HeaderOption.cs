using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using Eyassa.Interfaces;

namespace Eyassa.Models.Options;


public abstract class HeaderOption : OptionBase<HeaderSetting>
{
    
    public virtual bool GetApplyPadding(Player player) => false;

    protected sealed override void UpdateOption(Player? player, bool overrideValue = true)
    {
        if(player==null)
            return;
        var setting = GetSetting(player);
        setting.UpdateLabelAndHint(GetLabel(player), GetHint(player));
    }

    public sealed override SettingBase BuildBase(Player? player)
    {
        if (player == null)
            return new HeaderSetting(Id, "Default" ,"Default", false);
        return new HeaderSetting(Id, GetLabel(player), GetHint(player), GetApplyPadding(player));
    }
}