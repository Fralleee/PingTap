using UnityEngine;

namespace Fralle.Core.CameraControls
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