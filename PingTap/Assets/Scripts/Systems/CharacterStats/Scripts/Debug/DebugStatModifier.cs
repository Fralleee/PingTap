using UnityEngine;
using UnityEngine.UI;

namespace CharacterStats
{
	public class DebugStatModifier : MonoBehaviour
	{
		public StatAttribute attribute;

		public Button decreasePercentage;
		public Button decreaseFlat;
		public Button increaseFlat;
		public Button increasePercentage;

		StatsController statsController;

		void Awake()
		{
			statsController = GetComponentInParent<StatsController>();

			decreasePercentage.onClick.AddListener(DecreasePercentage);
			decreaseFlat.onClick.AddListener(DecreaseFlat);
			increaseFlat.onClick.AddListener(IncreaseFlat);
			increasePercentage.onClick.AddListener(IncreasePercentage);
		}

		void DecreasePercentage()
		{
			statsController.ModifyStat(attribute, -0.05f, StatModType.PercentAdd);
		}

		void DecreaseFlat()
		{
			statsController.ModifyStat(attribute, -1, StatModType.Flat);
		}

		void IncreaseFlat()
		{
			statsController.ModifyStat(attribute, 1, StatModType.Flat);
		}

		void IncreasePercentage()
		{
			statsController.ModifyStat(attribute, 0.05f, StatModType.PercentAdd);
		}


		void OnDestroy()
		{
			decreasePercentage.onClick.RemoveAllListeners();
			decreaseFlat.onClick.RemoveAllListeners();
			increaseFlat.onClick.RemoveAllListeners();
			increasePercentage.onClick.RemoveAllListeners();
		}

	}
}
