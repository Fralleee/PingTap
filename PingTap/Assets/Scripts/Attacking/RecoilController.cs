using System.Linq;
using UnityEngine;

namespace Fralle
{
  [RequireComponent(typeof(Weapon))]
  public class RecoilController : MonoBehaviour
  {
    [SerializeField] bool randomizeRecoil;
    [SerializeField] float kickbackForce = 0.15f;
    [SerializeField] float recoverTime = 20f;
    [SerializeField] Vector2 randomRecoilConstraints;
    [SerializeField] Vector2[] recoilPattern;

    int recoilPatternStep;
    MouseLook mouseLook;
    Weapon weapon;
    Vector3[] startPositions;
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
        weapon.muzzles[i].parent.localPosition = Vector3.Lerp(weapon.muzzles[i].parent.localPosition, startPositions[i], recoverTime * Time.deltaTime);
      }
      //foreach (Transform weaponMuzzle in weapon.muzzles) weaponMuzzle.parent.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, recoverTime * Time.deltaTime);
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

        Vector2 recoil = new Vector2(xRecoil, yRecoil);
        mouseLook.AddRotation(recoil);
      }
      else if (recoilPattern.Length > 0)
      {
        if (recoilPatternStep > recoilPattern.Length - 1) recoilPatternStep = 0;
        mouseLook.AddRotation(recoilPattern[recoilPatternStep]);
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