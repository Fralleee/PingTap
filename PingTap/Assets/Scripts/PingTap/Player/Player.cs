using CombatSystem.Combat;
using Fralle.Core.Extensions;
using Fralle.Gameplay;
using QFSW.QC;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Fralle
{
	public class Player : MonoBehaviour
	{
		[SerializeField] LayerMask ignoreLayers;

		[HideInInspector] public new Camera camera;
		[HideInInspector] public Combatant combatant;

		PlayerInput playerInput;

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
			playerInput = GetComponent<PlayerInput>();
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
			if (gameState == GameState.PauseMenu)
				playerInput.DeactivateInput();
			else
				playerInput.ActivateInput();
		}

		void ConsoleActivated()
		{
			playerInput.DeactivateInput();
		}

		void ConsoleDeactivated()
		{
			playerInput.ActivateInput();
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
