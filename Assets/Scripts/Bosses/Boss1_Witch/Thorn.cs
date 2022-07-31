using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thorn : MonoBehaviour {
   const float SCALE = 1.5f;
   float thornForce;
   float thornDamage;
   Vector3 direction;
   float spawnTime = 1f;
   float time;

   public GameObject bloodEffect;
   public GameObject thornDestroyEffect;

   public void SetThornForceDamageDirection(float thornForce, float thornDamage, Vector3 direction) {
      this.thornForce = thornForce;
      this.thornDamage = thornDamage;
      this.direction = direction;
   }

   IEnumerator ScaleUp(float duration) {
      float progress = 0;
      Vector3 initialScale = Vector3.zero;
      Vector3 finalScale = new Vector3(SCALE, SCALE, SCALE);

      while (progress < duration) {
         this.transform.localScale = Vector3.Lerp(initialScale, finalScale, progress);
         progress += Time.deltaTime;
         yield return null;
      }
   }

   // Start is called before the first frame update
   IEnumerator Start() {
      time = Time.time + spawnTime;
      Destroy(this.gameObject, 7f);
      AudioManager.GetInstance().Play("Thorn Growth");
      yield return StartCoroutine(ScaleUp(spawnTime));
      AudioManager.GetInstance().Play("Thorn Shot");
   }

   // Update is called once per frame
   void Update() {
      if (Time.time >= time)
         this.transform.position += direction * thornForce * Time.deltaTime;
   }

   private void OnCollisionEnter2D(Collision2D collision) {
      GameObject entered = collision.gameObject;
      bool destroyed = false;

      // Check if Thorn has hit Player's Weapon
      if (entered.transform.childCount > 0) {
         foreach (Transform child in entered.transform) {
            if (child.gameObject.activeInHierarchy) {
               if (child.name == "Sword") {
                  AudioManager.GetInstance().Play("Thorn Cut");
                  Instantiate(thornDestroyEffect, transform.position, Quaternion.identity);
                  Destroy(this.gameObject);
                  destroyed = true;
               }
               else if (child.name == "BowPivot")
                  destroyed = true;
               else if (child.name == "GunPivot")
                  destroyed = true;
            }
         }
      }

     if (entered.layer == LayerMask.NameToLayer("Player") && !destroyed) {
        AudioManager.GetInstance().Play("Sword Hit Metal");

           // If Player is alive
           if (PlayerHealth.curHealth > 0)
           {
               entered.GetComponentInParent<PlayerHealth>().takeDamage(thornDamage);
               Instantiate(bloodEffect, entered.transform.position, Quaternion.identity);

               // If Player gets killed
               if (PlayerHealth.curHealth <= 0)
               {
                   entered.GetComponent<HingeJoint2D>().enabled = false;
                   entered.GetComponent<ParticleSystem>().Play();
                   Instantiate(bloodEffect, entered.transform.position, Quaternion.identity);
               }
           }
           else // If Player is dead, then still allow to dismember
           {
               if (entered.GetComponent<HingeJoint2D>().enabled == true)
               {
                   entered.GetComponent<HingeJoint2D>().enabled = false;
                   entered.GetComponent<ParticleSystem>().Play();
               }
               Instantiate(bloodEffect, entered.transform.position, Quaternion.identity);
           }

           Destroy(this.gameObject);
      }
      else if (entered.layer == LayerMask.NameToLayer("Environment") && !destroyed) {
         AudioManager.GetInstance().Play("Thorn Thud");
         Instantiate(thornDestroyEffect, transform.position, Quaternion.identity);
         Destroy(this.gameObject);
      }
   }
}
