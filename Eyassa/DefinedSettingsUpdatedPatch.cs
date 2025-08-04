using HarmonyLib;
using UserSettings.ServerSpecific;

namespace Eyassa;

[HarmonyPatch(typeof(ServerSpecificSettingsSync), nameof(ServerSpecificSettingsSync.DefinedSettings),
    MethodType.Setter)]
public static class DefinedSettingsUpdatedPatch
{
    private static void Prefix(ServerSpecificSettingBase[] value)
    {
        IdManager.UsedNumbers.Clear();
        foreach (var setting in value)
        {
            IdManager.UsedNumbers.Add(setting.SettingId);
        }
    }
}