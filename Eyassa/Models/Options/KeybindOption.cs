using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using Eyassa.Interfaces;
using UnityEngine;

namespace Eyassa.Models.Options;

public abstract class KeybindOption : OptionBase<KeybindSetting>
{
    public abstract KeyCode GetSuggestedKey(Player player);

    public abstract bool GetPreventInteractionOnGUI(Player player);
    
    public override void UpdateOption(Player? player, bool overrideValue = true)
    {
        if(player==null)
            return;
        var setting = GetSetting(player);

        setting.UpdateLabelAndHint(GetLabel(player), GetHint(player));
    }

    public override SettingBase BuildBase(Player? player)
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