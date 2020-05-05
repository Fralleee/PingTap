using Fralle.Core.Attributes;
using System.Linq;
using UnityEngine;

namespace Fralle.AI.Spawning
{
  public class Spawner : MonoBehaviour
  {
    [Readonly] public Army army;

    public int SpawnRound(int round)
    {
      if (army.waypointSchemas.Length == 0) return 0;

      var waveDefinition = army.NextWave(round);
      for (var i = 0; i < waveDefinition.count; i++)
      {
        var schema = army.waypointSchemas[i % army.waypointSchemas.Length];
        var enemy = Instantiate(waveDefinition.enemy, schema.waypoints.FirstOrDefault(), Quaternion.identity,
          transform);
        enemy.agentNavigation.wayPointSchema = schema;
      }

      return waveDefinition.count;
    }
  }
}