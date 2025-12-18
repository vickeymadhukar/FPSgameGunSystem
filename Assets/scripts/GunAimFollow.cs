using UnityEngine;

public class GunAimFollow : MonoBehaviour
{
    [Header("References")]
    public Transform bodyTransform;       // ✅ add this (the player's root or chest)
    public Transform cameraTransform;
    public Transform aimpos;

    [Header("Rotation Settings")]
    public float rotationSpeed = 10f;
    public Vector3 rotationOffset;        // tweak in Inspector
    public bool useSmoothRotation = true;

    [Header("Position Settings")]
    public float followDistance = 0.3f;   // base distance from body
    public float sideOffset = 0.15f;      // horizontal offset (right-hand hold)
    public float heightOffset = -0.1f;    // vertical fine-tuning
    public float positionSmooth = 10f;

    public bool aim = false;
    void LateUpdate()
    {
        if (aim==true)
        {
            return;
        }
        if (bodyTransform == null || cameraTransform == null || aimpos == null)
            return;

        // === BODY-BASED POSITION ===
        Vector3 bodyForward = bodyTransform.forward;
        Vector3 bodyRight = bodyTransform.right;
        Vector3 bodyUp = bodyTransform.up;

        // compute target position based on body, with side and height offset
        Vector3 targetPosition =
            bodyTransform.position +
            bodyRight * sideOffset +
            bodyUp * heightOffset +
            bodyForward * followDistance;

        // Smooth follow
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * positionSmooth);

        // === AIM ROTATION ===
        Vector3 direction = aimpos.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        targetRotation *= Quaternion.Euler(rotationOffset);

        if (useSmoothRotation)
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        else
            transform.rotation = targetRotation;
    }
}
