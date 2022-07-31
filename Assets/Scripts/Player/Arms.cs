using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arms : MonoBehaviour
{
    int speed = 50;
    public Rigidbody2D rb;
    public Camera cam;
    public KeyCode mouseButton;
    
    private void FixedUpdate()
    {
        Vector3 mousePos = new Vector3(cam.ScreenToWorldPoint(Input.mousePosition).x, cam.ScreenToWorldPoint(Input.mousePosition).y, 0);
        Vector3 difference = mousePos - transform.position;
        float rotationZ = Mathf.Atan2(difference.x, -difference.y) * Mathf.Rad2Deg - 90;

        // Comment this if statement out to make it so arms move to the cursor without an input
        //if (Input.GetKey(mouseButton))
        //{
            rb.MoveRotation(Mathf.LerpAngle(rb.rotation, rotationZ, speed * Time.deltaTime));
        //}
    }
}