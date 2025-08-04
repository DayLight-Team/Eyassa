using Exiled.API.Features;
using Eyassa.Interfaces;
using Eyassa.Models;

namespace Eyassa.Test.Options;

public class TestNode : SettingNode
{

    public override string GetHeaderName(Player player) => "Test node";
    public override string GetHeaderHintDescription(Player player) => "Is test";

    public override IOption[] Options { get; } =
    [
        new TestButton(), new TestSlider(), new TestDropdown(), new TestInputText(), new TestTextArea(), new TestTwoButton()
    ];
}