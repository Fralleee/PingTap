using System.Collections.Generic;
using UnityEngine;

namespace Fralle.Core.Audio
{
	[CreateAssetMenu(menuName = "Audio/MusicPlaylist")]
	public class MusicPlaylist : ScriptableObject
	{
		public List<AudioClip> Songs;
	}
}
