using System.Collections.Generic;
using UnityEngine;

namespace Fralle.Core.Pooling
{
	public class Pool : MonoBehaviour
	{
		public PoolBlock poolBlock;

		[HideInInspector] public Stack<PoolItem> pool;
		[HideInInspector] public List<PoolItem> masterPool; // only used when using EmptyBehavior.ReuseOldest

		int addedObjects;
		int failedSpawns;
		int reusedObjects;
		int peakObjects;
		int origSize;
		int initSize;
		bool loaded;

#if UNITY_EDITOR
		void OnValidate()
		{
			if (!loaded && poolBlock != null && poolBlock.maxSize <= poolBlock.size)
			{
				poolBlock.maxSize = poolBlock.size * 2;
			}
		}
#endif

		void Awake()
		{
			loaded = true;

			// required to allow creation or modification of pools at runtime. (Timing of script creation and initialization can get wonkey)
			if (poolBlock == null)
			{
				poolBlock = new PoolBlock(0, EmptyBehavior.Grow, 0, MaxEmptyBehavior.Fail, null, false);
			}
			else
			{
				poolBlock = new PoolBlock(poolBlock.size, poolBlock.emptyBehavior, poolBlock.maxSize, poolBlock.maxEmptyBehavior, poolBlock.prefab, poolBlock.printLogOnQuit);
			}
			pool = new Stack<PoolItem>();
			masterPool = new List<PoolItem>();

			origSize = Mathf.Max(0, poolBlock.size);
			poolBlock.size = 0;

			for (int i = 0; i < origSize; i++)
			{
				CreateObject(true);
			}
		}

		void Start()
		{
			//Invoke("StatInit", 0); // for logging after dynamic creation of pool objects from other scripts
			StatInit();
		}

		void StatInit()
		{ // for logging after dynamic creation of pool objects from other scripts
			initSize = poolBlock.size - origSize;
		}

		public GameObject Spawn()
		{ // use to call spawn directly from the pool, and also used by the "Spawn" button in the editor
			return Spawn(null, Vector3.zero, Quaternion.identity, false);
		}
		public GameObject Spawn(int? child)
		{ // use to call spawn directly from the pool
			return Spawn(child, Vector3.zero, Quaternion.identity, false);
		}
		public GameObject Spawn(Vector3 pos, Quaternion rot)
		{ // use to call spawn directly from the pool
			return Spawn(null, pos, rot, true);
		}
		public GameObject Spawn(int? child, Vector3 pos, Quaternion rot)
		{ // use to call spawn directly from the pool
			return Spawn(child, pos, rot, true);
		}
		public GameObject Spawn(int? child, Vector3 pos, Quaternion rot, bool usePosRot, Transform parent = null)
		{
			GameObject obj = GetObject();
			if (obj == null)
			{ return null; } // early out

			obj.SetActive(false); // reset item in case object is being reused, has no effect if object is already disabled
			obj.transform.SetParent(parent, false);
			obj.transform.position = usePosRot ? pos : transform.position;
			obj.transform.rotation = usePosRot ? rot : transform.rotation;

			obj.SetActive(true);

			if (child != null && child < obj.transform.childCount)
			{ // activate a specific child
				obj.transform.GetChild((int)child).gameObject.SetActive(true);
			}

			if (peakObjects < poolBlock.size - pool.Count)
			{ peakObjects = poolBlock.size - pool.Count; } // for logging
			return obj;
		}

		public void Despawn(GameObject obj, PooledObject pooledObject)
		{ // return an object back to this pool
			if (obj.transform.parent == transform)
			{ return; } // already in pool
			obj.SetActive(false);
			obj.transform.SetParent(transform, false);
			obj.transform.localPosition = Vector3.zero;
			obj.transform.localRotation = Quaternion.identity;
			pooledObject.CancelInvoke();
			pool.Push(new PoolItem(obj, pooledObject));
		}

		public GameObject GetObject()
		{ // get object from pool, creating one if necessary and if settings allow
			GameObject result = null;
			if (pool.Count == 0)
			{
				if (poolBlock.emptyBehavior == EmptyBehavior.Fail)
				{ failedSpawns++; return null; }

				if (poolBlock.emptyBehavior == EmptyBehavior.ReuseOldest)
				{
					result = FindOldest();
					if (result != null)
					{ reusedObjects++; }
				}

				if (poolBlock.emptyBehavior == EmptyBehavior.Grow)
				{
					if (poolBlock.size >= poolBlock.maxSize)
					{
						if (poolBlock.maxEmptyBehavior == MaxEmptyBehavior.Fail)
						{ failedSpawns++; return null; }
						if (poolBlock.maxEmptyBehavior == MaxEmptyBehavior.ReuseOldest)
						{
							result = FindOldest();
							if (result != null)
							{ reusedObjects++; }
						}
					}
					else
					{
						addedObjects++;
						return CreateObject();
					}
				}
			}
			else
			{
				pool.Peek().refScript.timeSpawned = Time.time;
				return pool.Pop().obj;
			}
			return result;
		}

		GameObject FindOldest()
		{ // will also set timeSpawned for returned object
			GameObject result = null;
			int oldestIndex = 0;
			float oldestTime = Mathf.Infinity;
			if (masterPool.Count > 0)
			{
				for (int i = 0; i < masterPool.Count; i++)
				{
					if (masterPool[i] == null || masterPool[i].obj == null)
					{ continue; } // make sure object still exsists
					if (masterPool[i].refScript.timeSpawned < oldestTime)
					{
						oldestTime = masterPool[i].refScript.timeSpawned;
						result = masterPool[i].obj;
						oldestIndex = i;
					}
				}
				masterPool[oldestIndex].refScript.timeSpawned = Time.time;
			}
			return result;
		}

		public GameObject CreateObject()
		{
			return CreateObject(false);
		}
		public GameObject CreateObject(bool createInPool)
		{ // true when creating an item in the pool without spawing it
			GameObject obj = null;
			if (poolBlock.prefab)
			{
				obj = Instantiate(poolBlock.prefab, transform.position, transform.rotation);
				PooledObject oprScript = obj.GetComponent<PooledObject>();
				if (oprScript == null)
				{ oprScript = obj.AddComponent<PooledObject>(); }
				oprScript.poolScript = this;
				oprScript.timeSpawned = Time.time;
				masterPool.Add(new PoolItem(obj, oprScript));

				if (createInPool)
				{
					pool.Push(new PoolItem(obj, oprScript));
					obj.SetActive(false);
					obj.transform.SetParent(transform, false);
				}
				poolBlock.size++;
			}
			return obj;
		}

		public int GetActiveCount()
		{
			return poolBlock.size - pool.Count;
		}

		public int GetAvailableCount()
		{
			return pool.Count;
		}

		void OnApplicationQuit()
		{
			if (poolBlock.printLogOnQuit)
			{
				PrintLog();
			}
		}

		public void PrintLog()
		{
			Debug.Log(transform.name + ":       Start Size: " + origSize + "    Init Added: " + initSize + "    Grow Objects: " + addedObjects + "    End Size: " + poolBlock.size + "\n" +
				"    Failed Spawns: " + failedSpawns + "    Reused Objects: " + reusedObjects + "     Most objects active at once: " + peakObjects);
		}

	}
}
