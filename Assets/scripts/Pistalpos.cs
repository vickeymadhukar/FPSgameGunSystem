using UnityEngine;

public class Pistalpos : MonoBehaviour
{

    [Header("Weapon IK Points")]
    public Transform weaponHint;   // weapon ka hint point
    public Transform weaponTarget; // weapon ka target point

    [Header("Rig IK Points")]
    public Transform leftHandHint;   // rig me jo TwoBoneIK ka hint assigned hai
    public Transform leftHandTarget; // rig me jo TwoBoneIK ka target assigned hai

    [Header("Settings")]
    public bool setPos = true; // agar true hoga to apply karega

    void LateUpdate()
    {
        if (setPos && weaponHint != null && weaponTarget != null)
        {
            // Copy position & rotation each frame
            leftHandHint.position = weaponHint.position;
            leftHandHint.rotation = weaponHint.rotation;

            leftHandTarget.position = weaponTarget.position;
            leftHandTarget.rotation = weaponTarget.rotation;
        }
    }


}
