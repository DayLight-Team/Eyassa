using Exiled.API.Features;
using Eyassa.Models.Options;
using PlayerRoles;

namespace Eyassa.Test.Options;

public class TestSlider : SliderOption
{
    protected override string GetLabel(Player player) => player.Nickname + "'s rizz level";
    protected override string GetHint(Player player) => $"Lowkey ka {player.Position}";
    protected override float GetMin(Player player) => 0;
    protected override float GetMax(Player player) => 50;
    protected override float GetDefaultValue(Player player) => 30;

    public override bool IsVisibleToPlayer(Player? player)
    {
        return player.Role.Type == RoleTypeId.Tutorial;
    }

    protected override void OnValueChanged(Player? player)
    {
        var setting = GetSetting(player);
    }
}