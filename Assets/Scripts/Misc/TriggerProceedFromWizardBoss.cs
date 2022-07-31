using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerProceedFromWizardBoss : MonoBehaviour
{
    private Boss1_Witch boss;
    public GameObject gunDrop;
    private bool didAction = false;

    [Header("Choose the text that this should display")]
    [SerializeField] private GameObject text;

    private void Start()
    {
        boss = transform.GetComponent<Boss1_Witch>();
    }

    // Update is called once per frame
    void Update()
    {
        if (boss.currHealth <= 0 && !didAction && PlayerHealth.curHealth > 0)
            StartCoroutine(Proceed());
    }

    private IEnumerator Proceed()
    {
        didAction = true;
        text.SetActive(true);
        Instantiate(gunDrop, transform.position, Quaternion.identity);

        yield return new WaitForSeconds(4);
        SceneManager.LoadScene("Castle Level");
    }
}
