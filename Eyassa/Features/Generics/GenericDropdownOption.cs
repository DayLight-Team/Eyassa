using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using Eyassa.Features.Options;
using UserSettings.ServerSpecific;

namespace Eyassa.Features.Generics;

public class GenericDropdownOption(
    string customId,
    string label,
    List<string> options,
    string? hint = null,
    int defaultIndex = 0,
    SSDropdownSetting.DropdownEntryType dropdownType = SSDropdownSetting.DropdownEntryType.Regular,
    Action<Player, string>? onChanged = null,
    Predicate<Player>? isVisible = null) : DropdownOption
{
    public override string CustomId { get; } = customId;
    protected override string GetLabel(Player player) => label;
    protected override string? GetHint(Player player) => hint;
    protected override int GetDefaultOptionIndex(Player player) => defaultIndex;
    protected override SSDropdownSetting.DropdownEntryType GetEntryType(Player player) => dropdownType;
    protected override void OnValueChanged(Player player, string value) => onChanged?.Invoke(player, value);
    public override bool IsVisibleToPlayer(Player player) => isVisible == null || isVisible(player);
    protected override List<string> GetOptions(Player player) => options;
}