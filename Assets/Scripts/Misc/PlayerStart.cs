using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;


public class PlayerStart : MonoBehaviour
{
    Balance[] balanceScripts;
    PlayerController playerController;
    public GameObject healthBar;
    public GameObject hotBar;
    public CinematicBars cinematicBars;
    public PlayableDirector intro;

    private void Start()
    {
        cinematicBars.Show(100, 100.3f);
        intro.Play();
        healthBar.SetActive(false);
        hotBar.SetActive(false);

        intro.stopped += CutsceneIsFinished;
    }

    private void CutsceneIsFinished(PlayableDirector timeline)
    {
        cinematicBars.Hide(100.3f);

        // Enable player balance
        balanceScripts = FindObjectsOfType<Balance>();

        foreach (Balance b in balanceScripts)
        {
            b.enabled = true;
        }

        // Enable player movement
        playerController = FindObjectOfType<PlayerController>();
        playerController.enabled = true;

        // Enable health bar and hot bar
        healthBar.SetActive(true);
        hotBar.SetActive(true);

        // Wait two seconds and destroy this script.
        StartCoroutine(Destroy());
    }

        IEnumerator Destroy()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject.GetComponent<PlayerStart>());
    }
}