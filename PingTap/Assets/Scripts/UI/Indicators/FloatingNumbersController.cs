using Fralle.Attack.Offense;
using System.Globalization;
using UnityEngine;

namespace Fralle.UI.Indicators
{
  public class FloatingNumbersController : MonoBehaviour
  {
    [SerializeField] FloatingText prefab;
    new Camera camera;

    void Awake()
    {
      camera = Camera.main;
      Health.OnAnyDamage += AddFloatingNumber;
    }

    void AddFloatingNumber(Damage damage)
    {
      var floatingText = Instantiate(prefab, transform);
      var damageText = Mathf.Round(damage.damageAmount).ToString(CultureInfo.InvariantCulture);
      floatingText.Setup(damageText, damage.position, camera);
    }

    void OnDestroy()
    {
      Health.OnAnyDamage -= AddFloatingNumber;
    }
  }
}