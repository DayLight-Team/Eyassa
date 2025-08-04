using Eyassa.Managers;
using HarmonyLib;
using UserSettings.ServerSpecific;

namespace Eyassa;

[HarmonyPatch(typeof(ServerSpecificSettingsSync), nameof(ServerSpecificSettingsSync.DefinedSettings),
    MethodType.Setter)]
public static class DefinedSettingsPatch
{
    private static void Prefix(ServerSpecificSettingBase[] value)
    {
        IdManager.UsedNumbers.Clear();
        foreach (var setting in value)
        {
            if(IdManager.UsedNumbers.Contains(setting.SettingId))
                continue;
            IdManager.UsedNumbers.Add(setting.SettingId);
        }
    }
}