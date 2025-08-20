using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using Eyassa.Models;

namespace Eyassa.Features.Options;


public abstract class TwoButtonOption : OptionBase<TwoButtonsSetting>
{
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
        setting?.Cast<TwoButtonsSetting>().UpdateSetting(GetFirstButtonText(player),GetSecondButtonText(player), overrideValue, filter: player1 => player1 == player);
        setting?.UpdateLabelAndHint(GetLabel(player), GetHint(player), filter: player1 => player1 == player);
    }

    public sealed override SettingBase BuildBase(Player player)
    {
        return new TwoButtonsSetting(Id, GetLabel(player), GetFirstButtonText(player),GetSecondButtonText(player), GetIsSecondsButtonDefault(player) , GetHint(player),
            255, onChanged: OnChanged);
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
        LastReceivedValues[player] = setting;
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