using System.Collections;
using UnityEngine;

namespace Fralle.Abilities.Turret
{
  public class TurretStabilizer : MonoBehaviour
  {
    [SerializeField] Transform drill = null;
    [SerializeField] ParticleSystem effect = null;
    [SerializeField] float stabilizeTime = 1f;

    TurretController turret;

    new Rigidbody rigidbody;
    new Transform transform;
    new BoxCollider collider;

    RaycastHit hitInfo;

    bool isAttached;

    void Awake()
    {
      turret = GetComponent<TurretController>();

      rigidbody = GetComponent<Rigidbody>();
      transform = GetComponent<Transform>();
      collider = GetComponent<BoxCollider>();
    }

    void Update()
    {
      Attach();
    }

    void Attach()
    {
      if (isAttached) return;
      if (rigidbody.isKinematic || rigidbody.velocity.magnitude > 0.2f) return;

      rigidbody.isKinematic = true;
      var distance = (collider.size.y / 2f) + 0.1f;

      if (Physics.Raycast(transform.position, Vector3.down, out hitInfo, distance, Physics.AllLayers, QueryTriggerInteraction.Ignore))
      {
        StartCoroutine(Stabilize());
      }

      isAttached = true;
    }

    IEnumerator Stabilize()
    {
      effect.Play();
      Quaternion startRotation = transform.rotation;
      Quaternion endRotation = hitInfo.normal + Vector3.down != Vector3.zero ? Quaternion.LookRotation(hitInfo.normal + Vector3.down) : transform.rotation;
      Vector3 startPosition = transform.position;
      Vector3 endPosition = hitInfo.point;

      for (float t = 0; t < stabilizeTime; t += Time.deltaTime)
      {
        transform.rotation = Quaternion.Lerp(startRotation, endRotation, t / stabilizeTime);
        transform.position = Vector3.Lerp(startPosition, endPosition, t / stabilizeTime);
        drill.RotateAround(transform.position, transform.up, t * 60);
        yield return null;
      }

      transform.rotation = endRotation;
      transform.position = endPosition;
      turret.IsDeployed = true;
      effect.Stop();

      Destroy(this);
    }
  }
}