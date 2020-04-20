using System.Collections;
using System.Linq;
using Fralle;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class ScopeAction : PlayerAction
{
  [SerializeField] MouseButton scopeButton = MouseButton.Right;
  [SerializeField] Vector3 scopePos = new Vector3(-0.225f, 0.1f);

  [SerializeField] bool interruptScopeOnFire = true;
  [SerializeField] float scopedFov = 50f;
  [SerializeField] float scopeTime = 1f;

  [Header("Scoped visuals")]
  [SerializeField] GameObject scopedUI;
  [SerializeField] GameObject scopedPostProcess;

  Weapon weapon;
  Camera playerCamera;
  GameObject scopedUIInstance;
  CanvasGroup scopeUICanvasGroup;
  GameObject scopedPostProcessInstance;
  Volume scopedVolume;
  MouseLook mouseLook;

  float defaultFov = 60f;
  bool isScoping;

  void Start()
  {
    weapon = GetComponent<Weapon>();
    playerCamera = weapon.playerCamera.GetComponent<Camera>();
    mouseLook = GetComponentInParent<MouseLook>();

    defaultFov = playerCamera.fieldOfView;

    if (interruptScopeOnFire) weapon.OnActiveWeaponActionChanged += HandleActiveWeaponChange;

    SetupVisuals();
  }

  void Update()
  {
    if (weapon.ActiveWeaponAction != ActiveWeaponAction.READY) return;

    if (!isScoping && Input.GetMouseButton((int)scopeButton))
    {
      StopAllCoroutines();
      StartCoroutine(Scoping());
    }
    else if (Input.GetMouseButtonUp((int)scopeButton))
    {
      StopAllCoroutines();
      StartCoroutine(EndScoping(scopeTime * 0.5f));
    }
  }

  void HandleActiveWeaponChange(ActiveWeaponAction newActiveWeaponAction)
  {
    if (!isScoping || newActiveWeaponAction == ActiveWeaponAction.READY) return;

    StopAllCoroutines();
    StartCoroutine(EndScoping(0.2f));
  }

  void SetupVisuals()
  {
    var ui = new GameObject("Visuals");
    ui.transform.parent = transform;
    scopedUIInstance = Instantiate(scopedUI, ui.transform);
    scopeUICanvasGroup = scopedUIInstance.GetComponent<CanvasGroup>();
    scopeUICanvasGroup.alpha = 0;

    scopedPostProcessInstance = Instantiate(scopedPostProcess, ui.transform);
    scopedVolume = scopedPostProcessInstance.GetComponent<Volume>();
    scopedVolume.weight = 0;
  }

  IEnumerator Scoping()
  {
    var time = 0f;
    isScoping = true;
    mouseLook.currentSensitivity = mouseLook.mouseZoomSensitivity;

    while (time < 1)
    {
      time += Time.deltaTime / scopeTime;
      time = Mathf.Clamp(time, 0, 1);
      float fadeTime = Mathf.Clamp(time + 0.5f, 0, 1);

      transform.localPosition = Vector3.Lerp(transform.localPosition, scopePos, time);
      playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, scopedFov, time);
      scopedVolume.weight = Mathf.Lerp(0, 1, fadeTime);
      scopeUICanvasGroup.alpha = Mathf.Lerp(0, 1, fadeTime);

      foreach (Material material in weapon.graphics.GetComponent<Renderer>().materials)
      {
        Color color = material.GetColor("_BaseColor");
        color.a = Mathf.Clamp(1 - fadeTime, 0, 1);
        material.SetColor("_BaseColor", color);
      }

      yield return null;
    }
  }

  IEnumerator EndScoping(float timer)
  {
    isScoping = false;
    var time = 0f;
    mouseLook.currentSensitivity = mouseLook.mouseSensitivity;

    while (time < 1)
    {
      time += Time.deltaTime / timer;
      time = Mathf.Clamp(time, 0, 1);
      float fadeTime = Mathf.Clamp(time * 4f, 0, 1);

      transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, time);
      playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, defaultFov, time);
      scopedVolume.weight = Mathf.Lerp(1, 0, fadeTime);
      scopeUICanvasGroup.alpha = Mathf.Lerp(1, 0, fadeTime);

      foreach (Material material in weapon.graphics.GetComponent<Renderer>().materials)
      {
        Color color = material.GetColor("_BaseColor");
        color.a = fadeTime;
        material.SetColor("_BaseColor", color);
      }

      yield return null;
    }
  }
}
