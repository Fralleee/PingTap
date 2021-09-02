using Fralle.FpsController;
using Fralle.PingTap.AI;
using UnityEngine;

namespace Fralle.PingTap
{
  [CreateAssetMenu(menuName = "AI/States/Battle/Basic")]
  public class BasicBattleState : BattleState
  {
    AIBrain aiBrain;
    AIAttack aiAttack;
    AITargetingSystem aiTargetingSystem;
    AIController controller;

    bool doRotate;
    readonly float rotateOnAngle = 35;
    readonly float defaultStoppingDistance = 0.5f;

    public override void OnEnter()
    {
      controller.Speed = controller.walkSpeed;
      controller.StoppingDistance = aiBrain.attackStoppingDistance;

      aiBrain.AlertOthers(aiTargetingSystem.TargetPosition, AIState.Chasing);
    }

    public override void OnLogic()
    {
      controller.SetDestination(aiTargetingSystem.TargetPosition);
      aiAttack.AimAt(aiTargetingSystem.TargetPosition);

      UpdateRotation();
      aiAttack.Attack(aiTargetingSystem.TargetPosition, aiBrain.attackRange);

      if (Time.time > aiBrain.lastAlert)
        aiBrain.AlertOthers(aiTargetingSystem.TargetPosition, AIState.Chasing);
    }

    public override void OnExit()
    {
      controller.StoppingDistance = defaultStoppingDistance;
      controller.Stop();
    }

    public override void Setup(AIBrain aiBrain)
    {
      this.aiBrain = aiBrain;
      aiAttack = aiBrain.GetComponent<AIAttack>();
      aiTargetingSystem = aiBrain.GetComponent<AITargetingSystem>();
      controller = aiBrain.GetComponent<AIController>();
    }

    void UpdateRotation()
    {
      if (controller.Velocity.magnitude > 0.1f)
        doRotate = false;
      else if (Vector3.Angle(controller.transform.forward, aiAttack.aim.forward) > rotateOnAngle)
        doRotate = true;

      if (!doRotate)
        return;

      controller.transform.rotation = Quaternion.Lerp(controller.transform.rotation, aiAttack.aim.rotation, Time.deltaTime * 10f);
      if (Vector3.Angle(controller.transform.forward, aiAttack.aim.forward) > 3f)
        return;

      controller.transform.rotation = aiAttack.aim.rotation;
      doRotate = false;
    }

  }
}
