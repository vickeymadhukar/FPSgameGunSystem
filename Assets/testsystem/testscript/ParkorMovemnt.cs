using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ParkorMovemnt : MonoBehaviour
{
    EnvirnmentScanner scanner;

    Animator animator;
    bool isAction;
    PlayerControllerr playermovement;

    [SerializeField] List<Parkor> parkoractions;
    
    private void Awake()
    {
        scanner = GetComponent<EnvirnmentScanner>();
        playermovement = GetComponent<PlayerControllerr>();
        animator = GetComponent<Animator>();

    }

    void Update()
    {
        
        scanner.ObsticleCheck();
        if(playermovement.inputActions.Player.Jump.triggered && !isAction)
        {
            var hitData = scanner.ObsticleCheck();
            if (hitData.forwordHitfound)
            {
                foreach(var action in parkoractions)
                {
                    if (action.ObsticleCheck(hitData, transform))
                    {
                        Debug.Log($"[Parkour Action] Matched {action.AnimaName} | Height = {action.height}");
                        StartCoroutine(DoParkorAction(action));
                        break;
                    }
                    else
                    {
                        Debug.Log($"[Parkour Failed] {action.AnimaName} | Height = {action.height}");
                    }
                }
            }
        }

    }

   IEnumerator DoParkorAction(Parkor action)
    {
        isAction = true;
        playermovement.setcontroll(false);
        animator.CrossFade(action.AnimaName, 0.2f);
        yield return null;
        var animstate= animator.GetNextAnimatorStateInfo(0);
        if (!animstate.IsName(action.AnimaName))
        {
            Debug.Log("wrong animation name");
        }


        float timer = 0f;
        IEnumerator DoParkorAction(Parkor action)
        {
            isAction = true;
            playermovement.setcontroll(false);

            animator.CrossFade(action.AnimaName, 0.2f);
            yield return null;

            var animstate = animator.GetNextAnimatorStateInfo(0);
            if (!animstate.IsName(action.AnimaName))
            {
                Debug.Log("wrong animation name");
            }

            float timer = 0f;
            while (timer < animstate.length)
            {
                if (action.EnableTargetMatching)
                {
                    Matchtarget(action);
                }

                timer += Time.deltaTime;   // ✅ advance timer
                yield return null;         // ✅ wait till next frame
            }

            playermovement.setcontroll(true);
            isAction = false;
        }


        playermovement.setcontroll(true);
        isAction = false;

    }

    void Matchtarget(Parkor action)
    {
        if (animator.isMatchingTarget) return;

        animator.MatchTarget(action.matchpos, transform.rotation, action.MacthBodypart, new MatchTargetWeightMask(new Vector3(0, 1, 0), 0), action.Starttime, action.Endtime);
    }


}
