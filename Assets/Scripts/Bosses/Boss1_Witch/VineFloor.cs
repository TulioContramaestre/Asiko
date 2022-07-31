using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineFloor : MonoBehaviour {
   [SerializeField] GameObject bloodEffect;
   [SerializeField] float vineDamage = 20f;
   [SerializeField] float spawnTime = 10f;
   [SerializeField] float invincibilityTime = 1f;
   float timeUntilVincible;
   bool hit = false;
   const float SCALE = 6.6f;

   IEnumerator ScaleUp(float duration) {
      float progress = 0;
      Vector3 initialScale = new Vector3(0, 0, 0);
      Vector3 finalScale = new Vector3(SCALE, SCALE, SCALE);

      while (progress < duration) {
         this.transform.localScale = Vector3.Lerp(initialScale, finalScale, progress / duration);
         progress += Time.deltaTime;
         yield return null;
      }
   }

   // Start is called before the first frame update
   void Start() {
      AudioManager.GetInstance().Play("Vine Growth");
      StartCoroutine(ScaleUp(spawnTime));
   }

   // Update is called once per frame
   void Update() {
      if (hit == true && Time.time >= timeUntilVincible)
         hit = false;
   }

   private void OnTriggerEnter2D(Collider2D collision) {
      GameObject entered = collision.gameObject;

     if (entered.layer == LayerMask.NameToLayer("Player") && !hit) {
        AudioManager.GetInstance().Play("Sword Hit Metal");
        timeUntilVincible = Time.time + invincibilityTime;
        hit = true;

           // If Player is alive
           if (PlayerHealth.curHealth > 0)
           {
               entered.GetComponentInParent<PlayerHealth>().takeDamage(vineDamage);
               Instantiate(bloodEffect, entered.transform.position, Quaternion.identity);

               // If Player gets killed
               if (PlayerHealth.curHealth <= 0)
               {
                   if (entered.GetComponent<HingeJoint2D>() != null)
                   {
                        entered.GetComponent<HingeJoint2D>().enabled = false;
                   }
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
                        entered.GetComponent<ParticleSystem>().Play();
                    }
               }
                
               Instantiate(bloodEffect, entered.transform.position, Quaternion.identity);
           }
      }
   }
}
