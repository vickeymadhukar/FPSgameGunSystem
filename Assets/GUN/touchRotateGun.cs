using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class touchRotateGun : MonoBehaviour
{
    // -------------------- ROTATION SETTINGS --------------------
    [Header("Rotation Settings")]
    [SerializeField] public float rotationSpeed = 0.2f;
    [SerializeField] public bool invertX = false;
    [SerializeField] public bool invertY = false;
    [SerializeField] public float smoothSpeed = 10f;

    [Header("Clamp Settings")]
    [SerializeField] private float minPitch = -40f;
    [SerializeField] private float maxPitch = 40f;
    [SerializeField] private bool clampYaw = false;
    [SerializeField] private float minYaw = -120f;
    [SerializeField] private float maxYaw = 120f;

    private Quaternion targetRotation;
    private Vector2 lastTouchPosition;
    private int activeFingerId = -1;

    private float pitch;
    private float yaw;

    // -------------------- ATTACHMENT UI --------------------
    [Header("UI")]
    [SerializeField] private Transform scopeSocket;
    [SerializeField] private Button scopeButton;

    // -------------------- ATTACHMENT SYSTEM --------------------
   
    [SerializeField]
    private List<AttachmentSocket> socketList;

    [SerializeField] private Attachment scopeVariant1;
    [SerializeField] private Attachment scopeVariant2;

    [System.Serializable]
    public struct AttachmentSocket
    {
        public Attachment.WeaponPart partType;
        public Transform socketTransform;
        public Attachment attachmnet;
        public Transform spawnedTransform;
    }
    private Dictionary<Attachment.WeaponPart, AttachmentSocket> attachmentSocketsMap;

    public WeapAttachmentList weapAttachmentList;



    // -------------------- UNITY METHODS --------------------
    private void Awake()
    {
        attachmentSocketsMap = new Dictionary<Attachment.WeaponPart, AttachmentSocket>();

        foreach (AttachmentSocket socket in socketList)
        {
            attachmentSocketsMap[socket.partType] = socket;//strong the postion of attachement part and which part like scope is to be position on scopeposiotion
        }
    }

    private void Start()
    {
        Vector3 eulerRotation = transform.rotation.eulerAngles;

        pitch = (eulerRotation.x > 180f) ? eulerRotation.x - 360f : eulerRotation.x;
        yaw = (eulerRotation.y > 180f) ? eulerRotation.y - 360f : eulerRotation.y;

        targetRotation = Quaternion.Euler(pitch, yaw, 0f);
        Load();
    }

    private void Update()
    {
        if (Touchscreen.current == null)
            return;

        foreach (var touch in Touchscreen.current.touches)
        {
            TouchPhase phase = touch.phase.ReadValue();
            Vector2 position = touch.position.ReadValue();
            int fingerId = touch.touchId.ReadValue();

            // Touch down
            if (phase == TouchPhase.Began && activeFingerId == -1)
            {
                activeFingerId = fingerId;
                lastTouchPosition = position;
            }

            // Moving finger
            if (fingerId == activeFingerId && phase == TouchPhase.Moved)
            {
                Vector2 delta = position - lastTouchPosition;
                lastTouchPosition = position;

                float dx = invertX ? -delta.x : delta.x;
                float dy = invertY ? -delta.y : delta.y;

                pitch += dy * rotationSpeed * Time.deltaTime;
                yaw += dx * rotationSpeed * Time.deltaTime;

                pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
                if (clampYaw)
                    yaw = Mathf.Clamp(yaw, minYaw, maxYaw);

                targetRotation = Quaternion.Euler(pitch, yaw, 0f);
            }

            // Touch end
            if (fingerId == activeFingerId &&
               (phase == TouchPhase.Ended || phase == TouchPhase.Canceled))
            {
                activeFingerId = -1;
            }
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * smoothSpeed);
    }

    // -------------------- PUBLIC ATTACHMENT BUTTONS --------------------
    public void EquipScope1()
    {
        SetAttachment(scopeVariant1);
    }

    public void EquipScope2()
    {
        SetAttachment(scopeVariant2);
    }

    // -------------------- ATTACHMENT CORE FUNCTION --------------------
    private void SetAttachment(Attachment attachment)
    {
       
        if (attachmentSocketsMap.ContainsKey(attachment.part) && attachmentSocketsMap[attachment.part].spawnedTransform!=null)
        {
            Destroy(attachmentSocketsMap[attachment.part].spawnedTransform.gameObject);
        }

     
        Transform newAttachment = Instantiate(attachment.prefabs);


        AttachmentSocket attachemntsocket = attachmentSocketsMap[attachment.part];

        attachemntsocket.spawnedTransform = newAttachment;

        Transform attachpointtransform = attachemntsocket.socketTransform; 

        newAttachment.parent = attachpointtransform;

       
         newAttachment.localPosition = Vector3.zero;
         newAttachment.localRotation = Quaternion.identity;
       //  newAttachment.localScale = new Vector3(0.2f, 0.2f, 0.2f);


        attachemntsocket.attachmnet = attachment;
        attachmentSocketsMap[attachment.part] = attachemntsocket;


    }

    public void changeScopeUsingButton()
    {
        AttachmentSocket attachmentSocket = attachmentSocketsMap[Attachment.WeaponPart.Scope];
        if (attachmentSocket.partType == null)
        {
            SetAttachment(weapAttachmentList.GetweaponAttachmentList(Attachment.WeaponPart.Scope)[0]);
        }
        else
        {
            List<Attachment> weaponstparts = weapAttachmentList.GetweaponAttachmentList(Attachment.WeaponPart.Scope);
            int partsindex = weaponstparts.IndexOf(attachmentSocket.attachmnet);
            int nextindex = (partsindex + 1) % weaponstparts.Count;

            SetAttachment(weaponstparts[nextindex]);

        }

    }


    public void changeBarrelUsingButton()
    {
        AttachmentSocket attachmentSocket = attachmentSocketsMap[Attachment.WeaponPart.Barrel];
        if (attachmentSocket.partType == null)
        {
            SetAttachment(weapAttachmentList.GetweaponAttachmentList(Attachment.WeaponPart.Barrel)[0]);
        }
        else
        {
            List<Attachment> weaponstparts = weapAttachmentList.GetweaponAttachmentList(Attachment.WeaponPart.Barrel);
            int partsindex = weaponstparts.IndexOf(attachmentSocket.attachmnet);
            int nextindex = (partsindex + 1) % weaponstparts.Count;

            SetAttachment(weaponstparts[nextindex]);

        }

    }



    public void changeGripUsingButton()
    {
        AttachmentSocket attachmentSocket = attachmentSocketsMap[Attachment.WeaponPart.ForwordGrip];
        if (attachmentSocket.partType == null)
        {
            SetAttachment(weapAttachmentList.GetweaponAttachmentList(Attachment.WeaponPart.ForwordGrip)[0]);
        }
        else
        {
            List<Attachment> weaponstparts = weapAttachmentList.GetweaponAttachmentList(Attachment.WeaponPart.ForwordGrip);
            int partsindex = weaponstparts.IndexOf(attachmentSocket.attachmnet);
            int nextindex = (partsindex + 1) % weaponstparts.Count;

            SetAttachment(weaponstparts[nextindex]);

        }

    }



    public void Save()
    {
        List<Attachment> weaponPart = new List<Attachment>();
        foreach(Attachment.WeaponPart parts in attachmentSocketsMap.Keys)
        {
            AttachmentSocket attachementpoint = attachmentSocketsMap[parts];

            if(attachementpoint.attachmnet != null)
            {
                weaponPart.Add(attachementpoint.attachmnet);
            }

        }


       

        SaveWeaponObject saveWeaponObject = new SaveWeaponObject()
        {
            
            attachemntpart = weaponPart,

        };

        string json = JsonUtility.ToJson(saveWeaponObject);
        Debug.Log(json);
        PlayerPrefs.SetString("SAVED_WEAPON", json);
        PlayerPrefs.Save();

    }


    public void Load()
    {
        if (!PlayerPrefs.HasKey("SAVED_WEAPON"))
        {
            Debug.Log("No saved weapon found");
            return;
        }

        string json = PlayerPrefs.GetString("SAVED_WEAPON");

        SaveWeaponObject saveWeaponObject =
            JsonUtility.FromJson<SaveWeaponObject>(json);

        foreach (Attachment attachment in saveWeaponObject.attachemntpart)
        {
            SetAttachment(attachment);
        }

        Debug.Log("Weapon Loaded");
    }


    [Serializable]
    private class SaveWeaponObject
    {
       
        public List<Attachment> attachemntpart;
    }


}
  