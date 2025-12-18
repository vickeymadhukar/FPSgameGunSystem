using UnityEngine;

public class Enviorment : MonoBehaviour
{
    [Header("Forward Ray Settings")]
    [SerializeField] private Vector3 forwordrayposition = new Vector3(0, 0.25f, 0);
    [SerializeField] private float forwordrayrange = 8f;
    [SerializeField] private LayerMask obsticle;

    [Header("Ray Heights (from player base)")]
    [SerializeField] private float feetHeight = 0.2f;
    [SerializeField] private float kneeHeight = 0.9f;
    [SerializeField] private float waistHeight = 1.5f;

    public ObsticalData ObscticalCheck()
    {
        ObsticalData hitdata = new ObsticalData();

        Vector3 forwardPos = transform.position + forwordrayposition;

        // Forward detection
        hitdata.forwordhitfound = Physics.Raycast(
            forwardPos,
            transform.forward,
            out hitdata.forwordhitdata,
            forwordrayrange,
            obsticle
        );
        Debug.DrawRay(forwardPos, transform.forward * forwordrayrange, hitdata.forwordhitfound ? Color.red : Color.white);

        if (hitdata.forwordhitfound)
        {
            // Multi-level rays
            hitdata.feetHit = Physics.Raycast(transform.position + Vector3.up * feetHeight, transform.forward, forwordrayrange, obsticle);
            hitdata.kneeHit = Physics.Raycast(transform.position + Vector3.up * kneeHeight, transform.forward, forwordrayrange, obsticle);
            hitdata.waistHit = Physics.Raycast(transform.position + Vector3.up * waistHeight, transform.forward, forwordrayrange, obsticle);

            // Debug rays
            Debug.DrawRay(transform.position + Vector3.up * feetHeight, transform.forward * forwordrayrange, hitdata.feetHit ? Color.green : Color.white);
            Debug.DrawRay(transform.position + Vector3.up * kneeHeight, transform.forward * forwordrayrange, hitdata.kneeHit ? Color.blue : Color.white);
            Debug.DrawRay(transform.position + Vector3.up * waistHeight, transform.forward * forwordrayrange, hitdata.waistHit ? Color.yellow : Color.white);
        }

        return hitdata;
    }

   
    public struct ObsticalData
    {
        public bool forwordhitfound;
        public RaycastHit forwordhitdata;

        public bool feetHit;
        public bool kneeHit;
        public bool waistHit;
    }
}
