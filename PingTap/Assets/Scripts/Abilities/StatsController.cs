using Fralle.CharacterStats;
using UnityEngine;

namespace Fralle.PingTap
{
  public class StatsController : StatsControllerBase
  {
    [Header("Major stats")]
    public CharacterMajorStat agility;
    public CharacterMajorStat dexterity;
    public CharacterMajorStat strength;


    [Header("Minor stats")]
    public CharacterMinorStat aim;
    public CharacterMinorStat jumpPower;
    public CharacterMinorStat reloadSpeed;
    public CharacterMinorStat runSpeed;


    Combatant combatatant;

    protected override void Awake()
    {
      base.Awake();

      AddMajorStatToDict(StatAttribute.Dexterity, dexterity);
      AddMajorStatToDict(StatAttribute.Agility, agility);
      AddMajorStatToDict(StatAttribute.Strength, strength);

      AddMinorStatToDict(StatAttribute.Aim, aim);
      AddMinorStatToDict(StatAttribute.Jumppower, jumpPower);
      AddMinorStatToDict(StatAttribute.Reloadspeed, reloadSpeed);
      AddMinorStatToDict(StatAttribute.Runspeed, runSpeed);

      combatatant = GetComponent<Combatant>();
    }

    void Start()
    {
      // Event handlers	
      aim.OnChanged += AimChanged;
    }

    void AimChanged(CharacterStat aim)
    {
      combatatant.modifiers.ExtraAccuracy = aim.Value;
    }

    void OnDestroy()
    {
      aim.OnChanged -= AimChanged;
    }
  }
}
