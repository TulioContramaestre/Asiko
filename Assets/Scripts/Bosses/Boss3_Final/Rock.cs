using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour {
   const float SCALE = 1f;
   float rockSpeed;
   Rigidbody2D rb;
   CircleCollider2D cc;
   [HideInInspector] public float spawntime = 1.2f;
   float time;

   public GameObject bloodEffect;
   public GameObject rockDestroyEffect;

   public void SetRockDamage(float rockDamage) {
      //rockDamage = Boss3_Final.GetInstance().rockDamage;
      this.spawntime = spawntime;
   }

   void FallFromCeiling() {
      this.transform.localScale = new Vector3(1, 1, 1);
      rb.bodyType = RigidbodyType2D.Dynamic;
      rb.AddTorque(Random.Range(80, 120));
      cc.enabled = true;

      // Play sound-effects
      AudioManager.GetInstance().Play("Rock Crumble");
      AudioManager.GetInstance().Play("Axe Hit");
   }

   // Start is called before the first frame update
   void Start() {
      Destroy(this.gameObject, 7f);
      rb = this.transform.GetComponent<Rigidbody2D>();
      cc = this.transform.GetComponent<CircleCollider2D>();

      // Delay spawn
      time = Time.time + spawntime;
      this.transform.localScale = Vector3.zero;
      Invoke("FallFromCeiling", spawntime);
   }

   private void OnCollisionEnter2D(Collision2D collision) {
      GameObject entered = collision.gameObject;
      bool sword = false;

      // Check if Thorn has hit Player's sword
      if (entered.transform.childCount > 0)
         foreach (Transform child in entered.transform)
            if (child.gameObject.activeInHierarchy)
               if (child.name == "Sword")
                  sword = true;

     if (entered.layer == LayerMask.NameToLayer("Player") && !sword) {
        AudioManager.GetInstance().Play("Sword Hit Metal");
        Instantiate(rockDestroyEffect, transform.position, Quaternion.identity);
           // If Player is alive
           if (PlayerHealth.curHealth > 0)
           {
               entered.GetComponentInParent<PlayerHealth>().takeDamage(Boss3_Final.GetInstance().rockDamage);
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
      else if (entered.layer == LayerMask.NameToLayer("Enemy") && entered.GetComponentInParent<Boss3_Final>() != null) {
         if (entered.GetComponentInParent<Boss3_Final>().currHealth > 0) {
            AudioManager.GetInstance().Play("Sword Hit Metal");
            Instantiate(rockDestroyEffect, transform.position, Quaternion.identity);
            Instantiate(bloodEffect, entered.transform.position, Quaternion.identity);
            entered.GetComponentInParent<Boss3_Final>().takeDamage(2 * Boss3_Final.GetInstance().rockDamage);
         }
         Destroy(this.gameObject);
      }
      else if (entered.layer == LayerMask.NameToLayer("Enemy") && entered.GetComponentInParent<EnemyHealth>().curHealth <= 0)
      {
         AudioManager.GetInstance().Play("Sword Hit Metal");
         Instantiate(bloodEffect, entered.transform.position, Quaternion.identity);
         Instantiate(rockDestroyEffect, transform.position, Quaternion.identity);
         Destroy(this.gameObject);
      }
      else if (entered.layer == LayerMask.NameToLayer("Environment")) {
         Instantiate(rockDestroyEffect, transform.position, Quaternion.identity);
         AudioManager.GetInstance().Play("Rock Thud");
         Destroy(this.gameObject);
      }
   }
}
