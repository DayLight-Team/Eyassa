using Exiled.API.Features;
using Eyassa.Features.Options;
using TMPro;

namespace Eyassa.Test.Options;

public class TestInputText : TextInputOption
{
    public override string CustomId { get; } = "test_input_text";
    protected override string GetLabel(Player player) => player.Nickname + $"'s rizz level {player.Rotation}";
    protected override string? GetHint(Player player) => $"Lowkey ka {player.Position}";
    protected override string GetPlaceholder(Player player) => "joa";
    protected override int GetMaxLength(Player player) => 25;

    protected override TMP_InputField.ContentType GetContentType(Player player) => TMP_InputField.ContentType.Standard;
    protected override void OnValueChanged(Player player, string text)
    {
        Log.Info(text);
    }
}