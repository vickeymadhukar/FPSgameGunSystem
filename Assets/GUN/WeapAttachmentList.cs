using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu()]
public class WeapAttachmentList : ScriptableObject
{
    public List<Attachment> weaponAttachmentPart;

    public List<Attachment> GetweaponAttachmentList(Attachment.WeaponPart part)
    {
        List<Attachment> returnweaponattachment = new List<Attachment>();

        foreach(Attachment attachment in weaponAttachmentPart)
        {
            if (attachment.part == part)
            {
                returnweaponattachment.Add(attachment);
            }
        }

        return returnweaponattachment;
    }



}

