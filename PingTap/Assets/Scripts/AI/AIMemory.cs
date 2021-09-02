using UnityEngine;

namespace Fralle.PingTap
{
  public class AIMemory
  {
    public float Age => Time.time - LastSeen;
    public DamageController DamageController;
    public GameObject GameObject;
    public Vector3 Position;
    public Vector3 Direction;
    public bool Hostile;
    public float Distance;
    public float Angle;
    public float LastSeen;
    public float Score;
  }
}
