using CombatSystem.Combat.Damage;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class DamageIK : MonoBehaviour
{
	[SerializeField] Transform body;
	[SerializeField] Transform target;
	[SerializeField] float resetTime = 0.25f;
	[SerializeField] [Range(0,1)] float maxWeight = 0.15f;

	DamageController damageController;
	ChainIKConstraint chainIKConstraint;

	float resetTimer;

	void Awake()
	{
		damageController = GetComponent<DamageController>();
		damageController.OnReceiveAttack += HandleReceiveAttack;

		chainIKConstraint = target.GetComponentInParent<ChainIKConstraint>();
	}

	private void Update()
	{
		if (resetTimer <= 0)
			return;

		float percentage = 1 - (resetTimer * (1 / resetTime));
		chainIKConstraint.weight = Mathf.Lerp(maxWeight, 0, percentage);
		resetTimer -= Time.deltaTime;
	}

	void HandleReceiveAttack(DamageController dc, DamageData dd)
	{
		if (resetTimer > 0)
			return;

		Vector3 direction = dd.Force.normalized * 2f;
		target.position = body.position + direction; // .With(y: -1.5f);
		chainIKConstraint.weight = maxWeight;
		resetTimer = resetTime;
	}
}
