using Exiled.API.Features;
using Eyassa.Models.Options;

namespace Eyassa.Test.Options;

public class TestTwoButton : TwoButtonOption
{
    protected override string GetLabel(Player player) => player.Nickname + "'s rizz level";

    protected override string GetHint(Player player) => "idfk";

    protected override void OnValueChanged(Player? player)
    {
        var twoButtonsSetting = GetSetting(player);
        Log.Info(twoButtonsSetting.IsFirst + " " + twoButtonsSetting.IsSecond);
    }

    protected override string GetFirstButtonText(Player player) => player.Role.Type.ToString();

    protected override string GetSecondButtonText(Player player) => player.Position.ToString();

    protected override bool GetIsSecondsButtonDefault(Player player) => true;
}