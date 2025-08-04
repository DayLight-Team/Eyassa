using Exiled.API.Features;
using Eyassa.Models.Options;

namespace Eyassa.Test.Options;

public class TestTextArea : TextAreaOption
{
    public override string GetLabel(Player player) => player.Nickname + $"'s rizz level {player.Rotation}";
    public override string GetHint(Player player) => $"Lowkey ka {player.Position}";

    public override void OnValueChanged(Player? player)
    {
        var setting = GetSetting(player);
    }


}