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
      if (Input.GetKeyDown(menuButton))
      {
        menuIsOpen = !menuIsOpen;
        if (!menuIsOpen) EnableBehaviours(); // Disable stuff if we press Escape
        else DisableBehaviours(); // Disable stuff if we press Escape
      }
    }

    void DisableBehaviours()
    {
      overlay.SetActive(true);
      foreach (var behaviour in behaviours) behaviour.enabled = false;

      var actionBehaviours = gameObject.GetComponentsInChildren<PlayerAction>();
      foreach (var action in actionBehaviours) action.enabled = false;
    }

    void EnableBehaviours()
    {
      overlay.SetActive(false);
      foreach (var behaviour in behaviours) behaviour.enabled = true;

      var actionBehaviours = gameObject.GetComponentsInChildren<PlayerAction>();
      foreach (var action in actionBehaviours) action.enabled = true;
    }

    void OnApplicationFocus(bool hasFocus)
    {
      if (hasFocus && !menuIsOpen) EnableBehaviours(); // Enable stuff on Alt+tab if we didnt press escape
      else DisableBehaviours(); // Disable stuff on alt+tab if the window is not in focus
    }


  }
}