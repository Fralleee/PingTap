using UnityEngine;

namespace Fralle.PingTap
{
  public class AIMemory
  {
    public float Age => Time.time - lastSeen;
    public DamageController damageController;
    public GameObject gameObject;
    public Vector3 position;
    public Vector3 direction;
    public bool hostile;
    public float distance;
    public float angle;
    public float lastSeen;
    public float score;
  }
}
