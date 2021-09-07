using Fralle.AbilitySystem;
using System.Collections;
using UnityEngine;

namespace Fralle.PingTap
{
  [CreateAssetMenu(menuName = "Abilities/ZigZag")]
  public class ZigZagAbility : ActiveAbility
  {
    [Header("Settings")]
    [SerializeField] int zigZags = 4;
    [SerializeField] float strafeFactor = 0.75f;
    [SerializeField] float dashPower = 20;
    [SerializeField] float delayBetweenDashes = 0.05f;
    [SerializeField] float dashTime = 0.2f;

    AbilityController abilityController;
    PlayerController playerController;
    Rigidbody rigidBody;

    Coroutine coroutine;
    bool zagRight;

    public override void Setup(AbilityController ac)
    {
      abilityController = ac;
      playerController = ac.GetComponent<PlayerController>();
      rigidBody = ac.GetComponentInChildren<Rigidbody>();
    }

    public override void Perform()
    {
      base.Perform();

      playerController.isLocked = true;
      coroutine = abilityController.StartCoroutine(ZigZag());
    }

    IEnumerator ZigZag()
    {
      isActive = true;
      for (int i = 0; i < zigZags; i++)
      {
        rigidBody.velocity = Vector3.zero;
        yield return new WaitForSeconds(delayBetweenDashes);
        Vector3 direction = (playerController.cameraRig.forward + (zagRight ? playerController.cameraRig.right : -playerController.cameraRig.right) * strafeFactor).normalized;
        rigidBody.AddForce(direction * dashPower, ForceMode.VelocityChange);
        zagRight = !zagRight;
        yield return new WaitForSeconds(dashTime);
      }

      Reset();
    }

    void Reset()
    {
      rigidBody.velocity = Vector3.zero;
      zagRight = false;
      playerController.isLocked = false;
      isActive = false;
    }

    public override void Abort()
    {
      abilityController.StopCoroutine(coroutine);
      Reset();
    }
  }
}
