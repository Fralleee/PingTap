using Fralle.Attack;
using Fralle.Player;
using UnityEngine;

namespace Fralle.Movement
{
  public class Sway : MonoBehaviour
  {
    [SerializeField] float swaySize = 0.004f;
    [SerializeField] float swaySmooth = 25f;
    [SerializeField] float idleSmooth = 1f;

    PlayerAttack playerAttack;
    PlayerInputController input;

    Vector3 nextIdlePosition = Vector3.zero;

    void Awake()
    {
      playerAttack = GetComponentInParent<PlayerAttack>();
      input = GetComponentInParent<PlayerInputController>();
    }

    void LateUpdate()
    {
      if (playerAttack.equippedWeapon == null) return;
      if (Cursor.visible) return;

      var delta = -input.mouseRaw;
      if (delta.magnitude > 0) PerformSway(delta);
      else PerformIdle();
    }

    void PerformSway(Vector2 delta)
    {
      transform.localPosition += (Vector3)delta * swaySize * 0.001f;
      transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, swaySmooth * Time.deltaTime);
    }

    void PerformIdle()
    {
      transform.localPosition = Vector3.Lerp(transform.localPosition, nextIdlePosition, idleSmooth * Time.deltaTime);
      if (Vector3.Distance(transform.localPosition, nextIdlePosition) < 0.005f) NewIdlePosition();
    }

    void NewIdlePosition()
    {
      nextIdlePosition = Random.insideUnitCircle * 0.01f;
    }

  }
}