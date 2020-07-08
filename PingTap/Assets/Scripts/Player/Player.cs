using CombatSystem.Combat;
using Fralle.Core.Extensions;
using UnityEngine;

namespace Fralle
{
  public class Player : MonoBehaviour
  {
    [SerializeField] Transform ui = null;

    [SerializeField] GameObject crosshair = null;
    [SerializeField] GameObject resourceUi = null;
    [SerializeField] GameObject enemyHealthBarUi = null;
    [SerializeField] GameObject damageNumbersUi = null;
    [SerializeField] GameObject minimapUi = null;
    [SerializeField] GameObject compassUi = null;

    [HideInInspector] public new Camera camera;
    [HideInInspector] public Combatant combatant;

    public static void Disable()
    {
      var players = FindObjectsOfType<Player>();
      foreach (var player in players)
      {
        player.gameObject.SetActive(false);
      }
    }

    void Awake()
    {
      combatant = GetComponent<Combatant>();
    }

    void Start()
    {
      var layer = LayerMask.NameToLayer("Player");
      var ignoreLayer = LayerMask.NameToLayer("First Person Objects");
      gameObject.SetLayerRecursively(layer, ignoreLayer);

      camera = Camera.main;
      SetupUi();
    }

    void SetupUi()
    {
      Instantiate(crosshair, ui);
      Instantiate(resourceUi, ui);
      Instantiate(enemyHealthBarUi, ui);
      Instantiate(damageNumbersUi, ui);
      Instantiate(minimapUi, ui);
      Instantiate(compassUi, ui);
    }
  }
}