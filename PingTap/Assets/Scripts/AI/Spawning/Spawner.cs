using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Fralle;
using UnityEngine;

public class Spawner : MonoBehaviour
{
  [Readonly] public Army army;

  public int SpawnRound(int round)
  {
    WaveDefinition waveDefinition = army.NextWave(round);

    foreach (WaypointSchema waypointSchema in army.waypointSchemas)
    {
      for (var i = 0; i < waveDefinition.count; i++)
      {
        Enemy enemy = Instantiate(waveDefinition.enemy, waypointSchema.waypoints.FirstOrDefault(), Quaternion.identity, transform);
        enemy.agentNavigation.wayPointSchema = waypointSchema;
      }
    }

    return waveDefinition.count * army.waypointSchemas.Length;
  }
}
