using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    Sprite selected;

    [Header("New Game")]
    [SerializeField] private string start;

    [Header("Dark Forest")]
    [SerializeField] private string forest_start;
    [SerializeField] private string Dark_Wizard;

    [Header("Castle")]
    [SerializeField] private string castle_start;

    [SerializeField] private string general_Crain;

    void Start()
    {
        selected = Resources.Load<Sprite>("Sprite/hotbar icon 3");
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(start);
    }

    public void LevelForestStart()
    {
        SceneManager.LoadScene(forest_start);
        PlayerHealth.hasSword = true;
        PlayerHealth.hasGrappleHook = true;
        PlayerHealth.hasCrossbow = false;
        PlayerHealth.hasFlintlock = false;

        //if (PlayerHotBar.playerSword.activeInHierarchy)
        //    PlayerHotBar.playerSword.SetActive(true);

        //GameObject.Find("slot1").GetComponent<Image>().sprite = selected;
    }

    public void LevelForestBoss()
    {
        SceneManager.LoadScene(Dark_Wizard);
        PlayerHealth.hasSword = true;
        PlayerHealth.hasGrappleHook = true;
        PlayerHealth.hasCrossbow = true;
        PlayerHealth.hasFlintlock = false;
        PlayerHealth.potionCount = 2;

        //if (PlayerHotBar.playerSword.activeInHierarchy)
        //    PlayerHotBar.playerSword.SetActive(true);

        //GameObject.Find("slot1").GetComponent<Image>().sprite = selected;
    }

    public void LevelDarkCastleStart()
    {
        SceneManager.LoadScene(castle_start);
        PlayerHealth.hasSword = true;
        PlayerHealth.hasGrappleHook = true;
        PlayerHealth.hasCrossbow = true;
        PlayerHealth.hasFlintlock = true;
        PlayerHealth.potionCount = 1;

        //if (PlayerHotBar.playerSword.activeInHierarchy)
        //    PlayerHotBar.playerSword.SetActive(true);

        //GameObject.Find("slot1").GetComponent<Image>().sprite = selected;
    }

    public void LevelCastleBoss()
    {
        SceneManager.LoadScene(general_Crain);
        PlayerHealth.hasSword = true;
        PlayerHealth.hasGrappleHook = true;
        PlayerHealth.hasCrossbow = true;
        PlayerHealth.hasFlintlock = true;
        PlayerHealth.potionCount = 2;

        //if (PlayerHotBar.playerSword.activeInHierarchy)
        //    PlayerHotBar.playerSword.SetActive(true);

        //GameObject.Find("slot1").GetComponent<Image>().sprite = selected;
    }


    public void QuitGame()
    {
        Debug.Log("It has Quit");
        Application.Quit();
    }
}
