//using QFSW.QC;

namespace Fralle.Gameplay.Commands
{
  public enum MusicPlayerAction
  {
    Play,
    Pause,
    Stop,
    Next,
    Prev
  }

  //public class MusicPlayerActionParser : BasicQcParser<MusicPlayerAction>
  //{
  //	public override MusicPlayerAction Parse(string value)
  //	{
  //		value = value.ToLower().Trim();
  //		switch (value)
  //		{
  //			case "play":
  //				return MusicPlayerAction.PLAY;
  //			case "pause":
  //				return MusicPlayerAction.PAUSE;
  //			case "stop":
  //				return MusicPlayerAction.STOP;
  //			case "next":
  //				return MusicPlayerAction.NEXT;
  //			case "prev":
  //				return MusicPlayerAction.PREV;
  //			case "previous":
  //				return MusicPlayerAction.PREV;
  //			default:
  //				throw new ParserInputException($"Cannot parse '{value}' to a MusicPlayerAction.");
  //		}
  //	}
  //}
}
