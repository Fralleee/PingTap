using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace Fralle.Attack
{
  public class Scope : Action
  {
    [SerializeField] MouseButton scopeButton = MouseButton.Right;
    [SerializeField] Vector3 scopePos = new Vector3(-0.225f, 0.1f);

    [SerializeField] bool interruptScopeOnFire = true;
    [SerializeField] float scopedFov = 50f;
    [SerializeField] float scopeTime = 1f;

    [FormerlySerializedAs("scopedUI")]
    [Header("Scoped visuals")]
    [SerializeField] GameObject scopedUi;
    [SerializeField] GameObject scopedPostProcess;

    Weapon weapon;
    Camera playerCamera;
    GameObject scopedUiInstance;
    CanvasGroup scopeUiCanvasGroup;
    GameObject scopedPostProcessInstance;
    Volume scopedVolume;
    MouseLook mouseLook;
    Renderer weaponRenderer;

    float defaultFov = 60f;
    bool isScoping;

    void Start()
    {
      weapon = GetComponent<Weapon>();
      playerCamera = weapon.playerCamera.GetComponent<Camera>();
      mouseLook = GetComponentInParent<MouseLook>();
      weaponRenderer = weapon.GetComponentInChildren<Renderer>();

      defaultFov = playerCamera.fieldOfView;

      if (interruptScopeOnFire) weapon.OnActiveWeaponActionChanged += HandleActiveWeaponChange;

      SetupVisuals();
    }

    void Update()
    {
      if (weapon.ActiveWeaponAction != WeaponStatus.Ready) return;

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

    void HandleActiveWeaponChange(WeaponStatus newActiveWeaponAction)
    {
      if (!isScoping || newActiveWeaponAction == WeaponStatus.Ready) return;

      StopAllCoroutines();
      StartCoroutine(EndScoping(0.2f));
    }

    void SetupVisuals()
    {
      var ui = new GameObject("Visuals");
      ui.transform.parent = transform;
      scopedUiInstance = Instantiate(scopedUi, ui.transform);
      scopeUiCanvasGroup = scopedUiInstance.GetComponent<CanvasGroup>();
      scopeUiCanvasGroup.alpha = 0;

      scopedPostProcessInstance = Instantiate(scopedPostProcess, ui.transform);
      scopedVolume = scopedPostProcessInstance.GetComponent<Volume>();
      scopedVolume.weight = 0;
    }

    IEnumerator Scoping()
    {
      var time = 0f;
      isScoping = true;
      mouseLook.currentSensitivity = mouseLook.mouseSensitivity * mouseLook.mouseZoomModifier;

      while (time < 1)
      {
        time += Time.deltaTime / scopeTime;
        time = Mathf.Clamp(time, 0, 1);
        float fadeTime = Mathf.Clamp(time + 0.7f, 0, 1);
        transform.localPosition = Vector3.Lerp(transform.localPosition, scopePos, time);
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, scopedFov, time);
        scopedVolume.weight = Mathf.Lerp(0, 1, fadeTime);
        scopeUiCanvasGroup.alpha = Mathf.Lerp(0, 1, fadeTime);
        if (time > scopeTime * 0.5f) weaponRenderer.enabled = false;
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
        scopeUiCanvasGroup.alpha = Mathf.Lerp(1, 0, fadeTime);
        if (time > scopeTime * 0.5f) weaponRenderer.enabled = true;
        yield return null;
      }
    }
  }
}