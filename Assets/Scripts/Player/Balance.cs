using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balance : MonoBehaviour
{
    [Range(0f, 100f)] public float restingAngle = 0f;
    public float force = 1f;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.MoveRotation(Mathf.LerpAngle(rb.rotation, restingAngle, force * Time.deltaTime));
    }
}
