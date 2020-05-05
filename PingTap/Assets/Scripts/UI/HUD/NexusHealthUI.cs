using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NexusHealthUI : MonoBehaviour
{
  [SerializeField] Image foregroundImage;

  Nexus nexus;

  void Update()
  {
    if (!nexus) return;

    nexus.health.OnHealthChange += UpdateHealthbar;
  }

  public void SetNexus(Nexus nexus)
  {
    this.nexus = nexus;
  }

  public void UpdateHealthbar(float currentHealth, float maxHealth)
  {
    float percentage = currentHealth / maxHealth;
    foregroundImage.fillAmount = percentage;
  }
}
