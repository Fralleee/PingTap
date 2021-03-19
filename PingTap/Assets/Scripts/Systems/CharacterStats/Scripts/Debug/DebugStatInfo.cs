using UnityEngine;
using UnityEngine.UI;

namespace CharacterStats
{
	public class DebugStatInfo : MonoBehaviour
	{
		public StatAttribute attribute;
		public Text valueText;

		CharacterStat characterStat;

		void Start()
		{
			var statsController = GetComponentInParent<StatsController>();
			characterStat = statsController.GetStat(attribute);
			if (characterStat != null)
			{
				characterStat.OnChanged += StatChanged;
				StatChanged(characterStat);
			}
		}

		private void StatChanged(CharacterStat obj)
		{
			valueText.text = obj.Value.ToString();
		}

		private void OnDestroy()
		{
			if (characterStat != null)
				characterStat.OnChanged -= StatChanged;
		}
	}
}
