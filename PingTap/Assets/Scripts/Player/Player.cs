using Fralle.Core.Extensions;
using UnityEngine;

namespace Fralle
{
  public class Player : MonoBehaviour
  {
    [SerializeField] Transform Ui;

    [SerializeField] GameObject crosshair;
    [SerializeField] GameObject resourceUi;
    [SerializeField] GameObject enemyHealthBarUi;
    [SerializeField] GameObject damageNumbersUi;
    [SerializeField] GameObject minimapUi;

    public PlayerStats stats;
    [HideInInspector] public new Camera camera;

    public static void Disable()
    {
      var players = FindObjectsOfType<Player>();
      foreach (var player in players)
      {
        player.gameObject.SetActive(false);
      }
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
  }
}