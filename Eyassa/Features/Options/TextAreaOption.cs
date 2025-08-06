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

    public sealed override void UpdateOption(Player? player, bool overrideValue = true)
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

    protected sealed override void OnValueChanged(Player player)
    {
        throw new InvalidOperationException("This method should never be called.");
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