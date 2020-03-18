using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NavPoint : MonoBehaviour
{
  [SerializeField] List<NavPoint> navPoints;

  public NavPoint GetRandomNextPoint()
  {
    if (navPoints.Count > 0) return navPoints.GetRandomElement();
    return null;
  }

  public NavPoint GetNearestPoint(Vector3 position)
  {
    if (navPoints.Count > 0) return navPoints.OrderBy(x => Vector3.Distance(position, x.transform.position)).FirstOrDefault();
    return null;
  }

  void OnDrawGizmosSelected()
  {
    Gizmos.color = new Color(0, 0, 1, 1F);
    Gizmos.DrawWireSphere(transform.position, 5f);
    foreach (var navPoint in navPoints)
    {
      Gizmos.color = new Color(0.5f, 0, 1, 1F);
      Gizmos.DrawLine(transform.position, navPoint.transform.position);
      Gizmos.color = new Color(0, 1, 0, 1F);
      Gizmos.DrawWireSphere(navPoint.transform.position, 3f);
    }
  }
}
