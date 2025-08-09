using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using Eyassa.Models;

namespace Eyassa.Features.Options;


public abstract class ButtonOption : OptionBase<ButtonSetting>
{
    protected abstract string GetButtonText(Player player);

    protected virtual float GetHoldTime(Player player) => 0f;

    public override bool IsIdCached => false;

    public sealed override string CustomId { get; } = "";

    public sealed override void UpdateOption(Player? player, bool overrideValue = true)
    {
        if(player==null)
            return;
        var setting = GetSetting(player);
        setting?.Cast<ButtonSetting>().UpdateSetting(GetButtonText(player), GetHoldTime(player), overrideValue);
        setting?.UpdateLabelAndHint(GetLabel(player), GetHint(player));
    }

    public sealed override SettingBase BuildBase(Player? player)
    {
        if(player == null)
            return new ButtonSetting(Id, "Default", "Default", 0, "Default", null, OnChanged)
            {
                Base = {  }
            };
        
        return new ButtonSetting(Id, GetLabel(player), GetButtonText(player), GetHoldTime(player), GetHint(player), null,
            OnChanged);
    }
    
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
            OnValueChanged(player);
        }
        catch (Exception e)
        {
            Log.Error(e);
        }
    }
}