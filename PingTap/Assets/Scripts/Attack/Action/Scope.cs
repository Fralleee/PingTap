using Fralle.Core.Enums;
using Fralle.Movement;
using Fralle.Player;
using Fralle.UI.Menu;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace Fralle.Attack.Action
{
  public class Scope : Active
  {
    [SerializeField] MouseButton scopeButton = MouseButton.Right;
    [SerializeField] Vector3 scopePos = new Vector3(-0.225f, 0.1f);

    [SerializeField] bool interruptScopeOnFire = true;
    [SerializeField] float scopedFov = 50f;
    [SerializeField] float scopeTime = 1f;

    [Header("Scoped visuals")]
    [SerializeField] GameObject scopedUi;
    [SerializeField] GameObject scopedPostProcess;

    Weapon weapon;
    Camera playerCamera;
    GameObject scopedUiInstance;
    CanvasGroup scopeUiCanvasGroup;
    GameObject scopedPostProcessInstance;
    Volume scopedVolume;
    PlayerMouseLook playerMouseLook;
    PlayerInputController input;
    Renderer weaponRenderer;

    float defaultFov = 60f;
    bool isScoping;

    void Awake()
    {
      MainMenu.OnMenuToggle += HandleMenuToggle;
    }

    void Start()
    {
      weapon = GetComponent<Weapon>();
      input = weapon.GetComponentInParent<PlayerInputController>();
      playerCamera = weapon.playerCamera.GetComponent<Camera>();
      playerMouseLook = GetComponentInParent<PlayerMouseLook>();
      weaponRenderer = weapon.GetComponentInChildren<Renderer>();

      defaultFov = playerCamera.fieldOfView;

      if (interruptScopeOnFire) weapon.OnActiveWeaponActionChanged += HandleActiveWeaponChange;

      SetupVisuals();
    }

    void Update()
    {
      if (weapon.ActiveWeaponAction != Status.Ready) return;

      var startScoping = input.GetMouseButton(scopeButton, MouseButtonState.Hold) && !isScoping;
      if (startScoping)
      {
        StopAllCoroutines();
        StartCoroutine(Scoping());
      }
      else if (input.GetMouseButton(scopeButton, MouseButtonState.Up))
      {
        StopAllCoroutines();
        StartCoroutine(EndScoping(scopeTime * 0.5f));
      }
    }

    void HandleActiveWeaponChange(Status newActiveWeaponAction)
    {
      if (!isScoping || newActiveWeaponAction == Status.Ready) return;

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
      input.mouseSensitivity = playerMouseLook.mouseSensitivity * playerMouseLook.mouseZoomModifier;

      while (time < 1)
      {
        time += Time.deltaTime / scopeTime;
        time = Mathf.Clamp(time, 0, 1);
        var fadeTime = Mathf.Clamp(time + 0.7f, 0, 1);
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
      input.mouseSensitivity = playerMouseLook.mouseSensitivity;

      while (time < 1)
      {
        time += Time.deltaTime / timer;
        time = Mathf.Clamp(time, 0, 1);
        var fadeTime = Mathf.Clamp(time * 4f, 0, 1);
        transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, time);
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, defaultFov, time);
        scopedVolume.weight = Mathf.Lerp(1, 0, fadeTime);
        scopeUiCanvasGroup.alpha = Mathf.Lerp(1, 0, fadeTime);
        if (time > scopeTime * 0.5f) weaponRenderer.enabled = true;
        yield return null;
      }
    }

    void EndScopingInstant()
    {
      isScoping = false;
      input.mouseSensitivity = playerMouseLook.mouseSensitivity;
      transform.localPosition = Vector3.zero;
      playerCamera.fieldOfView = defaultFov;
      scopedVolume.weight = 0;
      scopeUiCanvasGroup.alpha = 0;
      weaponRenderer.enabled = true;
    }

    void HandleMenuToggle(bool isMenuOpen)
    {
      enabled = !isMenuOpen;
      if (!isMenuOpen) return;

      StopAllCoroutines();
      EndScopingInstant();
    }

    void OnDestroy()
    {
      MainMenu.OnMenuToggle -= HandleMenuToggle;
      StopAllCoroutines();
      EndScopingInstant();
    }
  }
}