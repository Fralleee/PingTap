using Fralle.Core.Extensions;
using UnityEngine;

namespace Fralle.Core.CameraControls
{
  public class BillboardSizeCamera : MonoBehaviour
  {
    new Camera camera;

    [SerializeField] float maxDistance = 50f;
    [SerializeField] Vector2 scaleLimits = new Vector2(0.5f, 1.5f);
    [SerializeField] Vector2 yOffsetLimits = new Vector2(0.5f, 1.5f);

    void Awake()
    {
      camera = Camera.main;
    }

    void LateUpdate()
    {
      UpdateSize();
    }

    void UpdateSize()
    {
      var distance = Vector3.Distance(camera.transform.position, transform.position);
      var yPositionOffset = Mathf.Lerp(yOffsetLimits.x, yOffsetLimits.y, distance / maxDistance);
      transform.position = transform.position.With(y: yPositionOffset);

      var scale = Mathf.Lerp(scaleLimits.x, scaleLimits.y, distance / maxDistance);
      transform.localScale = Vector3.one * scale;
    }
  }
}