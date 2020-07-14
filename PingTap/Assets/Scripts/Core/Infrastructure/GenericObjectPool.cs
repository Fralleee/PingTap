using Fralle.Core.Infrastructure;
using System.Collections.Generic;
using UnityEngine;

public abstract class GenericObjectPool<T> : Singleton<GenericObjectPool<T>> where T : Component
{
  [SerializeField] T prefab;

  Queue<T> objects = new Queue<T>();

  public T Get()
  {
    if (objects.Count == 0) AddObjects(1);
    return objects.Dequeue();
  }

  public void ReturnToPool(T objectToReturn)
  {
    objectToReturn.gameObject.SetActive(false);
    objects.Enqueue(objectToReturn);
  }

  void AddObjects(int count)
  {
    var newObject = Instantiate(prefab);
    newObject.gameObject.SetActive(false);
    objects.Enqueue(newObject);
  }

}
