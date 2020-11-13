using UnityEngine;

namespace Fralle.Core.Gameplay
{
	[CreateAssetMenu(menuName = "Core/GameScene")]
	public class GameSceneSO : ScriptableObject
	{
		[Header("Information")]
		public string sceneName;
		public string shortDescription;

		[Header("Sounds")]
		public AudioClip music;

	}
}
