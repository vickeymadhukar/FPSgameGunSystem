using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Parkor", menuName = "Parkor")]
public class Parkor : ScriptableObject
{
    [SerializeField] string animaname;
    [SerializeField] float minheight;
    [SerializeField] float maxheight;

    public float height;

  [SerializeField]  bool enableTargetMatching=true;
    [SerializeField] AvatarTarget macthBodypart;
    [SerializeField] float starttime;
    [SerializeField] float endtime;

    public Vector3 matchpos {  get;  set; }
    public bool ObsticleCheck(ObsticleHitData hitdata, Transform player)
    {
        // Use the forward raycast origin's y-position as the reference point
        float playerReferenceY = player.position.y + hitdata.forwordRayOrigin.y;

        // Calculate the height from the reference point to the obstacle hit point
        height = hitdata.heighthitdata.point.y - playerReferenceY;
       

        if (height < minheight || height > maxheight)
        {
            return false;
        }

        if (enableTargetMatching)
        {
            matchpos = hitdata.heighthitdata.point;
        }

        return true;
    }

    public string AnimaName => animaname;
   public bool EnableTargetMatching => enableTargetMatching;
 public  AvatarTarget MacthBodypart=> macthBodypart;
  public float Starttime=> starttime;
  public   float Endtime=> endtime;

}