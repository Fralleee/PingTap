using UnityEngine;

namespace Fralle.Movement
{
  public class MoveCameraRig : MonoBehaviour
  {

    public Transform player;

    void Update()
    {
      transform.position = player.transform.position;
    }
  }
}