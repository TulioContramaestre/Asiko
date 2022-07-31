using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHotBar : MonoBehaviour
{
    Sprite selected;
    Sprite unSelected;
    Sprite grapplingHookIcon;
    Sprite swordIcon;
    Sprite crossbowIcon;
    Sprite arrowIcon;
    Sprite flintlockIcon;

    GameObject playerGrappleHook;
    public static GameObject playerSword;
    GameObject playerCrossbow;
    GameObject playerFlintlock;

    [HideInInspector] public Image grappleIcon;

    [Header("Keep track of how many potions Player has")]
    public TextMeshProUGUI potionCountText;

    public static PlayerHotBar instance;

    public static PlayerHotBar GetInstance()
    {
        return instance;
    }

    void Start()
    {
        grappleIcon = GameObject.Find("/Canvas/Hotbar/GrappleHookIconContainer/GrappleHookIcon").GetComponent<Image>();

        if (instance != null)
        {
            //print("Hotbar Instance was not NULL. Destroy the current Hotbar in scene");
            Destroy(gameObject);
            return;
        }
        //print("Created a new Hotbar Instance!");
        instance = this;

        playerGrappleHook = GameObject.Find("/Player_Stickman/LeftHand/GunPivot");
        playerSword = GameObject.Find("/Player_Stickman/RightHand/Sword");
        playerCrossbow = GameObject.Find("/Player_Stickman/RightHand/BowPivot");
        playerFlintlock = GameObject.Find("/Player_Stickman/RightHand/GunPivot");
        //GameObject.Find("crossbow").SetActive(false);
        //GameObject.Find("flintlock").SetActive(false);
        sprite();
        highlightSlot();
        displayIcons();

        potionCountText.text = "x" + PlayerHealth.potionCount.ToString();

        UpdatePotionCount();

        //PlayerHealth.swordEquipped = false;
        //PlayerHealth.crossbowEquipped = false;
        //PlayerHealth.flintlockEquipped = false;
    }

    void sprite()
    {
        selected = Resources.Load<Sprite>("Sprite/hotbar icon 3");
        unSelected = Resources.Load<Sprite>("Sprite/hotbar icon 2");

        grapplingHookIcon = Resources.Load<Sprite>("HotBarIcons/grapplingHook");
        swordIcon = Resources.Load<Sprite>("HotBarIcons/weapons_03");
        crossbowIcon = Resources.Load<Sprite>("HotBarIcons/Crossbow1");
        arrowIcon = Resources.Load<Sprite>("HotBarIcons/Bolt Asset1");
        flintlockIcon = Resources.Load<Sprite>("HotBarIcons/Flintlock");
    }

    public void highlightSlot()
    {
        if (PlayerHealth.swordEquipped)
        {
            GameObject.Find("slot1").GetComponent<Image>().sprite = selected;
            GameObject.Find("slot2").GetComponent<Image>().sprite = unSelected;
            GameObject.Find("slot3").GetComponent<Image>().sprite = unSelected;
        }

        if (PlayerHealth.crossbowEquipped)
        {
            GameObject.Find("slot1").GetComponent<Image>().sprite = unSelected;
            GameObject.Find("slot2").GetComponent<Image>().sprite = selected;
            GameObject.Find("slot3").GetComponent<Image>().sprite = unSelected;

        }

        if (PlayerHealth.flintlockEquipped)
        {
            GameObject.Find("slot1").GetComponent<Image>().sprite = unSelected;
            GameObject.Find("slot2").GetComponent<Image>().sprite = unSelected;
            GameObject.Find("slot3").GetComponent<Image>().sprite = selected;
        }
    }

    // Display the icons of weapons and tools the Player has unlocked.
    public void displayIcons()
    {
        if (PlayerHealth.hasGrappleHook || PlayerInstance.GetInstance().debugHasGrappleHook)
        {
            print("Grapple Hook Icon ENABLED");
            print("PlayerHealth.hasGrappleHook is " + PlayerHealth.hasGrappleHook);
            print("PlayerInstance.GetInstance().debugHasGrappleHook is " + PlayerInstance.GetInstance().debugHasGrappleHook);
            //GameObject.Find("GrappleHookIconContainer/GrappleHookIcon").GetComponent<Image>().enabled = true;

            //if (!GameObject.Find("Player_Stickman/LeftHand/GunPivot").activeInHierarchy)
            //GameObject.Find("Player_Stickman/LeftHand/GunPivot").SetActive(true);
            playerGrappleHook.SetActive(true);
        }
        else
            GameObject.Find("GrappleHookIconContainer/GrappleHookIcon").GetComponent<Image>().enabled = false;

        if (PlayerHealth.hasSword || PlayerInstance.GetInstance().debugHasSword)
        {
            print("Sword Icon ENABLED");
            GameObject.Find("slot1/sword").GetComponent<Image>().enabled = true;
        }
        else
            GameObject.Find("slot1/sword").GetComponent<Image>().enabled = false;

        if (PlayerHealth.hasCrossbow || PlayerInstance.GetInstance().debugHasCrossbow)
        {
            GameObject.Find("CrossbowIconContainer/CrossbowIcon").GetComponent<Image>().enabled = true;
            for (int i = 1; i < 6; i++)
            {
                GameObject.Find("CrossbowIconContainer/Ammo/Arrow " + i).GetComponent<RawImage>().enabled = true;
            }
        }
        else
        {
            GameObject.Find("CrossbowIconContainer/CrossbowIcon").GetComponent<Image>().enabled = false;
            for (int i = 1; i < 6; i++)
            {
                GameObject.Find("CrossbowIconContainer/Ammo/Arrow " + i).GetComponent<RawImage>().enabled = false;
            }
        }

        if (PlayerHealth.hasFlintlock || PlayerInstance.GetInstance().debugHasFlintlock)
            GameObject.Find("FlintlockIconContainer/FlintlockIcon").GetComponent<Image>().enabled = true;
        else
            GameObject.Find("FlintlockIconContainer/FlintlockIcon").GetComponent<Image>().enabled = false;
    }

    public void UpdatePotionCount()
    {
        potionCountText.text = "x" + PlayerHealth.potionCount.ToString();

        if (PlayerHealth.potionCount == 0)
            GameObject.Find("potion").GetComponent<Image>().enabled = false;
        else
            GameObject.Find("potion").GetComponent<Image>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        displayIcons();

        if (PlayerHealth.curHealth > 0 && !PauseMenu.GamePaused)
        {
            if (!MeleeAttack.isSwinging)
            {
                if ((PlayerHealth.hasGrappleHook || PlayerInstance.GetInstance().debugHasGrappleHook))
                {
                    playerGrappleHook.SetActive(true);
                }

                if ((PlayerHealth.hasSword || PlayerInstance.GetInstance().debugHasSword) && !PlayerHealth.swordEquipped)
                {
                    if (Input.GetKeyUp(KeyCode.Alpha1))
                    {
                        AudioManager.GetInstance().Play("Draw Sword");
                        //GameObject.Find("slot3").GetComponent<Image>().sprite = unSelected;
                        //GameObject.Find("slot2").GetComponent<Image>().sprite = unSelected;
                        //GameObject.Find("slot1").GetComponent<Image>().sprite = selected;

                        playerSword.SetActive(true);
                        //playerSword.GetComponent<MeleeAttack>().enabled = true;
                        PlayerHealth.swordEquipped = true;

                        if (playerCrossbow.activeInHierarchy)
                        {
                            playerCrossbow.SetActive(false);
                            PlayerHealth.crossbowEquipped = false;
                        }

                        if (playerFlintlock.activeInHierarchy)
                        {
                            playerFlintlock.SetActive(false);
                            PlayerHealth.flintlockEquipped = false;
                        }

                        highlightSlot();
                    }
                }

                if ((PlayerHealth.hasCrossbow || PlayerInstance.GetInstance().debugHasCrossbow) && !PlayerHealth.crossbowEquipped)
                {
                    if (Input.GetKeyUp(KeyCode.Alpha2))
                    {
                        AudioManager.GetInstance().Play("Draw Crossbow");
                        //GameObject.Find("slot3").GetComponent<Image>().sprite = unSelected;
                        //GameObject.Find("slot1").GetComponent<Image>().sprite = unSelected;
                        //GameObject.Find("slot2").GetComponent<Image>().sprite = selected;

                        playerCrossbow.SetActive(true);
                        PlayerHealth.crossbowEquipped = true;

                        if (playerSword.activeInHierarchy)
                        {
                            playerSword.SetActive(false);
                            //playerSword.GetComponent<MeleeAttack>().enabled = false;
                            PlayerHealth.swordEquipped = false;
                        }

                        if (playerFlintlock.activeInHierarchy)
                        {
                            playerFlintlock.SetActive(false);
                            PlayerHealth.flintlockEquipped = false;
                        }

                        highlightSlot();
                    }
                }

                if ((PlayerHealth.hasFlintlock || PlayerInstance.GetInstance().debugHasFlintlock) && !PlayerHealth.flintlockEquipped)
                {
                    if (Input.GetKeyUp(KeyCode.Alpha3))
                    {
                        AudioManager.GetInstance().Play("Draw Flintlock");
                        //GameObject.Find("slot1").GetComponent<Image>().sprite = unSelected;
                        //GameObject.Find("slot2").GetComponent<Image>().sprite = unSelected;
                        //GameObject.Find("slot3").GetComponent<Image>().sprite = selected;

                        playerFlintlock.SetActive(true);
                        PlayerHealth.flintlockEquipped = true;

                        if (playerSword.activeInHierarchy)
                        {
                            playerSword.SetActive(false);
                            //playerSword.GetComponent<MeleeAttack>().enabled = false;
                            PlayerHealth.swordEquipped = false;
                        }

                        if (playerCrossbow.activeInHierarchy)
                        {
                            playerCrossbow.SetActive(false);
                            PlayerHealth.crossbowEquipped = false;
                        }

                        highlightSlot();
                    }
                }
            }

            if (PlayerHealth.potionCount > 0)
            {
                if (Input.GetKeyUp(KeyCode.Alpha4))
                {
                    AudioManager.GetInstance().Play("HealthPotionPickup");
                    AudioManager.GetInstance().Play("Heal");
                    PlayerHealth.curHealth = DataValues.playerHeal;
                    HealthBar.GetInstance().setHealth(PlayerHealth.curHealth);
                    PlayerHealth.potionCount--;

                    UpdatePotionCount();
                }
            }
        }
    }
}
