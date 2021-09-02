using EPOOutline;
using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Fralle.PingTap
{
  [Serializable]
  public class DamageUIHandler
  {
    [Header("UI")]
    [SerializeField] GameObject healthbarPrefab;
    [SerializeField] GameObject floatingCombatText;
    [SerializeField] bool useOutlinable;

    DamageController damageController;
    Transform ui;
    Outlinable outlinable;

    float delay = 0.5f;
    float timer;
    bool isShowing;

    public void Setup(DamageController damageController)
    {
      this.damageController = damageController;
      this.damageController.OnReceiveAttack += HandleReceiveAttack;

      outlinable = this.damageController.GetComponentInChildren<Outlinable>();
      if (outlinable)
        outlinable.enabled = false;

      ui = this.damageController.transform.Find("TargetUI");
      if (!ui)
        return;

      ui.gameObject.SetActive(false);
      SetupUi();
    }

    public void Timer()
    {
      if (!isShowing)
        return;

      timer -= Time.deltaTime;

      if (timer <= 0)
        Toggle(false);
    }

    public void Clean()
    {
      damageController.OnReceiveAttack -= HandleReceiveAttack;
    }

    public void Toggle(bool show, float? customDelay = null)
    {
      timer = customDelay ?? delay;

      if (ui)
        ui.gameObject.SetActive(show);

      if (outlinable && useOutlinable)
        outlinable.enabled = show;

      isShowing = show;
    }

    void SetupUi()
    {
      if (healthbarPrefab)
        Object.Instantiate(healthbarPrefab, ui.transform);
      if (floatingCombatText)
        Object.Instantiate(floatingCombatText, ui.transform);
    }

    void HandleReceiveAttack(DamageController dc, DamageData dd)
    {
      if (dd.Attacker && dd.Attacker.hasActiveCamera)
        Toggle(true, 1f);
    }
  }
}
