using Exiled.API.Features;
using Eyassa.Features.Options;
using TMPro;
using UserSettings.ServerSpecific;

namespace Eyassa.Features.Generics;

public class GenericTextAreaOption(
    string label,
    string? hint = null,
    TextAlignmentOptions alignment = TextAlignmentOptions.TopLeft,     SSTextArea.FoldoutMode foldoutMode = SSTextArea.FoldoutMode.NotCollapsable,
    Predicate<Player>? isVisible = null) : TextAreaOption
{
    protected override string GetLabel(Player player) => label;
    protected override string? GetHint(Player player) => hint;
    protected override TextAlignmentOptions GetAlignment(Player player) => alignment;

    protected override SSTextArea.FoldoutMode GetFoldoutMode(Player player) => foldoutMode;
    public override bool IsVisibleToPlayer(Player player) => isVisible == null || isVisible(player);
}