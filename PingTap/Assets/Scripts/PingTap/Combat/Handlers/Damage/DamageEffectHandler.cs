using Fralle.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Fralle.Pingtap
{
	[Serializable]
	public class DamageEffectHandler
	{
		[HideInInspector] public List<DamageEffect> DamageEffects = new List<DamageEffect>();

		DamageController damageController;

		public void Setup(DamageController damageController)
		{
			this.damageController = damageController;
		}

		public void DamageEffectsTick()
		{
			for (var i = 0; i < DamageEffects.Count; i++)
			{
				DamageEffects[i].Tick(damageController);
				if (DamageEffects[i].Timer <= DamageEffects[i].Time)
					continue;
				DamageEffects[i].Exit(damageController);
				DamageEffects.RemoveAt(i);
			}
		}

		public void ApplyEffects(DamageData damageData)
		{
			foreach (var t in damageData.Effects)
			{
				var effect = t;
				var oldEffect = DamageEffects.FirstOrDefault(x => x.name == effect.name);
				effect = effect.Append(oldEffect);
				effect.Enter(damageController);
				DamageEffects.Upsert(oldEffect, effect);
			}
		}
	}
}
