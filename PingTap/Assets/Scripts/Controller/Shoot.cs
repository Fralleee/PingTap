using UnityEngine;

public class Shoot : MonoBehaviour
{
  [SerializeField] GameObject projectile;
  [SerializeField] Transform projectileOrigin;
  public float speed = 1000;
  new Camera camera;

  private void Start()
  {
    camera = Camera.main;
  }

  void Update()
  {

    if (Input.GetKeyDown(KeyCode.Mouse0))
    {
      {
        Vector3 position = projectileOrigin?.position ?? camera.transform.position + camera.transform.forward * 0.5f;
        GameObject instance = Instantiate(projectile, position, camera.transform.rotation);
        instance.GetComponent<Rigidbody>().AddForce(instance.transform.forward * speed);
      }
    }
  }
}
