using UnityEngine;
using UnityEngine.UI;

namespace Fralle.UI.General
{
  public class UiGradient : BaseMeshEffect
  {
    public Color MColor1 = Color.white;
    public Color MColor2 = Color.white;

    [Range(-180f, 180f)]
    public float MAngle = 0f;

    public bool MIgnoreRatio = true;

    public override void ModifyMesh(VertexHelper vh)
    {
      if (!enabled)
        return;
      Rect rect = graphic.rectTransform.rect;
      Vector2 dir = UiGradientUtils.RotationDir(MAngle);

      if (!MIgnoreRatio)
        dir = UiGradientUtils.CompensateAspectRatio(rect, dir);

      UiGradientUtils.Matrix2X3 localPositionMatrix = UiGradientUtils.LocalPositionMatrix(rect, dir);

      UIVertex vertex = default(UIVertex);
      for (int i = 0; i < vh.currentVertCount; i++)
      {
        vh.PopulateUIVertex(ref vertex, i);
        Vector2 localPosition = localPositionMatrix * vertex.position;
        vertex.color *= Color.Lerp(MColor2, MColor1, localPosition.y);
        vh.SetUIVertex(vertex, i);
      }
    }
  }
}
