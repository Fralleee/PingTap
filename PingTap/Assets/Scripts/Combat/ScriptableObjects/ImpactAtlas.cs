using System.Collections.Generic;
using UnityEngine;

namespace Fralle.PingTap
{
	[CreateAssetMenu(menuName = "Combat/Impact Atlas")]
	public class ImpactAtlas : ScriptableObject
	{
		[SerializeField] List<GameObject> impactEffects;
		[SerializeField] GameObject fallback;

		Dictionary<string, GameObject> dictionary = new Dictionary<string, GameObject>();

		public GameObject GetImpactEffectFromTag(string tag) => dictionary.ContainsKey(tag) ? dictionary[tag] : fallback;

		void MapToDictionary()
		{
			foreach (var effect in impactEffects)
			{
				if (dictionary.ContainsKey(effect.tag))
					Debug.LogWarning($"Dictionary alread contains effect for tag: {effect.tag}");
				else
					dictionary.Add(effect.tag, effect);
			}
		}

		void Awake()
		{
			MapToDictionary();
		}
	}
}
