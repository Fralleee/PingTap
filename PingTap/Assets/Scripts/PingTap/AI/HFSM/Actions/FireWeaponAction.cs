using UnityEngine;

namespace Fralle.PingTap
{
	public class FireWeaponAction : StateMachineBehaviour
	{
		override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			Debug.Log("FireWeapon::Enter");
		}

		override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{

		}

		override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{

		}
	}
}
