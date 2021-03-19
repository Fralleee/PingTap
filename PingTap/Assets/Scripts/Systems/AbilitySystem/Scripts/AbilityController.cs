using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Fralle.AbilitySystem
{
	public class AbilityController : MonoBehaviour
	{
		[SerializeField] List<Ability> abilities;

		AbilityArsenal arsenal;

		void Start()
		{
			arsenal = GenerateArsenal(abilities);
		}

		void Update()
		{
			foreach (var ability in arsenal.ActiveAbilities)
			{
				if (Input.GetButtonDown(ability.ActivateButton))
					ability.Perform();

				ability.Update();
			}
		}

		protected virtual AbilityArsenal GenerateArsenal(List<Ability> abilitySchemas)
		{
			AbilityArsenal abilitiesArsenal = new AbilityArsenal();
			List<Ability> abilitiesList = new List<Ability>();

			foreach (Ability ability in abilitySchemas)
			{
				var instance = Instantiate(ability);
				instance.Setup(this);
				abilitiesList.Add(instance);
			}

			abilitiesArsenal.ActiveAbilities.AddRange(abilitiesList.OfType<ActiveAbility>());
			abilitiesArsenal.PassiveAbilities.AddRange(abilitiesList.OfType<PassiveAbility>());


			return abilitiesArsenal;
		}

	}
}
