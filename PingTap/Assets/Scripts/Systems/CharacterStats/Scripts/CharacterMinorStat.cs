using System;
using UnityEngine;

namespace CharacterStats
{
	[Serializable]
	public class CharacterMinorStat : CharacterStat
	{
		public StatAttribute parentAttribute;

		StatModifier parentStatModifier;
		CharacterStat parentStat;
		[SerializeField] float parentStatFactor = 1f;

		public void SetupParentListener(CharacterStat stat)
		{
			parentStat = stat;
			parentStat.OnChanged += ParentStat_OnChanged;
			ParentStat_OnChanged(parentStat);
		}

		void ParentStat_OnChanged(CharacterStat obj)
		{
			isDirty = true;
			parentStatModifier = new StatModifier(parentStat.Value * parentStatFactor, StatModType.Flat);
			OnChangedDispatcher();
		}

		protected override float CalculateFinalValue()
		{
			float finalValue = BaseValue;
			finalValue += parentStatModifier.Value;
			return (float)Math.Round(CalculateModifiers(finalValue), 4);
		}
	}
}
