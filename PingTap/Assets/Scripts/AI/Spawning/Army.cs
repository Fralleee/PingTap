using Fralle.Movement;
using UnityEngine;

namespace Fralle.AI.Spawning
{
  [CreateAssetMenu(menuName = "AI/Army")]
  public class Army : ScriptableObject
  {
    public int repeatCount = 0;
    public float powerModifier = 1.33f;
    public WaypointSchema[] waypointSchemas;
    public WaveDefinition[] waveDefinitions;

    public int MaxRounds => waveDefinitions.Length * (repeatCount + 1);

    public WaveDefinition NextWave(int roundNo)
    {
      var roundIndex = roundNo - 1;
      var diff = (int)Mathf.Floor(roundIndex / waveDefinitions.Length) * waveDefinitions.Length;
      var waveDefinitionNo = roundIndex - diff;
      return waveDefinitions[waveDefinitionNo];
    }
  }
}