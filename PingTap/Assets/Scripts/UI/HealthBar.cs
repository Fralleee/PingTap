using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Fralle
{
  public class HealthBar : MonoBehaviour
  {
    [SerializeField] Image foregroundImage;

    DamageController damageController;
    new Camera camera;
    new Renderer renderer;
    Vector3 defaultScale;
    Canvas canvas;

    public void Initialize(DamageController damageController)
    {
      defaultScale = transform.localScale;

      this.damageController = damageController;
      this.damageController.OnHealthChange += HandleHealthChange;
      this.damageController.OnDeath += HandleDeath;

      camera = Camera.main;

      renderer = damageController.gameObject.GetComponentInChildren<Renderer>();
      canvas = GetComponent<Canvas>();
    }

    void HandleHealthChange(float currentHealth, float maxHealth)
    {
      float percentage = currentHealth / maxHealth;
      foregroundImage.fillAmount = percentage;
    }

    void HandleDeath(DamageController damageController, DamageData damageData)
    {
      Destroy(gameObject);
    }

    void LateUpdate()
    {
      bool isVisible = ToggleIfVisible();
      if (isVisible) UpdatePosition();
    }

    bool ToggleIfVisible()
    {
      bool isvisible = renderer && !renderer.isVisible;
      bool isDestroyed = damageController == null || damageController.isDead;
      if (isvisible || isDestroyed)
      {
        canvas.enabled = false;
        return false;
      }
      canvas.enabled = true;
      return true;
    }

    void UpdatePosition()
    {
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