using Fralle.Core.Attributes;
using UnityEngine;

namespace Fralle.Gameplay
{
	public class SettingsManager : MonoBehaviour
	{
		[Space(10)]
		[Readonly] public int enemiesSpawned;
		[Readonly] public int enemiesKilled;
		[Readonly] public int totalEnemies;
		[Readonly] public float prepareTimer;
		[Readonly] public float totalTimer;
		[Readonly] public float waveTimer;

		[SerializeField] float prepareTime = 5f;

		void Update()
		{
			totalTimer += Time.deltaTime;
		}

		public void ResetPreparationTimer()
		{
			prepareTime = prepareTimer;
		}

	}
}
