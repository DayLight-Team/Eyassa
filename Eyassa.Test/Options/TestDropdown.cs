using System.Collections.Generic;
using Exiled.API.Features;
using Eyassa.Features.Options;

namespace Eyassa.Test.Options;

public class TestDropdown : DropdownOption
{
    public override string CustomId { get; } = "test_dropdown";
    protected override string GetLabel(Player player) => player.Nickname + "'s rizz level";

    protected override string? GetHint(Player player) => "idfk";
    protected override void OnValueChanged(Player player)
    {
        var setting = GetSetting(player);
        Log.Info(setting.SelectedOption);
    }

    protected override List<string> GetOptions(Player player)
    {
        return new List<string>()
        {
            "1", "2", "3"
        };
    }
}