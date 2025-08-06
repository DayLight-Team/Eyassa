using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using Eyassa.Models;
using UnityEngine;

namespace Eyassa.Features.Options;

public abstract class KeybindOption : OptionBase<KeybindSetting>
{
    protected abstract KeyCode GetSuggestedKey(Player player);
    protected virtual bool GetPreventInteractionOnGUI(Player player) => false;

    public sealed override void UpdateOption(Player? player, bool overrideValue = true)
    {
        if(player==null)
            return;
        var setting = GetSetting(player);

        setting.UpdateLabelAndHint(GetLabel(player), GetHint(player));
    }

    public sealed override SettingBase BuildBase(Player? player)
    {
        if(player == null)
            return new KeybindSetting(Id, "Default", KeyCode.None , true, "Default", null, OnChanged);
        
        return new KeybindSetting(Id, GetLabel(player), GetSuggestedKey(player), GetPreventInteractionOnGUI(player), GetHint(player), null,
            OnChanged);
    }

    private void OnChanged(Player player, SettingBase setting)
    {
        if(Id != setting.Id)
            return;
        if(Id != setting.Id)
            return;
        if(!setting.Cast<KeybindSetting>().IsPressed)
            return;
        LastReceivedValues[player] = setting;
        try
        {
            OnValueChanged(player);
        }
        catch (Exception e)
        {
            Log.Error(e);
            throw;
        }
    }
}