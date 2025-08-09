using Exiled.API.Features;
using Eyassa.Features.Options;
using TMPro;
using UserSettings.ServerSpecific;

namespace Eyassa.Features.Generics;

public class GenericTextAreaOption : TextAreaOption
{
    private readonly string _label;
    private readonly string? _hint;
    private readonly TextAlignmentOptions _alignment;
    private readonly SSTextArea.FoldoutMode _foldoutMode;
    private readonly Predicate<Player>? _isVisible;

    public GenericTextAreaOption(string label,
        string? hint = null,
        TextAlignmentOptions alignment = TextAlignmentOptions.TopLeft,     SSTextArea.FoldoutMode foldoutMode = SSTextArea.FoldoutMode.NotCollapsable,
        Predicate<Player>? isVisible = null)
    {
        _label = label;
        _hint = hint;
        _alignment = alignment;
        _foldoutMode = foldoutMode;
        _isVisible = isVisible;
    }

    public GenericTextAreaOption(Func<Player, string> label,
        Func<Player, string> hint,
        TextAlignmentOptions alignment = TextAlignmentOptions.TopLeft,     SSTextArea.FoldoutMode foldoutMode = SSTextArea.FoldoutMode.NotCollapsable,
        Predicate<Player>? isVisible = null)
    {

        _labelFunc = label;
        _hintFunc = hint;
        _alignment = alignment;
        _foldoutMode = foldoutMode;
        _isVisible = isVisible;
    }

    private Func<Player, string> _labelFunc;
    private Func<Player, string> _hintFunc;
    
    protected override string GetLabel(Player player) => GenerateLabel(player);
    protected override string? GetHint(Player player) => GenerateHint(player);
    protected override TextAlignmentOptions GetAlignment(Player player) => _alignment;

    protected override SSTextArea.FoldoutMode GetFoldoutMode(Player player) => _foldoutMode;
    public override bool IsVisibleToPlayer(Player player) => _isVisible == null || _isVisible(player);
    private string GenerateLabel(Player player)
    {
        if (_labelFunc != null)
        {
            try
            {
                return _labelFunc(player);
            }
            catch (Exception e)
            {
                return "Error while generating label: " + e;
            }
        }

        return _label;
    }
    private string GenerateHint(Player player)
    {
        if (_hintFunc != null)
        {
            try
            {
                return _hintFunc(player);
            }
            catch (Exception e)
            {
                return "Error while generating hint: " + e;
            }
        }

        return _hint;
    }
}