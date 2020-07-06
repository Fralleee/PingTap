using System;
using UnityEngine;

namespace Fralle.FpsController.Moves
{
  public class Crouching : MonoBehaviour
  {
    public static event Action<bool> OnCrouch = delegate { };

    [SerializeField] float crouchingSpeed = 8f;
    [SerializeField] float crouchHeight = 1f;

    InputController input;

    Transform body;
    CapsuleCollider capsuleCollider;

    Vector3 crouchingScale;
    Vector3 defaultScale;

    float defaultHeight;
    float roofCheckHeight;

    bool isCrouching;

    bool crouchButtonHold;

    void Awake()
    {
      input = GetComponentInParent<InputController>();

      capsuleCollider = GetComponentInParent<CapsuleCollider>();
      body = GetComponent<Transform>();

      defaultScale = body.localScale;
      crouchingScale = new Vector3(1, 0.5f, 1);

      defaultHeight = capsuleCollider.height;
      crouchHeight = capsuleCollider.height * crouchingScale.y * body.localScale.y;

      roofCheckHeight = defaultHeight - crouchHeight * 0.5f - 0.01f;
    }

    void Update()
    {
      GatherInputs();
    }

    void GatherInputs()
    {
      crouchButtonHold = input.CrouchButtonHold;
    }

    public void ControlledFixedUpdate()
    {
      Crouch();
    }

    void Crouch()
    {
      if (crouchButtonHold)
      {
        isCrouching = true;
        if (body.localScale != crouchingScale)
        {
          body.localScale = Vector3.Lerp(body.localScale, crouchingScale, Time.deltaTime * crouchingSpeed);
        }
      }
      else if (isCrouching)
      {
        if (!Physics.Raycast(transform.position, Vector3.up, roofCheckHeight))
        {
          isCrouching = false;
          OnCrouch(false);
        }
      }

      if (!isCrouching && body.localScale != defaultScale)
      {
        body.localScale = Vector3.Lerp(body.localScale, defaultScale, Time.deltaTime * crouchingSpeed);
      }
    }
  }
}