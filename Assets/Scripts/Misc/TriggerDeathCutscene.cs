using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TriggerDeathCutscene : MonoBehaviour
{
    static TriggerDeathCutscene instance;

    public static TriggerDeathCutscene GetInstance()
    {
        return instance;
    }

    void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public CinematicBars cinematicBars;
    public PlayableDirector timeline;
    public AudioManager audioManager;
    [SerializeField] private GameObject retryButton;

    [Range(-1, 1)] [SerializeField] private float decreasePitchAmount;

    public void Play()
    {
        timeline.Play();
        cinematicBars.Show(1000, 1000f);
        audioManager.Distort(decreasePitchAmount);
        //StartCoroutine(FinishCutscene());
        timeline.stopped += CutsceneIsFinished;
    }

    private void CutsceneIsFinished(PlayableDirector timeline)
    {
        retryButton.SetActive(true);
        timeline.Stop();
    }

    //IEnumerator FinishCutscene()
    //{
    //    yield return new WaitForSeconds(11);
    //    retryButton.SetActive(true);
    //    yield return new WaitForSeconds((float)timeline.duration);
    //    timeline.Stop();
    //}
}