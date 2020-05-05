using UnityEngine;

namespace Fralle.Movement
{
  public class MoveCamera : MonoBehaviour
  {

    public Transform player;

    void Update()
    {
      transform.position = player.transform.position;
    }
  }
}