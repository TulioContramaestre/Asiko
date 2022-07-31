using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAmbush : MonoBehaviour
{
    public GameObject[] enemies;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If it's the player and they're alive
        if (collision.gameObject.layer == 6 && PlayerHealth.curHealth > 0)
        {
            foreach (GameObject e in enemies)
            {
                e.SetActive(true);
            }
        }
    }
}
