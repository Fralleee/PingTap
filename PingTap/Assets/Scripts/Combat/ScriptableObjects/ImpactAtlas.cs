using System.Collections.Generic;
using UnityEngine;

namespace Fralle.PingTap
{
  [CreateAssetMenu(menuName = "Combat/Impact Atlas")]
  public class ImpactAtlas : ScriptableObject
  {
    [SerializeField] List<GameObject> impactEffects;
    [SerializeField] GameObject fallback;

    [SerializeField] bool debug;

    Dictionary<string, GameObject> dictionary = new Dictionary<string, GameObject>();

    public GameObject GetImpactEffectFromTag(string tag)
    {
      if (debug)
        DebugLog(tag);

      return dictionary.TryGetValue(tag, out GameObject effect) ? effect : fallback;
    }

    void MapToDictionary()
    {
      foreach (GameObject effect in impactEffects)
      {
        if (dictionary.ContainsKey(effect.tag))
          Debug.LogWarning($"Dictionary alread contains effect for tag: {effect.tag}");
        else
          dictionary.Add(effect.tag, effect);
      }
    }

    void DebugLog(string tag)
    {
      foreach (KeyValuePair<string, GameObject> kvp in dictionary)
      {
        Debug.Log($"{kvp.Key}: {kvp.Value.name}");
      }

      Debug.Log($"Tag: {tag}. Dictionary.ContainsKey: {dictionary.ContainsKey(tag)}");
    }

    void OnEnable()
    {
      MapToDictionary();
    }
  }
}
