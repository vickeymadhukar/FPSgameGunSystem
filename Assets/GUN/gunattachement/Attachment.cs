using UnityEngine;

[CreateAssetMenu()]
public class Attachment : ScriptableObject
{
    public enum WeaponPart { 
      Barrel,
      Stock,
      Scope,
      Mag,
      Grip,
      ForwordGrip,
    }

    public WeaponPart part;
    public Transform prefabs;
         
}
