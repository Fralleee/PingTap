using CombatSystem.Combat.Damage;
using System.Globalization;
using UnityEngine;

namespace Fralle.UI.Indicators
{
  public class FloatingNumbersController : MonoBehaviour
  {
    [SerializeField] FloatingText prefab = null;
    new Camera camera;

    void Awake()
    {
      camera = Camera.main;
      DamageController.OnAnyDamage += AddFloatingNumber;
    }

    void AddFloatingNumber(DamageData damageData)
    {
      var floatingText = Instantiate(prefab, transform);
      var damageText = Mathf.Round(damageData.damageAmount).ToString(CultureInfo.InvariantCulture);
      floatingText.Setup(damageText, damageData.position, camera);
    }

    void OnDestroy()
    {
      DamageController.OnAnyDamage -= AddFloatingNumber;
    }
  }
}