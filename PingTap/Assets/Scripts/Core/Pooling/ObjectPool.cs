using UnityEngine;

namespace Fralle.Core.Pooling
{
	public static class ObjectPool
	{
		static PoolManager poolManager;

		// may be be called early and won't create a spawn, but will create a pool reference and return true if the reference was created or already exsists.
		// use if you'd like to link pool references before the first spawn of a particular pool. (probably not necessary except for the most demanding of scenes.)
		// Additionaly, can be used to dynamically create pools at runtime.
		public static bool InitializeSpawn(GameObject prefab) => InitializeSpawn(prefab, 0f, 0);

		// parameters assigned can be used to create pools at runtime
		// if addPool is < 1, it will be used to increase the exsisting pool by a percentage. Otherwise it will round to the nearest integer and increase by that ammount
		// minPool is the min object that must be in that pool. If the current pool + addPool < minPool, minPool will be used
		public static bool InitializeSpawn(GameObject prefab, float addPool, int minPool) => InitializeSpawn(prefab, addPool, minPool, EmptyBehavior.Grow, MaxEmptyBehavior.Fail, false);
		public static bool InitializeSpawn(GameObject prefab, float addPool, int minPool, EmptyBehavior emptyBehavior, MaxEmptyBehavior maxEmptyBehavior) => InitializeSpawn(prefab, addPool, minPool, emptyBehavior, maxEmptyBehavior, true);

		static bool InitializeSpawn(GameObject prefab, float addPool, int minPool, EmptyBehavior emptyBehavior, MaxEmptyBehavior maxEmptyBehavior, bool modBehavior)
		{
			if (prefab == null)
				return false;
			FindPoolManager();
			if (poolManager == null)
				return false;

			return poolManager.InitializeSpawn(prefab, addPool, minPool, emptyBehavior, maxEmptyBehavior, modBehavior);
		}

		public static GameObject Instantiate(GameObject prefab) => Spawn(prefab, null, Vector3.zero, Quaternion.identity, false);
		public static GameObject Spawn(GameObject prefab, int? child) => Spawn(prefab, child, Vector3.zero, Quaternion.identity, false);
		public static GameObject Spawn(GameObject prefab, Vector3 pos, Quaternion rot) => Spawn(prefab, null, pos, rot, true);
		public static GameObject Spawn(GameObject prefab, Vector3 pos, Quaternion rot, Transform parent) => Spawn(prefab, null, pos, rot, true, parent);
		public static GameObject Spawn(GameObject prefab, int? child, Vector3 pos, Quaternion rot) => Spawn(prefab, child, pos, rot, true);
		static GameObject Spawn(GameObject prefab, int? child, Vector3 pos, Quaternion rot, bool usePosRot, Transform parent = null)
		{
			FindPoolManager();
			if (poolManager == null)
				return null;

			return poolManager.Spawn(prefab, child, pos, rot, usePosRot, parent);
		}

		public static bool Despawn(GameObject obj)
		{
			if (obj == null)
			{ return false; }
			return Despawn(obj.GetComponent<PooledObject>(), -1f);
		}
		public static bool Despawn(GameObject obj, float time)
		{
			if (obj == null)
			{ return false; }
			return Despawn(obj.GetComponent<PooledObject>(), time);
		}
		public static bool Despawn(PooledObject po)
		{
			return Despawn(po, -1f);
		}
		public static bool Despawn(PooledObject po, float time)
		{
			if (po == null)
			{ return false; }
			return po.Despawn(time);
		}

		public static int GetActiveCount(GameObject obj)
		{
			FindPoolManager();
			if (poolManager == null)
				return 0;

			return poolManager.GetActiveCount(obj);
		}

		public static int GetAvailableCount(GameObject obj)
		{
			FindPoolManager();
			if (poolManager == null)
				return 0;

			return poolManager.GetAvailableCount(obj);
		}

		public static bool DespawnPool(GameObject obj)
		{
			FindPoolManager();
			if (poolManager == null)
				return false;

			return poolManager.DespawnPool(obj);
		}

		public static bool DespawnAll()
		{
			FindPoolManager();
			if (poolManager == null)
				return false;

			return poolManager.DespawnAll();
		}

		public static bool RemovePool(GameObject obj)
		{
			FindPoolManager();
			if (poolManager == null)
				return false;
			else
			{
				bool result = poolManager.RemovePool(obj);
				if (result)
				{ poolManager.poolRef.Remove(obj); }
				return result;
			}
		}

		public static bool RemoveAll()
		{
			FindPoolManager();
			if (poolManager == null)
				return false;

			return poolManager.RemoveAll();
		}

		static void FindPoolManager()
		{
			if (poolManager == null)
				poolManager = Object.FindObjectOfType<PoolManager>();
		}

	}
}
