using System;
using UnityEngine;

namespace Fralle.PingTap
{
  [Serializable]
  public class DamageAudioHandler
  {
    [SerializeField] AudioClip damageSound;
    [SerializeField] AudioClip deathSound;

    DamageController damageController;
    AudioSource audioSource;

    public void Setup(DamageController damageController)
    {
      this.damageController = damageController;
      this.damageController.OnDamageTaken += HandleDamageTaken;
      this.damageController.OnDeath += HandleDeath;

      audioSource = this.damageController.GetComponent<AudioSource>();
    }

    public void Clean()
    {
      damageController.OnDamageTaken -= HandleDamageTaken;
      damageController.OnDeath -= HandleDeath;
    }

    void HandleDamageTaken(DamageController damageController, DamageData damageData)
    {
      if (!audioSource || !damageSound)
        return;

      audioSource.clip = damageSound;
      audioSource.Play();
    }

    void HandleDeath(DamageController damageController, DamageData damageData)
    {
      if (!audioSource || !deathSound)
        return;

      audioSource.clip = deathSound;
      audioSource.Play();
    }
  }
}
