using Exiled.API.Features;

namespace Eyassa.Interfaces;

internal interface IValueReceiver
{
    void OnValueChanged(Player player);
}



internal interface IValueReceiver<T> : IValueReceiver
{
    void UpdateValue(T value, bool overrideValue = true, Predicate<Player> filter = null);
}