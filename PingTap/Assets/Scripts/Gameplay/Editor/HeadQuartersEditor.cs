using UnityEditor;
using UnityEngine;

namespace Fralle.Gameplay
{
	[CustomEditor(typeof(HeadQuarters)), CanEditMultipleObjects]
	public class HeadQuartersEditor : Editor
	{
		protected virtual void OnSceneGUI()
		{
			HeadQuarters playerHome = (HeadQuarters)target;

			Handles.DrawWireDisc(playerHome.Entry, Vector3.up, 3f);

			EditorGUI.BeginChangeCheck();
			Vector3 newTargetPosition = Handles.PositionHandle(playerHome.Entry, Quaternion.identity);
			if (EditorGUI.EndChangeCheck())
			{
				playerHome.entryCoordinates = newTargetPosition - playerHome.transform.position;
			}
		}
	}
}
