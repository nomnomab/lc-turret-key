using UnityEngine;
using Random = UnityEngine.Random;

namespace Nomnom.TurretKey;

[HarmonyPatch(typeof(Turret))]
public static class TurretPatches {
    [HarmonyPatch(nameof(Turret.Start))]
    [HarmonyPostfix]
    private static void StartPostfix(Turret __instance) {
        var triggerCube = new GameObject("Trigger");
        triggerCube.tag = "InteractTrigger";
        triggerCube.layer = LayerMask.NameToLayer("InteractableObject");
        triggerCube.transform.SetParent(__instance.transform, false);
        
        var turretBoxCollider = __instance.GetComponent<BoxCollider>();
        var boxCollider = triggerCube.AddComponent<BoxCollider>();
        boxCollider.size = turretBoxCollider.size;
        boxCollider.center = turretBoxCollider.center;
        boxCollider.isTrigger = true;
        
        var trigger = triggerCube.AddComponent<InteractTrigger>();
        trigger.interactable = false;
    }
    
    [HarmonyPatch(nameof(Turret.Update))]
    [HarmonyPostfix]
    private static void UpdatePostfix(Turret __instance) {
        var trigger = __instance.gameObject.GetComponentInChildren<InteractTrigger>();
        if (!trigger) return;

        var localPlayer = GameNetworkManager.Instance.localPlayerController;
        var heldItem = localPlayer.currentlyHeldObjectServer;
        var isHoldingKey = heldItem && heldItem.TryGetComponent(out KeyItem _);
        var isEnabled = __instance.turretActive;

        trigger.disabledHoverTip = isHoldingKey && isEnabled ? "Disable turret : [ LMB ]" : string.Empty;
        trigger.oneHandedItemAllowed = true;
    }
    
    [HarmonyPatch(nameof(Turret.ToggleTurretEnabledLocalClient))]
    [HarmonyPostfix]
    private static void ToggleTurretEnabledLocalClientPostfix(Turret __instance, bool enabled) {
        if (enabled) return;
        
        __instance.turretModeLastFrame = TurretMode.Detection;
        __instance.turretMode = TurretMode.Detection;
        __instance.rotatingClockwise = false;
        __instance.mainAudio.clip = null;
        __instance.farAudio.clip = null;
        __instance.berserkAudio.Stop();
        if (__instance.fadeBulletAudioCoroutine != null) {
            __instance.StopCoroutine(__instance.fadeBulletAudioCoroutine);
        }
        __instance.fadeBulletAudioCoroutine = __instance.StartCoroutine(__instance.FadeBulletAudio());
        __instance.bulletParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        __instance.rotationSpeed = 28f;
        __instance.rotatingSmoothly = true;
        __instance.turretAnimator.SetInteger("TurretMode", 0);
        __instance.turretInterval = Random.Range(0f, 0.15f);
    }
    
}