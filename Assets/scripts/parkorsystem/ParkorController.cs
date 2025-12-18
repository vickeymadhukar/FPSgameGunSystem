using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkorController : MonoBehaviour
{
    Enviorment enviormentscacnner;
  public  Animator animator;
    private PlayerController playerinput;
    Movement movement;
    bool InAction;

    [Header("Assign Parkour Actions")]
    [SerializeField] private List<ParkorAction> parkoractions;

    private void Awake()
    {
        enviormentscacnner = GetComponent<Enviorment>();
      
        movement = GetComponent<Movement>();
        playerinput = new PlayerController();
    }

    private void OnEnable() => playerinput.Enable();
    private void OnDisable() => playerinput.Disable();

    void Update()
    {
        var hitdata = enviormentscacnner.ObscticalCheck();

        if (playerinput.Player.Jump.triggered && !InAction && hitdata.forwordhitfound)
        {
            // Loop through all parkour actions
            foreach (var action in parkoractions)
            {
                string animToPlay = action.GetAnimation(hitdata);
                if (!string.IsNullOrEmpty(animToPlay))
                {
                    StartCoroutine(Doparkor(animToPlay));
                    break; // play first valid animation
                }
            }
        }
    }

    IEnumerator Doparkor(string animName)
    {
        InAction = true;
        movement.setControll(false);
        movement.CanJump = false;
        movement.rig.weight = 0;
        animator.CrossFade(animName, 0.2f);

        // Wait for animation to finish
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        movement.CanJump = true;

        movement.rig.weight = 1;
        movement.setControll(true);
        InAction = false;
    }
}
