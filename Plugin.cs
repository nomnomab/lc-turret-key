using BepInEx.Configuration;

namespace Nomnom.TurretKey;

[BepInPlugin(GeneratedPluginInfo.Identifier, GeneratedPluginInfo.Name, GeneratedPluginInfo.Version)]
public sealed class Plugin : BaseUnityPlugin {
    public static ConfigEntry<float>? ChanceForKeyToBreak { get; private set; }
    public static ConfigEntry<float>? ResetTurretTime { get; private set; }

    private void Awake() {
        Harmony.CreateAndPatchAll(typeof(KeyPatches), GeneratedPluginInfo.Identifier);
        Harmony.CreateAndPatchAll(typeof(TurretPatches), GeneratedPluginInfo.Identifier);

        CreateConfig();

        Logger.LogInfo("Loaded TurretKey");
    }

    private void CreateConfig() {
        ChanceForKeyToBreak = Config.Bind(
            "General",
            "Chance for key to break",
            1f,
            """
            Chance for key to break when used on a turret.
            Should be a value between 0.0 and 1.0.
            0.8 = 80% chance to break
            """
        );
        
        ResetTurretTime = Config.Bind(
            "General",
            "Reset Turret after * sec",
            0f,
            """
            Set time to restart turret
            Vanilla reset time is 5
            """
        );
    }
}