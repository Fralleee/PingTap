using Fralle.Core.Extensions;
using Fralle.Player;
using System;
using UnityEngine;

namespace Fralle.Movement.Moves
{
  public class MovementCrouch : MonoBehaviour
  {
    public static event Action<bool> OnCrouch = delegate { };

    [SerializeField] float crouchingSpeed = 4f;
    [SerializeField] float crouchHeight = 1f;

    PlayerInputController input;
    MovementLimitSpeed limitSpeed;

    CapsuleCollider capsuleCollider;

    bool isCrouching;
    float defaultHeight;
    float goalHeight;
    float roofCheckHeight;

    void Awake()
    {
      input = GetComponentInParent<PlayerInputController>();
      limitSpeed = GetComponent<MovementLimitSpeed>();
      capsuleCollider = GetComponentInParent<CapsuleCollider>();

      defaultHeight = capsuleCollider.height;
      goalHeight = defaultHeight;
      crouchHeight = Mathf.Clamp(crouchHeight, capsuleCollider.radius, defaultHeight);
      roofCheckHeight = defaultHeight - crouchHeight * 0.5f - 0.01f;
    }

    void Update()
    {
      if (input.jumpButtonDown)
      {
        goalHeight = defaultHeight;
        capsuleCollider.height = defaultHeight;
        isCrouching = false;
        limitSpeed.SetExternalSpeedModifier(1f);
        OnCrouch(false);
      }
      else if (input.crouchButtonHold && !isCrouching)
      {
        goalHeight = crouchHeight;
        isCrouching = true;
        limitSpeed.SetExternalSpeedModifier(0.5f);
        OnCrouch(true);
      }
      else if (!input.crouchButtonHold)
      {
        if (!Physics.Raycast(transform.position, Vector3.up, roofCheckHeight))
        {
          goalHeight = defaultHeight;
          isCrouching = false;
          limitSpeed.SetExternalSpeedModifier(1f);
          OnCrouch(false);
        }
      }

      if (!capsuleCollider.height.EqualsWithTolerance(goalHeight)) capsuleCollider.height = Mathf.Lerp(capsuleCollider.height, goalHeight, Time.deltaTime * crouchingSpeed);
    }
  }
}