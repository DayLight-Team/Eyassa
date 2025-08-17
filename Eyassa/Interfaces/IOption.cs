using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;

namespace Eyassa.Interfaces;

public interface IOption
{
    internal int Id { get; }
    public string CustomId { get; }
    public bool SendOnJoin { get; }

    public bool IsVisibleToPlayer(Player player);
    internal SettingBase BuildBase(Player? player);
    public bool IsCurrentlyVisible(Player player);
    internal void OnFirstUpdate(Player? player);
    internal bool CheckForUpdate(Player? player);
    internal void UpdateOption(Player? player, bool overrideValue = true);
    internal void Register();
}