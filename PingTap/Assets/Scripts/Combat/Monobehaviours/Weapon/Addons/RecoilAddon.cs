using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Fralle.PingTap
{
  [RequireComponent(typeof(Weapon))]
  public class RecoilAddon : MonoBehaviour
  {
    [Header("Kickback")]
    [SerializeField] float kickbackForce = 0.15f;
    [FormerlySerializedAs("recoverTime")] [SerializeField] float kickbackRecoverTime = 20f;

    [Header("RecoilAddon")]
    [SerializeField] bool randomizeRecoil = false;
    [SerializeField] float recoilSpeed = 15f;
    [SerializeField] float recoilRecoverTime = 10f;
    [SerializeField] Vector3 randomRecoilConstraints = Vector3.zero;
    [SerializeField] Vector3[] recoilPattern = new Vector3[0];

    Weapon weapon;
    RecoilTransformer recoilTransformer;

    Vector3[] startPositions;
    Vector3 recoil;

    int recoilPatternStep;
    int nextMuzzle;
    float recoilStatMultiplier = 1f;

    void Awake()
    {
      weapon = GetComponent<Weapon>();

      startPositions = weapon.Muzzles.Select(x => x.parent.localPosition).ToArray();
    }

    void Update()
    {
      if (!weapon.IsEquipped)
        return;
      if (kickbackForce <= 0)
        return;

      for (int i = 0; i < startPositions.Length; i++)
      {
        weapon.Muzzles[i].parent.localPosition = Vector3.Lerp(weapon.Muzzles[i].parent.localPosition, startPositions[i], kickbackRecoverTime * Time.deltaTime);
      }
    }
    public void Activate()
    {
      recoilTransformer = weapon.Combatant.AimTransform.GetComponentInChildren<RecoilTransformer>();
      if (recoilTransformer != null)
        recoilTransformer.Setup(recoilSpeed, recoilRecoverTime);
      else
        Debug.Log($"{weapon.Combatant.name} does not have RecoilTransformer behaviour on AimTransform");
    }

    public void AddRecoil()
    {

      AddKickback();

      if (randomizeRecoil)
      {
        float xRecoil = Random.Range(-randomRecoilConstraints.x, randomRecoilConstraints.x);
        float yRecoil = Random.Range(-randomRecoilConstraints.y, randomRecoilConstraints.y);
        float zRecoil = Random.Range(-randomRecoilConstraints.z, randomRecoilConstraints.z);
        recoilTransformer.AddRecoil(new Vector3(xRecoil, yRecoil, zRecoil) * recoilStatMultiplier);
      }
      else if (recoilPattern.Length > 0)
      {
        recoilTransformer.AddRecoil(recoilPattern[recoilPatternStep] * recoilStatMultiplier);
        recoilPatternStep++;
        if (recoilPatternStep > recoilPattern.Length - 1)
          recoilPatternStep = 0;
      }
    }

    void AddKickback()
    {
      if (kickbackForce <= 0)
        return;

      Transform muzzle = GetMuzzle();
      muzzle.parent.localPosition -= new Vector3(0, 0, kickbackForce);
    }

    Transform GetMuzzle()
    {
      Transform muzzle = weapon.Muzzles[nextMuzzle];
      if (weapon.Muzzles.Length <= 1)
        return muzzle;

      nextMuzzle++;
      if (nextMuzzle > weapon.Muzzles.Length - 1)
        nextMuzzle = 0;

      return muzzle;
    }
  }
}
