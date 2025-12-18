using UnityEngine;
using UnityEngine.InputSystem;

public class Weapons : MonoBehaviour
{
    [Header("fire rate")]
    [SerializeField] float fireaRate;
    float fireRateTimer;
    [SerializeField] bool semiAuto;

    private PlayerController playerInput;
    public bool pistolinhand = false;

    [Header("bulletsproperties")]
    [SerializeField] GameObject bullet;
    [SerializeField] Transform bulltepos;
    [SerializeField] float bulltespeed;
    [SerializeField] int bulltespershoot;

    SimplePlayerTouchRotation aim; // purana
    public Recoil gunrecoil;       // gun model ke liye (optional)

    [Header("Camera Recoil Settings")]
    [SerializeField] SimplePlayerTouchRotationCinemachine cameraLook; // drag from inspector
    [SerializeField] float camRecoilUp = 3f;      // is gun ka vertical recoil
    [SerializeField] float camRecoilSide = 1f;    // is gun ka horizontal recoil

    private void Awake()
    {
        playerInput = new PlayerController();
        playerInput.Enable();

        aim = GetComponent<SimplePlayerTouchRotation>();

        // agar same object / parent par script laga ho:
        if (cameraLook == null)
        {
            cameraLook = GetComponentInParent<SimplePlayerTouchRotationCinemachine>();
        }
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    void Update()
    {
        if (SHouldFire()) fire();
    }

    bool SHouldFire()
    {
        fireRateTimer += Time.deltaTime;
        if (fireRateTimer < fireaRate) return false;
        if (semiAuto && playerInput.Player.Shoot.WasPressedThisFrame()) return true;
        if (!semiAuto && playerInput.Player.Shoot.IsPressed()) return true;
        return false;
    }

    void fire()
    {
        fireRateTimer = 0;

        bulltepos.LookAt(aim.aimpos);

        // total recoil ko bullets me split kar diya
        float perBulletUp = camRecoilUp / Mathf.Max(1, bulltespershoot);
        float perBulletSide = camRecoilSide / Mathf.Max(1, bulltespershoot);

        for (int i = 0; i < bulltespershoot; i++)
        {
            GameObject currentBullte = Instantiate(bullet, bulltepos.position, bulltepos.rotation);
            Rigidbody rb = currentBullte.GetComponent<Rigidbody>();
            rb.AddForce(bulltepos.forward * bulltespeed, ForceMode.Impulse);

            // gun ka visual kick (optional)
            if (gunrecoil != null)
                gunrecoil.RecoilFire();

            // CAMERA RECOIL per bullet
            if (cameraLook != null)
                cameraLook.AddCameraRecoil(perBulletUp, perBulletSide);
        }
    }

}
