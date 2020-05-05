using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AnimationRecorderRagdollHelper : MonoBehaviour
{
  [FormerlySerializedAs("_rigidBody")] [SerializeField] private Rigidbody rigidBody;
  [FormerlySerializedAs("_applyingForce")] [SerializeField] private Vector3 applyingForce;
  [FormerlySerializedAs("_startingDelay")] [SerializeField] private float startingDelay;
  [FormerlySerializedAs("_recordAnimation")] [SerializeField] public bool recordAnimation;

  void Start()
  {
    StartCoroutine(Handle());
  }

  private IEnumerator Handle()
  {
    yield return new WaitForSeconds(startingDelay);

    Animator animator = GetComponent<Animator>();
    if (animator != null)
    {
      animator.enabled = false;
    }

    Rigidbody[] rBodies = GetComponentsInChildren<Rigidbody>();
    for (int i = 0; i < rBodies.Length; ++i)
    {
      rBodies[i].isKinematic = false;
    }

    BoxCollider[] bColliders = GetComponentsInChildren<BoxCollider>();
    for (int i = 0; i < bColliders.Length; ++i)
    {
      bColliders[i].isTrigger = false;
    }

    CapsuleCollider[] cColliders = GetComponentsInChildren<CapsuleCollider>();
    for (int i = 0; i < cColliders.Length; ++i)
    {
      cColliders[i].isTrigger = false;
    }

    SphereCollider[] sColliders = GetComponentsInChildren<SphereCollider>();
    for (int i = 0; i < sColliders.Length; ++i)
    {
      sColliders[i].isTrigger = false;
    }

    Debug.Log($"Apply force {applyingForce}");
    rigidBody.AddForce(applyingForce, mode: ForceMode.VelocityChange);

    if (recordAnimation)
    {
      gameObject.AddComponent<AnimationRecorder>().StartRecording();
    }
  }
}