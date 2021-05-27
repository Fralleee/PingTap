using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimation : MonoBehaviour
{
	Animator animator;
	NavMeshAgent navMeshAgent;

	void Awake()
	{
		animator = GetComponent<Animator>();
		navMeshAgent = GetComponentInParent<NavMeshAgent>();
	}

	void Update()
	{
		animator.SetBool("IsMoving", navMeshAgent.remainingDistance > 0f);
	}
}
