#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace EPOOutline
{
	public class ModelPostprocessor : AssetPostprocessor, IPreprocessBuildWithReport
	{
		private static readonly int UVIndex = 6;

		private static readonly float MinVertexDistance = 0.02f;

		public int callbackOrder
		{
			get
			{
				return int.MaxValue;
			}
		}

		private class VertexGroup
		{
			public Vector3 Position;
			public List<int> Others = new List<int>();
			public Vector3 Normal;
		}

		[MenuItem("Tools/Easy performant outline/Check models")]
		private static void CheckModelMenu()
		{
			EditorPrefs.DeleteKey("Models checked");
			CheckModels();
		}

		[InitializeOnLoadMethod]
		private static void CheckModels()
		{
			if (EditorPrefs.HasKey("Models checked"))
				return;

			EditorPrefs.SetString("Models checked", "true");
			string[] models = AssetDatabase.FindAssets("t:Model");

			try
			{
				int index = 0;
				foreach (string modelGUID in models)
				{
					GameObject model = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(modelGUID));

					string title = "Checking models for easy performant outlines";
					string info = "Some model postprocessing will be applied. Checked {0}/{1}";

					EditorUtility.DisplayProgressBar(title, string.Format(info, index, models.Length), index / (float)models.Length);
					index++;

					Mesh mesh = GetMesh(model);
					if (mesh == null)
						continue;

					PostprocessModel(model, mesh);
				}
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
			}

			EditorUtility.ClearProgressBar();
		}

		private static Mesh GetMesh(GameObject model)
		{
			Mesh mesh = null;
			Renderer renderer = model.GetComponent<Renderer>();
			if (renderer is MeshRenderer)
				mesh = renderer.GetComponent<MeshFilter>().sharedMesh;
			else if (renderer is SkinnedMeshRenderer)
				mesh = (renderer as SkinnedMeshRenderer).sharedMesh;

			return mesh;
		}

		public void OnPostprocessModel(GameObject model)
		{
			Mesh mesh = GetMesh(model);
			if (mesh == null)
				return;

			try
			{
				PostprocessModel(model, mesh);
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
			}
		}

		private static void PostprocessModel(GameObject model, Mesh mesh)
		{
			Mesh meshCopy = new Mesh();
			meshCopy.vertices = mesh.vertices;
			meshCopy.triangles = mesh.triangles;

			meshCopy.RecalculateNormals();

			Vector3[] vertices = meshCopy.vertices;
			Vector3[] normals = meshCopy.normals;

			List<Vector3> uvs = new List<Vector3>(new Vector3[vertices.Length]);

			for (int submesh = 0; submesh < mesh.subMeshCount; submesh++)
			{
				HashSet<int> verticesOfTheSubmesh = new HashSet<int>();
				int[] triangles = mesh.GetTriangles(submesh);

				foreach (int index in triangles)
					verticesOfTheSubmesh.Add(index);

				List<VertexGroup> similarVertices = new List<VertexGroup>();
				foreach (int vertex in verticesOfTheSubmesh)
				{
					Vector3 vertexPosition = vertices[vertex];
					VertexGroup similar = similarVertices.Find(x => Vector3.Distance(x.Position, vertexPosition) < MinVertexDistance);
					if (similar == null)
					{
						similar = new VertexGroup() { Position = vertexPosition };
						similarVertices.Add(similar);
					}

					similar.Normal += normals[vertex];
					similar.Others.Add(vertex);
				}

				foreach (VertexGroup group in similarVertices)
				{
					Vector3 normal = (group.Normal / group.Others.Count).normalized;
					foreach (int other in group.Others)
						uvs[other] = normal;
				}
			}

			mesh.SetUVs(UVIndex, uvs);

			mesh.UploadMeshData(false);
		}

		public void OnPreprocessBuild(BuildReport report)
		{
			EditorPrefs.DeleteKey("Models checked");
			CheckModels();
		}
	}
}
#endif
