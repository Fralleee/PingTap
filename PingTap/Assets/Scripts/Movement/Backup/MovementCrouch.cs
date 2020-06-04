//using Fralle.Player;
//using UnityEngine;

//namespace Fralle.Movement
//{
//  public class MovementCrouch : MonoBehaviour
//  {
//    [SerializeField] float crouchingSpeed = 4f;
//    [SerializeField] float crouchHeight = 1f;

//    PlayerMovement movement;
//    PlayerInputController input;

//    CapsuleCollider capsuleCollider;

//    float defaultHeight;
//    float goalHeight;
//    float roofCheckHeight;

//    void Awake()
//    {
//      movement = GetComponentInParent<PlayerMovement>();
//      input = GetComponentInParent<PlayerInputController>();
//      capsuleCollider = GetComponentInParent<CapsuleCollider>();

//      defaultHeight = capsuleCollider.height;
//      goalHeight = defaultHeight;
//      crouchHeight = Mathf.Clamp(crouchHeight, capsuleCollider.radius, defaultHeight);
//      roofCheckHeight = defaultHeight - crouchHeight * 0.5f - 0.01f;
//    }

//    void Update()
//    {
//      if (movement.state == PlayerMovementState.Crouching) CrouchingUpdate();
//      else if (input.crouchButtonHold && movement.state == PlayerMovementState.Ready)
//      {
//        CrouchingEnter();
//      }

//      capsuleCollider.height = Mathf.Lerp(capsuleCollider.height, goalHeight, Time.deltaTime * crouchingSpeed);
//    }

//    void CrouchingUpdate()
//    {
//      if (input.crouchButtonHold) return;
//      if (Physics.Raycast(transform.position, Vector3.up, roofCheckHeight)) return;

//      CrouchingExit();
//    }

//    void CrouchingEnter()
//    {
//      goalHeight = crouchHeight;
//      movement.Crouch(true);
//    }

//    void CrouchingExit()
//    {
//      goalHeight = defaultHeight;
//      movement.Crouch(false);
//    }
//  }
//}