using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battleaxe : MonoBehaviour {
   public GameObject bloodEffect;
   [SerializeField] float invincibilityTime = 1f;
   float timeUntilVincible;
   float axeDamage;
   bool hit = false;

   void Start() {
      axeDamage = this.GetComponentInParent<Boss3_Final>().chargeDamage;
   }

   // Update is called once per frame
   void Update() {
      if (hit == true && Time.time >= timeUntilVincible)
         hit = false;
   }

   private void OnTriggerEnter2D(Collider2D collider) {
      GameObject entered = collider.gameObject;
      bool sword = false;

      // Check if has hit Player's Weapon
      if (collider.tag == "Weapon")
         sword = true;

     if (!sword) {
        if (entered.layer == LayerMask.NameToLayer("Player") && !hit) {
           AudioManager.GetInstance().Play("Sword Hit Metal");
           timeUntilVincible = Time.time + invincibilityTime;
           hit = true;

              // If Player is alive
              if (PlayerHealth.curHealth > 0)
              {
                  entered.GetComponentInParent<PlayerHealth>().takeDamage(axeDamage);
                  Instantiate(bloodEffect, entered.transform.position, Quaternion.identity);

                  // If Player gets killed
                  if (PlayerHealth.curHealth <= 0)
                  {
                      if (entered.GetComponent<HingeJoint2D>() != null)
                        entered.GetComponent<HingeJoint2D>().enabled = false;

                      if (entered.GetComponent<ParticleSystem>() != null)
                        entered.GetComponent<ParticleSystem>().Play();

                      Instantiate(bloodEffect, entered.transform.position, Quaternion.identity);
                  }
              }
              else // If Player is dead, then still allow to dismember
              {
                 if (entered.GetComponent<HingeJoint2D>() != null)
                 {
                     if (entered.GetComponent<HingeJoint2D>().enabled == true)
                     {
                         entered.GetComponent<HingeJoint2D>().enabled = false;

                         if (entered.GetComponent<ParticleSystem>() != null)
                           entered.GetComponent<ParticleSystem>().Play();
                     }
                 }
                  Instantiate(bloodEffect, entered.transform.position, Quaternion.identity);
              }
         }
      }
   }
}
