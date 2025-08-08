using Exiled.API.Features;
using Eyassa.Features.Options;

namespace Eyassa.Test.Options;

public class TestTextArea : TextAreaOption
{
    protected override string GetLabel(Player player) => player.Nickname + $"'s rizz level {player.Rotation}";
    protected override string? GetHint(Player player) => $"Lowkey ka {player.Position}";
}