using Fralle.Gameplay;
using Fralle.PingTap;
using UnityEngine;

namespace Fralle
{
  [SelectionBase]
  public class Player : MonoBehaviour
  {
    [HideInInspector] public Camera Camera;
    [HideInInspector] public Combatant Combatant;

    public static PlayerControls controls;

    //GameObject ui;

    public static void Disable()
    {
      Player[] players = FindObjectsOfType<Player>();
      foreach (Player player in players)
      {
        player.gameObject.SetActive(false);
      }
    }

    public static void Toggle(bool enabled)
    {
      if (enabled)
        controls.Enable();
      else
        controls.Disable();
    }

    void Awake()
    {
      controls = new PlayerControls();
      controls.Movement.Enable();
      controls.Ability.Enable();
      controls.Weapon.Enable();

      Combatant = GetComponent<Combatant>();
      //ui = transform.Find("UI").gameObject;
      //ui.SetActive(true);
    }

    void Start()
    {
      Camera = Camera.main;
      //QuantumConsole.Instance.OnActivate += ConsoleActivated;
      //QuantumConsole.Instance.OnDeactivate += ConsoleDeactivated;
    }

    static void OnGamestateChanged(GameState gameState)
    {
    }

    void ConsoleActivated()
    {
      //PlayerInput.DeactivateInput();
    }

    void ConsoleDeactivated()
    {
      //PlayerInput.ActivateInput();
    }

    void OnEnable()
    {
      StateManager.OnGamestateChanged += OnGamestateChanged;
    }

    void OnDisable()
    {
      StateManager.OnGamestateChanged -= OnGamestateChanged;
    }

    void OnDestroy()
    {
      //QuantumConsole.Instance.OnActivate -= ConsoleActivated;
      //QuantumConsole.Instance.OnDeactivate -= ConsoleDeactivated;
    }

  }
}
