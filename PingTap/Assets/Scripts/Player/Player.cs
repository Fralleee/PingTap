using UnityEngine;

namespace Fralle
{
  public class Player : MonoBehaviour
  {
    [SerializeField] GameObject characterUI;
    [SerializeField] GameObject resourceUI;
    [SerializeField] GameObject enemyHealthBarUI;
    [SerializeField] GameObject damageNumbersUI;
    [SerializeField] GameObject menuUI;

    [HideInInspector] public GameObject menu;
    [HideInInspector] public new Camera camera;

    void Start()
    {
      int layer = LayerMask.NameToLayer("Self");
      int ignoreLayer = LayerMask.NameToLayer("First Person Objects");
      gameObject.SetLayerRecursively(layer, ignoreLayer);

      camera = Camera.main;
      SetupUI();
    }

    void SetupUI()
    {
      var ui = new GameObject("UI");
      ui.transform.parent = transform;
      Instantiate(characterUI, ui.transform);
      Instantiate(resourceUI, ui.transform);
      Instantiate(enemyHealthBarUI, ui.transform);
      Instantiate(damageNumbersUI, ui.transform);

      menu = Instantiate(menuUI, ui.transform);
      menu.SetActive(false);
    }
  }
}