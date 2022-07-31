using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerProceedFromCrainBoss : MonoBehaviour
{
    private Boss3_Final boss;

    private void Start()
    {
        boss = transform.GetComponent<Boss3_Final>();
    }

    // Update is called once per frame
    void Update()
    {
        if (boss.currHealth <= 0 && PlayerHealth.curHealth > 0)
            StartCoroutine(Proceed());
    }

    private IEnumerator Proceed()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("Credits");
    }
}
