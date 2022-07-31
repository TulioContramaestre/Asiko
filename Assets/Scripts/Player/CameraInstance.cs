using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraInstance : MonoBehaviour
{
    static CameraInstance instance;

    public static CameraInstance GetInstance()
    {
        return instance;
    }

    void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }
}
