using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using Eyassa.Features.Options;
using TMPro;

namespace Eyassa.Features.Generics;

public class GenericTextInputOption(
    string customId,
    string label,
    string? hint = null,
    string placeHolder = "",
    TMP_InputField.ContentType contentType = TMP_InputField.ContentType.Standard,
    int maxLength = 64,
    Action<Player, UserTextInputSetting>? onChanged = null,
    Predicate<Player>? isVisible = null) : TextInputOption
{
    public override string CustomId { get; } = customId;
    protected override string GetLabel(Player player) => label;
    protected override string? GetHint(Player player) => hint;
    protected override void OnValueChanged(Player player, string text) => onChanged?.Invoke(player, text);
    public override bool IsVisibleToPlayer(Player player) => isVisible == null || isVisible(player);
    protected override string GetPlaceholder(Player player) => placeHolder;
    protected override int GetMaxLength(Player player) => maxLength;
    protected override TMP_InputField.ContentType GetContentType(Player player) => contentType;
}