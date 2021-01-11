using UnityEngine;

namespace Fralle.Core.Pooling
{
	[System.Serializable]
	public class PoolItem
	{
		public GameObject obj;
		public PooledObject refScript;

		public PoolItem(GameObject obj, PooledObject refScript)
		{
			this.obj = obj;
			this.refScript = refScript;
		}
	}
}
