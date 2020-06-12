using Fralle.Core.Extensions;
using Fralle.Movement.Moves;
using UnityEngine;

namespace Fralle.Player
{
  public class PlayerMain : MonoBehaviour
  {
    [SerializeField] Transform ui = null;

    [SerializeField] GameObject crosshair = null;
    [SerializeField] GameObject resourceUi = null;
    [SerializeField] GameObject enemyHealthBarUi = null;
    [SerializeField] GameObject damageNumbersUi = null;
    [SerializeField] GameObject minimapUi = null;
    [SerializeField] GameObject compassUi = null;

    public PlayerStats stats;
    [HideInInspector] public new Camera camera;

    public float extraAccuracy = 1f;

    public static void Disable()
    {
      var players = FindObjectsOfType<PlayerMain>();
      foreach (var player in players)
      {
        player.gameObject.SetActive(false);
      }
    }

    void Awake()
    {
      MovementCrouch.OnCrouch += HandleCrouch;
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

    void HandleCrouch(bool isCrouching)
    {
      extraAccuracy = isCrouching ? 1.5f : 1f;
    }
  }
}