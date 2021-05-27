using UnityEngine;

namespace Fralle.PingTap
{
	public class AimAtTargetAction : StateMachineBehaviour
	{
		override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			Debug.Log("AimAtTarget::Enter");
		}

		override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{

		}

		override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{

		}
	}
}
