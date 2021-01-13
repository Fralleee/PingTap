using Fralle.Core.Infrastructure;
using Fralle.Core.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Fralle.Gameplay
{
	public class StateManager : MonoBehaviour
	{
		public static event Action<GameState> OnGamestateChanged = delegate { };

		[SerializeField] bool startGame;

		public static GameState gameState = GameState.Playing;
		public MatchState matchState;
		public IState currentState;

		StateMachine stateMachine;
		readonly Dictionary<MatchState, IState> States = new Dictionary<MatchState, IState>();

		void Start()
		{
			SetupStateMachine();
			Debug.Log(gameState);
		}

		void SetupStateMachine()
		{
			stateMachine = new StateMachine();

			States.Add(MatchState.Prepare, new MatchStatePrepare());
			States.Add(MatchState.Live, new MatchStateLive());
			States.Add(MatchState.End, new MatchStateEnd());

			if (startGame)
			{
				SetState(MatchState.Prepare);
			}
		}

		void Update()
		{
			stateMachine.Tick();
		}

		public void SetState(MatchState newState)
		{
			stateMachine.SetState(States[newState]);
			matchState = newState;
			EventManager.Broadcast(new GameStateChangeEvent(matchState, newState));
		}

		public static void SetGameState(GameState newState)
		{
			OnGamestateChanged(newState);
			gameState = newState;
		}

		void OnEnable()
		{
			SceneManager.sceneLoaded += OnSceneFinishedLoading;
			SceneManager.sceneUnloaded += OnSceneFinishedUnloading;
		}

		void OnDisable()
		{
			SceneManager.sceneLoaded -= OnSceneFinishedLoading;
			SceneManager.sceneUnloaded -= OnSceneFinishedUnloading;
		}

		static void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
		{
			if (scene.name == "Main Menu")
			{
				OnGamestateChanged(GameState.MenuActive);
				gameState = GameState.MenuActive;
			}
		}

		static void OnSceneFinishedUnloading(Scene scene)
		{
			if (scene.name == "Main Menu")
			{
				OnGamestateChanged(GameState.Playing);
				gameState = GameState.Playing;
			}
		}

	}
}
