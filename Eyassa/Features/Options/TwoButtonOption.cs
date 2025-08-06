using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using Eyassa.Models;

namespace Eyassa.Features.Options;


public abstract class TwoButtonOption : OptionBase<TwoButtonsSetting>
{
    protected abstract string GetFirstButtonText(Player player);
    protected abstract string GetSecondButtonText(Player player);
    protected abstract bool GetIsSecondsButtonDefault(Player player);

    public sealed override void UpdateOption(Player? player, bool overrideValue = true)
    {
        if(player==null)
            return;
        var setting = GetSetting(player);
        setting.Cast<TwoButtonsSetting>().UpdateSetting(GetFirstButtonText(player),GetSecondButtonText(player), overrideValue);
        setting.UpdateLabelAndHint(GetLabel(player), GetHint(player));
    }

    public sealed override SettingBase BuildBase(Player? player)
    {
        if(player == null)
            return new TwoButtonsSetting(Id, "Default", "Default","Default",false, "Default", null, OnChanged);
        
        return new TwoButtonsSetting(Id, GetLabel(player), GetFirstButtonText(player),GetSecondButtonText(player), GetIsSecondsButtonDefault(player) , GetHint(player), null,
            OnChanged);
    }
    
    private void OnChanged(Player? player, SettingBase setting)
    {
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