using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Loader;

namespace Ayassa;

public class AyassaPlugin : Plugin<Configs, AyassaTranslations>
{
    public override string Name { get; } = "Ayassa";

    public override PluginPriority Priority { get; } = PluginPriority.First;

    public override Version Version { get; } = new(1, 0, 0);

    public AyassaPlugin Instance { get; private set; }
    
    public override void OnEnabled()
    {
        Instance = this;
        base.OnEnabled();
    }
}