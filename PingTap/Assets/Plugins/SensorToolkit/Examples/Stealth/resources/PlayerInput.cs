using UnityEngine;

namespace SensorToolkit.Example
{
  [RequireComponent(typeof(CharacterControls))]
  public class PlayerInput : MonoBehaviour
  {
    public Sensor InteractionRange;

    CharacterControls cc;

    void Start()
    {
      cc = GetComponent<CharacterControls>();
    }

    void Update()
    {
      float horiz = Input.GetAxis("Horizontal");
      float vert = Input.GetAxis("Vertical");
      cc.Move = new Vector3(horiz, 0f, vert);

      // Project mouse position onto worlds plane at y=0
      Vector3 mousePosScreen = Input.mousePosition;
      Vector3 camPosition = Camera.main.transform.position;
      Vector3 camForward = Camera.main.transform.forward;
      float camDepthToGroundPlane = -camPosition.y / camForward.y;
      mousePosScreen.z = camDepthToGroundPlane;
      Vector3 mousePosGroundPlane = Camera.main.ScreenToWorldPoint(mousePosScreen);
      cc.Face = (mousePosGroundPlane - transform.position).normalized;

      // Pickup the pickup if its in range
      Holdable pickup = InteractionRange.GetNearestByComponent<Holdable>();
      if (pickup != null && !pickup.IsHeld)
      {
        cc.PickUp(pickup);
      }
    }
  }
}