using UnityEngine;

namespace Fralle.PingTap
{
  [CreateAssetMenu(menuName = "Settings/Headbob")]
  public class HeadbobConfiguration : ScriptableObject
  {
    [Range(0f, 20f)] public float BobbingSpeed = 2f;
    [Range(0f, 3f)] public float CameraBobbingAmount = 0.05f;
    [Range(0f, 3f)] public float WeaponBobbingAmount = 0.05f;
    [Range(0f, 3f)] public float WeaponRotationAmount = 1.75f;
  }
}
