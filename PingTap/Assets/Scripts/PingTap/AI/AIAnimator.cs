using CombatSystem.Combat.Damage;
using UnityEngine;
using UnityEngine.AI;

namespace Fralle.AI
{
	public class AIAnimator : MonoBehaviour
	{
		[SerializeField] bool useSetSpeed;

		Animator animator;
		AIController aiController;
		NavMeshAgent navMeshAgent;

		void Awake()
		{
			animator = GetComponent<Animator>();
			aiController = GetComponentInParent<AIController>();
			navMeshAgent = GetComponentInParent<NavMeshAgent>();
			var damageController = GetComponentInParent<DamageController>();
			damageController.OnDeath += HandleDeath;
		}

		void Update()
		{
			animator.SetBool("IsMoving", aiController.IsMoving);
		}

		void OnAnimatorMove()
		{
			if (!useSetSpeed)
				navMeshAgent.speed = (animator.deltaPosition / Time.deltaTime).magnitude;
		}

		void HandleDeath(DamageController damageController, DamageData damageData)
		{
			animator.enabled = false;
			this.enabled = false;
		}
	}
}
