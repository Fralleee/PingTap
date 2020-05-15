using Fralle.AI;
using System;

[Serializable]
public abstract class OnDeathAction
{
  public abstract void PerformAction(Enemy enemy);
}
