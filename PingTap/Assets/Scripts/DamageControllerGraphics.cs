using CombatSystem.Combat.Damage;
using UnityEngine;

namespace Fralle
{
  [RequireComponent(typeof(DamageController))]
  public class DamageControllerGraphics : MonoBehaviour
  {
    static readonly int RendererColor = Shader.PropertyToID("_EmissionColor");

    [SerializeField] new Renderer renderer = null;
    [SerializeField] GameObject deathModel = null;

    DamageController damageController;
    MaterialPropertyBlock propBlock;
    Color defaultColor;
    Color currentColor;
    float colorLerpTime;

    void Start()
    {
      damageController = GetComponent<DamageController>();
      damageController.OnDamageTaken += HandleDamageTaken;
      damageController.OnDeath += HandleDeath;

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

    void HandleDamageTaken(DamageController damageController, DamageData damageData)
    {
      if (damageData.hitAngle <= 0) return;

      currentColor = Color.white;
      propBlock.SetColor(RendererColor, currentColor);
      renderer.SetPropertyBlock(propBlock);
      colorLerpTime = 1f;
    }

    void HandleDeath(DamageController damageController, DamageData damageData)
    {
      if (!renderer || !deathModel) return;
      var deathModelInstance = Instantiate(deathModel, transform.position, transform.rotation);
      Destroy(deathModelInstance, 3f);

      foreach (var rigidBody in deathModelInstance.GetComponentsInChildren<Rigidbody>())
      {
        rigidBody.AddForceAtPosition(damageData.force, damageData.position);
      }

      Destroy(gameObject);
    }

  }
}
