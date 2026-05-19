using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using Eyassa.Models;
using TMPro;
using UserSettings.ServerSpecific;

namespace Eyassa.Features.Options;

public abstract class TextAreaOption : OptionBase<TextInputSetting>
{
    protected virtual SSTextArea.FoldoutMode GetFoldoutMode(Player player) => SSTextArea.FoldoutMode.NotCollapsable;
    protected virtual TextAlignmentOptions GetAlignment(Player player) => TextAlignmentOptions.TopLeft;
    public override bool IsIdCached => false;
    public sealed override string CustomId { get; } = "";
    public sealed override void UpdateOption(Player? player, bool overrideValue = true)
    {
        if(player==null)
            return;
        var setting = GetSetting(player);
        UpdateLabelAndHintIfChanged(setting, player, overrideValue);
    }
    internal override void OnRegisteredInternal()
    {
        OriginalDefinition = new TextInputSetting(Id, "Default");
    }
    public sealed override SettingBase BuildBase(Player player)
    {
        var label = GetLabel(player);
        var hint = GetHint(player);
        CacheLabelAndHint(player, label, hint);
        return new TextInputSetting(Id, label, GetFoldoutMode(player), GetAlignment(player), hint);
    }
}
