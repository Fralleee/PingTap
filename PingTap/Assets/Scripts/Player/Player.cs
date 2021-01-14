using CombatSystem.Combat;
using Fralle.Core.Extensions;
using Fralle.FpsController;
using Fralle.Gameplay;
using UnityEngine;

namespace Fralle
{
	public class Player : MonoBehaviour
	{
		[SerializeField] LayerMask ignoreLayers;

		[HideInInspector] public new Camera camera;
		[HideInInspector] public Combatant combatant;

		InputController inputController;

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
			inputController = GetComponent<InputController>();
			transform.Find("UI").gameObject.SetActive(true);
		}

		void Start()
		{
			var layer = LayerMask.NameToLayer("Player");
			gameObject.SetLayerRecursively(layer, ignoreLayers);

			camera = Camera.main;
		}

		void OnEnable()
		{
			StateManager.OnGamestateChanged += OnGamestateChanged;
		}

		void OnDisable()
		{
			StateManager.OnGamestateChanged -= OnGamestateChanged;
		}

		void OnGamestateChanged(GameState gameState)
		{
			inputController.Lock(gameState == GameState.PauseMenu);
		}

	}
}
