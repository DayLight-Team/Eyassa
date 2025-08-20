using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using Eyassa.Features.Options;

namespace Eyassa.Features.Generics;

public class GenericSliderOption(
    string customId,
    string label,
    float min,
    float max,
    float defaultValue,
    string? hint = null,
    bool isInteger = false,
    string displayFormat = "{0}",
    string stringFormat = "0.##",
    Action<Player, float>? onChanged = null,
    Predicate<Player>? isVisible = null) : SliderOption
{
    public override string CustomId { get; } = customId;
    protected override string GetLabel(Player player) => label;
    protected override string? GetHint(Player player) => hint;
    protected override string GetDisplayFormat(Player player) => displayFormat;

    protected override bool GetIsInteger(Player player) => isInteger;

    protected override string GetStringFormat(Player player) => stringFormat;
    

    protected override void OnValueChanged(Player player, float value) => onChanged?.Invoke(player, value);
    public override bool IsVisibleToPlayer(Player player) => isVisible == null || isVisible(player);
    protected override float GetMin(Player player) => min;
    protected override float GetMax(Player player) => max;

    protected override float GetDefaultValue(Player player) => defaultValue;
}