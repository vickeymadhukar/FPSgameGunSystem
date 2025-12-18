using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Unity.Cinemachine;

public class SimplePlayerTouchRotationCinemachine : MonoBehaviour
{
    [Header("Touch Area")]
    [SerializeField] private RectTransform rotationPanel;

    [Header("Cinemachine Camera (optional)")]
    [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private Transform cameraTransform;   // camerapivot

    [Header("Look Sensitivity")]
    [SerializeField] private float sensitivityX = 7f;
    [SerializeField] private float sensitivityY = 5f;
    [SerializeField] private float verticalClamp = 90f;

    [Header("Aim / Weapon Rotation")]
    [SerializeField] private Transform aimTransform;

    [Header("Aim Point Settings")]
    public Transform aimpos;
    [SerializeField] private float aimsmooth = 20f;
    [SerializeField] private LayerMask aimlayer;
    [SerializeField] private Camera aimCamera;    // main camera for rays

    // -------- Base rotation (user touch + recoil) --------
    private float baseYaw;    // player body yaw
    private float basePitch;  // camera pitch

    // touch state
    private int activeFingerId = -1;
    private Vector2 lastTouchPosition;

    private void Awake()
    {
        baseYaw = transform.eulerAngles.y;

        if (cameraTransform != null)
        {
            var e = cameraTransform.localEulerAngles;
            basePitch = (e.x > 180f) ? e.x - 360f : e.x;
        }
    }

    private void Update()
    {
        HandleTouchLook();
        ApplyFinalRotation();
        DoAimingRaycast();
    }

    // ---------------- TOUCH LOOK ----------------
    private void HandleTouchLook()
    {
        if (Touchscreen.current == null)
            return;

        foreach (var touch in Touchscreen.current.touches)
        {
            int fingerId = touch.touchId.ReadValue();
            Vector2 touchPos = touch.position.ReadValue();
            var phase = touch.phase.ReadValue();

            if (phase == UnityEngine.InputSystem.TouchPhase.Began &&
                IsTouchInsidePanel(touchPos))
            {
                if (activeFingerId == -1)
                {
                    activeFingerId = fingerId;
                    lastTouchPosition = touchPos;
                }
            }

            if (activeFingerId == fingerId)
            {
                if (phase == UnityEngine.InputSystem.TouchPhase.Moved)
                {
                    Vector2 delta = touchPos - lastTouchPosition;
                    lastTouchPosition = touchPos;

                    // Touch se base rotation change
                    baseYaw += delta.x * sensitivityX * Time.deltaTime;
                    basePitch -= delta.y * sensitivityY * Time.deltaTime;
                    basePitch = Mathf.Clamp(basePitch, -verticalClamp, verticalClamp);
                }

                if (phase == UnityEngine.InputSystem.TouchPhase.Ended ||
                    phase == UnityEngine.InputSystem.TouchPhase.Canceled)
                {
                    activeFingerId = -1;
                }
            }
        }
    }

    private void ApplyFinalRotation()
    {
        // Final rotation = base (touch + recoil)
        float finalYaw = baseYaw;
        float finalPitch = Mathf.Clamp(basePitch, -verticalClamp, verticalClamp);

        // player yaw
        transform.rotation = Quaternion.Euler(0f, finalYaw, 0f);

        // camera pitch on pivot
        if (cameraTransform != null)
        {
            cameraTransform.localRotation = Quaternion.Euler(finalPitch, 0f, 0f);
        }

        // weapon / arms yaw
        if (aimTransform != null)
        {
            aimTransform.localRotation = Quaternion.Euler(0f, finalYaw, 0f);
        }
    }

    private void DoAimingRaycast()
    {
        Camera cam = aimCamera != null ? aimCamera : Camera.main;
        if (cam == null) return;

        Vector2 screencenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = cam.ScreenPointToRay(screencenter);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, aimlayer))
        {
            if (aimpos != null)
            {
                aimpos.position = Vector3.Lerp(
                    aimpos.position,
                    hit.point,
                    aimsmooth * Time.deltaTime
                );
            }
        }
    }

    private bool IsTouchInsidePanel(Vector2 screenPos)
    {
        if (rotationPanel == null) return false;

        Vector2 localPoint;
        Canvas canvas = rotationPanel.GetComponentInParent<Canvas>();
        Camera uiCamera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;

        bool isInside = RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rotationPanel,
            screenPos,
            uiCamera,
            out localPoint
        );

        if (isInside)
            return rotationPanel.rect.Contains(localPoint);

        return false;
    }

    // ------------- PUBLIC API FOR WEAPONS -------------
    // recoilUp   = kitna upar jaye (positive value)
    // recoilSide = left/right random max amount
    public void AddCameraRecoil(float recoilUp, float recoilSide)
    {
        // PUBG style: pitch/yaw me direct add, NO auto return
        basePitch += recoilUp;
        basePitch = Mathf.Clamp(basePitch, -verticalClamp, verticalClamp);

        float side = Random.Range(-recoilSide, recoilSide);
        baseYaw += side;
    }
}
