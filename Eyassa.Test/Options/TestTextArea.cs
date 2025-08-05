using Exiled.API.Features;
using Eyassa.Models.Options;

namespace Eyassa.Test.Options;

public class TestTextArea : TextAreaOption
{
    public override string CustomId { get; } = "test_text_area";
    protected override string GetLabel(Player player) => player.Nickname + $"'s rizz level {player.Rotation}";
    protected override string GetHint(Player player) => $"Lowkey ka {player.Position}";

    protected override void OnValueChanged(Player? player)
    {
        var setting = GetSetting(player);
    }


}