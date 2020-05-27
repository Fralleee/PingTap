using UnityEngine;

public class MovementCrouch : MonoBehaviour
{
  [SerializeField] float crouchingSpeed = 4f;
  [SerializeField] float crouchHeight = 1f;

  PlayerInputController input;

  CapsuleCollider capsuleCollider;

  float defaultHeight;

  void Awake()
  {
    input = GetComponentInParent<PlayerInputController>();
    capsuleCollider = GetComponentInParent<CapsuleCollider>();

    defaultHeight = capsuleCollider.height;
  }

  void Update()
  {
    capsuleCollider.height = Mathf.Lerp(capsuleCollider.height, input.crouchButtonHold ? crouchHeight : defaultHeight, Time.deltaTime * crouchingSpeed);
  }
}
