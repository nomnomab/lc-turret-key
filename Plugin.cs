using BepInEx.Logging;

namespace Nomnom.TurretKey;

[BepInPlugin(GeneratedPluginInfo.Identifier, GeneratedPluginInfo.Name, GeneratedPluginInfo.Version)]
public sealed class Plugin : BaseUnityPlugin {
    public static ManualLogSource Logger { get; private set; }
    
    private void Awake() {
        Logger = base.Logger;
        Harmony.CreateAndPatchAll(typeof(KeyPatches), GeneratedPluginInfo.Identifier);
        Harmony.CreateAndPatchAll(typeof(TurretPatches), GeneratedPluginInfo.Identifier);
        Logger.LogInfo("Loaded TurretKey");
    }
}