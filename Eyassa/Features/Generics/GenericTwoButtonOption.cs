using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using Eyassa.Features.Options;

namespace Eyassa.Features.Generics;

public class GenericTwoButtonOption(string customId,
    string label,
    string firstButtonText,
    string secondButtonText,
    string? hint,
    bool isSecondButtonDefault = false,
    Action<Player, TwoButtonsSetting>? onChanged = null,  
    Predicate<Player>? isVisible = null) : TwoButtonOption
{
    public override string CustomId { get; } = customId;
    protected override string GetLabel(Player player) => label;
    protected override string? GetHint(Player player) => hint;
    protected override void OnValueChanged(Player player) => onChanged?.Invoke(player, GetSetting(player));
    public override bool IsVisibleToPlayer(Player player) => isVisible == null || isVisible(player);
    protected override string GetFirstButtonText(Player player) => firstButtonText;
    protected override string GetSecondButtonText(Player player) => secondButtonText;
    protected override bool GetIsSecondsButtonDefault(Player player) => isSecondButtonDefault;
}