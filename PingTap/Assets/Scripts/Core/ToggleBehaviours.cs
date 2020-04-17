using UnityEngine;
using UnityEngine.InputSystem;

namespace Fralle
{
  public class ToggleBehaviours : MonoBehaviour
  {
    [SerializeField] Behaviour[] behaviours;

    Player player;
    bool menuIsOpen;

    void Start()
    {
      player = GetComponent<Player>();
    }

    void Update()
    {
      if (Input.GetKeyDown(KeyCode.Escape)) ToggleMenu();
    }

    void DisableBehaviours()
    {
      if (player.menu != null) player.menu.SetActive(true);
      foreach (Behaviour behaviour in behaviours) behaviour.enabled = false;

      PlayerAction[] actionBehaviours = gameObject.GetComponentsInChildren<PlayerAction>();
      foreach (PlayerAction action in actionBehaviours) action.enabled = false;
    }

    void EnableBehaviours()
    {
      if (player.menu != null) player.menu.SetActive(false);
      foreach (Behaviour behaviour in behaviours) behaviour.enabled = true;

      PlayerAction[] actionBehaviours = gameObject.GetComponentsInChildren<PlayerAction>();
      foreach (PlayerAction action in actionBehaviours) action.enabled = true;
    }

    public void ToggleMenu()
    {
      menuIsOpen = !menuIsOpen;
      if (menuIsOpen) DisableBehaviours();
      else EnableBehaviours();
    }
  }
}