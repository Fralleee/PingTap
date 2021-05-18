using Fralle.Core.Enums;

namespace CombatSystem.Interfaces
{
	public interface IAction
	{
		MouseButton Button { get; set; }
		void Perform();
	}
}
