using Fralle.Core.Extensions;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Fralle.UI.HUD
{
  public class CompassUi : MonoBehaviour
  {
    [SerializeField] RawImage compassImage = null;
    [SerializeField] TextMeshProUGUI compassDirectionText = null;

    Transform playerOrientation;
    Dictionary<int, string> directionOverride = new Dictionary<int, string>
    {
      {0, "N"},
      {360, "N"},
      {45, "NE"},
      {90, "E"},
      {135, "SE"},
      {180, "S"},
      {225, "SW"},
      {270, "W"},
      {315, "NW"}
    };

    void Start()
    {
      var playerMain = GetComponentInParent<Player>();
      playerOrientation = playerMain.transform.FindRecursively("Orientation");
    }

    void Update()
    {
      compassImage.uvRect = new Rect(playerOrientation.localEulerAngles.y / 360, 0, 1, 1);

      var forward = playerOrientation.forward.With(y: 0);
      float headingAngle = Quaternion.LookRotation(forward).eulerAngles.y;
      int displayAngle = 5 * (Mathf.RoundToInt(headingAngle / 5f));

      compassDirectionText.text = directionOverride.TryGetValue(displayAngle, out string textValue) ? textValue : displayAngle.ToString();
    }
  }

}
