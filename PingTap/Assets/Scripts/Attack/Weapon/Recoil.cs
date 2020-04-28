using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Fralle.Attack
{
  [RequireComponent(typeof(Weapon))]
  public class Recoil : MonoBehaviour
  {
    [Header("Kickback")]
    [SerializeField] float kickbackForce = 0.15f;
    [FormerlySerializedAs("recoverTime")] [SerializeField] float kickbackRecoverTime = 20f;

    [Header("Recoil")]
    [SerializeField] bool randomizeRecoil;
    [SerializeField] float recoilSpeed = 15f;
    [SerializeField] float recoilRecoverTime = 10f;
    [SerializeField] Vector2 randomRecoilConstraints;
    [SerializeField] Vector2[] recoilPattern;

    int recoilPatternStep;
    MouseLook mouseLook;
    Weapon weapon;
    Vector3[] startPositions;
    Vector2 recoil;
    int nextMuzzle;

    void Awake()
    {
      weapon = GetComponent<Weapon>();
      if (!(kickbackForce > 0)) return;
      startPositions = weapon.muzzles.Select(x => x.parent.localPosition).ToArray();
    }

    void Update()
    {
      if (!(kickbackForce > 0)) return;
      for (var i = 0; i < startPositions.Length; i++)
      {
        weapon.muzzles[i].parent.localPosition = Vector3.Lerp(weapon.muzzles[i].parent.localPosition, startPositions[i], kickbackRecoverTime * Time.deltaTime);
      }

      weapon.playerCamera.localRotation = Quaternion.RotateTowards(weapon.playerCamera.localRotation, Quaternion.Euler(recoil.y, recoil.x, 0), recoilSpeed * Time.deltaTime);
      recoil = Vector2.Lerp(recoil, Vector2.zero, recoilRecoverTime * Time.deltaTime);
    }

    public void Initiate(Transform playerCamera)
    {
      mouseLook = playerCamera.GetComponentInParent<MouseLook>();
    }

    public void AddRecoil()
    {
      if (mouseLook == null) return;

      AddKickback();

      if (randomizeRecoil)
      {
        float xRecoil = Random.Range(-randomRecoilConstraints.x, randomRecoilConstraints.x);
        float yRecoil = Random.Range(-randomRecoilConstraints.y, randomRecoilConstraints.y);
        recoil += new Vector2(xRecoil, yRecoil);
      }
      else if (recoilPattern.Length > 0)
      {
        if (recoilPatternStep > recoilPattern.Length - 1) recoilPatternStep = 0;
        recoil += recoilPattern[recoilPatternStep];
        recoilPatternStep++;
      }
    }

    void AddKickback()
    {
      if (!(kickbackForce > 0)) return;

      Transform muzzle = GetMuzzle();
      muzzle.parent.localPosition -= new Vector3(0, 0, kickbackForce);
    }

    Transform GetMuzzle()
    {
      Transform muzzle = weapon.muzzles[nextMuzzle];
      if (weapon.muzzles.Length <= 1) return muzzle;

      nextMuzzle++;
      if (nextMuzzle > weapon.muzzles.Length - 1) nextMuzzle = 0;

      return muzzle;
    }
  }
}