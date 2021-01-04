using CombatSystem.Combat;
using Fralle.Core.Extensions;
using UnityEngine;

namespace Fralle
{
	public class Player : MonoBehaviour
	{
		[SerializeField] LayerMask ignoreLayers;

		[HideInInspector] public new Camera camera;
		[HideInInspector] public Combatant combatant;

		public static void Disable()
		{
			var players = FindObjectsOfType<Player>();
			foreach (var player in players)
			{
				player.gameObject.SetActive(false);
			}
		}

		void Awake()
		{
			combatant = GetComponent<Combatant>();
			transform.Find("UI").gameObject.SetActive(true);
		}

		void Start()
		{
			var layer = LayerMask.NameToLayer("Player");
			gameObject.SetLayerRecursively(layer, ignoreLayers);

			camera = Camera.main;
		}

	}
}
