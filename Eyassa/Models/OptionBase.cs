using System.Reflection;
using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using Eyassa.Attributes;
using Eyassa.Interfaces;

namespace Eyassa.Models;

public abstract class OptionBase<T> : IOption where T : SettingBase
{
    public virtual int Id { get; }
    private string _customId;
    public string CustomId => _customId;
    
    public void Init()
    {
        var type = GetType();
        var attribute = type.GetCustomAttribute<OptionAttribute>();
        if (attribute == null)
        {
            Log.Error($"Option {type.Name} does not have a custom id");
            return;
        }
        _customId = attribute.CustomId;
    }

    public abstract void UpdateOption(Player player, bool overrideValue = true);
    protected abstract T BuildBase(Player player);
}