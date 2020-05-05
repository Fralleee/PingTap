using Fralle.Attack.Offense;
using UnityEngine;
using UnityEngine.UI;

namespace Fralle.UI.Indicators
{
  public class HealthBar : MonoBehaviour
  {
    [SerializeField] Image foregroundImage;

    Health health;
    new Camera camera;
    new Renderer renderer;
    Vector3 defaultScale;
    Canvas canvas;
    RectTransform rectTransform;

    public void Initialize(Health health)
    {
      defaultScale = transform.localScale;

      this.health = health;
      this.health.OnHealthChange += HandleHealthChange;
      this.health.OnDeath += HandleDeath;

      camera = Camera.main;

      renderer = health.gameObject.GetComponentInChildren<Renderer>();
      rectTransform = GetComponent<RectTransform>();
      canvas = GetComponent<Canvas>();
    }

    void HandleHealthChange(float currentHealth, float maxHealth)
    {
      float percentage = currentHealth / maxHealth;
      foregroundImage.fillAmount = percentage;
    }

    void HandleDeath(Health health, Damage damage)
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
      bool isDestroyed = health == null || health.isDead;
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
      float distance = Vector3.Distance(camera.transform.position, health.transform.position);
      float yPositionOffset = Mathf.Lerp(2, 3.5f, distance / 40);

      var screenPosition = camera.WorldToScreenPoint(health.transform.position + Vector3.up * yPositionOffset);

      // extra check
      if (screenPosition.z < 0)
      {
        canvas.enabled = false;
        return;
      }

      canvas.enabled = true;

      rectTransform.anchoredPosition = screenPosition;

      float scale = Mathf.Lerp(1.6f, 0.8f, distance / 40);
      rectTransform.localScale = defaultScale * scale;
    }

    void OnDestroy()
    {
      health.OnHealthChange -= HandleHealthChange;
      health.OnDeath -= HandleDeath;
    }
  }
}