using TMPro;
using UnityEngine;

namespace Fralle
{
  public class FloatingText : MonoBehaviour
  {
    [HideInInspector] public Vector3 position;
    [HideInInspector] public Camera camera;

    [SerializeField] float destroyTime = 2f;
    [SerializeField] Vector2 randomPosition = new Vector2(75f, 75f);

    TextMeshProUGUI text;
    Vector3 generatedRandomPosition;

    void Awake()
    {
      text = GetComponentInChildren<TextMeshProUGUI>();
      generatedRandomPosition = new Vector3(Random.Range(-randomPosition.x, randomPosition.x), Random.Range(-randomPosition.y, randomPosition.y), 0);
    }

    void Start()
    {
      Destroy(gameObject, destroyTime);
    }

    void LateUpdate()
    {
      bool isVisible = ToggleIfVisible();
      if (isVisible) UpdatePosition();
    }

    bool ToggleIfVisible()
    {
      Vector3 viewPortPoint = camera.WorldToViewportPoint(position);
      if (!viewPortPoint.InViewPort())
      {
        text.enabled = false;
        return false;
      }

      text.enabled = true;
      return true;
    }

    public void Setup(string text, Vector3 position, Camera camera)
    {
      this.text.text = text;
      this.position = position + generatedRandomPosition;
      this.camera = camera;
    }

    void UpdatePosition()
    {
      float distance = Vector3.Distance(camera.transform.position, position);
      float yPositionOffset = Mathf.Lerp(0.5f, 3f, distance / 40);

      Vector3 newPosition = camera.WorldToScreenPoint(position + Vector3.up * yPositionOffset);
      transform.position = newPosition;
    }
  }
}