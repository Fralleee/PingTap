using System;
using System.Collections.Generic;
using UnityEngine;

namespace StatsSystem
{
	public class Stats : MonoBehaviour
	{
		public event Action<Stats> OnStatisticUpdated = delegate { };

		[SerializeField] List<Statistic> stats = new List<Statistic>();

		public float recoilReduction = 1f;
		public float spreadReduction = 1f;
		public float runSpeedMultiplier = 1f;
		public float jumpPowerMultiplier = 1f;
		public float reloadSpeedMultiplier = 1f;
		public float meleeRecoveryMultiplier = 1f;
		public float meleePowerMultiplier = 1f;
		public float healthMultiplier = 1f;
		public float regenerationMultiplier = 1f;

		void Awake()
		{
			stats.Add(new Aim());
			stats.Add(new Agility());
			stats.Add(new Dexterity());
			stats.Add(new Stamina());
			stats.Add(new Strength());
		}

		public void UpgradeStatistic(Statistic statistic)
		{
			foreach (var stat in stats)
			{
				if (stat.name == statistic.name)
				{
					stat.level++;
					stat.Apply(this);
					OnStatisticUpdated(this);
					return;
				}
			}

			Debug.LogWarning($"Tried to add statistic: {statistic.name} to PlayerStats but was not found in list.");
		}

		public void UpgradeStatistic(string name)
		{
			foreach (var stat in stats)
			{
				if (stat.name.ToLower() == name.ToLower())
				{
					stat.level++;
					stat.Apply(this);
					OnStatisticUpdated(this);
					return;
				}
			}

			Debug.LogWarning($"Tried to add statistic: {name} to PlayerStats but was not found in list.");
		}

		public int GetStatisticLevel(string name)
		{
			foreach (var stat in stats)
			{
				if (stat.name.ToLower() == name.ToLower())
				{
					return stat.level;
				}
			}
			return 0;
		}

#if UNITY_EDITOR
		void OnValidate()
		{
			OnStatisticUpdated(this);
		}
#endif

	}
}
