using System;

namespace Fralle.PingTap
{
  [Serializable]
  public class ArmorElementModifier
  {
    public Element Element;
    public float Modifier = 1;

    public ArmorElementModifier(Element element) { Element = element; }
  }
}
