using Fralle.Attack.Defense;
using TMPro;
using UnityEngine;

public class WaveInformationUI : MonoBehaviour
{
  [SerializeField] TextMeshProUGUI enemyName;
  [SerializeField] TextMeshProUGUI enemyCurrentCount;
  [SerializeField] TextMeshProUGUI enemyTotalCount;

  [SerializeField] TextMeshProUGUI armorAmount;
  [SerializeField] TextMeshProUGUI elementModifiers;
  [SerializeField] TextMeshProUGUI protections;

  [SerializeField] TextMeshProUGUI totalTimer;

  MatchManager matchManager;
  UITweener tweener;

  void Awake()
  {
    matchManager = GetComponentInParent<MatchManager>();
    tweener = GetComponentInParent<UITweener>();

    Enemy.OnAnyEnemyDeath += HandleEnemyDeath;
    WaveManager.OnNewWave += HandleNewWave;
  }

  void Update()
  {
    SetTimer(matchManager.totalTimer);
  }

  void HandleNewWave(WaveManager waveManager)
  {
    WaveDefinition wave = waveManager.GetCurrentWave;
    SetupText(wave);
  }

  void SetupText(WaveDefinition wave)
  {
    Enemy enemy = wave.enemy;
    var armor = enemy.GetComponent<Armor>();
    enemyName.text = enemy.name;
    enemyCurrentCount.text = wave.count.ToString();
    enemyTotalCount.text = wave.count.ToString();
    armorAmount.text = $"Armor {(armor ? armor.armor.ToString() : "0")}";

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
}