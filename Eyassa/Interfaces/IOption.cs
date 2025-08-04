using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;

namespace Eyassa.Interfaces;

public interface IOption
{
    internal int Id { get; }
    bool IsVisibleToPlayer(Player? player);

    internal SettingBase BuildBase(Player? player);

    void OnFirstUpdate(Player? player);
    void Init();
}