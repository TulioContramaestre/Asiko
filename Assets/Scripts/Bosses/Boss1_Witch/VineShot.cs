using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineShot : MonoBehaviour {
   [SerializeField] GameObject bloodEffect;
   [SerializeField] float vineDamage = 20f;
   [SerializeField] float spawnTime = 0.25f;
   [SerializeField] float stayTime = 1f;
   bool hit = false;
   const float SCALE = 3f;

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

   IEnumerator ScaleDown(float duration) {
      float progress = 0;
      Vector3 initialScale = new Vector3(SCALE, SCALE, SCALE);
      Vector3 finalScale = new Vector3(SCALE, 0, SCALE);

      while (progress < duration) {
         this.transform.localScale = Vector3.Lerp(initialScale, finalScale, progress / duration);
         progress += Time.deltaTime;
         yield return null;
      }
   }


   // Start is called before the first frame update
   IEnumerator Start() {
      Destroy(this.gameObject, (stayTime + 2*spawnTime));
      AudioManager.GetInstance().Play("Vine Growth");
      StartCoroutine(ScaleUp(spawnTime));
      yield return new WaitForSeconds(1f);
      StartCoroutine(ScaleDown(spawnTime));
   }

   private void OnTriggerEnter2D(Collider2D collision) {
      GameObject entered = collision.gameObject;

     if (entered.layer == LayerMask.NameToLayer("Player") && !hit) {
        AudioManager.GetInstance().Play("Sword Hit Metal");
        hit = true;

           // If Player is alive
           if (PlayerHealth.curHealth > 0)
           {
               entered.GetComponentInParent<PlayerHealth>().takeDamage(vineDamage);
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
      }
   }
}
