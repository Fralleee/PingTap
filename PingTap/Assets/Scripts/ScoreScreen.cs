using TMPro;
using UnityEngine;

public class ScoreScreen : MonoBehaviour
{
  [SerializeField] TextMeshProUGUI killsValue;
  [SerializeField] TextMeshProUGUI totalDamageValue;

  [SerializeField] TextMeshProUGUI nerveHitsValue;
  [SerializeField] TextMeshProUGUI majorHitsValue;
  [SerializeField] TextMeshProUGUI minorHitsValue;

  [SerializeField] TextMeshProUGUI totalShotsValue;
  [SerializeField] TextMeshProUGUI totalHitsValue;
  [SerializeField] TextMeshProUGUI accuracyValue;

  public void InitPlayerStats(PlayerStats stats)
  {
    killsValue.text = stats.killingBlows.ToString();
    totalDamageValue.text = stats.totalDamage.ToString("# ##0.00");

    nerveHitsValue.text = stats.nerveHits.ToString();
    majorHitsValue.text = stats.majorHits.ToString();
    minorHitsValue.text = stats.minorHits.ToString();

    totalShotsValue.text = stats.totalShotsFired.ToString();
    totalHitsValue.text = stats.totalShotsHit.ToString();
    accuracyValue.text = $"{stats.accuracyPercentage * 100:##.#}%";
  }
}
