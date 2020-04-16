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
    new Camera camera;
    new Renderer renderer;
    Vector3 defaultScale;

    public void Initialize(DamageController damageController)
    {
      defaultScale = transform.localScale;

      this.damageController = damageController;
      this.damageController.OnHealthChange += HandleHealthChange;
      this.damageController.OnDeath += HandleDeath;

      camera = Camera.main;

      renderer = damageController.gameObject.GetComponentInChildren<Renderer>();
    }

    void HandleHealthChange(float currentHealth, float maxHealth, bool animate)
    {
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
      if (renderer && !renderer.isVisible) return;
      float distance = Vector3.Distance(camera.transform.position, damageController.transform.position);
      float yPositionOffset = Mathf.Lerp(damageController.yLowestOffset, damageController.yHighestOffset, distance / 40);
      float scale = Mathf.Lerp(1.6f, 0.8f, distance / 40);

      transform.localScale = defaultScale * scale;
      transform.position = camera.WorldToScreenPoint(damageController.transform.position + Vector3.up * yPositionOffset);

    }

    void OnDestroy()
    {
      damageController.OnHealthChange -= HandleHealthChange;
      damageController.OnDeath -= HandleDeath;
    }

  }

}