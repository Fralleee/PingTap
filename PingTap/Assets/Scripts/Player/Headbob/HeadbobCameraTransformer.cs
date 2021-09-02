using Fralle.Core;
using UnityEngine;

namespace Fralle.PingTap
{
  public class HeadbobCameraTransformer : LocalTransformer, IPositioner, IRotator
  {
    [Header("Configurations")]
    [SerializeField] HeadbobConfiguration defaultConfiguration;
    public HeadbobConfiguration overrideConfguration;
    [HideInInspector] public HeadbobConfiguration Configuration => overrideConfguration ?? defaultConfiguration;

    [Header("Speed")]
    public float SmoothSpeed = 10f;

    [Header("Controls")]
    public bool Pause;

    public float CurvePosition => Mathf.Sin(timer) * playerController.modifiedMovementSpeed * 0.1f;
    public float BobAmount => Mathf.Abs(CurvePosition);

    PlayerController playerController;
    Vector3 currentPosition = Vector3.zero;
    float timer;

    void Awake()
    {
      playerController = GetComponentInParent<PlayerController>();
    }

    public Vector3 GetPosition() => currentPosition;
    public Quaternion GetRotation() => Quaternion.identity;
    public override void Calculate()
    {
      if (Pause || !playerController.isMoving || !playerController.isGrounded)
      {
        Reset();
        return;
      }

      PerformBob();
      UpdateTimer();
    }

    void PerformBob()
    {
      currentPosition = Vector3.zero;
      currentPosition.y = BobAmount * Configuration.CameraBobbingAmount;
    }
    void Reset()
    {
      timer = 0;
      currentPosition = Vector3.Lerp(currentPosition, Vector3.zero, Time.deltaTime * SmoothSpeed);
    }
    void UpdateTimer()
    {
      timer += Configuration.BobbingSpeed * Time.deltaTime;
      if (timer > Mathf.PI * 2)
        timer -= (Mathf.PI * 2);
    }
  }
}
