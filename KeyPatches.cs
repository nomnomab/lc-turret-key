using UnityEngine;
using Random = UnityEngine.Random;

namespace Nomnom.TurretKey;

[HarmonyPatch(typeof(KeyItem))]
public static class KeyPatches
{
    [HarmonyPatch(nameof(KeyItem.ItemActivate))]
    [HarmonyPostfix]
    private static void ItemActivatePostfix(KeyItem __instance) {
        if (!__instance.playerHeldBy) return;
        if (!__instance.IsOwner) return;
        
        var playerHeldBy = __instance.playerHeldBy;
        var transform = playerHeldBy.gameplayCamera.transform;
        var ray = new Ray(transform.position, transform.forward);
        if (!Physics.Raycast(ray, out var hit, 3f, LayerMask.GetMask("MapHazards"))) {
            return;
        }

        if (!hit.transform.TryGetComponent(out Turret turret)) return;

        if (!turret.turretActive) return;

        if (Plugin.ResetTurretTime?.Value > 0)
            turret.StartCoroutine(TurnOffAndOnTurret(turret, Plugin.ResetTurretTime.Value));
        else
            turret.ToggleTurretEnabled(false);

        var chance = Random.value;
        if (chance <= (Plugin.ChanceForKeyToBreak?.Value ?? 1))
        {
            if (__instance.itemProperties.dropSFX)
            {
                var tmpAudio = SoundManager.Instance.tempAudio1;
                tmpAudio.transform.position = turret.transform.position;
                tmpAudio.PlayOneShot(__instance.itemProperties.dropSFX);
            }

            playerHeldBy.DespawnHeldObject();
        }

    }

    private static IEnumerator TurnOffAndOnTurret(Turret turret, float restartTime)
    {
        turret.ToggleTurretEnabled(false);
        yield return new WaitForSeconds(restartTime);
        turret.ToggleTurretEnabled(true);
    }

}