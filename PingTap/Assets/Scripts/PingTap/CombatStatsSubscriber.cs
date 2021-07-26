using CombatSystem;
using Fralle.CharacterStats;
using UnityEngine;

namespace Fralle.PingTap
{
	public class CombatStatsSubscriber : MonoBehaviour
	{
		Combatant combatatant;
		StatsController statsController;

		void Awake()
		{
			

			statsController.Aim.OnChanged += AimChanged;
		}

		private void AimChanged(CharacterStat aim)
		{
			combatatant.Modifiers.ExtraAccuracy = aim.Value;
		}

		void OnDestroy()
		{
			statsController.Aim.OnChanged -= AimChanged;
		}
	}
}
