using Fralle.Core.Infrastructure;
using System.Collections.Generic;
using UnityEngine;

namespace Fralle.AI
{
	public class EnemyAtlas : Singleton<EnemyAtlas>
	{
		[SerializeField] GameObject zombiePrefab;
		[SerializeField] GameObject ogrePrefab;

		public Dictionary<EnemyType, GameObject> Atlas = new Dictionary<EnemyType, GameObject>();

		public GameObject GetPrefab(EnemyType enemyType)
		{
			if (Atlas.TryGetValue(enemyType, out GameObject prefab))
			{
				return prefab;
			}
			throw new KeyNotFoundException($"EnemyType {enemyType} as not found in EnemyAtlas");
		}

		protected override void Awake()
		{
			base.Awake();
			SetupAtlas();
		}

		void SetupAtlas()
		{
			Atlas.Add(EnemyType.Zombie, zombiePrefab);
			Atlas.Add(EnemyType.Ogre, ogrePrefab);
		}
	}
}
