using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using Eyassa.Models;
using TMPro;

namespace Eyassa.Features.Options;

public abstract class TextInputOption : OptionBase<UserTextInputSetting>
{
    protected virtual string GetPlaceholder(Player player) => "";
    protected virtual int GetMaxLength(Player player) => 32;
    protected virtual TMP_InputField.ContentType GetContentType(Player player) => TMP_InputField.ContentType.Standard;

    public sealed override void UpdateOption(Player? player, bool overrideValue = true)
    {
        if(player==null)
            return;
        var setting = GetSetting(player);
        setting?.UpdateLabelAndHint(GetLabel(player), GetHint(player), filter: player1 => player1 == player);
    }
    internal override void OnRegisteredInternal()
    {
        OriginalDefinition = new UserTextInputSetting(Id, "Default", "", 255, TMP_InputField.ContentType.Standard, "Default", 255, onChanged: OnChanged);
    }
    public sealed override SettingBase BuildBase(Player player)
    {
        return new UserTextInputSetting(Id, GetLabel(player),GetPlaceholder(player),  GetMaxLength(player), GetContentType(player), GetHint(player),255, onChanged: OnChanged);
    }
    protected abstract void OnValueChanged(Player player, string text);
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
            OnValueChanged(player, setting.Cast<UserTextInputSetting>().Text);

   
        }
        catch (Exception e)
        {
            Log.Error(e);
        }
    }
}