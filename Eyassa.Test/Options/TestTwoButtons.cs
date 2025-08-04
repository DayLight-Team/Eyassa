using Exiled.API.Features;
using Eyassa.Models.Options;

namespace Eyassa.Test.Options;

public class TestTwoButton : TwoButtonOption
{
    public override string GetLabel(Player player) => player.Nickname + "'s rizz level";

    public override string GetHint(Player player) => "idfk";

    public override void OnValueChanged(Player? player)
    {
        var twoButtonsSetting = GetSetting(player);
        Log.Info(twoButtonsSetting.IsFirst + " " + twoButtonsSetting.IsSecond);
    }

    public override string GetFirstButtonText(Player player) => player.Role.Type.ToString();

    public override string GetSecondButtonText(Player player) => player.Position.ToString();

    public override bool GetIsSecondsButtonDefault(Player player) => true;
}