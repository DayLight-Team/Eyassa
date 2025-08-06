using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using Eyassa.Models;
using UnityEngine;

namespace Eyassa.Features.Options;

public abstract class KeybindOption : OptionBase<KeybindSetting>
{
    protected abstract KeyCode GetSuggestedKey(Player player);
    protected virtual bool GetPreventInteractionOnGUI(Player player) => false;
    protected sealed override void UpdateOption(Player? player, bool overrideValue = true)
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

    private void OnChanged(Player arg1, SettingBase arg2)
    {
        if(Id != arg2.Id)
            return;
        LastReceivedValues[arg1] = arg2;
        OnValueChanged(arg1);
    }
}