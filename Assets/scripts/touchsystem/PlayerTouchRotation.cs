using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class SimplePlayerTouchRotation : MonoBehaviour
{
    [SerializeField] private RectTransform rotationPanel;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float sensitivityX = 7f;
    [SerializeField] private float sensitivityY = 5f;
    private float horizontalRotation;
    private float verticalRotation;
    [SerializeField] private float verticalClamp = 90f;
    [SerializeField] private Transform aimTransform;

    public Transform aimpos;
    [SerializeField] float aimsmooth = 20f;
    [SerializeField] LayerMask aimlayer;

    private int activeFingerId = -1;
    private Vector2 lastTouchPosition;

    private void Update()
    {
        if (Touchscreen.current == null)
            return;

        foreach (var touch in Touchscreen.current.touches)
        {
            int fingerId = touch.touchId.ReadValue();
            Vector2 touchPos = touch.position.ReadValue();

            // Check if a new touch has started and is inside the panel
            if (touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began && IsTouchInsidePanel(touchPos))
            {
                // If no finger is currently rotating, assign this one
                if (activeFingerId == -1)
                {
                    activeFingerId = fingerId;
                    lastTouchPosition = touchPos;
                }
            }

            // If this is the active finger, process its movement
            if (activeFingerId == fingerId)
            {
                if (touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Moved)
                {
                    Vector2 delta = touchPos - lastTouchPosition;
                    lastTouchPosition = touchPos;

                    horizontalRotation += delta.x * sensitivityX * Time.deltaTime;
                    verticalRotation -= delta.y * sensitivityY * Time.deltaTime;
                    verticalRotation = Mathf.Clamp(verticalRotation, -verticalClamp, verticalClamp);

                    transform.rotation = Quaternion.Euler(0f, horizontalRotation, 0f);

                    if (cameraTransform != null)
                        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);

                    if (aimTransform != null)
                        aimTransform.localRotation = Quaternion.Euler(0f, horizontalRotation, 0f);
                }

                if (touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Ended || touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Canceled)
                {
                    // Reset the active finger when it's lifted
                    activeFingerId = -1;
                }
            }
        }

        // Aiming logic remains the same
        Vector2 screencenter = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = Camera.main.ScreenPointToRay(screencenter);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, aimlayer))
        {
            aimpos.position = Vector3.Lerp(aimpos.position, hit.point, aimsmooth * Time.deltaTime);
        }
    }

    private bool IsTouchInsidePanel(Vector2 screenPos)
    {
        if (rotationPanel == null) return false;

        Vector2 localPoint;
        Canvas canvas = rotationPanel.GetComponentInParent<Canvas>();
        Camera uiCamera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;

        // Use RectTransformUtility to check if the touch is within the panel
        bool isInside = RectTransformUtility.ScreenPointToLocalPointInRectangle(rotationPanel, screenPos, uiCamera, out localPoint);

        // Explicitly check if the local point is within the panel's rect
        if (isInside)
        {
            return rotationPanel.rect.Contains(localPoint);
        }

        return false;
    }
}