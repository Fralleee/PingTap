using Fralle.Core;
using Fralle.PingTap;
using System.Collections;
using UnityEngine;

public class WeaponAnimationTransformer : LocalTransformer, IWeaponAnimator, IRotator, IPositioner
{
  Vector3 currentPosition = Vector3.zero;
  Quaternion currentRotation = Quaternion.identity;

  public Vector3 GetPosition() => currentPosition;
  public Quaternion GetRotation() => currentRotation;
  public override void Calculate() { }

  public void AnimateEquip(Combatant combatant, float duration)
  {
    Vector3 position = new Vector3(0, -0.1f, 0);
    Quaternion rotation = Quaternion.Euler(0, 0, 0);

    combatant.StartCoroutine(Animate(duration, combatant.weaponHolder, position, rotation));
  }

  public void AnimateUnequip(Combatant combatant, float duration)
  {
    Vector3 position = new Vector3(0, -0.3f, 0);
    Quaternion rotation = Quaternion.Euler(60f, 0, 0);

    combatant.StartCoroutine(Animate(duration, combatant.weaponHolder, position, rotation));
  }

  IEnumerator Animate(float duration, Transform transform, Vector3 position, Quaternion rotation)
  {
    Vector3 startPosition = currentPosition;
    Quaternion startRotation = currentRotation;
    float timeElapsed = 0;

    while (timeElapsed < duration)
    {
      currentPosition = Vector3Easings.EaseInOutBack(startPosition, position, timeElapsed / duration);
      currentRotation = Quaternion.Euler(Vector3Easings.EaseInOutBack(startRotation.eulerAngles, rotation.eulerAngles, timeElapsed / duration));
      timeElapsed += Time.deltaTime;

      yield return null;
    }

    currentPosition = position;
    currentRotation = rotation;
  }

}
