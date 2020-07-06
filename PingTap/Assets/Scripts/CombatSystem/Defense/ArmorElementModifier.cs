using CombatSystem.Enums;
using System;

namespace CombatSystem.Defense
{
  [Serializable]
  public class ArmorElementModifier
  {
    public Element element;
    public float modifier = 1;
  }
}