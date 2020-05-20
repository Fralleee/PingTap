using UnityEngine;

namespace Fralle.Core.Component
{
  public class ToggleBehaviours : MonoBehaviour
  {
    [SerializeField] Behaviour[] behaviours;

    void DisableBehaviours()
    {
      foreach (var behaviour in behaviours) behaviour.enabled = false;
    }

    void EnableBehaviours()
    {
      foreach (var behaviour in behaviours) behaviour.enabled = true;
    }

    public void Toggle(bool disable)
    {
      if (disable) DisableBehaviours();
      else EnableBehaviours();
    }
  }
}