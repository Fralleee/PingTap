using UnityEngine;

namespace Fralle.AbilitySystem
{
	public abstract class Ability : ScriptableObject
	{
		public abstract void Setup(AbilityController abilityController);
	}
}
