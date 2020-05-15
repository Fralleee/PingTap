using Fralle.AI;
using Fralle.AI.Spawning;
using Fralle.Core.Animation;
using Fralle.Gameplay;
using TMPro;
using UnityEngine;

namespace Fralle.UI.HUD
{
  public class WaveInformationUi : MonoBehaviour
  {
    [SerializeField] TextMeshProUGUI enemyName;
    [SerializeField] TextMeshProUGUI enemyCurrentCount;
    [SerializeField] TextMeshProUGUI enemyTotalCount;

    [SerializeField] TextMeshProUGUI armorAmount;
    [SerializeField] TextMeshProUGUI elementModifiers;
    [SerializeField] TextMeshProUGUI protections;

    [SerializeField] TextMeshProUGUI totalTimer;

    MatchManager matchManager;
    UiTweener tweener;

    void Awake()
    {
      matchManager = GetComponentInParent<MatchManager>();
      tweener = GetComponentInParent<UiTweener>();

      Enemy.OnAnyEnemyDeath += HandleEnemyDeath;
      WaveManager.OnNewWave += HandleNewWave;
    }

    void Update()
    {
      SetTimer(matchManager.totalTimer);
    }

    void HandleNewWave(WaveManager waveManager)
    {
      var wave = waveManager.GetCurrentWave;
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
      enemyCurrentCount.text = matchManager.enemiesAlive.ToString();
      tweener.HandleTween();
    }

    void OnDestroy()
    {
      Enemy.OnAnyEnemyDeath -= HandleEnemyDeath;
      WaveManager.OnNewWave -= HandleNewWave;
    }
  }
}