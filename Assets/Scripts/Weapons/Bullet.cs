using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

   [SerializeField] LayerMask bulletLayers;
   Vector3 shotDir;
   float shotForce;

   public GameObject bloodEffect;

   private bool shot; // This is purely so attacks only register once

   public void SetShotDirectionAndForce(Vector3 shotDir, float shotForce) {
      this.shotDir = shotDir;
      this.shotForce = shotForce;
   }

   // Start is called before the first frame update
   void Start() {
      Destroy(this.gameObject, 10f);
      shot = true;
    }

   // Update is called every frame
   void Update() {
      // Find distance that bullet would move in 1 frame
      float dist = shotForce * Time.deltaTime;

      // Send out Raycast to see if bullet hit something
      RaycastHit2D hit = Physics2D.Raycast(transform.position, (Vector2)shotDir, dist, bulletLayers);
      if (hit.collider != null)
         CheckBulletCollider(hit.collider);

      // Move bullet along its path
      transform.position += shotDir * dist;
   }

   // Checks various LayerMasks to determine what to do with bullet collision
   void CheckBulletCollider(Collider2D collider) {
      GameObject entered = collider.gameObject;

      // If Player hit average enemy
      if (entered.layer == LayerMask.NameToLayer("Enemy") && shot) {
         // Check for EnemyHealth component
         if (entered.GetComponentInParent<EnemyHealth>() != null) {
            AudioManager.GetInstance().Play("Sword Hit Metal");
            shot = true;

            // Check to see if Enemy that Player hit is still alive
            if (entered.GetComponentInParent<EnemyHealth>().curHealth > 0) {
               Instantiate(bloodEffect, entered.transform.position, Quaternion.identity);
               entered.GetComponentInParent<EnemyHealth>().takeDamage(DataValues.playerBulletDamage);

               // Check to see if taking that damage killed enemy
               if (entered.GetComponentInParent<EnemyHealth>().curHealth <= 0) {
                  entered.GetComponentInParent<HingeJoint2D>().enabled = false;
                  entered.GetComponentInParent<ParticleSystem>().Play();
                  Instantiate(bloodEffect, entered.transform.position, Quaternion.identity);
               }
            }
         }
         // Check for Boss1_Witch component
         else if (entered.GetComponentInParent<Boss1_Witch>() != null) {
            if (entered.GetComponentInParent<Boss1_Witch>().currHealth > 0 && shot) {
               AudioManager.GetInstance().Play("Sword Hit Metal");
               shot = false;

               Instantiate(bloodEffect, entered.transform.position, Quaternion.identity);
               entered.GetComponentInParent<Boss1_Witch>().takeDamage(DataValues.playerBulletDamage);
            }
         }
        // Check for Boss3_Final component
        else if (entered.GetComponentInParent<Boss3_Final>() != null)
        {
            if (entered.GetComponentInParent<Boss3_Final>().currHealth > 0 && shot)
            {
                AudioManager.GetInstance().Play("Sword Hit Metal");
                shot = false;

                Instantiate(bloodEffect, entered.transform.position, Quaternion.identity);
                entered.GetComponentInParent<Boss3_Final>().takeDamage(DataValues.playerBulletDamage);
            }
        }
        }

      Destroy(this.gameObject);
   }



   // // Check if Bullet hit a collider
   // private void OnCollisionEnter2D(Collision2D collision) {
   //      // If what the Player hit is an average enemy.
   //      if (collision.gameObject.GetComponentInParent<EnemyHealth>() != null)
   //      {
   //          // Bullet hit Enemy
   //          if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy") && shot)
   //          {
   //              AudioManager.GetInstance().Play("Sword Hit Metal");
   //              shot = true;
   //
   //
   //              // If the enemy the player is hitting is alive
   //              if (collision.gameObject.GetComponentInParent<EnemyHealth>().curHealth > 0)
   //              {
   //                  //gameObject.GetComponent<ParticleSystem>().Play(); // Play the arrow bleed effect
   //                  Instantiate(bloodEffect, collision.gameObject.transform.position, Quaternion.identity);
   //                  collision.gameObject.GetComponentInParent<EnemyHealth>().takeDamage(DataValues.playerBulletDamage);
   //
   //                  // If the enemy the player is hitting gets killed
   //                  if (collision.gameObject.GetComponentInParent<EnemyHealth>().curHealth <= 0)
   //                  {
   //                      collision.gameObject.GetComponent<HingeJoint2D>().enabled = false;
   //                      collision.gameObject.GetComponent<ParticleSystem>().Play();
   //                      Instantiate(bloodEffect, collision.gameObject.transform.position, Quaternion.identity);
   //                  }
   //              }
   //              // If the enemy the player is hitting is dead, then still allow to dismember
   //              else
   //              {
   //                  collision.gameObject.GetComponent<HingeJoint2D>().enabled = false;
   //                  collision.gameObject.GetComponent<ParticleSystem>().Play();
   //                  Instantiate(bloodEffect, collision.gameObject.transform.position, Quaternion.identity);
   //              }
   //
   //          }
   //      }
   //      else if (collision.gameObject.GetComponentInParent<Boss1_Witch>() != null)
   //      {
   //          print("Hit witch");
   //          if (collision.gameObject.GetComponentInParent<Boss1_Witch>().currHealth > 0 && shot)
   //          {
   //              AudioManager.GetInstance().Play("Sword Hit Metal");
   //              shot = false;
   //
   //              Instantiate(bloodEffect, collision.gameObject.transform.position, Quaternion.identity);
   //              collision.gameObject.GetComponentInParent<Boss1_Witch>().takeDamage(DataValues.playerBulletDamage);
   //          }
   //      }
   //
   //      Destroy(this.gameObject);
   // }
}
