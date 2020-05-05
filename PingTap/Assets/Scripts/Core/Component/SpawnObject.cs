using Fralle.Core.Enums;
using UnityEngine;

namespace Fralle.Core.Component
{
  public class SpawnObject : MonoBehaviour
  {
    [SerializeField] GameObject[] objects;
    [SerializeField] bool asChild;
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
      foreach (var obj in objects)
      {
        if (asChild) Instantiate(obj, transform);
        else Instantiate(obj);

      }
    }
  }
}