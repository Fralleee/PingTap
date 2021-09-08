using Fralle.Core.CameraControls;
using Fralle.Gameplay;
using Fralle.PingTap;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Fralle
{
  [SelectionBase]
  public class Player : MonoBehaviour
  {
    [Required] [HideInInspector] public PlayerController playerController;
    [Required] [HideInInspector] public PlayerCamera playerCamera;
    public static PlayerControls controls;

    [SerializeField] PlayerCamera playerCameraPrefab;

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
    }

    void Start()
    {
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

    [Button]
    void SetupLocalPlayer()
    {
      playerController = GetComponent<PlayerController>();
      playerCamera = FindObjectOfType<PlayerCamera>();
      if (playerCamera == null)
      {
#if UNITY_EDITOR
        playerCamera = (PlayerCamera)PrefabUtility.InstantiatePrefab(playerCameraPrefab);
        playerCamera.transform.SetSiblingIndex(transform.GetSiblingIndex());
#else
        playerCamera = Instantiate(playerCameraPrefab).GetComponent<PlayerCamera>();
#endif
      }
      playerCamera.controller = playerController;

      var playerAttack = GetComponent<PlayerAttack>();
      playerAttack.weaponCamera = playerCamera.weaponCamera;

      var followTransformOffset = GetComponentInChildren<FollowTransformOffset>();
      followTransformOffset.transformToFollow = playerCamera.transform;

      var combatant = GetComponent<Combatant>();
      combatant.aimTransform = playerCamera.transform;
      combatant.weaponHolder = playerCamera.weaponHolder;

#if UNITY_EDITOR
      EditorUtility.SetDirty(playerAttack);
      EditorUtility.SetDirty(followTransformOffset);
      EditorUtility.SetDirty(combatant);
#endif
    }
  }
}
