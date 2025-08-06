using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using Eyassa.Features.Options;

namespace Eyassa.Features.Generics;

public class GenericButtonOption(string customId,
    string label,
    string buttonText,
    string? hint = null,
    float holdTime = 0,
    Action<Player, ButtonSetting>? onPressed = null,  
    Predicate<Player>? isVisible = null) : ButtonOption
{
    public override string CustomId { get; } = customId;
    protected override string GetLabel(Player player) => label;
    protected override string? GetHint(Player player) => hint;
    protected override string GetButtonText(Player player) => buttonText;
    protected override float GetHoldTime(Player player) => holdTime;
    protected override void OnValueChanged(Player player) => onPressed?.Invoke(player, GetSetting(player));
    public override bool IsVisibleToPlayer(Player player) => isVisible == null || isVisible(player);
}