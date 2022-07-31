using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyGameObject : MonoBehaviour
{
    [Header("Choose the amount in seconds you want the object to destroy in")]
    [SerializeField] float time;
    void Start()
    {
        Destroy(gameObject, time);
    }
}
