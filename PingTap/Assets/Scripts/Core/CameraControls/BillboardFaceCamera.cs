using UnityEngine;

namespace Fralle.Core.CameraControls
{
  public class BillboardFaceCamera : MonoBehaviour
  {
    new Camera camera;

    void Awake()
    {
      camera = Camera.main;
    }

    void LateUpdate()
    {
      FaceCamera();
    }

    void FaceCamera()
    {
      transform.LookAt(transform.position + camera.transform.rotation * Vector3.forward, camera.transform.rotation * Vector3.up);
    }
  }
}