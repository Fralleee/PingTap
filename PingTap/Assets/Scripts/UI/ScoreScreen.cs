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

    public void InitPlayerStats()
    {
      Debug.Log($"ScoreScreen: Stats are not currently used");
      Debug.Log(killsValue.text);
      Debug.Log(totalDamageValue.text);
      Debug.Log(nerveHitsValue.text);
      Debug.Log(majorHitsValue.text);
      Debug.Log(minorHitsValue.text);
      Debug.Log(totalShotsValue.text);
      Debug.Log(totalHitsValue.text);
      Debug.Log(accuracyValue.text);
      //killsValue.text = stats.killingBlows.ToString();
      //totalDamageValue.text = stats.totalDamage.ToString("# ##0.00");

      //totalShotsValue.text = stats.totalShotsFired.ToString();
      //totalHitsValue.text = stats.totalShotsHit.ToString();
      //accuracyValue.text = $"{stats.accuracyPercentage * 100:##.#}%"; 
    }
  }
}