using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    PlayerHealth playerHealth;
     
    public void ResetTheGame()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
        StartCoroutine(PlaySound());
    }

    IEnumerator PlaySound()
    {
        // Play the sound only once even if the Player clicks rapidly
        if (!AudioManager.GetInstance().IsPlaying("Retry"))
        {
            AudioManager.GetInstance().Play("Retry");
        }
        yield return new WaitForSeconds(1.5f);
        playerHealth.Revive();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
