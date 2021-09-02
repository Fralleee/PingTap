using Fralle.Core;
using SensorToolkit;
using System.Collections.Generic;
using UnityEngine;

namespace Fralle.PingTap
{
  public class AISensoryMemory
  {
    public List<AIMemory> Memories = new List<AIMemory>();

    TeamController teamController;

    public AISensoryMemory(TeamController teamController)
    {
      this.teamController = teamController;
    }

    public void UpdateSenses(Sensor sensor)
    {
      foreach (var target in sensor.DetectedObjects)
      {
        RefreshMemory(sensor.gameObject, target);
      }
    }

    public void RefreshMemory(GameObject agent, GameObject target)
    {
      AIMemory memory = FetchMemory(target);
      memory.DamageController = target.GetComponentInParent<DamageController>();
      memory.GameObject = target;
      memory.Hostile = teamController.CheckIfHostile(target);
      memory.Position = target.transform.position;
      memory.Direction = target.transform.position - agent.transform.position;
      memory.Distance = memory.Direction.magnitude;
      memory.Angle = Vector3.Angle(agent.transform.forward, memory.Direction);
      memory.LastSeen = Time.time;
    }

    public AIMemory FetchMemory(GameObject gameObject)
    {
      AIMemory memory = Memories.Find(x => x.GameObject == gameObject);
      if (memory != null)
        return memory;

      memory = new AIMemory();
      Memories.Add(memory);

      return memory;
    }

    public void ForgetMemories(float olderThan)
    {
      Memories.RemoveAll(m => m.Age > olderThan);
      Memories.RemoveAll(m => !m.GameObject);
    }
  }
}
