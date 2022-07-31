using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth;
    [SerializeField] public static float curHealth;

    // The following will be variables that track what the Player had equipped when they died.
    // They are static because the Hotbar script will also use these to indicate what the player
    // has equipped.
    public static bool swordEquipped;
    public static bool crossbowEquipped;
    public static bool flintlockEquipped;
    public static bool grappleHookEquipped;

    // The following variables will keep track of what weapons and tools the player has
    // unlocked for us in displaying and functionality of the hotbar.
    public static bool hasGrappleHook;
    public static bool hasSword;
    public static bool hasCrossbow;
    public static bool hasFlintlock;

    // Keeps track of how many potions the Player has.
    public static int potionCount = 0;

    GameObject playerGrappleHook;
    GameObject playerSword;
    GameObject playerCrossbow;
    GameObject playerFlintlock;

    //private void Awake()
    //{
    //    print("PlayerHealth is awaked");
    //}

    // Start is called before the first frame update
    void Start()
    {
        playerGrappleHook = GameObject.Find("/Player_Stickman/LeftHand/GunPivot/GrapplingGun");
        playerSword = GameObject.Find("/Player_Stickman/RightHand/Sword");
        playerCrossbow = GameObject.Find("/Player_Stickman/RightHand/BowPivot");
        playerFlintlock = GameObject.Find("/Player_Stickman/RightHand/GunPivot");

        curHealth = maxHealth;

        //playerGrappleHook.SetActive(false);

        //Debug.Log("curHealth = " + curHealth + " maxHealth = " + maxHealth);

        //if (HealthBar.GetInstance() == null)
        //{
        //    Debug.Log("HealthBar.GetInstance() returned null");
        //    return;
        //}

        //HealthBar.GetInstance().setMaxHealth(100);
        //
        // Set the Health bar to visually display the red filling of the Health bar when loading into a scene for the first time
        if (HealthBar.GetInstance() != null)
            HealthBar.GetInstance().setHealth(curHealth);

        // On start, the weapon that is active is equipped.
        if (playerGrappleHook.activeSelf)
        {
            grappleHookEquipped = true;
            hasGrappleHook = true;
        }

        if (playerSword.activeSelf)
        {
            swordEquipped = true;
            hasSword = true;
        }

        if (playerCrossbow.activeSelf)
        {
            crossbowEquipped = true;
            hasCrossbow = true;
        }

        if (playerFlintlock.activeSelf)
        {
            flintlockEquipped = true;
            hasFlintlock = true;
        }
    }

    void Update()
    {
        // Check whether you want the Player to have certain weapons and tools.
        if (curHealth <= 0)
            death();
    }

    public void takeDamage(float damage)
    {
        curHealth -= damage;
        HealthBar.GetInstance().setHealth(curHealth);
    }

    public void death()
    {
        // If the Player died
        TriggerDeathCutscene.GetInstance().Play();

        foreach (Behaviour childCompnent in gameObject.GetComponents<Behaviour>())
            childCompnent.enabled = false;

        // Disable all body parts from balancing.
        transform.Find("LeftLeg").GetComponent<Balance>().enabled = false;
        transform.Find("LeftFoot").GetComponent<Balance>().enabled = false;
        transform.Find("RightLeg").GetComponent<Balance>().enabled = false;
        transform.Find("RightFoot").GetComponent<Balance>().enabled = false;
        transform.Find("Hips").GetComponent<Balance>().enabled = false;
        transform.Find("Torso").GetComponent<Balance>().enabled = false;
        transform.Find("Head").GetComponent<Balance>().enabled = false;

        // Disable all Player related scripts.
        transform.Find("Hips").GetComponent<PlayerController>().enabled = false;
        transform.Find("LeftHand").GetComponent<SpringJoint2D>().enabled = false;
        transform.Find("LeftHand").Find("GunPivot").Find("GrapplingGun").Find("FirePoint").Find("Rope").GetComponent<LineRenderer>().enabled = false;
        transform.Find("LeftHand").Find("GunPivot").Find("GrapplingGun").Find("FirePoint").Find("Rope").GetComponent<Tutorial_GrapplingRope>().enabled = false;

        // Disable all scripts that the player had active at the time of death. Store what they had equipped
        // for use in the Revive() method.
        if (GameObject.Find("/Player_Stickman/RightHand/Sword").activeSelf)
        {
            transform.Find("RightHand").Find("Sword").GetComponent<MeleeAttack>().enabled = false;
            swordEquipped = true;
            crossbowEquipped = false;
            flintlockEquipped = false;
        }

        if (GameObject.Find("/Player_Stickman/RightHand/BowPivot").activeSelf)
        {
            transform.Find("RightHand").Find("BowPivot").Find("Crossbow").GetComponent<Crossbow>().enabled = false;
            crossbowEquipped = true;
            flintlockEquipped = false;
            swordEquipped = false;
        }

        if (GameObject.Find("/Player_Stickman/RightHand/GunPivot").activeSelf)
        {
            transform.Find("RightHand").Find("GunPivot").Find("Flintlock").GetComponent<Flintlock>().enabled = false;
            flintlockEquipped = true;
            crossbowEquipped = false;
            swordEquipped = false;
        }

        if (transform.Find("LeftHand").Find("GunPivot").Find("GrapplingGun").GetComponent<Tutorial_GrapplingGun>().enabled)
        {
            transform.Find("LeftHand").Find("GunPivot").Find("GrapplingGun").GetComponent<Tutorial_GrapplingGun>().enabled = false;
            grappleHookEquipped = true;
        }
        
    }

    public void Revive()
    {
        // Give Player the same amount of potions that they started with when they loaded the scene.
        potionCount = PlayerInstance.GetInstance().tempPotionCount;

        curHealth = maxHealth;
        //print("curHealth after Revive = " + curHealth);
        foreach (Behaviour childCompnent in gameObject.GetComponents<Behaviour>())
            childCompnent.enabled = true;

        // Stop all the blood effects when player revives.
        foreach (ParticleSystem childComponent in gameObject.GetComponents<ParticleSystem>())
            childComponent.Stop();

        // Enable all body parts from balancing.
        transform.Find("LeftLeg").GetComponent<Balance>().enabled = true;
        transform.Find("LeftFoot").GetComponent<Balance>().enabled = true;
        transform.Find("RightLeg").GetComponent<Balance>().enabled = true;
        transform.Find("RightFoot").GetComponent<Balance>().enabled = true;
        transform.Find("Hips").GetComponent<Balance>().enabled = true;
        transform.Find("Torso").GetComponent<Balance>().enabled = true;
        transform.Find("Head").GetComponent<Balance>().enabled = true;

        // Reattach body parts.
        transform.Find("LeftHand").GetComponent<HingeJoint2D>().enabled = true;
        transform.Find("LeftArm").GetComponent<HingeJoint2D>().enabled = true;
        transform.Find("LeftLeg").GetComponent<HingeJoint2D>().enabled = true;
        transform.Find("LeftFoot").GetComponent<HingeJoint2D>().enabled = true;
        transform.Find("RightHand").GetComponent<HingeJoint2D>().enabled = true;
        transform.Find("RightArm").GetComponent<HingeJoint2D>().enabled = true;
        transform.Find("RightLeg").GetComponent<HingeJoint2D>().enabled = true;
        transform.Find("RightFoot").GetComponent<HingeJoint2D>().enabled = true;
        transform.Find("Hips").GetComponent<HingeJoint2D>().enabled = true;
        transform.Find("Torso").GetComponent<HingeJoint2D>().enabled = true;
        transform.Find("Head").GetComponent<HingeJoint2D>().enabled = true;

        // Enable all mandatory scripts.
        transform.Find("Hips").GetComponent<PlayerController>().enabled = true;
        transform.Find("LeftHand").GetComponent<SpringJoint2D>().enabled = false;

        // Enable all Player related scripts if they had it equipped.
        if (swordEquipped)
            transform.Find("RightHand").Find("Sword").GetComponent<MeleeAttack>().enabled = true;

        if (crossbowEquipped)
            transform.Find("RightHand").Find("BowPivot").Find("Crossbow").GetComponent<Crossbow>().enabled = true;

        if (flintlockEquipped)
            transform.Find("RightHand").Find("GunPivot").Find("Flintlock").GetComponent<Flintlock>().enabled = true;

        if (grappleHookEquipped)
        {
            transform.Find("LeftHand").Find("GunPivot").Find("GrapplingGun").GetComponent<Tutorial_GrapplingGun>().enabled = true;
        }
    }

    // Cheat code! Use this method to give the Player three health potions when clicked
    // within the Pause Menu.
    public void GivePotion()
    {
        potionCount += 3;
        PlayerHotBar.GetInstance().UpdatePotionCount();
    }
}