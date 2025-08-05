using System.Collections.Generic;
using Exiled.API.Features;
using Eyassa.Interfaces;
using Eyassa.Models;

namespace Eyassa.Test.Options;

public class TestNode : SettingNode
{

    public override string GetHeaderName(Player player) => "Test node";
    public override string GetHeaderHintDescription(Player player) => "Is test";

    public override List<IOption> Options { get; } = new List<IOption>()
    {
        new TestButton(), new TestSlider(), new TestDropdown(), new TestInputText(), new TestTextArea(),
        new TestTwoButton()
    };
}