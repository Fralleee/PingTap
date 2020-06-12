using Fralle.Core.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Fralle.UI.HUD
{
  public class WaveStatusUi : MonoBehaviour
  {
    [SerializeField] Image foreGround = null;

    UiTweener uiTweener;

    void Awake()
    {
      uiTweener = GetComponent<UiTweener>();
    }

    public void SetFill(float percentage)
    {
      if (foreGround.fillAmount.EqualsWithTolerance(1f)) return;

      foreGround.fillAmount = percentage;
      if (percentage.EqualsWithTolerance(1f)) Animate();
    }

    void Animate()
    {
      uiTweener.HandleTween();
    }
  }
}