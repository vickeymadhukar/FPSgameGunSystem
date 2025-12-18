using UnityEngine;


public class IKControl : MonoBehaviour
  {

        public Animator _animator;

      [Header("Gun References")]
       public GameObject gun1;
       public GameObject gun2;



        public bool IkActive = true;
        public Transform RightHandTarget;
        public Transform LeftHandTarget;

        void Start()
        {
            
        }
    private void Update()
    {
        GameObject activeGun = null;

        if (gun1.activeInHierarchy)
            activeGun = gun1;
        else if (gun2.activeInHierarchy)
            activeGun = gun2;

        if (activeGun != null)
        {
            RightHandTarget = FindChildWithTag(activeGun.transform, "righthandgrip");
            LeftHandTarget = FindChildWithTag(activeGun.transform, "lefthandgrip"); ;
        }
    
    }

    void OnAnimatorIK(int layerIndex)
        {
            if (_animator)
            {

                float v = IkActive ? 1 : 0;

                _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, v);
                _animator.SetIKRotationWeight(AvatarIKGoal.RightHand, v);
                _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, v);
                _animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, v);

                if (IkActive)
                {
                    // Set the right hand target position and rotation, if one has been assigned
                    _animator.SetIKPosition(AvatarIKGoal.RightHand, RightHandTarget.position);
                    _animator.SetIKRotation(AvatarIKGoal.RightHand, RightHandTarget.rotation);
                    _animator.SetIKPosition(AvatarIKGoal.LeftHand, LeftHandTarget.position);
                    _animator.SetIKRotation(AvatarIKGoal.LeftHand, LeftHandTarget.rotation);
                }
            }
        }

    Transform FindChildWithTag(Transform parent, string tag)
    {
        foreach (Transform child in parent.GetComponentsInChildren<Transform>())
        {
            if (child.CompareTag(tag))
                return child;
        }
        return null;
    }

}
