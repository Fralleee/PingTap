using System.Collections.Generic;

namespace Fralle.AbilitySystem
{
	public class AbilityArsenal
	{
		public List<ActiveAbility> ActiveAbilities;
		public List<PassiveAbility> PassiveAbilities;

		public AbilityArsenal()
		{
			ActiveAbilities = new List<ActiveAbility>();
			PassiveAbilities = new List<PassiveAbility>();
		}
	}
}
