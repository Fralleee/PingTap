using Fralle.Attack;
using TMPro;
using UnityEngine;

namespace Fralle
{
  public class FloatingText : MonoBehaviour
  {
    [HideInInspector] public Vector3 position;
    [HideInInspector] public new Camera camera;

    [SerializeField] float destroyTime = 2f;
    [SerializeField] Vector2 randomPosition = new Vector2(75f, 75f);

    TextMeshProUGUI text;
    Vector3 generatedRandomPosition;
    float defaultSize;

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

    public void Setup(string text, Vector3 position, Camera camera, HitBoxType hitBoxType)
    {
      this.text.text = text;
      this.position = position + generatedRandomPosition;
      this.camera = camera;

      switch (hitBoxType)
      {
        case HitBoxType.Major:
          this.text.alpha = 0.75f;
          break;
        case HitBoxType.Nerve:
          ColorUtility.TryParseHtmlString("#FA800B", out Color deepOrange);
          this.text.color = deepOrange;
          this.text.fontStyle = FontStyles.Bold;
          this.text.fontSize = 28f;
          break;
        case HitBoxType.Minor:
          ColorUtility.TryParseHtmlString("#EEF0F2", out Color gray);
          this.text.color = gray;
          this.text.alpha = 0.65f;
          break;
      }

      defaultSize = this.text.fontSize;
    }

    void UpdatePosition()
    {
      float distance = Vector3.Distance(camera.transform.position, position);
      float yPositionOffset = Mathf.Lerp(1f, 3f, distance / 40);
      float sizeOffset = Mathf.Lerp(2f, 1f, distance / 40);
      text.fontSize = defaultSize * sizeOffset;

      Vector3 newPosition = camera.WorldToScreenPoint(position + Vector3.up * yPositionOffset);
      transform.position = newPosition;
    }
  }
}