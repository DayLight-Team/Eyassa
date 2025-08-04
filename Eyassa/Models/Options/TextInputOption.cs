using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using Eyassa.Interfaces;
using TMPro;
using UserSettings.ServerSpecific;
using YamlDotNet.Serialization;

namespace Eyassa.Models.Options;

public abstract class TextInputOption : OptionBase<UserTextInputSetting>
{

    public abstract string GetPlaceholder(Player player);
    public abstract int GetMaxLength(Player player);
    public abstract TMP_InputField.ContentType GetContentType(Player player);
    public override void UpdateOption(Player? player, bool overrideValue = true)
    {
        if(player==null)
            return;
        var setting = GetSetting(player);
        setting.UpdateLabelAndHint(GetLabel(player), GetHint(player));
    }
    public sealed override SettingBase BuildBase(Player? player)
    {
        if (player == null)
            return new UserTextInputSetting(Id, "Default", onChanged: OnChanged);
        return new UserTextInputSetting(Id, GetLabel(player),GetPlaceholder(player),  GetMaxLength(player), GetContentType(player), GetHint(player));
    }

    private void OnChanged(Player arg1, SettingBase arg2)
    {
        if(Id != arg2.Id)
            return;
        LastReceivedValues[arg1] = arg2;
        OnValueChanged(arg1);
    }
}