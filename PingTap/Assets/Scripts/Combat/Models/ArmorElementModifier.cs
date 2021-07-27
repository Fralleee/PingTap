using System;

namespace Fralle.Pingtap
{
	[Serializable]
	public class ArmorElementModifier
	{
		public Element Element;
		public float Modifier = 1;

		public ArmorElementModifier(Element element) { Element = element; }
	}
}
