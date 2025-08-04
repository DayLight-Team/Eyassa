using System;
using Exiled.API.Interfaces;
using Eyassa.Test.Options;
using LabApi.Loader.Features.Plugins;

namespace Eyassa.Test;

public class Plugin : Exiled.API.Features.Plugin<TestConfigs>
{

    public override void OnEnabled()
    {
        new TestNode().Register();
        base.OnEnabled();
    }

    public override string Name { get; } = "Eyassa.Test";
    public override string Author { get; } = "Tili :3";
    public override Version Version { get; } = new Version(1, 0, 0);
}

public class TestConfigs : IConfig
{
    public bool IsEnabled { get; set; } = true;
    public bool Debug { get; set; } = false;
}