using System.Collections.Generic;
using UnityEngine;

namespace Fralle.Core.Pooling
{
	public class PoolManager : MonoBehaviour
	{
		public bool allowCreate = true;
		public bool allowModify = true;

		[Tooltip("When the scene is stopped, creates a report showing pool usage:\n\n" +
			"Start Size - Size of pool when beginning the scene.\n\n" +
			"Init Added - Number of objects added by InitializeSpawn() at runtime.\n\n" +
			"Grow Objects - Number of objects added with EmptyBehavior.Grow.\n\n" +
			"End Size - Total objects of this pool, active and inactive, at the time of the log report.\n\n" +
			"Failed Spawns - Number of Spawn() requests that didn't return a spawn.\n\n" +
			"Reused Objects - Number of times an object was reused before despawning normally.\n\n" +
			"Most Objects Active - The most items for this pool active at once.")]
		public bool printAllLogsOnQuit;

		[HideInInspector] public Dictionary<GameObject, Pool> poolRef;

		void Awake()
		{
			CheckDict();
		}

		void CheckDict()
		{
			if (poolRef == null)
			{ // dictionary hasn't been created yet
				poolRef = new Dictionary<GameObject, Pool>();
			}
		}

		public bool InitializeSpawn(GameObject obj, float addPool, int minPool, EmptyBehavior emptyBehavior, MaxEmptyBehavior maxEmptyBehavior, bool modBehavior)
		{
			if (obj == null)
			{ return false; }
			CheckDict();
			bool tempModify = false;

			if (poolRef.ContainsKey(obj) && poolRef[obj] == null)
			{ // check for broken reference
				poolRef.Remove(obj); // remove it
			}
			bool result;
			if (poolRef.ContainsKey(obj))
			{
				result = true; // already have refrence
			}
			else
			{
				if (MakePoolRef(obj) == null)
				{ // ref not found
					if (allowCreate)
					{
						CreatePool(obj, 0, 0, emptyBehavior, maxEmptyBehavior);
						tempModify = true; // may modify a newly created pool
						result = true;
					}
					else
					{
						result = false;
					}
				}
				else
				{
					result = true; // ref was created
				}
			}

			if (result && (allowModify || tempModify) && (addPool > 0 || minPool > 0))
			{
				int size = poolRef[obj].poolBlock.size;
				int l1 = 0;
				int l2 = 0;
				if (addPool >= 0)
				{ // not negative
					if (addPool < 1)
					{ // is a percentage
						l2 = Mathf.RoundToInt(size * addPool);
					}
					else
					{ // not a percentage
						l1 = Mathf.RoundToInt(addPool);
					}
				}
				int loop = 0;
				int a = size == 0 ? 0 : Mathf.Max(l1, l2);
				if (size < minPool)
				{ loop = minPool - size; }
				loop += a;
				for (int i = 0; i < loop; i++)
				{
					poolRef[obj].CreateObject(true);
				}
				poolRef[obj].poolBlock.maxSize = poolRef[obj].poolBlock.size * 2;
				if (modBehavior)
				{
					poolRef[obj].poolBlock.emptyBehavior = emptyBehavior;
					poolRef[obj].poolBlock.maxEmptyBehavior = maxEmptyBehavior;
				}
			}

			return result;
		}

		public GameObject Spawn(GameObject obj, int? child, Vector3 pos, Quaternion rot, bool usePosRot, Transform parent)
		{
			if (obj == null)
			{ return null; } // object wasn't defined
			CheckDict();

			if (poolRef.ContainsKey(obj))
			{ // reference already created
				if (poolRef[obj] != null)
				{ // make sure pool still exsists
					return poolRef[obj].Spawn(child, pos, rot, usePosRot, parent); // create spawn
				}
				else
				{ // pool no longer exsists
					poolRef.Remove(obj); // remove reference
					return null;
				}
			}
			else
			{ // ref not yet created
				Pool childScript = MakePoolRef(obj); // create ref
				if (childScript == null)
				{ // ref not found
					return null;
				}
				else
				{
					return childScript.Spawn(child, pos, rot, usePosRot, parent); // create spawn
				}
			}
		}

		Pool MakePoolRef(GameObject obj)
		{ // attempt to create and return script reference
			for (int i = 0; i < transform.childCount; i++)
			{
				Pool childScript = transform.GetChild(i).GetComponent<Pool>();
				if (childScript && obj == childScript.poolBlock.prefab)
				{
					poolRef.Add(obj, childScript);
					return childScript;
				}
			}
			return null;
		}

		public int GetActiveCount(GameObject prefab)
		{
			if (prefab == null)
			{ return 0; } // object wasn't defined
			Pool childScript;
			if (poolRef.ContainsKey(prefab))
			{ // reference already created
				childScript = poolRef[prefab];
			}
			else
			{ // ref not yet created
				childScript = MakePoolRef(prefab); // create ref
			}
			if (childScript == null)
			{ // pool not found
				return 0;
			}
			else
			{
				return childScript.poolBlock.size - childScript.pool.Count;
			}
		}

		public int GetAvailableCount(GameObject prefab)
		{
			if (prefab == null)
			{ return 0; } // object wasn't defined
			Pool childScript;
			if (poolRef.ContainsKey(prefab))
			{ // reference already created
				childScript = poolRef[prefab];
			}
			else
			{ // ref not yet created
				childScript = MakePoolRef(prefab); // create ref
			}
			if (childScript == null)
			{ // pool not found
				return 0;
			}
			else
			{
				return childScript.pool.Count;
			}
		}

		public bool RemoveAll()
		{
			bool result = true;
			GameObject[] tempObj = new GameObject[poolRef.Count];
			int i = 0;
			foreach (GameObject obj in poolRef.Keys)
			{
				if (poolRef[obj] != null)
				{
					tempObj[i] = obj;
					i++;
				}
			}
			for (int t = 0; t < tempObj.Length; t++)
			{
				if (tempObj[t] != null && !RemovePool(tempObj[t]))
					return false;
			}
			return result;
		}

		public bool DespawnAll()
		{
			bool result = true;
			foreach (GameObject obj in poolRef.Keys)
			{
				if (!DespawnPool(obj))
				{ result = false; }
			}
			return result;
		}

		public bool RemovePool(GameObject prefab)
		{
			if (prefab == null)
			{ return false; } // object wasn't defined

			Pool childScript;
			if (poolRef.ContainsKey(prefab))
			{ // reference already created
				childScript = poolRef[prefab];
			}
			else
			{ // ref not yet created
				childScript = MakePoolRef(prefab); // create ref
			}
			if (childScript == null)
			{ // pool not found
				return false;
			}
			else
			{
				bool result = DespawnPool(prefab);
				Destroy(childScript.gameObject);
				poolRef.Remove(prefab);
				return result;
			}
		}

		public bool DespawnPool(GameObject prefab)
		{
			if (prefab == null)
			{ return false; } // object wasn't defined
			Pool childScript;
			if (poolRef.ContainsKey(prefab))
			{ // reference already created
				childScript = poolRef[prefab];
			}
			else
			{ // ref not yet created
				childScript = MakePoolRef(prefab); // create ref
			}
			if (childScript == null)
			{ // pool not found
				return false;
			}
			else
			{
				for (int i = 0; i < childScript.masterPool.Count; i++)
				{
					childScript.Despawn(childScript.masterPool[i].obj, childScript.masterPool[i].refScript);
				}
				return true;
			}
		}

		public void CreatePool()
		{
			CreatePool(null, 32, 64, EmptyBehavior.Grow, MaxEmptyBehavior.Fail);
		}
		public void CreatePool(GameObject prefab, int size, int maxSize, EmptyBehavior emptyBehavior, MaxEmptyBehavior maxEmptyBehavior)
		{
			GameObject obj = new GameObject("Object Pool");
			obj.transform.parent = transform;
			obj.transform.localPosition = Vector3.zero;
			obj.transform.localRotation = Quaternion.identity;
			Pool script = obj.AddComponent<Pool>();
			if (Application.isPlaying)
			{
				obj.name = prefab.name;
				script.poolBlock.size = size;
				script.poolBlock.maxSize = maxSize;
				script.poolBlock.emptyBehavior = emptyBehavior;
				script.poolBlock.maxEmptyBehavior = maxEmptyBehavior;
				script.poolBlock.prefab = prefab;
				if (prefab)
				{ MakePoolRef(prefab); }
			}
		}

		void OnApplicationQuit()
		{
			if (printAllLogsOnQuit)
			{
				PrintAllLogs();
			}
		}

		public void PrintAllLogs()
		{
			foreach (Pool script in poolRef.Values)
			{
				script.PrintLog();
			}
		}

	}
}
