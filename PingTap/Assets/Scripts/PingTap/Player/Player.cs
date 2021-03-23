using CombatSystem.Combat;
using Fralle.Core.Extensions;
using Fralle.FpsController;
using Fralle.Gameplay;
using QFSW.QC;
using UnityEngine;

namespace Fralle
{
	public class Player : MonoBehaviour
	{
		[SerializeField] LayerMask ignoreLayers;

		[HideInInspector] public new Camera camera;
		[HideInInspector] public Combatant combatant;
		[HideInInspector] public PlayerController playerController;

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
			playerController = GetComponent<PlayerController>();
			transform.Find("UI").gameObject.SetActive(true);
		}

		void Start()
		{
			var layer = LayerMask.NameToLayer("Player");
			gameObject.SetLayerRecursively(layer, ignoreLayers);

			camera = Camera.main;

			QuantumConsole.Instance.OnActivate += ConsoleActivated;
			QuantumConsole.Instance.OnDeactivate += ConsoleDeactivated;
		}

		void OnGamestateChanged(GameState gameState)
		{
			playerController.Lock(gameState == GameState.PauseMenu);
		}

		void ConsoleActivated()
		{
			playerController.Lock();
		}

		void ConsoleDeactivated()
		{
			playerController.Lock(false);
		}

		void OnEnable()
		{
			StateManager.OnGamestateChanged += OnGamestateChanged;
		}

		void OnDisable()
		{
			StateManager.OnGamestateChanged -= OnGamestateChanged;
		}

		void OnDestroy()
		{
			QuantumConsole.Instance.OnActivate -= ConsoleActivated;
			QuantumConsole.Instance.OnDeactivate -= ConsoleDeactivated;
		}


	}
}
