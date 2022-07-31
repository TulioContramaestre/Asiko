using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerProceed : MonoBehaviour
{
    [Header("Choose the enemy that must die for the player to proceed")]
    [SerializeField] private EnemyHealth enemy;

    // Update is called once per frame
    void Update()
    {
        if (enemy.curHealth <= 0)
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }
}
