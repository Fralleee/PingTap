using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Fralle.Core.Infrastructure
{
  public static class ObjectPool
  {
    static readonly Dictionary<string, GameObject> Pool = new Dictionary<string, GameObject>();

    public static GameObject GetFromPool(GameObject gameObject, Transform transform, Transform parent)
    {
      var objectToPool = Find(gameObject);
      GameObject go;

      if (objectToPool && !objectToPool.activeSelf)
      {
        go = Reuse(gameObject, transform.position, transform.rotation);
        Pool.Remove(gameObject.name);
      }
      else
      {
        go = Object.Instantiate(gameObject, transform.position, transform.rotation, parent);
        go.name = gameObject.name;

        if (!Pool.Keys.Contains(go.name)) Pool.Add(go.name, go);
      }

      return go;
    }

    public static void RemoveFromPool(GameObject gameObject)
    {
      var poolObject = Find(gameObject);
      if (!poolObject) return;

      if (gameObject == poolObject) gameObject.SetActive(false);
      else Object.Destroy(gameObject);
    }

    static GameObject Find(Object go)
    {
      Pool.TryGetValue(go.name, out var result);
      return result;
    }

    static GameObject Reuse(Object go, Vector3 position, Quaternion rotation)
    {
      var obj = Find(go);

      obj.SetActive(true);
      obj.transform.localPosition = position;
      obj.transform.localRotation = rotation;
      return obj;
    }
  }
}