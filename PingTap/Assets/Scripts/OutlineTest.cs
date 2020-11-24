using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OutlineTest : MonoBehaviour
{
	[Serializable]
	private class ListVector3
	{
		public List<Vector3> data;
	}

	[SerializeField] Color outlineColor = Color.white;
	[SerializeField, Range(0f, 10f)] float outlineWidth = 2f;

	Material outlineFillMaterial;

	void Awake()
	{
		SetupMaterials();
	}

	void Start()
	{
		ShowOutline();
	}

	void SetupMaterials()
	{
		Material outlineMaskMaterial = Instantiate(Resources.Load<Material>(@"Materials/OutlineMask"));
		outlineFillMaterial = Instantiate(Resources.Load<Material>(@"Materials/OutlineFill"));

		Renderer[] renderers = GetComponentsInChildren<Renderer>();
		foreach (var renderer in renderers)
		{
			var materials = renderer.sharedMaterials.ToList();
			materials.Add(outlineMaskMaterial);
			materials.Add(outlineFillMaterial);
			renderer.materials = materials.ToArray();
		}
	}

	public void ShowOutline()
	{
		outlineFillMaterial.SetColor("_OutlineColor", outlineColor);
		outlineFillMaterial.SetFloat("_OutlineWidth", outlineWidth);
	}

	public void HideOutline()
	{
		outlineFillMaterial.SetFloat("_OutlineWidth", outlineWidth);
	}
}
