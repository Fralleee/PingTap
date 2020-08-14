using Fralle.Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace Fralle.UI.HUD
{
  public class NexusHealthUi : MonoBehaviour
  {
    [SerializeField] Image foregroundImage = null;

    HeadQuarters playerHome;

    void Awake()
    {
      playerHome = FindObjectOfType<HeadQuarters>();
    }

    void Update()
    {
      if (!playerHome) return;
      playerHome.damageController.OnHealthChange += UpdateHealthbar;
    }

    public void UpdateHealthbar(float currentHealth, float maxHealth)
    {
      var percentage = currentHealth / maxHealth;
      foregroundImage.fillAmount = percentage;
    }
  }
}