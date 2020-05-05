using Fralle.Core.Animation;
using UnityEngine;
using UnityEngine.UI;

namespace Fralle.UI.HUD
{
  public class WaveStatusUi : MonoBehaviour
  {
    [SerializeField] Image foreGround;

    UiTweener uiTweener;

    void Awake()
    {
      uiTweener = GetComponent<UiTweener>();
    }

    public void SetFill(float percentage)
    {
      if (foreGround.fillAmount == 1) return;

      foreGround.fillAmount = percentage;
      if (percentage == 1) Animate();
    }

    void Animate()
    {
      uiTweener.HandleTween();
    }
  }
}