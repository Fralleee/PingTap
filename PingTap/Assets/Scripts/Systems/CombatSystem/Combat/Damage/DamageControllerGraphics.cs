using CombatSystem.Combat.Damage;
using UnityEngine;

namespace Fralle
{
	[RequireComponent(typeof(DamageController))]
	public class DamageControllerGraphics : MonoBehaviour
	{
		static readonly int RendererColor = Shader.PropertyToID("_BaseColor");

		[SerializeField] GameObject gibModel;

		Rigidbody[] rigidbodies;
		new Renderer renderer;
		MaterialPropertyBlock propBlock;
		Color defaultColor;
		Color currentColor;
		float colorLerpTime;

		void Awake()
		{
			SetupRagdoll();

			var damageController = GetComponent<DamageController>();
			damageController.OnDamageTaken += HandleDamageTaken;
			damageController.OnDeath += HandleDeath;

			propBlock = new MaterialPropertyBlock();
			renderer = GetComponentInChildren<Renderer>();
			renderer.GetPropertyBlock(propBlock);
			defaultColor = renderer.material.GetColor(RendererColor);
		}

		void Update()
		{
			if (colorLerpTime <= 0)
				return;

			currentColor = Color.Lerp(currentColor, defaultColor, 1 - colorLerpTime);
			propBlock.SetColor(RendererColor, currentColor);
			renderer.SetPropertyBlock(propBlock);
			colorLerpTime -= Time.deltaTime * 0.25f;
		}

		void SetupRagdoll()
		{
			rigidbodies = GetComponentsInChildren<Rigidbody>();
			ToggleRagdoll(false);
		}

		void ToggleRagdoll(bool enable)
		{
			foreach (var collider in GetComponentsInChildren<Collider>())
			{
				collider.enabled = !enable;
			}

			foreach (var rigidBody in rigidbodies)
			{
				rigidBody.isKinematic = !enable;
				rigidBody.GetComponent<Collider>().enabled = enable;
				rigidBody.velocity = Vector3.zero;
			}
		}

		void HandleDamageTaken(DamageController damageController, DamageData damageData)
		{
			if (!damageData.damageFromHit)
				return;

			currentColor = Color.white;
			propBlock.SetColor(RendererColor, currentColor);
			renderer.SetPropertyBlock(propBlock);
			colorLerpTime = 1f;
		}

		void HandleDeath(DamageController damageController, DamageData damageData)
		{
			Destroy(gameObject, 3f);

			if (damageData.gib)
			{
				Debug.Log("Spawn gib stuff");
			}
			else
			{
				ToggleRagdoll(true);
				foreach (var rigidBody in rigidbodies)
				{
					rigidBody.AddForceAtPosition(damageData.force, damageData.position);
				}
			}
		}

	}
}
