using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Loader;

namespace Eyassa;

public class EyassaPlugin : Plugin<Configs, EyassaTranslations>
{
    public override string Name { get; } = "Eyassa";

    public override PluginPriority Priority { get; } = PluginPriority.First;

    public override Version Version { get; } = new(1, 0, 0);

    public EyassaPlugin Instance { get; private set; }
    
    public override void OnEnabled()
    {
        Instance = this;
        base.OnEnabled();
    }
}