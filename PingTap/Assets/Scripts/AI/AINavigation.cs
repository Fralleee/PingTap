using Fralle.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace Fralle.AI
{
	public class AINavigation : MonoBehaviour
	{
		public event Action OnFinalDestination = delegate { };

		public float currentMovementModifier = 1f;

		NavMeshAgent navMeshAgent;
		readonly Dictionary<string, float> movementModifiers = new Dictionary<string, float>();

		float stopTime;
		float movementSpeed;

		public bool hasPurpose { get; private set; }

		void Awake()
		{
			navMeshAgent = GetComponent<NavMeshAgent>();
			movementSpeed = navMeshAgent.speed;
		}

		void Update()
		{
			if (!hasPurpose)
				return;

			if (PathComplete())
				FinalDestination();
			if (stopTime <= 0f)
				return;

			stopTime = Mathf.Clamp(stopTime - Time.deltaTime, 0, float.MaxValue);
			if (stopTime <= 0)
				RemoveModifier("StopMovement");
		}

		void FinalDestination()
		{
			OnFinalDestination();
		}

		public void SetDestination(Vector3 position)
		{
			navMeshAgent.destination = position;
			hasPurpose = true;
		}

		public void AddModifier(string modifierName, float modifier)
		{
			if (movementModifiers.ContainsKey(modifierName))
				movementModifiers[modifierName] = modifier;
			else
				movementModifiers.Add(modifierName, modifier);
			currentMovementModifier = movementModifiers.OrderBy(x => x.Value).FirstOrDefault().Value;

			SetSpeed(movementSpeed * currentMovementModifier);
		}

		public void RemoveModifier(string modifierName)
		{
			movementModifiers.Remove(modifierName);
			if (movementModifiers.Count > 0)
			{
				var value = movementModifiers.OrderBy(x => x.Value).FirstOrDefault().Value;
				currentMovementModifier = value;
			}
			else
				currentMovementModifier = 1f;

			SetSpeed(movementSpeed * currentMovementModifier);
		}

		public void SetSpeed(float speed)
		{
			navMeshAgent.speed = speed;
		}

		public void StopMovement(float time)
		{
			AddModifier("StopMovement", 0);
			stopTime = time;
		}

		public void Stop()
		{
			navMeshAgent.enabled = false;
		}

		public void Start()
		{
			navMeshAgent.enabled = true;
		}

		public bool PathComplete()
		{
			if (Vector3.Distance(navMeshAgent.destination, transform.position) > navMeshAgent.stoppingDistance)
				return false;
			return !navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude.EqualsWithTolerance(0f);
		}
	}
}
