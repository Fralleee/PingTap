using UnityEngine;

namespace Fralle
{
  public class ToggleBehaviours : MonoBehaviour
  {
    [SerializeField] Behaviour[] behaviours;
    [SerializeField] KeyCode menuButton = KeyCode.Escape;
    [SerializeField] GameObject overlay;

    bool menuIsOpen;

    void Update()
    {
      if (!Input.GetKeyDown(menuButton)) return;
      menuIsOpen = !menuIsOpen;
      if (!menuIsOpen) EnableBehaviours(); // Disable stuff if we press Escape
      else DisableBehaviours(); // Disable stuff if we press Escape
    }

    void DisableBehaviours()
    {
      if (overlay != null) overlay.SetActive(true);
      foreach (Behaviour behaviour in behaviours) behaviour.enabled = false;

      PlayerAction[] actionBehaviours = gameObject.GetComponentsInChildren<PlayerAction>();
      foreach (PlayerAction action in actionBehaviours) action.enabled = false;
    }

    void EnableBehaviours()
    {
      if (overlay != null) overlay.SetActive(false);
      foreach (Behaviour behaviour in behaviours) behaviour.enabled = true;

      PlayerAction[] actionBehaviours = gameObject.GetComponentsInChildren<PlayerAction>();
      foreach (PlayerAction action in actionBehaviours) action.enabled = true;
    }

    void OnApplicationFocus(bool hasFocus)
    {
      if (hasFocus && !menuIsOpen) EnableBehaviours(); // Enable stuff on Alt+tab if we didn't press escape
      else DisableBehaviours(); // Disable stuff on alt+tab if the window is not in focus
    }


  }
}