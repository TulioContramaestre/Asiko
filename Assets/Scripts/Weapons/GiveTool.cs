using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveTool : MonoBehaviour
{
    [Header("Choose the tools that this cutscene should give at the end")]
    [SerializeField] private bool giveGrappleHook;
    [SerializeField] private bool giveSword;
    [SerializeField] private bool giveCrossbow;
    [SerializeField] private bool giveFlintlock;

    // Start is called before the first frame update
    void Start()
    {
        if (giveGrappleHook)
            PlayerHealth.hasGrappleHook = true;

        if (giveSword)
            PlayerHealth.hasSword = true;

        if (giveCrossbow)
            PlayerHealth.hasCrossbow = true;

        if (giveFlintlock)
            PlayerHealth.hasFlintlock = true;
    }


}
