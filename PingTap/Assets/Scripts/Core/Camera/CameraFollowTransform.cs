using UnityEngine;

namespace Fralle.Core.Camera
{
  public class CameraFollowTransform : MonoBehaviour
  {

    public Transform transformToFollow;

    void Update()
    {
      transform.position = transformToFollow.position;
    }
  }
}