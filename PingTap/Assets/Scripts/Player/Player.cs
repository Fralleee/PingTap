using UnityEngine;

namespace Fralle
{
  public class Player : MonoBehaviour
  {
    [SerializeField] GameObject characterUI;
    [SerializeField] GameObject resourceUI;

    void Start()
    {
      int layer = LayerMask.NameToLayer("Self");
      gameObject.SetLayerRecursively(layer);

      SetupUI();
    }

    void SetupUI()
    {
      var ui = new GameObject("UI");
      ui.transform.parent = transform;
      Instantiate(characterUI, ui.transform);
      Instantiate(resourceUI, ui.transform);
    }
  }
}