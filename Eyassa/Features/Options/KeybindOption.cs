using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using Eyassa.Models;
using UnityEngine;

namespace Eyassa.Features.Options;

public abstract class KeybindOption : OptionBase<KeybindSetting>
{
    protected abstract KeyCode GetSuggestedKey(Player player);
    protected virtual bool GetPreventInteractionOnGUI(Player player) => false;

    protected virtual bool GetAllowTriggerSpectator(Player player) => false;
    public sealed override void UpdateOption(Player? player, bool overrideValue = true)
    {
        if(player==null)
            return;
        var setting = GetSetting(player);
        UpdateLabelAndHintIfChanged(setting, player, overrideValue);
    }

    internal override void OnRegisteredInternal()
    {
        OriginalDefinition = new KeybindSetting(Id, "Default", KeyCode.None, true, false,null, 255 ,null, OnChanged);
    }
    public sealed override SettingBase BuildBase(Player player)
    {
        var label = GetLabel(player);
        var hint = GetHint(player);
        CacheLabelAndHint(player, label, hint);
        return new KeybindSetting(Id, label, GetSuggestedKey(player), GetPreventInteractionOnGUI(player),GetAllowTriggerSpectator(player) ,hint, 255,null,
            OnChanged);
    }
    protected abstract void OnPressed(Player player);
    private void OnChanged(Player player, SettingBase setting)
    {
        if(!IsRegistered)
            return;
        if(Id != setting.Id)
            return;
        if(!setting.Cast<KeybindSetting>().IsPressed)
            return;
        LastReceivedValues[player] = setting;
        try
        {
            OnPressed(player);
        }
        catch (Exception e)
        {
            Log.Error(e);
            throw;
        }
    }
}
