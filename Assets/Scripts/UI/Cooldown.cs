using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Cooldown : MonoBehaviour
{
    [Header("Grapple Hook")]
    public Image grappleHookIconOverlay;
    public TextMeshProUGUI cooldownGrappleTimerText;
    public float cooldownGrappleHook;
    float tempGrappleCooldown;
    public static bool isGrappleCooldown = false;
    public KeyCode grappleShot;

    [Header("Crossbow")]
    public Image crossbowIconOverlay;
    public TextMeshProUGUI cooldownCrossbowTimerText;
    public float cooldownCrossbow;
    float tempCrossbowCooldown;
    public GameObject[] ammo;
    int crossbowShotCount = 0; // This is used to keep track how many times the Player has shot the crossbow.
    public static bool isCrossbowCooldown = false;
    bool needCrossbowReload;
    public KeyCode crossbowShot;
    public KeyCode reload;
    float nextAttackTimeCrossbow = 0f;

    [Header("Flintlock")]
    public Image flintlockIconOverlay;
    public TextMeshProUGUI cooldownFlintlockTimerText;
    public float cooldownFlintlock;
    float tempFlintlockCooldown;
    public static bool isFlintlockCooldown = false;
    public KeyCode flintlockShot;

    // Start is called before the first frame update
    void Start()
    {
        grappleHookIconOverlay.fillAmount = 0;
        crossbowIconOverlay.fillAmount = 0;
        flintlockIconOverlay.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerHealth.curHealth > 0)
        {
            GrappleHook();

            if (PlayerHealth.crossbowEquipped)
                CrossbowShoot();

            if (PlayerHealth.flintlockEquipped)
                FlintlockShoot();
        }
    }

    void GrappleHook()
    {
        if (Tutorial_GrapplingGun.didGrapple && !PauseMenu.GamePaused)
        {
            if (Input.GetKeyDown(grappleShot) && !isGrappleCooldown)
            {
                grappleHookIconOverlay.fillAmount = 1;
            }

            // Once the player releases the grapple rope, start the cooldown timer.
            if (Input.GetKeyUp(grappleShot) && !isGrappleCooldown)
            {
                tempGrappleCooldown = cooldownGrappleHook;
                isGrappleCooldown = true;
            }
        }

        if (isGrappleCooldown)
        {
            grappleHookIconOverlay.fillAmount -= 1 / cooldownGrappleHook * Time.deltaTime;
            cooldownGrappleTimerText.text = Mathf.RoundToInt(tempGrappleCooldown -= Time.deltaTime).ToString();
            if (grappleHookIconOverlay.fillAmount <= 0)
            {
                cooldownGrappleTimerText.text = "";
                grappleHookIconOverlay.fillAmount = 0;
                isGrappleCooldown = false;
            }
        }
    }

    void CrossbowShoot()
    {
        if (Input.GetKeyDown(crossbowShot) && crossbowShotCount < 5 && !isCrossbowCooldown && Time.time > nextAttackTimeCrossbow && !PauseMenu.GamePaused)
        {
            crossbowShotCount++;
            nextAttackTimeCrossbow = Time.time + (1 / Crossbow.GetInstance().attacksPerSec);

            //print("Crossbow shot count = " + crossbowShotCount + " (within Cooldown script).");
            switch (crossbowShotCount)
            {
                case 1:
                    ammo[4].SetActive(false);
                    break;
                case 2:
                    ammo[3].SetActive(false);
                    break;
                case 3:
                    ammo[2].SetActive(false);
                    break;
                case 4:
                    ammo[1].SetActive(false);
                    break;
                case 5:
                    ammo[0].SetActive(false);
                    needCrossbowReload = true;
                    break;
                default:
                    print("CrossbowShotCount not properly working");
                    break;
            }

            if (needCrossbowReload)
            {
                crossbowIconOverlay.fillAmount = 1;
                tempCrossbowCooldown = cooldownCrossbow;
                isCrossbowCooldown = true;
            }
        }

        // The player presses R to reload manually.
        if (Input.GetKeyDown(reload) && crossbowShotCount > 0 && !isCrossbowCooldown && !PauseMenu.GamePaused)
        {
            crossbowIconOverlay.fillAmount = 1;
            tempCrossbowCooldown = cooldownCrossbow;
            isCrossbowCooldown = true;

            foreach (GameObject arrow in ammo)
            {
                arrow.SetActive(false);
            }
        }
            

        if (isCrossbowCooldown)
        {
            crossbowIconOverlay.fillAmount -= 1 / cooldownCrossbow * Time.deltaTime;
            cooldownCrossbowTimerText.text = Mathf.RoundToInt(tempCrossbowCooldown -= Time.deltaTime).ToString();

            // Reset variables to a fresh crossbow
            if (crossbowIconOverlay.fillAmount <= 0)
            {
                //print("Crossbow reloaded");
                crossbowShotCount = 0;
                cooldownCrossbowTimerText.text = "";
                crossbowIconOverlay.fillAmount = 0;
                isCrossbowCooldown = false;
                needCrossbowReload = false;

                foreach (GameObject arrow in ammo)
                {
                    arrow.SetActive(true);
                }
            }
        }
    }

    void FlintlockShoot()
    {
        if (Input.GetKeyDown(flintlockShot) && !isFlintlockCooldown && !PauseMenu.GamePaused)
        {
            flintlockIconOverlay.fillAmount = 1;
        }

        // Once the player shoots, start the cooldown timer.
        if (!isFlintlockCooldown)
        {
            tempFlintlockCooldown = cooldownFlintlock;
            isFlintlockCooldown = true;
        }

        if (isFlintlockCooldown)
        {
            flintlockIconOverlay.fillAmount -= 1 / cooldownFlintlock * Time.deltaTime;
            cooldownFlintlockTimerText.text = Mathf.RoundToInt(tempFlintlockCooldown -= Time.deltaTime).ToString();
            if (flintlockIconOverlay.fillAmount <= 0)
            {
                cooldownFlintlockTimerText.text = "";
                flintlockIconOverlay.fillAmount = 0;
                isFlintlockCooldown = false;
            }
        }
    }
}
