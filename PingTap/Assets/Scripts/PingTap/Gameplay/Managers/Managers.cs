using Fralle.Core.Audio;
using Fralle.Core.Infrastructure;
using UnityEngine;

namespace Fralle.Gameplay
{
	public class Managers : Singleton<Managers>
	{
		public static bool Destroyed;

		[HideInInspector] public SettingsManager Settings;
		[HideInInspector] public StateManager State;
		[HideInInspector] public UiManager UiManager;
		[HideInInspector] public AudioManager Audio;
		[HideInInspector] public CameraManager Camera;
		[HideInInspector] public MatchManager Match;
		[HideInInspector] public EnemyManager Enemy;
		//[HideInInspector] public PoolManager Pool;
		[HideInInspector] public Spawner Spawner;

		protected override void Awake()
		{
			base.Awake();

			Settings = GetComponentInChildren<SettingsManager>();
			State = GetComponentInChildren<StateManager>();
			UiManager = GetComponentInChildren<UiManager>();
			Audio = GetComponentInChildren<AudioManager>();
			Camera = GetComponentInChildren<CameraManager>();
			Match = GetComponentInChildren<MatchManager>();
			Enemy = GetComponentInChildren<EnemyManager>();
			//Pool = GetComponentInChildren<PoolManager>();
			Spawner = GetComponentInChildren<Spawner>();
		}

		protected override void OnDestroy()
		{
			Debug.Log($"{name}:OnDestroy");
			Destroyed = true;
			base.OnDestroy();
		}
	}
}
