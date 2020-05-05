using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Fralle.UI.General
{
  public class UiGradient : BaseMeshEffect
  {
    [FormerlySerializedAs("m_color1")] public Color mColor1 = Color.white;
    [FormerlySerializedAs("m_color2")] public Color mColor2 = Color.white;

    [FormerlySerializedAs("m_angle")]
    [Range(-180f, 180f)]
    public float mAngle = 0f;

    [FormerlySerializedAs("m_ignoreRatio")]
    public bool mIgnoreRatio = true;

    public override void ModifyMesh(VertexHelper vh)
    {
      if (!enabled) return;
      var rect = graphic.rectTransform.rect;
      var dir = UiGradientUtils.RotationDir(mAngle);

      if (!mIgnoreRatio)
        dir = UiGradientUtils.CompensateAspectRatio(rect, dir);

      var localPositionMatrix = UiGradientUtils.LocalPositionMatrix(rect, dir);

      var vertex = default(UIVertex);
      for (var i = 0; i < vh.currentVertCount; i++)
      {
        vh.PopulateUIVertex(ref vertex, i);
        var localPosition = localPositionMatrix * vertex.position;
        vertex.color *= Color.Lerp(mColor2, mColor1, localPosition.y);
        vh.SetUIVertex(vertex, i);
      }
    }
  }
}