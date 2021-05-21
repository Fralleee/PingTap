using System.Collections.Generic;
using UnityEngine;
#if UNITY_5_3_OR_NEWER
using UnityEngine.AI;
#endif
#if UNITY_EDITOR
using UnityEditor;
#endif

public class NavMeshCleaner : MonoBehaviour
{
	public List<Vector3> MWalkablePoint = new List<Vector3>();
	public float Height = 1.0f;
	public float Offset = 0.0f;
	public int MidLayerCount = 3;

#if UNITY_EDITOR
	private void Awake()
	{
		SetMeshVisible(false);
	}

	List<GameObject> child = new List<GameObject>();

	void Reset()
	{
		Undo.RecordObject(this, "Reset");

		foreach (GameObject t in child)
		{
			Undo.DestroyObjectImmediate(t);
		}
		child.Clear();
	}

	void SetMeshVisible(bool visible)
	{
		foreach (GameObject t in child)
			t.SetActive(visible);
	}

	public bool HasMesh()
	{
		return child.Count != 0 ? true : false;
	}

	public bool MeshVisible()
	{
		return child.Count > 0 && child[0].activeSelf;
	}

	void Build()
	{
		Mesh[] m = CreateMesh();

		Undo.RegisterCreatedObjectUndo(this, "build");

		for (int i = 0; i < m.Length || i == 0; i++)
		{
			GameObject o;
			if (i >= child.Count)
			{
				o = new GameObject();

				//o.hideFlags = HideFlags.DontSave;
				o.name = gameObject.name + "_Mesh(DontSave)";
				o.AddComponent<MeshFilter>();

				MeshRenderer meshrenderer = o.AddComponent<MeshRenderer>();
				meshrenderer.sharedMaterial = AssetDatabase.GetBuiltinExtraResource<Material>("Default-Diffuse.mat");

				o.transform.parent = transform;
				o.transform.localScale = Vector3.one;
				o.transform.localPosition = Vector3.zero;
				o.transform.localRotation = Quaternion.identity;
				GameObjectUtility.SetStaticEditorFlags(o, GameObjectUtility.GetStaticEditorFlags(gameObject) | StaticEditorFlags.NavigationStatic);
				GameObjectUtility.SetNavMeshArea(o, 1);
				child.Add(o);
				Undo.RegisterCreatedObjectUndo(o, "");
			}
			else
			{
				o = child[i].gameObject;
			}

			o.hideFlags = i == 0 ? (HideFlags.DontSave | HideFlags.HideInHierarchy) : child[0].gameObject.hideFlags;

			MeshFilter meshfilter = child[i].GetComponent<MeshFilter>();
			Undo.RecordObject(meshfilter, "MeshUpdate");
			meshfilter.sharedMesh = m.Length == 0 ? null : m[i];
		}

		while (child.Count > m.Length)
		{
			Undo.DestroyObjectImmediate(child[child.Count - 1]);
			child.RemoveAt(child.Count - 1);
		}
	}

	static int Find(Vector3[] vtx, int left, int right, Vector3 v, float key)
	{
		int center = (left + right) / 2;

		if (center != left)
			return key <= vtx[center].x ? Find(vtx, left, center, v, key) : Find(vtx, center, right, v, key);
		for (int i = left; i < vtx.Length && vtx[i].x <= key + 0.002f; i++)
		{
			if (Vector3.Magnitude(vtx[i] - v) <= 0.01f)
				return i;
		}
		return -1;

	}

	class Tri
	{
		public Tri(int i1, int i2, int i3) { this.I1 = i1; this.I2 = i2; this.I3 = i3; Min = Mathf.Min(i1, i2, i3); Max = Mathf.Max(i1, i2, i3); }
		public int I1;
		public int I2 { get; }
		public int I3;
		public int Min, Max;
	};

	class Edge
	{
		public Edge(int i1, int i2) { this.I1 = i1; this.I2 = i2; }
		public int I1, I2;
	};

	static bool Find(Edge[] edge, int left, int right, int i1, int i2)
	{
		int center = (left + right) / 2;

		if (center != left)
			return i1 <= edge[center].I1 ? Find(edge, left, center, i1, i2) : Find(edge, center, right, i1, i2);
		for (int i = left; i < edge.Length && edge[i].I1 <= i1; i++)
		{
			if (edge[i].I1 == i1 && edge[i].I2 == i2)
				return true;
		}
		return false;

	}

	Mesh[] CreateMesh()
	{
		NavMeshTriangulation triangulatedNavMesh = NavMesh.CalculateTriangulation();

		Vector3[] navVertices = triangulatedNavMesh.vertices;
		List<Vector3> vertices = new List<Vector3>();
		vertices.AddRange(navVertices);
		vertices.Sort(delegate (Vector3 v1, Vector3 v2)
		{ return v1.x == v2.x ? (v1.z == v2.z ? 0 : (v1.z < v2.z ? -1 : 1)) : (v1.x < v2.x ? -1 : 1); });

		Vector3[] v = vertices.ToArray();

		int[] table = new int[triangulatedNavMesh.vertices.Length];

		for (int i = 0; i < table.Length; i++)
		{
			table[i] = Find(v, 0, vertices.Count, navVertices[i], navVertices[i].x - 0.001f);
			if ((i % 100) == 0)
				EditorUtility.DisplayProgressBar($"Export Nav-Mesh (Phase #1/3) {i}/{table.Length}", "Weld Vertex", Mathf.InverseLerp(0, table.Length, i));
		}

		int[] navTriangles = triangulatedNavMesh.indices;

		List<Tri> tri = new List<Tri>();
		for (int i = 0; i < navTriangles.Length; i += 3)
			tri.Add(new Tri(table[navTriangles[i + 0]], table[navTriangles[i + 1]], table[navTriangles[i + 2]]));
		tri.Sort((t1, t2) => t1.Min == t2.Min ? 0 : t1.Min < t2.Min ? -1 : 1);

		int[] boundmin = new int[(tri.Count + 127) / 128];
		int[] boundmax = new int[boundmin.Length];

		for (int i = 0, c = 0; i < tri.Count; i += 128, c++)
		{
			int min = tri[i].Min;
			int max = tri[i].Max;
			for (int j = 1; j < 128 && i + j < tri.Count; j++)
			{
				min = Mathf.Min(tri[i + j].Min, min);
				max = Mathf.Max(tri[i + j].Max, max);
			}
			boundmin[c] = min;
			boundmax[c] = max;
		}

		int[] triangles = new int[navTriangles.Length];
		for (int i = 0; i < triangles.Length; i += 3)
		{
			triangles[i + 0] = tri[i / 3].I1;
			triangles[i + 1] = tri[i / 3].I2;
			triangles[i + 2] = tri[i / 3].I3;
		}

		List<int> groupidx = new List<int>();
		List<int> groupcount = new List<int>();

		int[] group = new int[triangles.Length / 3];

		for (int i = 0; i < triangles.Length; i += 3)
		{
			int groupid = -1;
			int max = Mathf.Max(triangles[i], triangles[i + 1], triangles[i + 2]);
			int min = Mathf.Min(triangles[i], triangles[i + 1], triangles[i + 2]);

			for (int b = 0, c = 0; b < i; b += 3 * 128, c++)
			{
				if (boundmin[c] > max || boundmax[c] < min)
					continue;

				for (int j = b; j < i && j < b + 3 * 128; j += 3)
				{
					if (tri[j / 3].Min > max)
						break;

					if (tri[j / 3].Max < min)
						continue;

					if (groupidx[group[j / 3]] == groupid)
						continue;

					for (int k = 0; k < 3; k++)
					{
						int vi = triangles[j + k];
						if (triangles[i] != vi && triangles[i + 1] != vi && triangles[i + 2] != vi)
							continue;
						if (groupid == -1)
						{
							groupid = groupidx[@group[j / 3]];
							@group[i / 3] = groupid;
						}
						else
						{
							int curgroup = groupidx[@group[j / 3]];
							for (int l = 0; l < groupidx.Count; l++)
							{
								if (groupidx[l] == curgroup)
									groupidx[l] = groupid;
							}
						}
						break;
					}
				}
			}

			if (groupid == -1)
			{
				groupid = groupidx.Count;
				group[i / 3] = groupid;
				groupidx.Add(groupid);
				groupcount.Add(0);
			}

			if (((i / 3) % 100) == 0)
				EditorUtility.DisplayProgressBar("Collect (Phase #2/3)", "Classification Group", Mathf.InverseLerp(0, triangles.Length, i));
		}

		for (int i = 0; i < triangles.Length; i += 3)
		{
			group[i / 3] = groupidx[group[i / 3]];
			groupcount[group[i / 3]]++;
		}

		List<Mesh> result = new List<Mesh>();

		List<Vector3> vtx = new List<Vector3>();
		List<int> indices = new List<int>();

		int[] newtable = new int[vertices.Count];
		for (int i = 0; i < newtable.Length; i++)
			newtable[i] = -1;

		Vector3[] walkpoint = MWalkablePoint.ToArray();

		for (int g = 0; g < groupcount.Count; g++)
		{
			if (groupcount[g] == 0)
				continue;

			List<Vector3> isolatevtx = new List<Vector3>();
			List<int> iolateidx = new List<int>();

			for (int i = 0; i < triangles.Length; i += 3)
			{
				if (group[i / 3] == g)
				{
					for (int j = 0; j < 3; j++)
					{
						int idx = triangles[i + j];
						if (newtable[idx] != -1)
							continue;
						newtable[idx] = isolatevtx.Count;
						isolatevtx.Add(transform.InverseTransformPoint(vertices[idx] + Vector3.up * Offset));
					}
					iolateidx.Add(newtable[triangles[i + 0]]);
					iolateidx.Add(newtable[triangles[i + 1]]);
					iolateidx.Add(newtable[triangles[i + 2]]);
				}
			}

			if (Contains(isolatevtx.ToArray(), iolateidx.ToArray(), walkpoint) == true)
				continue;

			int maxvertex = 32768;

			if (vtx.Count > maxvertex || vtx.Count + isolatevtx.Count * (2 + MidLayerCount) >= 65536)
			{
				result.Add(CreateMesh(vtx.ToArray(), indices.ToArray()));
				vtx.Clear();
				indices.Clear();
			}

			Vector3 h = transform.InverseTransformVector(Vector3.up * Height);
			int vtxoffset = vtx.Count;
			int layer = 2 + MidLayerCount;
			foreach (Vector3 t in isolatevtx)
			{
				for (int j = 0; j < layer; j++)
					vtx.Add(t + h * ((float)j / (layer - 1)));
			}
			for (int i = 0; i < iolateidx.Count; i += 3)
			{
				for (int j = 0; j < layer; j++)
				{
					if (j == 0)
						indices.AddRange(new int[] { vtxoffset + iolateidx[i] * layer + j, vtxoffset + iolateidx[i + 2] * layer + j, vtxoffset + iolateidx[i + 1] * layer + j });
					else
						indices.AddRange(new int[] { vtxoffset + iolateidx[i] * layer + j, vtxoffset + iolateidx[i + 1] * layer + j, vtxoffset + iolateidx[i + 2] * layer + j });
				}
			}

			if (Height > 0)
			{
				List<Edge> edge = new List<Edge>();
				for (int i = 0; i < iolateidx.Count; i += 3)
				{
					edge.Add(new Edge(iolateidx[i + 0], iolateidx[i + 1]));
					edge.Add(new Edge(iolateidx[i + 1], iolateidx[i + 2]));
					edge.Add(new Edge(iolateidx[i + 2], iolateidx[i + 0]));
				}
				edge.Sort((e1, e2) => e1.I1 == e2.I1 ? 0 : (e1.I1 < e2.I1 ? -1 : 1));
				Edge[] e = edge.ToArray();

				for (int i = 0; i < iolateidx.Count; i += 3)
				{
					for (int i1 = 2, i2 = 0; i2 < 3; i1 = i2++)
					{
						int v1 = iolateidx[i + i1];
						int v2 = iolateidx[i + i2];

						if (!Find(e, 0, edge.Count, v2, v1))
						{
							if (vtx.Count + 4 >= 65536)
							{
								result.Add(CreateMesh(vtx.ToArray(), indices.ToArray()));
								vtx.Clear();
								indices.Clear();
							}

							indices.AddRange(new int[] { vtx.Count, vtx.Count + 1, vtx.Count + 3, vtx.Count, vtx.Count + 3, vtx.Count + 2 });
							vtx.AddRange(new Vector3[] { isolatevtx[v1], isolatevtx[v2], isolatevtx[v1] + h, isolatevtx[v2] + h });
						}
					}

					if ((i % 600) == 0)
						EditorUtility.DisplayProgressBar("Collect (Phase #3/3)", "Create Mesh", Mathf.InverseLerp(0, groupcount.Count * 100, g * 100 + i * 100 / (i - iolateidx.Count)));
				}
			}

			EditorUtility.DisplayProgressBar("Collect (Phase #3/3)", "Create Mesh", Mathf.InverseLerp(0, groupcount.Count, g));
		}

		if (vtx.Count > 0)
		{
			result.Add(CreateMesh(vtx.ToArray(), indices.ToArray()));
		}

		EditorUtility.ClearProgressBar();
		return result.ToArray();
	}

	static Mesh CreateMesh(Vector3[] vtx, int[] indices)
	{
		Mesh m = new Mesh { hideFlags = HideFlags.DontSave, vertices = vtx };
		m.SetIndices(indices, MeshTopology.Triangles, 0);
		m.RecalculateNormals();
		m.RecalculateBounds();
		return m;
	}

	static bool Contains(Vector3[] vtx, int[] indices, Vector3[] points)
	{
		foreach (Vector3 p in points)
		{
			for (int i = 0; i < indices.Length; i += 3)
			{
				if (indices[i] == indices[i + 1] || indices[i] == indices[i + 2] || indices[i + 1] == indices[i + 2])
					continue;

				if (PointInTriangle(vtx[indices[i]], vtx[indices[i + 2]], vtx[indices[i + 1]], p))
					return true;
			}
		}

		return false;
	}

	static bool PointInTriangle(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 p)
	{
		Vector3 up = Vector3.Cross(v3 - v1, v2 - v1);

		return Vector3.Dot(Vector3.Cross(p - v1, v2 - v1), up) > 0 &&
					 Vector3.Dot(Vector3.Cross(p - v2, v3 - v2), up) > 0 &&
					 Vector3.Dot(Vector3.Cross(p - v3, v1 - v3), up) > 0;
	}

	[UnityEditor.CustomEditor(typeof(NavMeshCleaner))]
	public class NavMeshCleanerEditor : Editor
	{
		NavMeshCleaner mTarget;

		void OnEnable()
		{
			mTarget = (NavMeshCleaner)target;

			Undo.undoRedoPerformed += OnUndoOrRedo;
		}

		void OnDisable()
		{
			Undo.undoRedoPerformed -= OnUndoOrRedo;
		}

		void OnUndoOrRedo()
		{
			Repaint();
		}

		public override void OnInspectorGUI()
		{
			EditorGUILayout.HelpBox(overPoint != -1 ? "Press Control and click to remove the point." : "Press Control and click to add a walkable point.", mTarget.MWalkablePoint.Count == 0 ? MessageType.Warning : MessageType.Info);

			base.OnInspectorGUI();

			NavMeshCleaner t = (NavMeshCleaner)target;

			if (t.child.Count > 0)
			{
				EditorGUI.BeginChangeCheck();
				bool hideInHierarchy = EditorGUILayout.Toggle("Hide Temp Mesh Object In Hierarchy", (t.child[0].gameObject.hideFlags & HideFlags.HideInHierarchy) != 0 ? true : false);
				if (EditorGUI.EndChangeCheck())
				{
					foreach (GameObject t1 in t.child)
						t1.gameObject.hideFlags = hideInHierarchy ? (t1.gameObject.hideFlags | HideFlags.HideInHierarchy) : (t1.gameObject.hideFlags & (~HideFlags.HideInHierarchy));

					try
					{
						EditorApplication.RepaintHierarchyWindow();
						EditorApplication.DirtyHierarchyWindowSorting();
					}
					catch
					{
						// ignored
					}
				}
			}

			if (GUILayout.Button(t.HasMesh() ? "Recalculate" : "Calculate", GUILayout.Height(30.0f)))
			{
				t.Build();
				t.SetMeshVisible(true);
				SceneView.RepaintAll();
			}

			if (t.HasMesh() && GUILayout.Button(t.MeshVisible() ? "Hide Mesh" : "Show Mesh", GUILayout.Height(30.0f)))
			{
				//StaticEditorFlags flags = GameObjectUtility.GetStaticEditorFlags(t.gameObject);
				bool enabled = !t.MeshVisible();
				t.SetMeshVisible(enabled);
				SceneView.RepaintAll();
			}
			if (t.HasMesh() && GUILayout.Button("Reset Mesh", GUILayout.Height(30.0f)))
			{
				t.Reset();
				SceneView.RepaintAll();
			}

			if (!t.HasMesh() || !GUILayout.Button("Reset WalkablePoints", GUILayout.Height(30.0f)))
				return;
			Undo.RecordObject(target, "reset");
			mTarget.MWalkablePoint.Clear();
			SceneView.RepaintAll();

		}

		static class Styles
		{
			public static GUIStyle Get(string id)
			{
				if (texture.TryGetValue(id, out GUIStyle style))
					return style;
				style = new GUIStyle(id);
				texture.Add(id, style);
				return style;
			}
			static Dictionary<string, GUIStyle> texture = new Dictionary<string, GUIStyle>();
		}

		static void DrawDisc(Vector3 p, Vector3 n, float radius)
		{
			Vector3[] v = new Vector3[20];
			Matrix4x4 tm = Matrix4x4.TRS(p, Quaternion.LookRotation(n), Vector3.one * radius);
			for (int i = 0; i < 20; i++)
			{
				v[i] = tm.MultiplyPoint3x4(new Vector3(Mathf.Cos(Mathf.PI * 2 * i / (20 - 1)), Mathf.Sin(Mathf.PI * 2 * i / (20 - 1)), 0));
			}
			Handles.DrawAAPolyLine(v);
		}

		void OnSceneGUI()
		{
			SceneView sceneview = SceneView.currentDrawingSceneView;

			Event guiEvent = Event.current;

			switch (guiEvent.type)
			{
				case EventType.Repaint:
				{
					// draw
					for (int i = 0; i < mTarget.MWalkablePoint.Count; i++)
					{
						Vector3 p = mTarget.transform.TransformPoint(mTarget.MWalkablePoint[i]);
						float unitsize = WorldSize(1.0f, sceneview.camera, p);

						Handles.color = Color.black;
						DrawDisc(p, Vector3.up, unitsize * 15);

						Handles.color = i == overPoint ? Color.red : Color.green;
						Handles.DrawSolidDisc(p, Vector3.up, unitsize * 10);
						Handles.DrawLine(p, p + Vector3.up * (unitsize * 200.0f));
					}

					break;
				}
				case EventType.Layout when guiEvent.control == true:
					HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
					break;
			}

			if (guiEvent.control == true)
			{
				EditorGUIUtility.AddCursorRect(new Rect(0, 0, Screen.width, Screen.height), overPoint == -1 ? MouseCursor.ArrowPlus : MouseCursor.ArrowMinus);
			}

			if ((guiEvent.type == EventType.MouseDown || guiEvent.type == EventType.MouseDrag || guiEvent.type == EventType.MouseMove || guiEvent.type == EventType.MouseUp) && guiEvent.button == 0)
			{
				MouseEvent(guiEvent.type, guiEvent.mousePosition, guiEvent.modifiers == EventModifiers.Control ? true : false);
			}
		}

		int overPoint = -1;

		void MouseEvent(EventType type, Vector2 mouseposition, bool controldown)
		{
			SceneView sceneview = SceneView.currentDrawingSceneView;

			Ray mouseRay = HandleUtility.GUIPointToWorldRay(mouseposition);

			if (type == EventType.MouseMove)
			{
				int pointindex = -1;

				for (int i = 0; i < mTarget.MWalkablePoint.Count; i++)
				{
					Vector3 p = mTarget.transform.TransformPoint(mTarget.MWalkablePoint[i]);
					float size = WorldSize(10.0f, sceneview.camera, p) * 1.5f;
					if (!(DistanceRayVsPoint(mouseRay, p) < size))
						continue;
					pointindex = i;
					break;
				}

				if (pointindex != overPoint)
				{
					overPoint = pointindex;
					HandleUtility.Repaint();
				}
			}

			if (type != EventType.MouseDown || controldown != true)
				return;
			{
				if (overPoint != -1)
				{
					Undo.RecordObject(mTarget, "Remove Point");
					mTarget.MWalkablePoint.RemoveAt(overPoint);
					overPoint = -1;
				}
				else
				{
					float mint = 1000.0f;

					if (Physics.Raycast(mouseRay, out RaycastHit hit, mint))
					{
						Undo.RecordObject(mTarget, "Add Point");
						mTarget.MWalkablePoint.Add(mTarget.transform.InverseTransformPoint(hit.point));
					}
					else
					{
						NavMeshTriangulation triangulatedNavMesh = NavMesh.CalculateTriangulation();

						Vector3[] navVertices = triangulatedNavMesh.vertices;
						int[] indices = triangulatedNavMesh.indices;

						Vector3 outNormal = Vector3.up;
						for (int i = 0; i < indices.Length; i += 3)
							mint = IntersectTest(mouseRay, navVertices[indices[i]], navVertices[indices[i + 1]], navVertices[indices[i + 2]], mint, ref outNormal);

						if (mint < 1000.0f)
						{
							Undo.RecordObject(mTarget, "Add Point");
							Vector3 point = mouseRay.origin + mouseRay.direction * mint;
							mTarget.MWalkablePoint.Add(mTarget.transform.InverseTransformPoint(point));
						}
					}
				}
				HandleUtility.Repaint();
			}
		}

		static float kEpsilon = 0.000001f;
		// https://en.wikipedia.org/wiki/Möller–Trumbore_intersection_algorithm
		static float IntersectTest(Ray ray, Vector3 v0, Vector3 v1, Vector3 v2, float mint, ref Vector3 outNormal)
		{
			// edges from v1 & v2 to v0.     
			Vector3 e1 = v1 - v0;
			Vector3 e2 = v2 - v0;

			Vector3 h = Vector3.Cross(ray.direction, e2);
			float a = Vector3.Dot(e1, h);
			if ((a > -kEpsilon) && (a < kEpsilon))
				return mint;

			float f = 1.0f / a;
			Vector3 s = ray.origin - v0;
			float u = f * Vector3.Dot(s, h);
			if ((u < 0.0f) || (u > 1.0f))
				return mint;

			Vector3 q = Vector3.Cross(s, e1);
			float v = f * Vector3.Dot(ray.direction, q);
			if ((v < 0.0f) || (u + v > 1.0f))
				return mint;

			float t = f * Vector3.Dot(e2, q);
			if (t > kEpsilon && t < mint)
			{
				outNormal = Vector3.Normalize(Vector3.Cross(e1.normalized, e2.normalized));
				return t;
			}
			return mint;
		}

		static float WorldSize(float screensize, Camera camera, Vector3 p)
		{
			if (!camera.orthographic)
			{
				Vector3 localPos = camera.transform.InverseTransformPoint(p);
				float height = Mathf.Tan(camera.fieldOfView * Mathf.Deg2Rad * 0.5f) * localPos.z;
				return height * screensize / camera.pixelHeight;
			}
			else
			{
				return camera.orthographicSize * screensize / camera.pixelHeight;
			}
		}

		static float DistanceRayVsPoint(Ray mouseRay, Vector3 pos)
		{
			Vector3 v = pos - mouseRay.origin;
			return Mathf.Sqrt(Vector3.Dot(v, v) - Vector3.Dot(mouseRay.direction, v) * Vector3.Dot(mouseRay.direction, v));
		}

		static Vector3 IntersectPlane(Vector3 inNormal, Vector3 inPoint, Ray mouseRay)
		{
			Plane p = new Plane(inNormal, inPoint);
			float dstToDrawPlane = p.GetDistanceToPoint(mouseRay.origin);
			return mouseRay.origin + mouseRay.direction * (dstToDrawPlane / Vector3.Dot(-p.normal, mouseRay.direction));
		}
	}
#endif
}
