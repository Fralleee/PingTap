using Fralle.AI;
using Fralle.AI.Spawning;
using Fralle.Gameplay;
using TMPro;
using UnityEngine;

namespace Fralle.UI.HUD
{
  public class WaveInformationUi : MonoBehaviour
  {
    [SerializeField] TextMeshProUGUI enemyName = null;
    [SerializeField] TextMeshProUGUI enemyCurrentCount = null;
    [SerializeField] TextMeshProUGUI enemyTotalCount = null;

    [SerializeField] TextMeshProUGUI armorAmount = null;
    [SerializeField] TextMeshProUGUI elementModifiers = null;
    [SerializeField] TextMeshProUGUI protections = null;

    [SerializeField] TextMeshProUGUI totalTimer = null;

    UiTweener uiTweener;

    void Awake()
    {
      uiTweener = GetComponentInParent<UiTweener>();

      Enemy.OnAnyEnemyDeath += HandleEnemyDeath;
      WaveManager.OnNewWave += HandleNewWave;
    }

    void Update()
    {
      SetTimer(MatchManager.Instance.totalTimer);
    }

    void HandleNewWave()
    {
      var wave = WaveManager.Instance.GetCurrentWave;
      SetupText(wave);
    }

    void SetupText(WaveDefinition wave)
    {
      var enemy = wave.enemy;
      var armor = enemy.health.armor;
      enemyName.text = enemy.name;
      enemyCurrentCount.text = wave.count.ToString();
      enemyTotalCount.text = wave.count.ToString();
      armorAmount.text = $"Armor {armor.armor.ToString()}";

      elementModifiers.text = "";
      armor.armorElementModifiers.ForEach(x => elementModifiers.text += $"{x.element}: {x.modifier * 100}%\n");
      protections.text = armor.protection ? armor.protection.name : "No protection";
    }

    void SetTimer(float num)
    {
      var minutes = Mathf.Floor(num / 60).ToString("00");
      var seconds = (num % 60).ToString("00");
      totalTimer.text = $"{minutes}:{seconds}";
    }

    void HandleEnemyDeath(Enemy enemy)
    {
      enemyCurrentCount.text = MatchManager.Instance.enemiesAlive.ToString();
      uiTweener.HandleTween();
    }

    void OnDestroy()
    {
      Enemy.OnAnyEnemyDeath -= HandleEnemyDeath;
      WaveManager.OnNewWave -= HandleNewWave;
    }
  }
}