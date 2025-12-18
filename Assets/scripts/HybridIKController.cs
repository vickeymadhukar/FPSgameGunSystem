using UnityEngine;

public class HybridIKController : MonoBehaviour
{
    [Header("References")]
    public Movement playerMovement;
    public Transform gun1;
    public Transform gun2;
    public Transform rightHandHolder;
    public Transform duringaimng;
    public Transform righthand;
    public IKControl handplace;
    public GunAimFollow aimfollow;

    [Header("Debug")]
    public bool isDetached = false;

    private Vector3 originalLocalPos1;
    private Quaternion originalLocalRot1;
    private Vector3 originalLocalPos2;
    private Quaternion originalLocalRot2;

    void Start()
    {
        if (gun1 != null)
        {
            originalLocalPos1 = gun1.localPosition;
            originalLocalRot1 = gun1.localRotation;
        }

        if (gun2 != null)
        {
            originalLocalPos2 = gun2.localPosition;
            originalLocalRot2 = gun2.localRotation;
        }
    }

    void Update()
    {
        Transform activeGun = null;
        Vector3 originalPos = Vector3.zero;
        Quaternion originalRot = Quaternion.identity;

        if (gun1 != null && gun1.gameObject.activeInHierarchy)
        {
            activeGun = gun1;
            originalPos = originalLocalPos1;
            originalRot = originalLocalRot1;
        }
        else if (gun2 != null && gun2.gameObject.activeInHierarchy)
        {
            activeGun = gun2;
            originalPos = originalLocalPos2;
            originalRot = originalLocalRot2;
        }

        if (activeGun == null) return; 

        bool isStandingStill = playerMovement.inputMovement.magnitude < 0.1f;
        bool isShooting = playerMovement.isAim;

        
        if (isStandingStill || isShooting)
        {
            if (!isDetached)
            {
               
                activeGun.SetParent(duringaimng, true);
                activeGun.position = duringaimng.position;
                activeGun.rotation = duringaimng.rotation;

                aimfollow.aim = false;
                handplace.IkActive = true;

                isDetached = true;
                Debug.Log("🔴 Gun detached (Aiming)");
            }
        }
        else
        {
            if (isDetached)
            {
                // Reattach the gun to right-hand holder
                activeGun.SetParent(rightHandHolder, false);
                activeGun.localPosition = originalPos;
                activeGun.localRotation = originalRot;

                aimfollow.aim = true;
                handplace.IkActive = false;

                isDetached = false;
                Debug.Log("🟢 Gun reattached (Normal)");
            }
        }
    }
}
