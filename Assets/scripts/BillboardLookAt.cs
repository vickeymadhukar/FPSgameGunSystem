using UnityEngine;

public class BillboardLookAt : MonoBehaviour
{
    void LateUpdate()
    {
        // Always face the camera
        transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);



    }
}
