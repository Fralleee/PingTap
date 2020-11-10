using CombatSystem.Combat.Damage;
using UnityEngine;

namespace Fralle
{
	[RequireComponent(typeof(DamageController))]
	public class DamageControllerGraphics : MonoBehaviour
	{
		static readonly int RendererColor = Shader.PropertyToID("_EmissionColor");

		[SerializeField] GameObject model = null;
		[SerializeField] GameObject gibModel = null;

		new Renderer renderer;
		MaterialPropertyBlock propBlock;
		Color defaultColor;
		Color currentColor;
		float colorLerpTime;

		void Start()
		{
			var damageController = GetComponent<DamageController>();
			damageController.OnDamageTaken += HandleDamageTaken;
			damageController.OnDeath += HandleDeath;

			propBlock = new MaterialPropertyBlock();
			renderer = model.GetComponentInChildren<Renderer>();
			renderer.GetPropertyBlock(propBlock);
			defaultColor = propBlock.GetColor(RendererColor);
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

		void HandleDamageTaken(DamageController damageController, DamageData damageData)
		{
			if (damageData.hitAngle <= 0)
				return;

			currentColor = Color.white;
			propBlock.SetColor(RendererColor, currentColor);
			renderer.SetPropertyBlock(propBlock);
			colorLerpTime = 1f;
		}

		void HandleDeath(DamageController damageController, DamageData damageData)
		{
			if (damageData.gib)
			{
				GameObject gibbed = Instantiate(gibModel, transform.position, transform.rotation);
				foreach (var rigidBody in gibbed.GetComponentsInChildren<Rigidbody>())
				{
					rigidBody.AddForceAtPosition(damageData.force, damageData.position);
				}
				Destroy(gibbed, 3f);
				Destroy(gameObject);
			}
			else
			{
				foreach (var rigidBody in model.GetComponentsInChildren<Rigidbody>())
				{
					rigidBody.isKinematic = false;
				}
				Destroy(gameObject, 3f);
			}
		}

	}
}
