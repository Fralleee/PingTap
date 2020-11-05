using UnityEngine;
using UnityEngine.AI;

namespace Fralle.AI
{
	public class AIAnimator : MonoBehaviour
	{
		Animator animator;
		AIController aiController;

		void Awake()
		{
			animator = GetComponent<Animator>();
			aiController = GetComponentInParent<AIController>();
		}

		void Update()
		{
			animator.SetBool("IsMoving", aiController.IsMoving);

		}

		void OnAnimatorMove()
		{
			GetComponentInParent<NavMeshAgent>().speed = (animator.deltaPosition / Time.deltaTime).magnitude;
		}
	}
}
