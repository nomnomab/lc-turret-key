namespace Nomnom.TurretKey;

[BepInPlugin(GeneratedPluginInfo.Identifier, GeneratedPluginInfo.Name, GeneratedPluginInfo.Version)]
public sealed class Plugin : BaseUnityPlugin {
    private void Awake() {
        Harmony.CreateAndPatchAll(typeof(KeyPatches), GeneratedPluginInfo.Identifier);
        Harmony.CreateAndPatchAll(typeof(TurretPatches), GeneratedPluginInfo.Identifier);
        Logger.LogInfo("Loaded TurretKey");
    }
}