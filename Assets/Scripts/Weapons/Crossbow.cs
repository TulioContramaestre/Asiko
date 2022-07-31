using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crossbow : MonoBehaviour {
   public static Crossbow instance;

   public static Crossbow GetInstance()
   {
       return instance;
   }

   public GameObject projectile;
   public Transform firePos;
   public Rigidbody2D rb;

   [SerializeField] float shotForce = 10f;
   public float attacksPerSec = 1f;
   [SerializeField] private int armSpeed = 10;

   float nextAttackTime = 0f;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    // Flip weapon based on mouse position
    void flipWeapon(Vector2 diff) {
      float rotz = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

      // Check if sprite needs to be flipped
      if (rotz < -90 || rotz > 90)
         transform.localRotation = Quaternion.Euler(180, 0, 0);
      else
         transform.localRotation = Quaternion.Euler(0, 0, 0);
   }

   // Shoot a bolt with <launchForce> in an arc
   void Shoot() {
      nextAttackTime = Time.time + (1 / attacksPerSec);
      AudioManager.GetInstance().Play("Crossbow Fire");
      GameObject newBolt = Instantiate(projectile, firePos.position, transform.rotation);
      newBolt.GetComponent<Rigidbody2D>().velocity = transform.right * shotForce;
   }

   // Update is called once per frame
   void Update() {
      // Shoot arrow on left click
      if (Input.GetMouseButtonDown(0) && Time.time > nextAttackTime && !Cooldown.isCrossbowCooldown && !PauseMenu.GamePaused)
         Shoot();
   }

    private void FixedUpdate()
    {
        // Point arm to the cursor
        Vector3 mousePos = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
        Vector3 difference = mousePos - transform.position;
        difference.Normalize();
        float rotationZ = Mathf.Atan2(difference.x, -difference.y) * Mathf.Rad2Deg - 90;

        rb.MoveRotation(Mathf.LerpAngle(rb.rotation, rotationZ, armSpeed * Time.deltaTime));

        // Get direction of mouse
        Vector2 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        diff.Normalize();

        // Check if need to flip weapon
        flipWeapon(diff);
    }
}
