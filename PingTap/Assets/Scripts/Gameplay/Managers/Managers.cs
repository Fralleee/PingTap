using Fralle.Core.Audio;
using Fralle.Core.Infrastructure;
using UnityEngine;

namespace Fralle.Gameplay
{
	public class Managers : Singleton<Managers>
	{
		[HideInInspector] public AudioManager Audio;
		[HideInInspector] public CameraManager Camera;
		[HideInInspector] public UiManager UiManager;

		[HideInInspector] public StateManager State;
		[HideInInspector] public MatchManager Match;
		[HideInInspector] public SettingsManager Settings;
		[HideInInspector] public EnemyManager Enemy;

		protected override void Awake()
		{
			base.Awake();

			Audio = GetComponent<AudioManager>();
			Camera = GetComponent<CameraManager>();
			UiManager = GetComponent<UiManager>();

			State = GetComponent<StateManager>();
			Match = GetComponent<MatchManager>();
			Settings = GetComponent<SettingsManager>();
			Enemy = GetComponent<EnemyManager>();
		}

	}
}
