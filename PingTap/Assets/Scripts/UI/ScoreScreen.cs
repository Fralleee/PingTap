using Fralle.Player;
using TMPro;
using UnityEngine;

namespace Fralle.UI
{
  public class ScoreScreen : MonoBehaviour
  {
    [SerializeField] TextMeshProUGUI killsValue = null;
    [SerializeField] TextMeshProUGUI totalDamageValue = null;

    [SerializeField] TextMeshProUGUI nerveHitsValue = null;
    [SerializeField] TextMeshProUGUI majorHitsValue = null;
    [SerializeField] TextMeshProUGUI minorHitsValue = null;

    [SerializeField] TextMeshProUGUI totalShotsValue = null;
    [SerializeField] TextMeshProUGUI totalHitsValue = null;
    [SerializeField] TextMeshProUGUI accuracyValue = null;

    public void InitPlayerStats(PlayerStats stats)
    {
      killsValue.text = stats.killingBlows.ToString();
      totalDamageValue.text = stats.totalDamage.ToString("# ##0.00");

      totalShotsValue.text = stats.totalShotsFired.ToString();
      totalHitsValue.text = stats.totalShotsHit.ToString();
      accuracyValue.text = $"{stats.accuracyPercentage * 100:##.#}%";
    }
  }
}