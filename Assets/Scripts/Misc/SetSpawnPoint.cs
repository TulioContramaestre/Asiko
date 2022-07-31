using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSpawnPoint : MonoBehaviour
{
    [SerializeField] private GameObject checkpointPopup;
    [Header("For example a spawn number of 1 would set the spawn to SpawnPoint1")]
    [Header("Choose which Spawn point to set spawn to.")]
    [SerializeField] private int spawnNumber;

    private void Start()
    {
        // When a scene is loaded, snap the camera immediately to the player. And then
        // give regular interpolation.
        StartCoroutine(SnapCamera());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If it's the player and they're alive
        if (collision.gameObject.layer == 6 && PlayerHealth.curHealth > 0 && PlayerInstance.GetInstance().spawnNumber != spawnNumber)
        {
            PlayerInstance.GetInstance().spawnNumber = spawnNumber;
            AudioManager.GetInstance().Play("Checkpoint");
            PlayerInstance.GetInstance().tempPotionCount = PlayerHealth.potionCount;
            StartCoroutine(DisplayPopup());
        }
    }

    private IEnumerator DisplayPopup()
    {
        checkpointPopup.SetActive(true);
        yield return new WaitForSeconds(3);
        checkpointPopup.SetActive(false);
    }

    private IEnumerator SnapCamera()
    {
        CameraFollow.GetInstance().interpolation = 1f;
        yield return new WaitForSeconds(0.1f);
        CameraFollow.GetInstance().interpolation = 0.082f;
    }
}
