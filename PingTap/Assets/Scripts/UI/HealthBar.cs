using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Fralle
{
  public class HealthBar : MonoBehaviour
  {
    [SerializeField] Image foregroundImage;
    [SerializeField] Image changeIndicator;

    float oldValue = 1f;
    const float updateSpeedSeconds = 0.2f;

    DamageController damageController;
    Canvas canvas;

    void Awake()
    {
      damageController = GetComponentInParent<DamageController>();
      damageController.OnHealthChange += HandleHealthChange;
      damageController.OnDeath += HandleDeath;

      canvas = GetComponent<Canvas>();
      canvas.enabled = false;
    }

    void HandleHealthChange(float currentHealth, float maxHealth, bool animate)
    {
      canvas.enabled = true;
      float percentage = currentHealth / maxHealth;
      if (animate)
      {
        StopAllCoroutines();
        StartCoroutine(percentage < oldValue ? AnimateHealthLoss(percentage) : AnimateHealthGain(percentage));
      }
      else
      {
        changeIndicator.fillAmount = percentage;
        foregroundImage.fillAmount = percentage;
      }

      oldValue = percentage;
    }

    void HandleDeath(DamageController damageController, DamageData damageData)
    {
      Destroy(gameObject);
    }

    void OnDestroy()
    {
      damageController.OnHealthChange -= HandleHealthChange;
      damageController.OnDeath -= HandleDeath;
    }

    IEnumerator AnimateHealthGain(float percentage)
    {
      changeIndicator.fillAmount = percentage;
      float beforeChange = foregroundImage.fillAmount;
      float elapsed = -0.25f;

      while (elapsed < updateSpeedSeconds)
      {
        elapsed += Time.deltaTime;
        foregroundImage.fillAmount = Mathf.Lerp(beforeChange, percentage, elapsed / updateSpeedSeconds);
        yield return null;
      }

      foregroundImage.fillAmount = percentage;
    }

    IEnumerator AnimateHealthLoss(float percentage)
    {
      foregroundImage.fillAmount = percentage;
      float beforeChange = changeIndicator.fillAmount;
      float elapsed = -0.5f;
      while (elapsed < updateSpeedSeconds)
      {
        elapsed += Time.deltaTime;
        changeIndicator.fillAmount = Mathf.Lerp(beforeChange, percentage, elapsed / updateSpeedSeconds);
        yield return null;
      }
      changeIndicator.fillAmount = percentage;
    }

    void LateUpdate()
    {
      transform.LookAt(Camera.main.transform);
      transform.Rotate(0, 180, 0);
    }

  }

}