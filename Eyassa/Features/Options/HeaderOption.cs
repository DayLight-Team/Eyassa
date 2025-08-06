using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using Eyassa.Models;

namespace Eyassa.Features.Options;


public abstract class HeaderOption : OptionBase<HeaderSetting>
{
    
    public virtual bool GetApplyPadding(Player player) => false;

    public sealed override void OnFirstUpdate(Player? player)
    {
        UpdateOption(player);
        base.OnFirstUpdate(player);
    }
    
    protected sealed override void OnValueChanged(Player player)
    {
        throw new InvalidOperationException("This method should never be called.");
    }

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