using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MovementDebugUI : MonoBehaviour
{
  [SerializeField] TextMeshProUGUI groundedText;
  [SerializeField] TextMeshProUGUI slopeAngleText;
  [SerializeField] TextMeshProUGUI velocityText;

  public void SetGroundedText(bool isGrounded, string groundedObject)
  {
    groundedText.text = isGrounded ? $"Grounded on {groundedObject}" : "Airborne";
  }

  public void SetSlopeAngleText(float angle)
  {
    slopeAngleText.text = $"Slope angle is {Mathf.Round(angle)}°";
  }

  public void SetVelocityText(float velocity)
  {
    velocityText.text = $"Velocity is {Mathf.Round(velocity * 3.6f)} km/h";
  }

}
