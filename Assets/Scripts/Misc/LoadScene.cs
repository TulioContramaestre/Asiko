using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [Header("Choose the scene that this should load")]
    [SerializeField] private string scene;

    void Start()
    {
        SceneManager.LoadScene(scene);
    }

}
