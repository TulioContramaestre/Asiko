using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.SceneManagement;

public class TriggerCutscene : MonoBehaviour
{
    public CinematicBars cinematicBars;
    PlayerController playerController;
    Animator PlayerAnim;

    GameObject Player;
    public GameObject healthBar;
    public GameObject hotBar;
    public PlayableDirector timeline;
    [Header("If needed, state the music you wish to stop before playing the cutscene")]
    public string stopMusic;

    private void Awake()
    {
        Player = GameObject.Find("Player_Stickman");
        PlayerAnim = GameObject.Find("Player_Stickman").GetComponent<Animator>();

        // Find the binding named "PlayerAnim". This allows us to find the Player's animator
        // even though it is in a DontDestroyOnLoad() so our cutscene will continue to work
        // even if the scene is reloaded.
        var director = timeline.playableAsset as TimelineAsset;
        foreach (var track in director.GetOutputTracks())
        {
            if (track.name == "PlayerAnim")
            {
                timeline.SetGenericBinding(track, PlayerAnim);
                break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If you wish to stop music, then stop it.
        if (stopMusic != "")
        {
            AudioManager.GetInstance().StopPlaying(stopMusic);
        }

        // If it's the player and they're alive
        if (collision.gameObject.CompareTag("PlayerHead") && PlayerHealth.curHealth > 0)
        {
            //playerController = GameObject.Find("Player_Stickman/Hips").GetComponent<PlayerController>();

            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            timeline.Play();
            //playerController.movementForce = 0f;
            healthBar.SetActive(false);
            hotBar.SetActive(false);

            // If the Player can MeleeAttack then disable it
            try
            {
                Player.transform.Find("RightHand").Find("Sword").GetComponent<MeleeAttack>().enabled = false;
            }
                catch
            {
                Debug.Log("Couldn't find MeleeAttack script during cutscene.");
            }

            // If the Player can use the Crossbow then disable it
            try
            {
                Player.transform.Find("RightHand").Find("BowPivot").Find("Crossbow").GetComponent<Crossbow>().enabled = false;
            }
            catch
            {
                Debug.Log("Couldn't find a Crossbow script during cutscene.");
            }

            // If the Player can use the Flintlock then disable it
            try
            {
                Player.transform.Find("RightHand").Find("GunPivot").Find("Flintlock").GetComponent<Flintlock>().enabled = false;
            }
            catch
            {
                Debug.Log("Couldn't find a Crossbow script during cutscene.");
            }

            Player.transform.Find("Hips").GetComponent<PlayerController>().enabled = false;
            Player.transform.Find("LeftHand").GetComponent<SpringJoint2D>().enabled = false;
            Player.transform.Find("LeftHand").Find("GunPivot").Find("GrapplingGun").GetComponent<Tutorial_GrapplingGun>().enabled = false;
            Player.transform.Find("LeftHand").Find("GunPivot").Find("GrapplingGun").Find("FirePoint").Find("Rope").GetComponent<LineRenderer>().enabled = false;
            Player.transform.Find("LeftHand").Find("GunPivot").Find("GrapplingGun").Find("FirePoint").Find("Rope").GetComponent<Tutorial_GrapplingRope>().enabled = false;
            
            cinematicBars.Show(100, 100.3f);
            //StartCoroutine(FinishCutscene());
            timeline.stopped += CutsceneIsFinished;
        }
    }

    private void CutsceneIsFinished(PlayableDirector timeline)
    {
        try
        {
            Player.transform.Find("RightHand").Find("Sword").GetComponent<MeleeAttack>().enabled = true;
        }
        catch
        {
            Debug.Log("Couldn't find MeleeAttack script after the Cutscene.");
        }

        try
        {
            Player.transform.Find("RightHand").Find("BowPivot").Find("Crossbow").GetComponent<Crossbow>().enabled = true;
        }
        catch
        {
            Debug.Log("Couldn't find a Crossbow script after the Cutscene.");
        }

        try
        {
            Player.transform.Find("RightHand").Find("GunPivot").Find("Flintlock").GetComponent<Flintlock>().enabled = true;
        }
        catch
        {
            Debug.Log("Couldn't find a Crossbow script during cutscene.");
        }

        // Cutscene is finished, so resume the music if it was stopped.
        if (stopMusic != "")
        {
            AudioManager.GetInstance().Play(stopMusic);
        }

        healthBar.SetActive(true);
        hotBar.SetActive(true);

        Scene currentScene = SceneManager.GetActiveScene();

        // Retrieve the name of this scene.
        string sceneName = currentScene.name;

        if (sceneName == "Village" || sceneName == "Village_Home" || sceneName == "Menu")
        {
            Player.transform.Find("LeftHand").Find("GunPivot").Find("GrapplingGun").GetComponent<Tutorial_GrapplingGun>().enabled = false;
        }
        else
        {
            Player.transform.Find("LeftHand").Find("GunPivot").Find("GrapplingGun").GetComponent<Tutorial_GrapplingGun>().enabled = true;
        }

        Player.transform.Find("Hips").GetComponent<PlayerController>().enabled = true;

        cinematicBars.Hide(100.3f);
        timeline.Stop();
    }

    //IEnumerator FinishCutscene()
    //{
    //    yield return new WaitUntil(timeline.stopped);
    //    try
    //    {
    //        Player.transform.Find("RightHand").Find("Sword").GetComponent<MeleeAttack>().enabled = true;
    //    }
    //    catch
    //    {
    //        Debug.Log("Couldn't find MeleeAttack script after the Cutscene.");
    //    }

        //    try
        //    {
        //        Player.transform.Find("RightHand").Find("BowPivot").Find("Crossbow").GetComponent<Crossbow>().enabled = true;
        //    }
        //    catch
        //    {
        //        Debug.Log("Couldn't find a Crossbow script after the Cutscene.");
        //    }

        //    healthBar.SetActive(true);
        //    Player.transform.Find("Hips").GetComponent<PlayerController>().enabled = true;
        //    Player.transform.Find("LeftHand").Find("GunPivot").Find("GrapplingGun").GetComponent<Tutorial_GrapplingGun>().enabled = true;


        //    cinematicBars.Hide(100.3f);
        //    timeline.Stop();
        //}
    }