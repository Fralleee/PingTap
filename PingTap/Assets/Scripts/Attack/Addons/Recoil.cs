using Fralle.Movement;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Fralle.Attack.Addons
{
  [RequireComponent(typeof(Weapon))]
  public class Recoil : MonoBehaviour
  {
    [Header("Kickback")]
    [SerializeField] float kickbackForce = 0.15f;
    [FormerlySerializedAs("recoverTime")] [SerializeField] float kickbackRecoverTime = 20f;

    [Header("Recoil")]
    [SerializeField] bool randomizeRecoil = false;
    [SerializeField] float recoilSpeed = 15f;
    [SerializeField] float recoilRecoverTime = 10f;
    [SerializeField] Vector3 randomRecoilConstraints = Vector3.zero;
    [SerializeField] Vector3[] recoilPattern = new Vector3[0];

    int recoilPatternStep;
    PlayerMouseLook playerMouseLook;
    Weapon weapon;
    Vector3[] startPositions;
    Vector3 recoil;
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

      weapon.playerCamera.localRotation = Quaternion.RotateTowards(weapon.playerCamera.localRotation, Quaternion.Euler(recoil.y, recoil.x, recoil.z), recoilSpeed * Time.deltaTime);
      recoil = Vector3.Lerp(recoil, Vector3.zero, recoilRecoverTime * Time.deltaTime);
    }

    public void Initiate(Transform playerCamera)
    {
      playerMouseLook = playerCamera.GetComponentInParent<PlayerMouseLook>();
    }

    public void AddRecoil()
    {
      if (playerMouseLook == null) return;

      AddKickback();

      if (randomizeRecoil)
      {
        var xRecoil = Random.Range(-randomRecoilConstraints.x, randomRecoilConstraints.x);
        var yRecoil = Random.Range(-randomRecoilConstraints.y, randomRecoilConstraints.y);
        var zRecoil = Random.Range(-randomRecoilConstraints.z, randomRecoilConstraints.z);
        recoil += new Vector3(xRecoil, yRecoil, zRecoil);
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

      var muzzle = GetMuzzle();
      muzzle.parent.localPosition -= new Vector3(0, 0, kickbackForce);
    }

    Transform GetMuzzle()
    {
      var muzzle = weapon.muzzles[nextMuzzle];
      if (weapon.muzzles.Length <= 1) return muzzle;

      nextMuzzle++;
      if (nextMuzzle > weapon.muzzles.Length - 1) nextMuzzle = 0;

      return muzzle;
    }
  }
}