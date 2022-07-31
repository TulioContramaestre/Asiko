using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPlayer : MonoBehaviour
{
    private PlayerController playerController;
    private GameObject grappleHook;
    private GameObject sword;
    private GameObject crossbow;
    private GameObject flintlock;

    void Start()
    {
        if (GameObject.Find("Player_Stickman") != null)
        {
            PlayerInstance.GetInstance().spawnNumber = 0;

            playerController = GameObject.Find("Player_Stickman/Hips").GetComponent<PlayerController>();
            grappleHook = GameObject.Find("Player_Stickman/LeftHand/GunPivot");
            sword = GameObject.Find("Player_Stickman/RightHand/Sword");
            crossbow = GameObject.Find("Player_Stickman/RightHand/BowPivot");
            flintlock = GameObject.Find("Player_Stickman/RightHand/GunPivot");

            // Reset Player as if they never played before.
            PlayerHealth.curHealth = 100;

            PlayerInstance.GetInstance().spawnNumber = 0;

            PlayerHealth.hasSword = false;
            PlayerHealth.hasGrappleHook = false;
            PlayerHealth.hasCrossbow = false;
            PlayerHealth.hasFlintlock = false;

            PlayerHealth.swordEquipped = false;
            PlayerHealth.grappleHookEquipped = false;
            PlayerHealth.crossbowEquipped = false;
            PlayerHealth.flintlockEquipped = false;

            PlayerInstance.GetInstance().debugHasGrappleHook = false;
            PlayerInstance.GetInstance().debugHasSword = false;
            PlayerInstance.GetInstance().debugHasCrossbow = false;
            PlayerInstance.GetInstance().debugHasFlintlock = false;

            // Ensure player isn't swinging their sword when they go to Main Menu.
            MeleeAttack.isSwinging = false;
            sword.GetComponent<TrailRenderer>().enabled = false;
            MeleeAttack.swung = false;

            // Ensure Player doesn't stay grappled to something when they go to Main Menu.
            //Tutorial_GrapplingGun.GetInstance().targetDistance = 1.3f;
            //Tutorial_GrapplingGun.GetInstance().grappleRope.enabled = false;
            //Tutorial_GrapplingGun.GetInstance().m_springJoint2D.enabled = false;
            //Tutorial_GrapplingGun.GetInstance().m_rigidbody.gravityScale = 1;

            //if (!playerController.enabled)
            //{
            playerController.enabled = true;
            //}

            if (grappleHook.activeInHierarchy)
            {
                grappleHook.SetActive(false);
            }

            if (sword.activeInHierarchy)
            {
                sword.SetActive(false);
            }

            if (crossbow.activeInHierarchy)
            {
                crossbow.SetActive(false);
            }

            if (flintlock.activeInHierarchy)
            {
                flintlock.SetActive(false);
            }

            PlayerHealth.potionCount = 0;
        }
    }
}
