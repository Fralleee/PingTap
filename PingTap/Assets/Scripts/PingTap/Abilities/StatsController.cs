using CombatSystem;
using Fralle.CharacterStats;
using UnityEngine;

namespace Fralle.PingTap
{
	public class StatsController : StatsControllerBase
	{
		[Header("Major stats")]
		public CharacterMajorStat Agility;
		public CharacterMajorStat Dexterity;
		public CharacterMajorStat Strength;

		[Header("Minor stats")]
		public CharacterMinorStat Aim;
		public CharacterMinorStat JumpPower;
		public CharacterMinorStat ReloadSpeed;
		public CharacterMinorStat RunSpeed;


		Combatant combatatant;

		protected override void Awake()
		{
			base.Awake();

			AddMajorStatToDict(StatAttribute.Dexterity, Dexterity);
			AddMajorStatToDict(StatAttribute.Agility, Agility);
			AddMajorStatToDict(StatAttribute.Strength, Strength);

			AddMinorStatToDict(StatAttribute.Aim, Aim);
			AddMinorStatToDict(StatAttribute.Jumppower, JumpPower);
			AddMinorStatToDict(StatAttribute.Reloadspeed, ReloadSpeed);
			AddMinorStatToDict(StatAttribute.Runspeed, RunSpeed);

			combatatant = GetComponent<Combatant>();
		}

		void Start()
		{
			// Event handlers	
			Aim.OnChanged += AimChanged;
		}

		void AimChanged(CharacterStat aim)
		{
			combatatant.Modifiers.ExtraAccuracy = aim.Value;
		}

		void OnDestroy()
		{
			Aim.OnChanged -= AimChanged;
		}
	}
}
