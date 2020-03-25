using UnityEngine;

namespace Fralle
{
  public class RecoilController : MonoBehaviour
  {
    [SerializeField] bool randomizeRecoil;
    [SerializeField] Vector2 randomRecoilConstraints;
    [SerializeField] Vector2[] recoilPattern;

    int recoilPatternStep;
    MouseLook mouseLook;

    public void Initiate(Transform playerCamera)
    {
      mouseLook = playerCamera.GetComponentInParent<MouseLook>();
    }

    public void AddRecoil()
    {
      if (mouseLook == null) return;
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
  }
}