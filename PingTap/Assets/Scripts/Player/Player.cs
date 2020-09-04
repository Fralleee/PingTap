using CombatSystem.Combat;
using Fralle.Core.Extensions;
using UnityEngine;

namespace Fralle
{
  public class Player : MonoBehaviour
  {
    [SerializeField] Transform ui = null;
    [SerializeField] LayerMask ignoreLayers;

    [SerializeField] GameObject crosshair = null;
    [SerializeField] GameObject resourceUi = null;
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
      gameObject.SetLayerRecursively(layer, ignoreLayers);

      camera = Camera.main;
      SetupUi();
    }

    void SetupUi()
    {
      Instantiate(crosshair, ui);
      Instantiate(resourceUi, ui);
      Instantiate(minimapUi, ui);
      Instantiate(compassUi, ui);
    }
  }
}