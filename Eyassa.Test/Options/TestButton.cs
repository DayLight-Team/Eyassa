using Exiled.API.Features;
using Eyassa.Features.Options;

namespace Eyassa.Test.Options;

public class TestButton : ButtonOption
{
    public override string CustomId { get; } = "test_button";
    protected override string GetLabel(Player player) => $"Hello {player.Nickname}";
    protected override string GetButtonText(Player player) => $"Heal {player.MaxHealth - player.Health} HP";
    protected override string? GetHint(Player player) => "Heals you to full health";
    protected override float GetHoldTime(Player player) => (player.MaxHealth - player.Health) / 10;

    protected override void OnValueChanged(Player? player)
    {
        player?.Heal(player.MaxHealth - player.Health);
    }
}