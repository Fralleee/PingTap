namespace Fralle.Gameplay.Commands
{
	//[CommandPrefix("audio.")]
	//public static class AudioCommands
	//{
	//	[Command(aliasOverride: "music", description: "Controls the music player (play/pause/stop/next/prev)")]
	//	public static void Music([CommandParameterDescription("PLAY/PAUSE/STOP/NEXT/PREV")] MusicPlayerAction action)
	//	{
	//		Debug.Log($"Music action: {action}");
	//		switch (action)
	//		{
	//			case MusicPlayerAction.PLAY:
	//				Managers.Instance.Audio.PlayMusic();
	//				break;
	//			case MusicPlayerAction.PAUSE:
	//				Managers.Instance.Audio.PauseMusic();
	//				break;
	//			case MusicPlayerAction.STOP:
	//				Managers.Instance.Audio.StopMusic(true);
	//				break;
	//			case MusicPlayerAction.NEXT:
	//				Managers.Instance.Audio.Next();
	//				Debug.Log(Managers.Instance.Audio.CurrentTrack);
	//				break;
	//			case MusicPlayerAction.PREV:
	//				Managers.Instance.Audio.Prev();
	//				Debug.Log(Managers.Instance.Audio.CurrentTrack);
	//				break;
	//			default:
	//				break;
	//		}
	//	}

	//	[Command(aliasOverride: "set_music_volume", description: "Set the music volume")]
	//	public static void MusicVolume(float volume)
	//	{
	//		Debug.Log($"Setting music volume to {volume}");
	//		Managers.Instance.Audio.MusicVolume = volume;
	//	}

	//	[Command(aliasOverride: "track", description: "Get current music track")]
	//	public static void Track()
	//	{
	//		Debug.Log(Managers.Instance.Audio.CurrentTrack);
	//	}

	//}
}
