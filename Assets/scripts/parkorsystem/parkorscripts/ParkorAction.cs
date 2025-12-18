using UnityEngine;

[CreateAssetMenu(menuName = "parkorsystem/ParkorAction")]
public class ParkorAction : ScriptableObject
{
    [Header("Animation Settings")]
    [SerializeField] private string animationName;
    [SerializeField] private string Obsticletag;
    [Header("Ray Trigger Settings")]
    [SerializeField] private bool forFeet = false;
    [SerializeField] private bool forKnee = false;
    [SerializeField] private bool forWaist = false;
    public string GetAnimation(Enviorment.ObsticalData hitdata)
    {
        if (Obsticletag != null && Obsticletag == hitdata.forwordhitdata.transform.tag)
            return animationName;

        if (hitdata.waistHit && forWaist)
            return animationName;

        if (hitdata.kneeHit && forKnee)
            return animationName;

        if (hitdata.feetHit && forFeet)
            return animationName;

        return null;
    }

   
    public string AnimName => animationName;
}
