using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthPickup : MonoBehaviour
{
    [Header("Choose the text that this should display")]
    [SerializeField] private GameObject text;


    private void OnTriggerStay2D(Collider2D collision)
    {
        // If it's the player and they're alive
        if (collision.gameObject.CompareTag("PlayerHead") && PlayerHealth.curHealth > 0)
        {
            text.SetActive(true);

            if (Input.GetKey(KeyCode.F))
            {
                AudioManager.GetInstance().Play("HealthPotionPickup");
                PlayerHealth.potionCount++;
                PlayerHotBar.GetInstance().UpdatePotionCount();
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            text.SetActive(false);
        }
    }
}
