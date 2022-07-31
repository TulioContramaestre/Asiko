using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decapitation : MonoBehaviour
{
    public void Decapitate()
    {
      transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }
}
