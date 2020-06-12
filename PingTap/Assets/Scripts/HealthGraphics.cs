using UnityEngine;

namespace Fralle.Attack.Offense
{
  public class HealthGraphics : MonoBehaviour
  {
    static readonly int RendererColor = Shader.PropertyToID("_EmissionColor");

    [SerializeField] new Renderer renderer = null;
    [SerializeField] GameObject deathModel = null;

    Health health;
    MaterialPropertyBlock propBlock;
    Color defaultColor;
    Color currentColor;
    float colorLerpTime;

    void Start()
    {
      health = GetComponent<Health>();
      health.OnDamageTaken += HandleDamageTaken;
      health.OnDeath += HandleDeath;

      propBlock = new MaterialPropertyBlock();
      renderer.GetPropertyBlock(propBlock);
      defaultColor = propBlock.GetColor(RendererColor);
    }

    void Update()
    {
      if (!(colorLerpTime > 0)) return;

      currentColor = Color.Lerp(currentColor, defaultColor, 1 - colorLerpTime);
      propBlock.SetColor(RendererColor, currentColor);
      renderer.SetPropertyBlock(propBlock);
      colorLerpTime -= Time.deltaTime * 0.25f;
    }

    void HandleDamageTaken(Health hp, Damage damage)
    {
      if (damage.hitAngle <= 0) return;

      currentColor = Color.white;
      propBlock.SetColor(RendererColor, currentColor);
      renderer.SetPropertyBlock(propBlock);
      colorLerpTime = 1f;
    }

    void HandleDeath(Health hp, Damage damage)
    {
      if (!renderer || !deathModel) return;
      var deathModelInstance = Instantiate(deathModel, transform.position, transform.rotation);
      Destroy(deathModelInstance, 3f);

      foreach (var rigidBody in deathModelInstance.GetComponentsInChildren<Rigidbody>())
      {
        rigidBody.AddForceAtPosition(damage.force, damage.position);
      }

      Destroy(gameObject);
    }

  }
}
