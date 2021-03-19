using Fralle.Core.Attributes;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterStats
{
	[Serializable]
	public abstract class CharacterStat
	{
		public event Action<CharacterStat> OnChanged = delegate { };

		public float BaseValue;

		protected bool isDirty = true;
		protected float lastBaseValue;

		[SerializeField, Readonly]
		protected float _value;
		public float Value
		{
			get
			{
				if (isDirty || lastBaseValue != BaseValue)
				{
					lastBaseValue = BaseValue;
					_value = CalculateFinalValue();
					isDirty = false;
				}
				return _value;
			}
		}

		protected readonly List<StatModifier> statModifiers;

		protected CharacterStat()
		{
			statModifiers = new List<StatModifier>();
		}

		protected CharacterStat(float baseValue) : this()
		{
			BaseValue = baseValue;
		}

		protected virtual void OnChangedDispatcher()
		{
			OnChanged(this);
		}

		public virtual void AddModifier(StatModifier mod)
		{
			isDirty = true;
			statModifiers.Add(mod);
			OnChangedDispatcher();
		}

		public virtual bool RemoveModifier(StatModifier mod)
		{
			if (statModifiers.Remove(mod))
			{
				isDirty = true;
				OnChangedDispatcher();
				return true;
			}
			return false;
		}

		public virtual bool RemoveAllModifiersFromSource(object source)
		{
			int numRemovals = statModifiers.RemoveAll(mod => mod.Source == source);

			if (numRemovals > 0)
			{
				isDirty = true;
				OnChangedDispatcher();
				return true;
			}
			return false;
		}

		protected virtual int CompareModifierOrder(StatModifier a, StatModifier b)
		{
			if (a.Order < b.Order)
				return -1;
			else if (a.Order > b.Order)
				return 1;
			return 0;
		}

		protected virtual float CalculateModifiers(float baseValue)
		{
			float modifierValue = baseValue;
			float sumPercentAdd = 0;

			statModifiers.Sort(CompareModifierOrder);

			for (int i = 0; i < statModifiers.Count; i++)
			{
				StatModifier mod = statModifiers[i];

				if (mod.Type == StatModType.Flat)
				{
					modifierValue += mod.Value;
				}
				else if (mod.Type == StatModType.PercentAdd)
				{
					sumPercentAdd += mod.Value;

					if (i + 1 >= statModifiers.Count || statModifiers[i + 1].Type != StatModType.PercentAdd)
					{
						modifierValue *= 1 + sumPercentAdd;
						sumPercentAdd = 0;
					}
				}
				else if (mod.Type == StatModType.PercentMult)
				{
					modifierValue *= 1 + mod.Value;
				}
			}

			// Workaround for float calculation errors, like displaying 12.00001 instead of 12
			return (float)Math.Round(modifierValue, 4);
		}

		protected virtual float CalculateFinalValue() => (float)Math.Round(CalculateModifiers(BaseValue), 4);
	}
}
