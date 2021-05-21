using System.Collections.Generic;

namespace EPOOutline
{
	public static class RendererFilteringUtility
	{
		private static List<Outlinable> filteredOutlinables = new List<Outlinable>();

		public static void Filter(OutlineParameters parameters)
		{
			filteredOutlinables.Clear();

			int mask = parameters.Mask.value & parameters.Camera.cullingMask;

			foreach (Outlinable outlinable in parameters.OutlinablesToRender)
			{
				if ((parameters.OutlineLayerMask & (1L << outlinable.OutlineLayer)) == 0)
					continue;

				UnityEngine.GameObject go = outlinable.gameObject;

				if (!go.activeInHierarchy)
					continue;

				if (((1 << go.layer) & mask) == 0)
					continue;

#if UNITY_EDITOR
				UnityEditor.Experimental.SceneManagement.PrefabStage stage = UnityEditor.Experimental.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();

				if (stage != null && !stage.IsPartOfPrefabContents(outlinable.gameObject))
					continue;
#endif

				filteredOutlinables.Add(outlinable);
			}

			parameters.OutlinablesToRender.Clear();
			parameters.OutlinablesToRender.AddRange(filteredOutlinables);
		}
	}
}
