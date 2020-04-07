using UnityEngine;

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
    int roundIndex = roundNo - 1;
    int diff = (int)Mathf.Floor(roundIndex / waveDefinitions.Length) * waveDefinitions.Length;
    int waveDefinitionNo = roundIndex - diff;
    return waveDefinitions[waveDefinitionNo];
  }
}