using Fralle.Attack;
using UnityEngine;

namespace Fralle
{
  public class ToggleBehaviours : MonoBehaviour
  {
    [SerializeField] Behaviour[] behaviours;

    void DisableBehaviours()
    {
      foreach (Behaviour behaviour in behaviours) behaviour.enabled = false;
      Action[] actionBehaviours = gameObject.GetComponentsInChildren<Action>();
      foreach (Action action in actionBehaviours) action.enabled = false;
    }

    void EnableBehaviours()
    {
      foreach (Behaviour behaviour in behaviours) behaviour.enabled = true;
      Action[] actionBehaviours = gameObject.GetComponentsInChildren<Action>();
      foreach (Action action in actionBehaviours) action.enabled = true;
    }

    public void Toggle(bool disable)
    {
      if (disable) DisableBehaviours();
      else EnableBehaviours();
    }
  }
}