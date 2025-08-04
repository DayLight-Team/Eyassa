using Exiled.API.Features;
using Eyassa.Models.Options;

namespace Eyassa.Test.Options;

public class TestButton : ButtonOption
{
    public override string GetLabel(Player player) => $"Hello {player.Nickname}";
    public override string GetButtonText(Player player) => $"Heal {player.MaxHealth - player.Health} HP";
    public override string GetHint(Player player) => "Heals you to full health";
    public override float GetHoldTime(Player player) => (player.MaxHealth - player.Health) / 10;
    public override void OnValueChanged(Player? player)
    {
        player?.Heal(player.MaxHealth - player.Health);
    }
}