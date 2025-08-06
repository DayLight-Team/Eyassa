using Exiled.API.Features;
using Eyassa.Features.Options;

namespace Eyassa.Features.Generics;

public class GenericHeaderOption(string customId, string label, string? hint = null, bool applyPadding = false, Predicate<Player>? isVisible = null) : HeaderOption
{
    public override string CustomId { get; } = customId;
    protected override string GetLabel(Player player) => label;
    protected override string? GetHint(Player player) => hint;
    public override bool GetApplyPadding(Player player) => applyPadding;

    public override bool IsVisibleToPlayer(Player player) => isVisible == null || isVisible(player);
}