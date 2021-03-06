﻿using Fralle.Core.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Fralle.Gameplay
{
	public class StateManager : MonoBehaviour
	{
		public static event Action<GameState> OnGamestateChanged = delegate { };

		[SerializeField] bool startGame;

		public static GameState GameState = GameState.Playing;
		public MatchState MatchState;
		public IState<MatchState> CurrentState;

		StateMachine<MatchState> stateMachine;
		readonly HashSet<IState<MatchState>> states = new HashSet<IState<MatchState>>();

		void Start()
		{
			SetupStateMachine();
		}

		void SetupStateMachine()
		{
			stateMachine = new StateMachine<MatchState>();

			states.Add(new MatchStatePrepare());
			states.Add(new MatchStateLive());
			states.Add(new MatchStateEnd());

			if (startGame)
			{
				SetState(MatchState.Prepare);
			}
		}

		void Update()
		{
			stateMachine.OnLogic();
		}

		public void SetState(MatchState newState)
		{
			var state = states.FirstOrDefault(x => x.identifier == newState);
			stateMachine.SetState(state);
			MatchState = state.identifier;
			EventManager.Broadcast(new GameStateChangeEvent(MatchState, newState));
		}

		public static void SetGameState(GameState newState)
		{
			OnGamestateChanged(newState);
			GameState = newState;
		}

	}
}
