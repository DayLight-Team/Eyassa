using Exiled.API.Interfaces;

namespace Eyassa;

public class Configs : IConfig
{
    public bool IsEnabled { get; set; } = true;
    public bool Debug { get; set; } = false;
    public float SettingUpdateInterval { get; set; } = 0.5f;
    public float VisibilityCheckInterval { get; set; } = 2f;
}
