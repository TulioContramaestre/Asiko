using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerNewScene : MonoBehaviour
{
    [Header("Choose the scene that this should load")]
    [SerializeField] private string scene;
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
                PlayerInstance.GetInstance().spawnNumber = 0;
                SceneManager.LoadScene(scene);
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
