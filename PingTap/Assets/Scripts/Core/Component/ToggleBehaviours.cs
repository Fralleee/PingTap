using Fralle.Attack;
using UnityEngine;

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

      Action[] actionBehaviours = gameObject.GetComponentsInChildren<Action>();
      foreach (Action action in actionBehaviours) action.enabled = false;
    }

    void EnableBehaviours()
    {
      if (player.menu != null) player.menu.SetActive(false);
      foreach (Behaviour behaviour in behaviours) behaviour.enabled = true;

      Action[] actionBehaviours = gameObject.GetComponentsInChildren<Action>();
      foreach (Action action in actionBehaviours) action.enabled = true;
    }

    public void ToggleMenu()
    {
      menuIsOpen = !menuIsOpen;
      if (menuIsOpen) DisableBehaviours();
      else EnableBehaviours();
    }
  }
}