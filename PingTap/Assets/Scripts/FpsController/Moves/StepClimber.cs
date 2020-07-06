using System.Collections.Generic;
using UnityEngine;

namespace Fralle.FpsController.Moves
{
  public class StepClimber : MonoBehaviour
  {
    public float stepSearchOvershoot = 0.01f;

    PlayerController controller;
    new Rigidbody rigidbody;
    readonly List<ContactPoint> allCPs = new List<ContactPoint>();
    Vector3 lastVelocity;

    void Awake()
    {
      controller = GetComponentInParent<PlayerController>();

      rigidbody = GetComponent<Rigidbody>();
    }

    public void ClimbSteps()
    {
      Vector3 velocity = rigidbody.velocity;

      var grounded = FindGround(out ContactPoint groundCP, allCPs);

      Vector3 stepUpOffset = default;
      bool stepUp = false;
      if (grounded) stepUp = FindStep(out stepUpOffset, allCPs, groundCP, velocity);

      if (stepUp)
      {
        rigidbody.position += stepUpOffset;
        rigidbody.velocity = lastVelocity;
      }

      allCPs.Clear();
      lastVelocity = velocity;
    }


    bool FindGround(out ContactPoint groundCP, List<ContactPoint> allCPs)
    {
      groundCP = default;
      var found = false;
      foreach (ContactPoint cp in allCPs)
      {
        if (cp.normal.y > 0.0001f && (found == false || cp.normal.y > groundCP.normal.y))
        {
          groundCP = cp;
          found = true;
        }
      }

      return found;
    }

    bool FindStep(out Vector3 stepUpOffset, List<ContactPoint> allCPs, ContactPoint groundCP, Vector3 currVelocity)
    {
      stepUpOffset = default;
      Vector2 velocityXZ = new Vector2(currVelocity.x, currVelocity.z);
      if (velocityXZ.sqrMagnitude < 0.0001f)
        return false;

      foreach (ContactPoint cp in allCPs)
      {
        bool test = ResolveStepUp(out stepUpOffset, cp, groundCP);
        if (test)
          return test;
      }
      return false;
    }

    bool ResolveStepUp(out Vector3 stepUpOffset, ContactPoint stepTestCP, ContactPoint groundCP)
    {
      stepUpOffset = default;
      Collider stepCol = stepTestCP.otherCollider;

      if (Mathf.Abs(stepTestCP.normal.y) >= 0.01f)
      {
        return false;
      }

      if (!(stepTestCP.point.y - groundCP.point.y < controller.stepHeight))
      {
        return false;
      }

      float stepHeight = groundCP.point.y + controller.stepHeight + 0.0001f;
      Vector3 stepTestInvDir = new Vector3(-stepTestCP.normal.x, 0, -stepTestCP.normal.z).normalized;
      Vector3 origin = new Vector3(stepTestCP.point.x, stepHeight, stepTestCP.point.z) + (stepTestInvDir * stepSearchOvershoot);
      Vector3 direction = Vector3.down;
      if (!(stepCol.Raycast(new Ray(origin, direction), out RaycastHit hitInfo, controller.stepHeight)))
      {
        return false;
      }

      Vector3 stepUpPoint = new Vector3(stepTestCP.point.x, hitInfo.point.y + 0.0001f, stepTestCP.point.z) + (stepTestInvDir * stepSearchOvershoot);
      Vector3 stepUpPointOffset = stepUpPoint - new Vector3(stepTestCP.point.x, groundCP.point.y, stepTestCP.point.z);

      stepUpOffset = stepUpPointOffset;
      return true;
    }

    void OnCollisionEnter(Collision col)
    {
      allCPs.AddRange(col.contacts);
    }

    void OnCollisionStay(Collision col)
    {
      allCPs.AddRange(col.contacts);
    }
  }
}