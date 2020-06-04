using Fralle.Core.Extensions;
using Fralle.Movement;
using UnityEngine;

namespace Fralle.Player
{
  public class PlayerMain : MonoBehaviour
  {
    [SerializeField] Transform Ui;

    [SerializeField] GameObject crosshair;
    [SerializeField] GameObject resourceUi;
    [SerializeField] GameObject enemyHealthBarUi;
    [SerializeField] GameObject damageNumbersUi;
    [SerializeField] GameObject minimapUi;

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
      var layer = LayerMask.NameToLayer("Self");
      var ignoreLayer = LayerMask.NameToLayer("First Person Objects");
      gameObject.SetLayerRecursively(layer, ignoreLayer);

      camera = Camera.main;
      SetupUi();
    }

    void SetupUi()
    {
      Instantiate(crosshair, Ui);
      Instantiate(resourceUi, Ui);
      Instantiate(enemyHealthBarUi, Ui);
      Instantiate(damageNumbersUi, Ui);
      Instantiate(minimapUi, Ui);
    }

    void HandleCrouch(bool isCrouching)
    {
      extraAccuracy = isCrouching ? 1.5f : 1f;
    }
  }
}