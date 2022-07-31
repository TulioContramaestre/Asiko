using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAI_Attack : MonoBehaviour
{
   public AudioManager audioManager;

   public GameObject projectile;
   public Transform firePos;

   [SerializeField] float shotForce = 10f;
   [SerializeField] float attacksPerSec = 0.5f;
   float nextAttackTime = 0f;

   GameObject player;
   RangedAI_Attack crossbow;

   void Start() {
      player = GameObject.Find("/Player_Stickman/Head");
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
   public IEnumerator Shoot() {
      nextAttackTime = Time.time + (1 / attacksPerSec);

      // Fire bolt at <angle>
      AudioManager.GetInstance().Play("Crossbow Fire");
      GameObject newBolt = Instantiate(projectile, firePos.position, transform.rotation);
      newBolt.GetComponent<Rigidbody2D>().velocity = transform.right * shotForce;
      yield return new WaitForSeconds(0.5f);
   }

   // Returns true if Crossbow can shoot
   public bool CanShoot() {
      return (Time.time >= nextAttackTime);
   }

   // Getter method
   public float GetShotForce() {
      return shotForce;
   }

   // Update is called once per frame
   void Update() {
      // Get direction of mouse
      Vector2 diff =  player.transform.position - transform.position;
      diff.Normalize();

      // Check if need to flip weapon
      flipWeapon(diff);
   }
}
