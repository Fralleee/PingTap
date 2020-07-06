using CombatSystem.Combat.Damage;
using UnityEngine;
using UnityEngine.UI;

namespace Fralle.UI.Indicators
{
  public class HealthBar : MonoBehaviour
  {
    [SerializeField] Image foregroundImage = null;

    DamageController damageController;
    new Camera camera;
    new Renderer renderer;
    Vector3 defaultScale;
    Canvas canvas;
    RectTransform rectTransform;

    public void Initialize(DamageController damageController)
    {
      defaultScale = transform.localScale;

      this.damageController = damageController;
      this.damageController.OnHealthChange += HandleDamageControllerChange;
      this.damageController.OnDeath += HandleDeath;

      camera = Camera.main;

      renderer = this.damageController.gameObject.GetComponentInChildren<Renderer>();
      rectTransform = GetComponent<RectTransform>();
      canvas = GetComponent<Canvas>();
    }

    void HandleDamageControllerChange(float currentHealth, float maxHealth)
    {
      var percentage = currentHealth / maxHealth;
      foregroundImage.fillAmount = percentage;
    }

    void HandleDeath(DamageController damageController, DamageData damageData)
    {
      Destroy(gameObject);
    }

    void LateUpdate()
    {
      var isVisible = ToggleIfVisible();
      if (isVisible) UpdatePosition();
    }

    bool ToggleIfVisible()
    {
      var isVisible = renderer && !renderer.isVisible;
      var isDestroyed = damageController == null || damageController.isDead;
      if (isVisible || isDestroyed)
      {
        canvas.enabled = false;
        return false;
      }
      canvas.enabled = true;
      return true;
    }

    void UpdatePosition()
    {
      var distance = Vector3.Distance(camera.transform.position, damageController.transform.position);
      var yPositionOffset = Mathf.Lerp(2, 3.5f, distance / 40);

      var screenPosition = camera.WorldToScreenPoint(damageController.transform.position + Vector3.up * yPositionOffset);

      // extra check
      if (screenPosition.z < 0)
      {
        canvas.enabled = false;
        return;
      }

      canvas.enabled = true;

      rectTransform.anchoredPosition = screenPosition;

      var scale = Mathf.Lerp(1.6f, 0.8f, distance / 40);
      rectTransform.localScale = defaultScale * scale;
    }

    void OnDestroy()
    {
      damageController.OnHealthChange -= HandleDamageControllerChange;
      damageController.OnDeath -= HandleDeath;
    }
  }
}