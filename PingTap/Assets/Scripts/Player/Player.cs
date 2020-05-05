
using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Fralle
{
  public class Player : MonoBehaviour
  {
    [SerializeField] GameObject crosshair;
    [SerializeField] GameObject resourceUi;
    [SerializeField] GameObject enemyHealthBarUi;
    [SerializeField] GameObject damageNumbersUi;
    [SerializeField] GameObject menuUi;
    [SerializeField] GameObject minimapUi;

    [HideInInspector] public new Camera camera;
    [HideInInspector] public ToggleBehaviours toggleBehaviours;

    void Awake()
    {
      toggleBehaviours = GetComponent<ToggleBehaviours>();
    }

    void Start()
    {
      int layer = LayerMask.NameToLayer("Self");
      int ignoreLayer = LayerMask.NameToLayer("First Person Objects");
      gameObject.SetLayerRecursively(layer, ignoreLayer);

      camera = Camera.main;
      SetupUi();
    }

    void SetupUi()
    {
      var ui = new GameObject("UI");
      ui.transform.parent = transform;
      Instantiate(crosshair, ui.transform);
      Instantiate(resourceUi, ui.transform);
      Instantiate(enemyHealthBarUi, ui.transform);
      Instantiate(damageNumbersUi, ui.transform);
      Instantiate(minimapUi, ui.transform);
      Instantiate(menuUi, ui.transform);
    }
  }
}