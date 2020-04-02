using UnityEngine;

namespace Fralle
{
  public class AddComponent : MonoBehaviour
  {
    [SerializeField] Behaviour[] components;
    [SerializeField] BehaviourLifecycle lifecycle;

    void Awake()
    {
      if (lifecycle == BehaviourLifecycle.Awake) AddBehaviours();
    }

    void Start()
    {
      if (lifecycle == BehaviourLifecycle.Start) AddBehaviours();
    }

    void OnEnable()
    {
      if (lifecycle == BehaviourLifecycle.OnEnable) AddBehaviours();
    }

    void AddBehaviours()
    {
      foreach (var behaviour in components)
      {
        var type = behaviour.GetType();
        gameObject.AddComponent(type);
      }
    }
  }
}