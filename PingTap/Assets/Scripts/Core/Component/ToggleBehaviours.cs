using Fralle.Attack.Action;
using UnityEngine;

namespace Fralle.Core.Component
{
  public class ToggleBehaviours : MonoBehaviour
  {
    [SerializeField] Behaviour[] behaviours;

    void DisableBehaviours()
    {
      foreach (var behaviour in behaviours) behaviour.enabled = false;
      Active[] actionBehaviours = gameObject.GetComponentsInChildren<Active>();
      foreach (var action in actionBehaviours) action.enabled = false;
    }

    void EnableBehaviours()
    {
      foreach (var behaviour in behaviours) behaviour.enabled = true;
      Active[] actionBehaviours = gameObject.GetComponentsInChildren<Active>();
      foreach (var action in actionBehaviours) action.enabled = true;
    }

    public void Toggle(bool disable)
    {
      if (disable) DisableBehaviours();
      else EnableBehaviours();
    }
  }
}