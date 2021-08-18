using Fralle.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Fralle.PingTap
{
  [Serializable]
  public class DamageEffectHandler
  {
    [HideInInspector] public List<DamageEffect> DamageEffects = new List<DamageEffect>();

    DamageController damageController;

    public void Setup(DamageController damageController)
    {
      this.damageController = damageController;
    }

    public void DamageEffectsTick()
    {
      for (int i = 0; i < DamageEffects.Count; i++)
      {
        DamageEffects[i].Tick(damageController);
        if (DamageEffects[i].Timer <= DamageEffects[i].Time)
          continue;
        DamageEffects[i].Exit(damageController);
        DamageEffects.RemoveAt(i);
      }
    }

    public void ApplyEffects(DamageData damageData)
    {
      foreach (DamageEffect t in damageData.Effects)
      {
        DamageEffect effect = t;
        DamageEffect oldEffect = DamageEffects.FirstOrDefault(x => x.name == effect.name);
        effect = effect.Append(oldEffect);
        effect.Enter(damageController);
        DamageEffects.Upsert(oldEffect, effect);
      }
    }
  }
}
