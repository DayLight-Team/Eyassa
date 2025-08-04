using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using Eyassa.Interfaces;
using TMPro;
using UserSettings.ServerSpecific;
using YamlDotNet.Serialization;

namespace Eyassa.Models.Options;

public abstract class TextAreaOption : OptionBase<TextInputSetting>
{
    public virtual SSTextArea.FoldoutMode GetFoldoutMode(Player player) => SSTextArea.FoldoutMode.NotCollapsable;
    public virtual TextAlignmentOptions GetAlignment(Player player) => TextAlignmentOptions.TopLeft;

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
            return new TextInputSetting(Id, "Default", onChanged: OnChanged);
        return new TextInputSetting(Id, GetLabel(player), GetFoldoutMode(player), GetAlignment(player), GetHint(player));
    }

    private void OnChanged(Player arg1, SettingBase arg2)
    {
        if(Id != arg2.Id)
            return;
        LastReceivedValues[arg1] = arg2;
        OnValueChanged(arg1);
    }
}