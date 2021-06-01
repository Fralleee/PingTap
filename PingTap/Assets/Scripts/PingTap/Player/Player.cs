using CombatSystem.Combat;
using Fralle.Core.Extensions;
using Fralle.Gameplay;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Fralle
{
	[SelectionBase]
	public class Player : MonoBehaviour
	{
		[HideInInspector] public Camera Camera;
		[HideInInspector] public Combatant Combatant;
		[HideInInspector] public PlayerInput PlayerInput;

		public static void Disable()
		{
			Player[] players = FindObjectsOfType<Player>();
			foreach (Player player in players)
			{
				player.gameObject.SetActive(false);
			}
		}

		void Awake()
		{
			Combatant = GetComponent<Combatant>();
			PlayerInput = GetComponent<PlayerInput>();
			transform.Find("UI").gameObject.SetActive(true);
		}

		void Start()
		{
			Camera = Camera.main;

			//QuantumConsole.Instance.OnActivate += ConsoleActivated;
			//QuantumConsole.Instance.OnDeactivate += ConsoleDeactivated;
		}

		void OnGamestateChanged(GameState gameState)
		{
			if (gameState == GameState.PauseMenu)
			{
				PlayerInput.DeactivateInput();
			}
			else
			{
				PlayerInput.ActivateInput();
			}
		}

		void ConsoleActivated()
		{
			PlayerInput.DeactivateInput();
		}

		void ConsoleDeactivated()
		{
			PlayerInput.ActivateInput();
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
			//QuantumConsole.Instance.OnActivate -= ConsoleActivated;
			//QuantumConsole.Instance.OnDeactivate -= ConsoleDeactivated;
		}

	}
}
