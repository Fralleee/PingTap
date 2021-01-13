using Fralle.Core.Enums;
using UnityEngine;

namespace Fralle.Core.Animation
{
	public class AnimatedUVs : MonoBehaviour
	{
		[SerializeField] float scrollSpeed = 0.25F;
		[SerializeField] Axis direction = Axis.X;
		Renderer render;
		static readonly int MainTex = Shader.PropertyToID("_MainTex");

		void Start()
		{
			render = GetComponent<Renderer>();
		}

		void LateUpdate()
		{
			var offset = Time.time * scrollSpeed;
			render.material.SetTextureOffset(MainTex, new Vector2(direction == Axis.X ? offset : 0, direction == Axis.Y ? offset : 0));
		}

	}
}
