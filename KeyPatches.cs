using UnityEngine;

namespace Nomnom.TurretKey;

[HarmonyPatch(typeof(KeyItem))]
public static class KeyPatches {
    [HarmonyPatch(nameof(KeyItem.ItemActivate))]
    [HarmonyPostfix]
    private static void ItemActivatePostfix(KeyItem __instance) {
        if (!__instance.playerHeldBy) return;
        if (!__instance.IsOwner) return;
        
        var playerHeldBy = __instance.playerHeldBy;
        var ray = new Ray(playerHeldBy.gameplayCamera.transform.position, playerHeldBy.gameplayCamera.transform.forward);
        if (!Physics.Raycast(ray, out var hit, 3f, LayerMask.GetMask("MapHazards"))) {
            return;
        }

        if (!hit.transform.TryGetComponent(out Turret turret)) return;
        if (!turret.turretActive) return;
        
        turret.ToggleTurretEnabled(false);
        playerHeldBy.DespawnHeldObject();
    }
}