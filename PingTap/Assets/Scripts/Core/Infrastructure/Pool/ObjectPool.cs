using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fralle.Core.Infrastructure
{
	public class ObjectPool : Singleton<ObjectPool>
	{
		[Serializable]
		public struct PreloadedPrefab
		{
			[Tooltip("The prefab that should be preloaded.")] [SerializeField] private GameObject prefab;
			[Tooltip("Number of prefabs to instantiate upon start.")] [SerializeField] private int count;
			public GameObject Prefab { get { return prefab; } }
			public int Count { get { return count; } }
		}

		[Tooltip("Specifies any prefabs that should be preloaded upon start.")] [SerializeField] protected PreloadedPrefab[] preloadedPrefabs;
		public PreloadedPrefab[] PreloadedPrefabs { get { return preloadedPrefabs; } }

		private Dictionary<int, Stack<GameObject>> gameObjectPool = new Dictionary<int, Stack<GameObject>>();
		private Dictionary<int, int> instantiatedGameObjects = new Dictionary<int, int>();
		private Dictionary<Type, object> genericPool = new Dictionary<Type, object>();
		private Dictionary<int, GameObject> originalObjectIDs = new Dictionary<int, GameObject>();


		private void Start()
		{
			if (preloadedPrefabs != null)
			{
				var instantiatedObjects = new List<GameObject>();
				for (int i = 0; i < preloadedPrefabs.Length; ++i)
				{
					if (preloadedPrefabs[i].Prefab == null || preloadedPrefabs[i].Count == 0)
					{
						continue;
					}

					// Create and destroy the preloaded prefab so it'll be ready in the pool.
					for (int j = 0; j < preloadedPrefabs[i].Count; ++j)
					{
						if (j < instantiatedObjects.Count)
						{
							// Reuse the list if possible.
							instantiatedObjects[j] = Instantiate(preloadedPrefabs[i].Prefab);
						}
						else
						{
							instantiatedObjects.Add(Instantiate(preloadedPrefabs[i].Prefab));
						}
					}
					for (int j = 0; j < preloadedPrefabs[i].Count; ++j)
					{
						Destroy(instantiatedObjects[j]);
					}
				}
			}
		}

		/// <summary>
		/// Instantiate a new GameObject. Use the object pool if a previously used GameObject is located in the pool, otherwise instaniate a new GameObject.
		/// </summary>
		/// <param name="original">The original GameObject to pooled a copy of.</param>
		/// <returns>The pooled/instantiated GameObject.</returns>
		public static GameObject Instantiate(GameObject original) => Instantiate(original, Vector3.zero, Quaternion.identity, null);

		/// <summary>
		/// Instantiate a new GameObject. Use the object pool if a previously used GameObject is located in the pool, otherwise instaniate a new GameObject.
		/// </summary>
		/// <param name="original">The original GameObject to pooled a copy of.</param>
		/// <param name="position">The position of the pooled GameObject.</param>
		/// <param name="rotation">The rotation of the pooled Gameobject.</param>
		/// <returns>The pooled/instantiated GameObject.</returns>
		public static GameObject Instantiate(GameObject original, Vector3 position, Quaternion rotation) => Instantiate(original, position, rotation, null);

		/// <summary>
		/// Instantiate a new GameObject. Use the object pool if a previously used GameObject is located in the pool, otherwise instaniate a new GameObject.
		/// </summary>
		/// <param name="original">The original GameObject to pooled a copy of.</param>
		/// <param name="position">The position of the pooled GameObject.</param>
		/// <param name="rotation">The rotation of the pooled Gameobject.</param>
		/// <param name="parent">The parent to assign to the pooled GameObject.</param>
		/// <returns>The pooled/instantiated GameObject.</returns>
		public static GameObject Instantiate(GameObject original, Vector3 position, Quaternion rotation, Transform parent) => Instance.InstantiateInternal(original, position, rotation, parent);

		public static bool InstantiatedWithPool(GameObject instantiatedObject) => Instance.InstantiatedWithPoolInternal(instantiatedObject);
		public static int OriginalInstanceID(GameObject instantiatedObject) => Instance.OriginalInstanceIDInternal(instantiatedObject);

		/// <summary>
		/// Return the specified GameObject back to the ObjectPool.
		/// </summary>
		/// <param name="instantiatedObject">The GameObject to return to the pool.</param>
		public static void Destroy(GameObject instantiatedObject)
		{
			if (Instance == null)
			{
				return;
			}

			Instance.DestroyInternal(instantiatedObject);
		}

		/// <summary>
		/// Returns the original GameObject that the specified object was instantiated from.
		/// </summary>
		/// <param name="instantiatedObject">The GameObject that was instantiated.</param>
		/// <returns>The original GameObject that the specified object was instantiated from.</returns>
		public static GameObject OriginalObject(GameObject instantiatedObject)
		{
			// Objects may be wanting to be destroyed as the game is stopping but the ObjectPool has already been destroyed. Ensure the ObjectPool is still valid.
			if (Instance == null)
			{
				return null;
			}

			return Instance.OriginalObjectInternal(instantiatedObject);
		}

		/// <summary>
		/// Return the object back to the generic object pool.
		/// </summary>
		/// <typeparam name="T">The type of object to return.</typeparam>
		/// <param name="obj">The object to return.</param>
		public static void Return<T>(T obj)
		{
			// Objects may be wanting to be returned as the game is stopping but the ObjectPool has already been destroyed. Ensure the ObjectPool is still valid.
			if (Instance == null)
			{
				return;
			}

			Instance.ReturnInternal<T>(obj);
		}

		/// <summary>
		/// Internal method to spawn a new GameObject. Use the object pool if a previously used GameObject is located in the pool, otherwise instaniate a new GameObject.
		/// </summary>
		/// <param name="original">The original GameObject to pooled a copy of.</param>
		/// <param name="position">The position of the pooled GameObject.</param>
		/// <param name="rotation">The rotation of the pooled Gameobject.</param>
		/// <param name="parent">The parent to assign to the pooled GameObject.</param>
		/// <returns>The pooled/instantiated GameObject.</returns>
		private GameObject InstantiateInternal(GameObject original, Vector3 position, Quaternion rotation, Transform parent)
		{
			var originalInstanceID = original.GetInstanceID();
			var instantiatedObject = ObjectFromPool(originalInstanceID, position, rotation, parent);
			if (instantiatedObject == null)
			{
				instantiatedObject = GameObject.Instantiate(original, position, rotation, parent);
				if (!originalObjectIDs.ContainsKey(originalInstanceID))
				{
					originalObjectIDs.Add(originalInstanceID, original);
				}
			}
			else
			{
				instantiatedObject.transform.position = position;
				instantiatedObject.transform.rotation = rotation;
				instantiatedObject.transform.parent = parent;
			}
			// Map the newly instantiated instance ID to the original instance ID so when the object is returned it knows what pool to go to.
			instantiatedGameObjects.Add(instantiatedObject.GetInstanceID(), originalInstanceID);

			return instantiatedObject;
		}

		/// <summary>
		/// An object is trying to be popped from the object pool. Return the pooled object if it exists otherwise null meaning one needs to be insantiated.
		/// </summary>
		/// <param name="originalInstanceID">The instance id of the GameObject trying to be popped from the pool.</param>
		/// <param name="position">The position of the pooled GameObject.</param>
		/// <param name="rotation">The rotation of the pooled Gameobject.</param>
		/// <param name="parent">The parent to assign to the pooled GameObject.</param>
		/// <returns>The pooled GameObject.</returns>
		private GameObject ObjectFromPool(int originalInstanceID, Vector3 position, Quaternion rotation, Transform parent)
		{
			if (gameObjectPool.TryGetValue(originalInstanceID, out Stack<GameObject> pool))
			{
				while (pool.Count > 0)
				{
					var instantiatedObject = pool.Pop();
					// The object may be null if it was removed from an additive scene. Keep popping from the pool until the pool has a valid object or is empty.
					if (instantiatedObject == null)
					{
						continue;
					}
					instantiatedObject.transform.position = position;
					instantiatedObject.transform.rotation = rotation;
					instantiatedObject.transform.parent = parent;
					instantiatedObject.SetActive(true);
					return instantiatedObject;
				}
			}
			return null;
		}

		private bool InstantiatedWithPoolInternal(GameObject instantiatedObject) => instantiatedGameObjects.ContainsKey(instantiatedObject.GetInstanceID());

		/// <summary>
		/// Internal method to return the instance ID of the prefab used to spawn the instantiated object.
		/// </summary>
		/// <param name="instantiatedObject">The GameObject to get the original instance ID</param>
		/// <returns>The original instance ID</returns>
		private int OriginalInstanceIDInternal(GameObject instantiatedObject)
		{
			var instantiatedInstanceID = instantiatedObject.GetInstanceID();
			if (!instantiatedGameObjects.TryGetValue(instantiatedInstanceID, out int originalInstanceID))
			{
				Debug.LogError("Unable to get the original instance ID of " + instantiatedObject + ": has the object already been placed in the ObjectPool?");
				return -1;
			}
			return originalInstanceID;
		}


		/// <summary>
		/// Internal method to return the specified GameObject back to the ObjectPool. Call the corresponding server or client method.
		/// </summary>
		/// <param name="instantiatedObject">The GameObject to return to the pool.</param>
		private void DestroyInternal(GameObject instantiatedObject)
		{
			var instantiatedInstanceID = instantiatedObject.GetInstanceID();
			if (!instantiatedGameObjects.TryGetValue(instantiatedInstanceID, out int originalInstanceID))
			{
				Debug.LogError("Unable to pool " + instantiatedObject.name + " (instance " + instantiatedInstanceID + "): the GameObject was not instantiated with ObjectPool.Instantiate " + Time.time);
				return;
			}

			// Map the instantiated instance ID back to the orignal instance ID so the GameObject can be returned to the correct pool.
			instantiatedGameObjects.Remove(instantiatedInstanceID);

			DestroyLocal(instantiatedObject, originalInstanceID);
		}

		/// <summary>
		/// Return the specified GameObject back to the ObjectPool.
		/// </summary>
		/// <param name="instantiatedObject">The GameObject to return to the pool.</param>
		/// <param name="originalInstanceID">The instance ID of the original GameObject.</param>
		private void DestroyLocal(GameObject instantiatedObject, int originalInstanceID)
		{
			instantiatedObject.SetActive(false);
			instantiatedObject.transform.parent = transform;

			if (gameObjectPool.TryGetValue(originalInstanceID, out Stack<GameObject> pool))
			{
				pool.Push(instantiatedObject);
			}
			else
			{
				pool = new Stack<GameObject>();
				pool.Push(instantiatedObject);
				gameObjectPool.Add(originalInstanceID, pool);
			}
		}


		/// <summary>
		/// Internal method which returns the original GameObject that the specified object was instantiated from.
		/// </summary>
		/// <param name="instantiatedObject">The GameObject that was instantiated.</param>
		/// <returns>The original GameObject that the specified object was instantiated from.</returns>
		private GameObject OriginalObjectInternal(GameObject instantiatedObject)
		{
			if (!instantiatedGameObjects.TryGetValue(instantiatedObject.GetInstanceID(), out int originalInstanceID))
			{
				return null;
			}

			GameObject original;
			if (!originalObjectIDs.TryGetValue(originalInstanceID, out original))
			{
				return null;
			}

			return original;
		}

		/// <summary>
		/// Get a pooled object of the specified type using a generic ObjectPool.
		/// </summary>
		/// <typeparam name="T">The type of object to get.</typeparam>
		/// <returns>A pooled object of type T.</returns>
		public static T Get<T>()
		{
			return Instance.GetInternal<T>();
		}

		/// <summary>
		/// Internal method to get a pooled object of the specified type using a generic ObjectPool.
		/// </summary>
		/// <typeparam name="T">The type of object to get.</typeparam>
		/// <returns>A pooled object of type T.</returns>
		private T GetInternal<T>()
		{
			if (genericPool.TryGetValue(typeof(T), out object value))
			{
				var pooledObjects = value as Stack<T>;
				if (pooledObjects.Count > 0)
				{
					return pooledObjects.Pop();
				}
			}
			return Activator.CreateInstance<T>();
		}


		/// <summary>
		/// Internal method to return the object back to the generic object pool.
		/// </summary>
		/// <typeparam name="T">The type of object to return.</typeparam>
		/// <param name="obj">The object to return.</param>
		private void ReturnInternal<T>(T obj)
		{
			if (obj == null)
			{
				return;
			}

			if (genericPool.TryGetValue(typeof(T), out object value))
			{
				var pooledObjects = value as Stack<T>;
				pooledObjects.Push(obj);
			}
			else
			{
				var pooledObjects = new Stack<T>();
				pooledObjects.Push(obj);
				genericPool.Add(typeof(T), pooledObjects);
			}
		}

	}
}
