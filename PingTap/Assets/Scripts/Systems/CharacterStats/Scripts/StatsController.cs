using System.Collections.Generic;
using UnityEngine;

namespace CharacterStats
{
	public class StatsController : MonoBehaviour
	{
		[HideInInspector]
		public Dictionary<StatAttribute, CharacterStat> stats;

		[Header("Major stats")]
		public CharacterMajorStat agility;
		public CharacterMajorStat dexterity;
		public CharacterMajorStat strength;

		[Header("Minor stats")]
		public CharacterMinorStat aim;
		public CharacterMinorStat jumpPower;
		public CharacterMinorStat reloadSpeed;
		public CharacterMinorStat runSpeed;

		void OnEnable()
		{
			stats = new Dictionary<StatAttribute, CharacterStat>();

			AddMajorStatToDict(StatAttribute.DEXTERITY, dexterity);
			AddMajorStatToDict(StatAttribute.AGILITY, agility);
			AddMajorStatToDict(StatAttribute.STRENGTH, strength);

			AddMinorStatToDict(StatAttribute.AIM, aim);
			AddMinorStatToDict(StatAttribute.JUMPPOWER, jumpPower);
			AddMinorStatToDict(StatAttribute.RELOADSPEED, reloadSpeed);
			AddMinorStatToDict(StatAttribute.RUNSPEED, runSpeed);
		}

		public void ModifyStat(StatAttribute statAttribute, float value, StatModType statModType)
		{
			stats[statAttribute].AddModifier(new StatModifier(value, statModType));
		}

		public void ClearModifiers(StatAttribute statAttribute, object source)
		{
			stats[statAttribute].RemoveAllModifiersFromSource(source);
		}

		void AddMajorStatToDict(StatAttribute attribute, CharacterMajorStat stat)
		{
			stats.Add(attribute, stat);
		}

		void AddMinorStatToDict(StatAttribute attribute, CharacterMinorStat stat)
		{
			var parent = stats[stat.parentAttribute];
			stat.SetupParentListener(parent);
			stats.Add(attribute, stat);
		}

		public CharacterStat GetStat(StatAttribute attribute)
		{
			if (stats.TryGetValue(attribute, out CharacterStat value))
				return value;
			return null;
		}

	}
}
