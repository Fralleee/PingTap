using CombatSystem.Combat.Damage;
using UnityEngine;
using UnityEngine.AI;

namespace Fralle.AI
{
	public class AIAnimator : MonoBehaviour
	{
		Animator animator;
		AIController aiController;
		DamageController damageController;

		void Awake()
		{
			animator = GetComponent<Animator>();
			aiController = GetComponentInParent<AIController>();
			damageController = GetComponentInParent<DamageController>();
			damageController.OnDeath += DamageController_OnDeath;
		}

		void DamageController_OnDeath(DamageController arg1, DamageData arg2)
		{
			animator.enabled = false;
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
