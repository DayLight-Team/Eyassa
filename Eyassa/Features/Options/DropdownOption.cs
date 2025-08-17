using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using Eyassa.Models;
using UserSettings.ServerSpecific;

namespace Eyassa.Features.Options;

public abstract class DropdownOption : OptionBase<DropdownSetting>
{
    protected abstract List<string> GetOptions(Player player);
    protected virtual int GetDefaultOptionIndex(Player player) => 0;
    protected virtual SSDropdownSetting.DropdownEntryType GetEntryType(Player player) => SSDropdownSetting.DropdownEntryType.Regular;

    public sealed override void UpdateOption(Player? player, bool overrideValue = true)
    {
        if(player==null)
            return;
        var setting = GetSetting(player);
        setting?.Cast<DropdownSetting>().UpdateSetting(GetOptions(player).ToArray());
        setting?.UpdateLabelAndHint(GetLabel(player), GetHint(player), filter: player1 => player1 == player);


    }

    public sealed override SettingBase BuildBase(Player? player)
    {
        if(player == null)
            return new DropdownSetting(Id, "Default", new []{"Default"},onChanged: OnChanged);
        var options = GetOptions(player);
        return new DropdownSetting(Id, GetLabel(player), options, GetDefaultOptionIndex(player),
            GetEntryType(player), GetHint(player), onChanged: OnChanged);

    }



    
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
            OnValueChanged(player);
        }
        catch (Exception e)
        {
            Log.Error(e);
        }
    }
}