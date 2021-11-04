using UnityEngine;

public class Rotation : MonoBehaviour
{
  [SerializeField] float rotationSpeed = 1f;
  [SerializeField] Vector3 axis = Vector3.up;

  void Update()
  {
    transform.Rotate(axis * (rotationSpeed * Time.deltaTime));
  }
}
