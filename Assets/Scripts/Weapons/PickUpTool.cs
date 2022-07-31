using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpTool : MonoBehaviour
{
    private GameObject tool;
    [Header("Choose the text that this should display")]
    [SerializeField] private GameObject text;

    private void Start()
    {
        tool = GameObject.Find("Player_Stickman/RightHand/Sword");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // If it's the player and they're alive
        if (collision.gameObject.CompareTag("PlayerHead") && PlayerHealth.curHealth > 0)
        {
            text.SetActive(true);

            if (Input.GetKey(KeyCode.F))
            {
                tool.SetActive(true);
                PlayerHealth.swordEquipped = true;
                PlayerHealth.hasSword = true;
                GameObject.Find("Canvas/Hotbar").GetComponent<PlayerHotBar>().highlightSlot();
                GameObject.Find("Canvas/Hotbar").GetComponent<PlayerHotBar>().displayIcons();
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
