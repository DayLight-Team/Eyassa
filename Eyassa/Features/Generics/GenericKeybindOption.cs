using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using Eyassa.Features.Options;
using UnityEngine;

namespace Eyassa.Features.Generics;

public class GenericKeybindOption(string customId, string label, KeyCode suggestedKey,string? hint = null, bool preventInteraction = false, Action<Player, KeybindSetting>? onPressed = null, Predicate<Player>? isVisible = null) : KeybindOption
{
    public override string CustomId { get; } = customId;
    protected override string GetLabel(Player player) => label;
    protected override string? GetHint(Player player) => hint;

    protected override bool GetPreventInteractionOnGUI(Player player) => preventInteraction;

    protected override void OnPressed(Player player) => onPressed?.Invoke(player, GetSetting(player));
    protected override KeyCode GetSuggestedKey(Player player) => suggestedKey;
    public override bool IsVisibleToPlayer(Player player) => isVisible == null || isVisible(player);
}