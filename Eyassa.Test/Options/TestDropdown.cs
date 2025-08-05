using System.Collections.Generic;
using Exiled.API.Features;
using Eyassa.Models.Options;

namespace Eyassa.Test.Options;

public class TestDropdown : DropdownOption
{
    public override string CustomId { get; } = "test_dropdown";
    protected override string GetLabel(Player player) => player.Nickname + "'s rizz level";

    protected override string GetHint(Player player) => "idfk";

    protected override void OnValueChanged(Player? player)
    {
        Log.Info($"Selected: {GetLastSentOptions(player)[GetSetting(player).SelectedIndex]}");
    }

    protected override List<string> GetOptions(Player player)
    {
        return new List<string>()
        {
            "1", "2", "3"
        };
    }
}