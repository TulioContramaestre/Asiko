using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SkipCutscene : MonoBehaviour
{
    private PlayableDirector timeline;
    private float timeSpan = 0.5f; // The time it takes for the cutscene to be skipped.
    private float time;
    [Header("Check this to instantly skip a cutscene")]
    [SerializeField] private bool instaSkip = false;

    private void Start()
    {
        timeline = gameObject.GetComponent<PlayableDirector>();
    }

    private void Update()
    {
        if (instaSkip)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                timeline.time = (float)timeline.duration - 0.1f;
                enabled = false;
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.Space))
            {
                time += Time.deltaTime;

                // If the player holds down space long enough, skip the cutscene.
                if (time > timeSpan)
                {
                    timeline.time = (float)timeline.duration - 0.1f;
                    enabled = false;
                }
            }
        }

        // If the player releases the Space button then reset the timer.
        if (Input.GetKeyUp(KeyCode.Space))
            time = 0;
    }
}
