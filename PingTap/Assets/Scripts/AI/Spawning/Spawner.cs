using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Fralle;
using UnityEngine;

public class Spawner : MonoBehaviour
{
  [Readonly] public Army army;

  public int SpawnRound(int round)
  {
    if (army.waypointSchemas.Length == 0) return 0;

    WaveDefinition waveDefinition = army.NextWave(round);
    for (var i = 0; i < waveDefinition.count; i++)
    {
      WaypointSchema schema = army.waypointSchemas[i % army.waypointSchemas.Length];
      Enemy enemy = Instantiate(waveDefinition.enemy, schema.waypoints.FirstOrDefault(), Quaternion.identity, transform);
      enemy.agentNavigation.wayPointSchema = schema;
    }

    return waveDefinition.count;
  }
}
