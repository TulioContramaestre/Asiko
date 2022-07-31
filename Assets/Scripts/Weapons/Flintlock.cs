using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flintlock : MonoBehaviour {

   public GameObject bullet;
   public Transform firePos;
   public Rigidbody2D rb;

   [SerializeField] float shotForce = 75f;
   [SerializeField] float attacksPerSec = 0.5f;
   [SerializeField] float recoil = 10f;
   [SerializeField] int armSpeed = 10;

   float nextAttackTime = 0;

    // Flip weapon based on mouse position
    void flipWeapon(Vector2 diff) {
      float rotz = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

      // Check if sprite needs to be flipped
      if (rotz < -90 || rotz > 90)
         transform.localRotation = Quaternion.Euler(180, 0, 0);
      else
         transform.localRotation = Quaternion.Euler(0, 0, 0);
   }

   // Shoot a bullet with <shotForce> in a straight line
   void Shoot() {
      // Take care of extraneous operations
      nextAttackTime = Time.time + (1 / attacksPerSec);
      AudioManager.GetInstance().Play("Flintlock Fire");

      // Shoot bullet out of <firePos> with <shotForce>
      GameObject newBullet = Instantiate(bullet, firePos.position, transform.rotation);
      Vector3 mousePos = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
      Vector3 shotDir = (mousePos - firePos.position).normalized;
      newBullet.GetComponent<Bullet>().SetShotDirectionAndForce(shotDir, shotForce);

      // Add recoil to RightHand <rb>
      rb.AddForce(Vector2.up * recoil, ForceMode2D.Impulse);
   }

   // Update is called once per frame
   void Update() {
      // Shoot Flintlock on left click
      if (Input.GetMouseButtonDown(0) && Time.time > nextAttackTime && !Cooldown.isFlintlockCooldown && !PauseMenu.GamePaused)
         Shoot();
   }

   void FixedUpdate() {
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
