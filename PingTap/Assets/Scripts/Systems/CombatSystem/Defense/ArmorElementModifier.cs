using CombatSystem.Enums;
using System;

namespace CombatSystem.Defense
{
  [Serializable]
  public class ArmorElementModifier
  {
    public Element Element;
    public float Modifier = 1;
  }
}
