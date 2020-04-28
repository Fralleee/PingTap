using Fralle.Attack;
using UnityEngine;
using UnityEngine.UI;

namespace Fralle
{
  public class HealthBar : MonoBehaviour
  {
    [SerializeField] Image foregroundImage;

    Health health;
    new Camera camera;
    new Renderer renderer;
    Vector3 defaultScale;
    Canvas canvas;

    public void Initialize(Health health)
    {
      defaultScale = transform.localScale;

      this.health = health;
      this.health.OnHealthChange += HandleHealthChange;
      this.health.OnDeath += HandleDeath;

      camera = Camera.main;

      renderer = health.gameObject.GetComponentInChildren<Renderer>();
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
      float scale = Mathf.Lerp(1.6f, 0.8f, distance / 40);

      transform.localScale = defaultScale * scale;
      transform.position = camera.WorldToScreenPoint(health.transform.position + Vector3.up * yPositionOffset);
    }

    void OnDestroy()
    {
      health.OnHealthChange -= HandleHealthChange;
      health.OnDeath -= HandleDeath;
    }
  }
}