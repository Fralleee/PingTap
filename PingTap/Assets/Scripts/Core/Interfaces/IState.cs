namespace Fralle.Core.Interfaces
{
  public interface IState
  {
    void Tick();
    void OnEnter();
    void OnExit();
  }
}