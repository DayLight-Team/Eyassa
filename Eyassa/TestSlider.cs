using Exiled.API.Features;
using Eyassa.Models.Options;
using PlayerRoles;

namespace Eyassa;

public class TestSlider : SliderOption
{
    public override string GetLabel(Player player) => player.Nickname + "'s rizz level";
    public override string GetHint(Player player) => $"Lowkey ka {player.Position}";
    public override float GetMin(Player player) => 0;
    public override float GetMax(Player player) => 50;
    public override float GetDefaultValue(Player player) => 30;

    public override bool IsVisibleToPlayer(Player? player)
    {
        return player.Role.Type == RoleTypeId.Tutorial;
    }

    public override void OnValueChanged(Player? player)
    {
        var setting = GetSetting(player);
        Log.Info(setting.SliderValue);
    }
}