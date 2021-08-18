using Fralle.Core;
using UnityEngine;

namespace Fralle.PingTap
{
  public class RecoilTransformer : LocalTransformer, IRotator
  {
    [Readonly] public float recoilSpeed = 15f;
    [Readonly] public float recoilRecoverTime = 10f;

    Vector3 recoil = Vector3.zero;
    Quaternion currentRotation = Quaternion.identity;

    public Quaternion GetRotation() => currentRotation;

    public override void Calculate()
    {
      Quaternion toRotation = Quaternion.Euler(recoil.y, recoil.x, recoil.z);
      currentRotation = Quaternion.RotateTowards(currentRotation, toRotation, recoilSpeed * Time.deltaTime);
      recoil = Vector3.Lerp(recoil, Vector3.zero, recoilRecoverTime * Time.deltaTime);
    }

    public void Setup(float recoilSpeed, float recoilRecoverTime)
    {
      this.recoilSpeed = recoilSpeed;
      this.recoilRecoverTime = recoilRecoverTime;
    }

    public void AddRecoil(Vector3 vector3)
    {
      recoil += vector3;
    }
  }
}
