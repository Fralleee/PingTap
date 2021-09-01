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
    Transform orientation;

    Coroutine coroutine;
    bool zagRight;

    public override void Setup(AbilityController ac)
    {
      abilityController = ac;
      playerController = ac.GetComponent<PlayerController>();
      rigidBody = ac.GetComponentInChildren<Rigidbody>();
      orientation = rigidBody.transform.Find("Orientation");
    }

    public override void Perform()
    {
      base.Perform();

      playerController.isLocked = true;
      coroutine = abilityController.StartCoroutine(ZigZag());
    }

    IEnumerator ZigZag()
    {
      IsActive = true;
      for (int i = 0; i < zigZags; i++)
      {
        rigidBody.velocity = Vector3.zero;
        yield return new WaitForSeconds(delayBetweenDashes);
        Vector3 direction = (orientation.forward + (zagRight ? orientation.right : -orientation.right) * strafeFactor).normalized;
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
      IsActive = false;
    }

    public override void Abort()
    {
      abilityController.StopCoroutine(coroutine);
      Reset();
    }

    void OnDestroy()
    {
    }
  }
}
