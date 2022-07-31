using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.SceneManagement;

public class CameraFollow : MonoBehaviour
{
    // Might not need this since we are using Cinemachine now. But currently Cinemachine is too jittery and bad.
    [Header("Camera Follow")]
    [Range(0f, 1f)] public float interpolation = 0.082f;
    public Vector3 offset = new Vector3(0f, 15f, -10f);

    public static CameraFollow instance;

    public static CameraFollow GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        //SceneManager.sceneLoaded += OnSceneLoaded;

        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    //public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    //{
    //    // Create a temporary reference to the current scene.
    //    // Used to disable camera script when in Main Menu.
    //    Scene currentScene = SceneManager.GetActiveScene();

    //    // Retrieve the name of this scene.
    //    string sceneName = currentScene.name;

    //    if (sceneName == "Menu")
    //    {
    //        enabled = false;
    //    }
    //    else
    //    {
    //        enabled = true;
    //    }
    //}

    // Update is called once per frame
        void FixedUpdate()
    {
        CameraFollowFunction();
    }

    private void CameraFollowFunction()
    {
        CameraInstance.GetInstance().transform.position = Vector3.Lerp(CameraInstance.GetInstance().transform.position, transform.position + offset, interpolation);
    }
}
