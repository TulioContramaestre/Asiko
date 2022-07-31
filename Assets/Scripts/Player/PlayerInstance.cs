using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;

public class PlayerInstance : MonoBehaviour
{
    // The following four variables are purely for debugging purposes. In the inspector, check
    // the boxes of whatever weapon you want the Player_Stickman to have.
    [Header("Select whether or not the Player has a tool (DEVELOPER TOOLS ONLY)")]
    public bool debugHasGrappleHook;
    public bool debugHasSword;
    public bool debugHasCrossbow;
    public bool debugHasFlintlock;

    [HideInInspector] public int tempPotionCount;
    // SpawnNumber dicates which spawn the player will spawn to. Ex: After reaching a checkpoint,
    // SpawnNumber should be set to 1, which will go to the second spawn point in the map.
    [HideInInspector] public int spawnNumber = 0;

    private Tutorial_GrapplingGun grappleHook;
    private SpringJoint2D springJoint;

    static PlayerInstance instance;
    //PlayerHealth playerHealth;

    public static PlayerInstance GetInstance()
    {
        return instance;
    }

    private SpriteRenderer grappleRenderer;

    //public static GameObject FindObject(GameObject parent, string name)
    //{
    //    Transform[] trs = parent.GetComponentsInChildren<Transform>(true);
    //    foreach (Transform t in trs)
    //    {
    //        if (t.name == name)
    //        {
    //            return t.gameObject;
    //        }
    //    }
    //    return null;
    //}

    void Start()
    {
        print("Does player have grapple shot? " + PlayerHealth.hasGrappleHook);
        print("Does player have sword " + PlayerHealth.hasSword);
        print("Does player have crossbow? " + PlayerHealth.hasCrossbow);
        print("Does player have flintlock? " + PlayerHealth.hasFlintlock);
        //print("Player instance start() ran");
        //playerHealth = FindObjectOfType<PlayerHealth>();
        // Create a temporary reference to the current scene.
        //Scene currentScene = SceneManager.GetActiveScene();
        //print("The last scene you were in was " + currentScene.name);

        grappleRenderer = GameObject.Find("/Player_Stickman/LeftHand/GunPivot/GrapplingGun").GetComponent<SpriteRenderer>();

        if (instance != null)
        {
            //print("Player Instance was not NULL. Destroy the current PlayerStickman in scene");
            Destroy(gameObject);
            return;
        }
        //print("Created a new Player Instance!");
        instance = this;

        //grappleHook = FindObject(instance.gameObject, "GrapplingGun").GetComponent<Tutorial_GrapplingGun>();
        //springJoint = FindObject(instance.gameObject, "LeftHand").GetComponent<SpringJoint2D>();

        //if (grappleHook == null)
        //    print("Cant find grapplehook");

        //if (springJoint == null)
        //    print("Cant find springjoint");

        // When a scene is loaded, then run the OnSceneLoaded function.
        SceneManager.sceneLoaded += OnSceneLoaded;

        GameObject spawner0 = GameObject.FindGameObjectWithTag("Spawn0");
        if (spawner0)
        {
            //print("WENT TO SPAWN POINT from PlayerInstance Start()");
            transform.position = spawner0.transform.position;
        }

        //print("Put this instance in a dont destroy on load");
        DontDestroyOnLoad(gameObject);
    }

    // Sets the player starting location to that of the Spawn Point.
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Create a temporary reference to the new scene.
        //Scene newScene = SceneManager.GetActiveScene();

        //string sceneName = newScene.name;
        //print("Scene name is " + sceneName);
        //print("The last scene you were in was " + currentScene); 
        // If we go back to the Main Menu then reset the character.
        //if (sceneName == "Menu")
        //{
        //    print("This is the main menu we're in");
        //    SceneManager.MoveGameObjectToScene(gameObject, currentScene);
        //}

        // Keep track of the amount of potions the player has when the scene loads.
        tempPotionCount = PlayerHealth.potionCount;
        //print("You had " + tempPotionCount + " when you loaded this scene");

        print("Does player have grapple shot? " + PlayerHealth.hasGrappleHook);
        print("Does player have sword " + PlayerHealth.hasSword);
        print("Does player have crossbow? " + PlayerHealth.hasCrossbow);
        print("Does player have flintlock? " + PlayerHealth.hasFlintlock);

        if (spawnNumber == 0)
        {
            GameObject spawner0 = GameObject.FindGameObjectWithTag("Spawn0");
            if (spawner0)
            {
                //print("WENT TO SPAWN POINT from PlayerInstance OnSceneLoaded()");
                // Set the Players spawn equal to the spawner's position and to that of the offset of the Player movements.
                Vector3 diff = spawner0.transform.position - GameObject.FindGameObjectWithTag("PlayerHead").transform.position;
                transform.position += diff;
            }
        }
        else if (spawnNumber == 1)
        {
            GameObject spawner1 = GameObject.FindGameObjectWithTag("Spawn1");
            if (spawner1)
            {
                //print("WENT TO SPAWN POINT from PlayerInstance OnSceneLoaded()");
                // Set the Players spawn equal to the spawner's position and to that of the offset of the Player movements.
                Vector3 diff = spawner1.transform.position - GameObject.FindGameObjectWithTag("PlayerHead").transform.position;
                transform.position += diff;
            }
        }
        else if (spawnNumber == 2)
        {
            GameObject spawner2 = GameObject.FindGameObjectWithTag("Spawn2");
            if (spawner2)
            {
                //print("WENT TO SPAWN POINT from PlayerInstance OnSceneLoaded()");
                // Set the Players spawn equal to the spawner's position and to that of the offset of the Player movements.
                Vector3 diff = spawner2.transform.position - GameObject.FindGameObjectWithTag("PlayerHead").transform.position;
                transform.position += diff;
            }
        }

        // Create a temporary reference to the current scene.
        // Used to see if to enable the lantern light.
        Scene currentScene = SceneManager.GetActiveScene();

        // Retrieve the name of this scene.
        string sceneName = currentScene.name;

        if (sceneName == "Dark_Forest" || sceneName == "DarkForestP2")
        {
            print("You're in dark forest");
            transform.GetComponentInChildren<LightFlicker>().enabled = true;
            transform.GetComponentInChildren<Light2D>().enabled = true;
            //transform.GetComponentInChildren<Tutorial_GrapplingGun>().enabled = true;
            PlayerHealth.hasGrappleHook = true;
            PlayerHealth.hasSword = true;
            PlayerHealth.hasCrossbow = true;
            PlayerHealth.hasFlintlock = false;
            // There is a bug where the grapple hook retractability skills won't
            // function unless you turn the grapple hook on and off again.
            // UPDATE: Actually if you initiate another grapple shot it will work
            // it's just the first grapple shot on a scene load that doesn't work.
            //StartCoroutine(ResetGrappleHook());
        }
        else
        {
            transform.GetComponentInChildren<LightFlicker>().enabled = false;
            transform.GetComponentInChildren<Light2D>().enabled = false;
        }

        if (sceneName == "Boss1_Testing")
        {
            PlayerHealth.hasGrappleHook = true;
            PlayerHealth.hasSword = true;
            PlayerHealth.hasCrossbow = true;
            PlayerHealth.hasFlintlock = false;

            //spawnNumber = 0;
        }

        if (sceneName == "Castle Level" || sceneName == "Boss3_Testing" || sceneName == "Credits")
        {
            PlayerHealth.hasGrappleHook = true;
            PlayerHealth.hasSword = true;
            PlayerHealth.hasCrossbow = true;
            PlayerHealth.hasFlintlock = true;
        }

        //if (sceneName != "Village" && sceneName != "Village_Home" && sceneName != "Menu")
        //{
        //    print("You're not in the Village scenes");
        //    grappleHook.enabled = true;
        //    springJoint.enabled = true;
        //}
        //else
        //{
        //    grappleHook.enabled = false;
        //    springJoint.enabled = false;
        //}

        //if (sceneName == "Boss3_Testing")
        //{
        //    spawnNumber = 0;
        //}
    }

    //private IEnumerator ResetGrappleHook()
    //{
    //    grappleHook = GameObject.Find("Player_Stickman/LeftHand/GunPivot/GrapplingGun").GetComponent<Tutorial_GrapplingGun>();
    //    grappleHook.enabled = false;
    //    yield return new WaitForSeconds(5);
    //    grappleHook.enabled = true;
    //}

    private void OnDestroy()
    {
        //print("Player Instance destroyed");
        spawnNumber = 0;
    }


    private void Update()
    {
        //    if (debugHasGrappleHook)
        //        GameObject.Find("Player_Stickman/LeftHand/GunPivot").SetActive(true);
        //    else if (!debugHasGrappleHook && !PlayerHealth.hasGrappleHook)
        //        GameObject.Find("Player_Stickman/LeftHand/GunPivot").SetActive(false);

        // Create a temporary reference to the current scene.
        // Used to see if to enable the lantern light.
        Scene currentScene = SceneManager.GetActiveScene();

        // Retrieve the name of this scene.
        string sceneName = currentScene.name;

        if (sceneName == "Village" || sceneName == "Village_Home" || sceneName == "Menu")
        {
            PlayerHotBar.GetInstance().grappleIcon.enabled = false;
            grappleRenderer.enabled = false;
        }
        else
        {
            PlayerHotBar.GetInstance().grappleIcon.enabled = true;
            grappleRenderer.enabled = true;
            transform.Find("LeftHand").Find("GunPivot").Find("GrapplingGun").GetComponent<Tutorial_GrapplingGun>().enabled = true;
        }

    }
}
