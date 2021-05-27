using UnityEngine;

namespace Fralle.PingTap
{
	public class SearchForTargetDecision : StateMachineBehaviour
	{
		override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			Debug.Log("SearchForTarget::Enter");
		}

		override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			// Save target to Enemy-script
			if (LookForTarget(animator.transform))
				animator.SetBool("HasTarget", true);
		}

		override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			Debug.Log("SearchForTarget::Exit");
		}

		bool LookForTarget(Transform transform)
		{
			if (Physics.SphereCast(transform.position, 1f, transform.forward, out RaycastHit raycastHit, 5f))
			{
				if (raycastHit.transform.CompareTag("Player"))
					return true;
			}
			return false;
		}
	}
}
