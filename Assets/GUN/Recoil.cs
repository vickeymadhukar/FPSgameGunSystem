using UnityEngine;

public class Recoil : MonoBehaviour
{
    private Vector3 currentRotation;
    private Vector3 targetRotation;

    [SerializeField] private float recoilX = 2f;
    [SerializeField] private float recoilY = 0.5f;
    [SerializeField] private float recoilZ = 0.5f;

    [SerializeField] private float snappiness = 10f;
    [SerializeField] private float returnSpeed = 6f;

    [SerializeField] private float maxX = 12f;
    [SerializeField] private float maxY = 6f;
    [SerializeField] private float maxZ = 6f;

    private const float rotationThreshold = 0.0001f;

    void Start()
    {
        currentRotation = Vector3.zero;
        targetRotation = Vector3.zero;
    }

    void LateUpdate()
    {
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Lerp(currentRotation, targetRotation, snappiness * Time.deltaTime);

        currentRotation.x = Mathf.Clamp(currentRotation.x, -maxX, maxX);
        currentRotation.y = Mathf.Clamp(currentRotation.y, -maxY, maxY);
        currentRotation.z = Mathf.Clamp(currentRotation.z, -maxZ, maxZ);

        if (currentRotation.sqrMagnitude > rotationThreshold)
            transform.localRotation = Quaternion.Euler(currentRotation);
    }

    public void RecoilFire()
    {
        Vector3 add = new Vector3(
            recoilX,
            Random.Range(-recoilY, recoilY),
            Random.Range(-recoilZ, recoilZ)
        );

        targetRotation += add;
        targetRotation.x = Mathf.Clamp(targetRotation.x, -maxX, maxX);
        targetRotation.y = Mathf.Clamp(targetRotation.y, -maxY, maxY);
        targetRotation.z = Mathf.Clamp(targetRotation.z, -maxZ, maxZ);
    }

    public void ResetRecoil()
    {
        currentRotation = Vector3.zero;
        targetRotation = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
}
