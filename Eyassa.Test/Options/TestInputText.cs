using Exiled.API.Features;
using Eyassa.Models.Options;
using TMPro;

namespace Eyassa.Test.Options;

public class TestInputText : TextInputOption
{
    public override string GetLabel(Player player) => player.Nickname + $"'s rizz level {player.Rotation}";
    public override string GetHint(Player player) => $"Lowkey ka {player.Position}";

    public override void OnValueChanged(Player? player)
    {
        var setting = GetSetting(player);
        Log.Info(setting.Text);
    }


    public override string GetPlaceholder(Player player) => "joa";
    public override int GetMaxLength(Player player) => 25;

    public override TMP_InputField.ContentType GetContentType(Player player) => TMP_InputField.ContentType.Standard;
}