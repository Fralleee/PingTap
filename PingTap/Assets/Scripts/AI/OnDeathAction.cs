using System;

namespace Fralle.AI
{
  [Serializable]
  public abstract class OnDeathAction
  {
    public abstract void PerformAction(Enemy enemy);
  }
}