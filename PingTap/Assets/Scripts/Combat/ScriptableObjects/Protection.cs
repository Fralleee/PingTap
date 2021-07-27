using UnityEngine;

namespace Fralle.Pingtap
{
	public abstract class Protection : ScriptableObject
	{
		public EffectProtection EffectProtection;
		public abstract ProtectionResult RunProtection(DamageData damageData, DamageController damageController, DamageEffectHandler effectHandler);
	}
}
