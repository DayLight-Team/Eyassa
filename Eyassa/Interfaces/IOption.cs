using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;

namespace Eyassa.Interfaces;

public interface IOption
{
    internal int Id { get; }
    public bool IsVisibleToPlayer(Player? player);

    internal SettingBase BuildBase(Player? player);

    internal void OnFirstUpdate(Player? player);
    internal void Register();
}