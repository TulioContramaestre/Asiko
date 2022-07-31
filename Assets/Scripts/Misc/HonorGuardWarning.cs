using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HonorGuardWarning : MonoBehaviour
{
    [Header("Choose the text that this should display")]
    [SerializeField] private GameObject text;

    [Header("Choose the enemy that must die for the player to proceed")]
    [SerializeField] private EnemyHealth enemy;

    private void OnTriggerStay2D(Collider2D collision)
    {
        // If it's the player and they're alive
        if (collision.gameObject.CompareTag("PlayerHead") && PlayerHealth.curHealth > 0)
        {
            text.SetActive(true);
            CheckEnemyIsDead();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            text.SetActive(false);
        }
    }

    // Update is called once per frame
    void CheckEnemyIsDead()
    {
        if (enemy.curHealth <= 0)
            Destroy(gameObject);    
    }
}
