using Fralle.Core;
using Fralle.Core.CameraControls;
using System;
using UnityEngine;
using UnityEngine.Rendering;
using Object = UnityEngine.Object;

namespace Fralle.PingTap
{
  [Serializable]
  public class DamageIndicatorHandler
  {
    [SerializeField] VolumeProfile postProcess;
    [SerializeField] ShakeTransformEventData bodyshotShake;
    [SerializeField] ShakeTransformEventData headshotShake;

    DamageController damageController;
    PlayerCamera playerCamera;
    Volume damageVolume;

    bool invalidConfiguration => !postProcess || !bodyshotShake || !headshotShake;

    public void Setup(DamageController damageController, PostProcessController postProcessController)
    {
      if (invalidConfiguration)
        return;

      this.damageController = damageController;
      this.damageController.OnReceiveAttack += HandleReceiveAttack;
      this.damageController.OnHealthChange += HandleHealthChange;

      damageVolume = postProcessController.AddProfile(postProcess);

      playerCamera = Object.FindObjectOfType<PlayerCamera>();
    }

    public void Clean()
    {
      if (invalidConfiguration)
        return;

      damageController.OnReceiveAttack -= HandleReceiveAttack;
      damageController.OnHealthChange -= HandleHealthChange;
    }

    void HandleHealthChange(float currentHealth, float totalHealth)
    {
      damageVolume.weight = 1 - currentHealth / totalHealth;
    }

    void HandleReceiveAttack(DamageController dc, DamageData dd)
    {
      playerCamera.AddShakeEvent(dd.HitArea == HitArea.Head ? headshotShake : bodyshotShake);
    }
  }
}
