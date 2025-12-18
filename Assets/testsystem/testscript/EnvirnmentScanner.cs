using UnityEngine;

public class EnvirnmentScanner : MonoBehaviour
{
    [SerializeField] public Vector3 forwordrayoffset = new Vector3(0, 2.5f, 0);
    [SerializeField] public float forwordraylength = 5f;
    [SerializeField] public LayerMask Obsticledetect;
    [SerializeField] public float heighthitlength = 5f;

    public ObsticleHitData ObsticleCheck()
    {
        ObsticleHitData hitData = new ObsticleHitData();

        // Store the forward raycast origin in the hitData struct
        hitData.forwordRayOrigin = forwordrayoffset;

        var forwordOrigin = transform.position + hitData.forwordRayOrigin;
        hitData.forwordHitfound = Physics.Raycast(forwordOrigin, transform.forward, out hitData.forwordHitdata, forwordraylength, Obsticledetect);

        Debug.DrawRay(forwordOrigin, transform.forward * forwordraylength, (hitData.forwordHitfound) ? Color.red : Color.white);

        if (hitData.forwordHitfound)
        {
            var heightHitOrigin = hitData.forwordHitdata.point + Vector3.up * heighthitlength;

            hitData.heighthitfound = Physics.Raycast(heightHitOrigin, Vector3.down, out hitData.heighthitdata, heighthitlength, Obsticledetect);
            Debug.DrawRay(heightHitOrigin, Vector3.down * heighthitlength, (hitData.heighthitfound) ? Color.red : Color.white);
        }

        return hitData;
    }


}

public struct ObsticleHitData
{
    public bool forwordHitfound;
    public bool heighthitfound;
    public RaycastHit heighthitdata;
    public RaycastHit forwordHitdata;
    public Vector3 forwordRayOrigin; // Add this to store the offset
};

