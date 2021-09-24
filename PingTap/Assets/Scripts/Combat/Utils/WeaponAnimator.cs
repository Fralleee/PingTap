using Fralle.Core;
using Fralle.PingTap;
using System.Collections;
using UnityEngine;

public static class WeaponAnimator
{
  //[SerializeField] Vector3 unequippedPosition;
  //[SerializeField] Quaternion unequippedRotation;

  //[SerializeField] Vector3 equippedPosition;
  //[SerializeField] Quaternion equippedRotation;

  //Coroutine lerp;

  public static void AnimateEquip(Combatant combatant, float duration)
  {
    Vector3 position = new Vector3(0, -0.1f, 0);
    Quaternion rotation = Quaternion.Euler(0, 0, 0);

    combatant.StartCoroutine(Animate(duration, combatant.weaponHolder, position, rotation));
  }

  public static void AnimateUnequip(Combatant combatant, float duration)
  {
    Vector3 position = new Vector3(0, -0.3f, 0);
    Quaternion rotation = Quaternion.Euler(60f, 0, 0);

    combatant.StartCoroutine(Animate(duration, combatant.weaponHolder, position, rotation));
  }

  //public void AnimateReload(float duration)
  //{

  //}

  //public void AnimateShoot(float duration)
  //{

  //}

  static IEnumerator Animate(float duration, Transform transform, Vector3 position, Quaternion rotation)
  {
    Vector3 startPosition = transform.localPosition;
    Quaternion startRotation = transform.localRotation;
    float timeElapsed = 0;

    while (timeElapsed < duration)
    {
      transform.localPosition = Vector3Easings.EaseInOutBounce(startPosition, position, timeElapsed / duration);
      transform.localRotation = Quaternion.Euler(Vector3Easings.EaseInOutBounce(startRotation.eulerAngles, rotation.eulerAngles, timeElapsed / duration));
      timeElapsed += Time.deltaTime;

      yield return null;
    }

    transform.localPosition = position;
    transform.localRotation = rotation;
  }
}
