using Fralle.Core.Infrastructure;
using System.Collections.Generic;
using UnityEngine;

public partial class ObjectPooler : Singleton<ObjectPooler>
{

  public List<Pool> pools = new List<Pool>();
  public Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();

  void Start()
  {
    foreach (var pool in pools)
    {
      var objectPool = new Queue<GameObject>();
      for (int i = 0; i < pool.size; i++)
      {
        GameObject obj = Instantiate(pool.prefab);
        obj.SetActive(false);
        objectPool.Enqueue(obj);
      }

      poolDictionary.Add(pool.tag, objectPool);
    }
  }

  public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation, Transform parent = null)
  {
    if (!poolDictionary.ContainsKey(tag))
    {
      Debug.LogWarning($"Pool with tag {tag} does not exist");
      return null;
    }

    GameObject objectToSpawn = poolDictionary[tag].Dequeue();

    objectToSpawn.SetActive(true);
    objectToSpawn.transform.position = position;
    objectToSpawn.transform.rotation = rotation;
    if (parent) objectToSpawn.transform.SetParent(parent);

    poolDictionary[tag].Enqueue(objectToSpawn);

    return objectToSpawn;
  }

}
