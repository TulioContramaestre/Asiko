using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_Witch : MonoBehaviour {
   [SerializeField] float maxHealth = 450f;
   public GameObject thorn;
   public float thornDamage = 5f;
   public float currHealth;

   Animator anim;

   [SerializeField] Arena1 arena;

   [Header("Attacks")]
   [SerializeField] float maxTimeBtwnAttacks = 8f;
   [SerializeField] float minTimeBtwnAttacks = 3f;
   int currPhase;
   float nextAttackTime;
   int NUM_ATTACKS = 2;
   int attackType;

   [Header("ThornRain")]
   public float thornRainForce = 10f;
   public Transform thornRainSpawnpoint;
   const int THORNRAIN = 0;
   const float FULLCOVER = 4;
   const float HALFCOVER = 8;

   [Header("TargetedThornBlast")]
   public float thornBlastForce = 40f;
   [HideInInspector] public Rigidbody2D target;
   const int THORNBLAST = 1;
   const float ANGLE_DIFF = 10;

   [Header("HealthBar")]
   public BossHealthBar healthbar;

   // Start is called before the first frame update
   void Start() {
      target = GameObject.Find("/Player_Stickman/Hips").GetComponent<Rigidbody2D>();

      // Get components from Hierarchy
        anim = GetComponent<Animator>();

      // Set status
      currHealth = maxHealth;
      currPhase = 1;

      healthbar.setMaxHealth(maxHealth);
      healthbar.setHealth(maxHealth);

      // Setup first attack time
      nextAttackTime = Random.Range(minTimeBtwnAttacks, maxTimeBtwnAttacks);
   }

   // health bar testing

   public void takeDamage(float damage)
   {
      currHealth -= damage;
      healthbar.setHealth(currHealth);
   }

   // Update is called once per frame
   void Update() {
      // Start Death animation if health <= 0
      if (currHealth <= 0) {
         anim.SetTrigger("death");
         Destroy(this);
      }

      // Switch phases if health is low enough
      if (currPhase < 2 && currHealth <= (maxHealth * 2 / 3)) {
         currPhase = 2;
         arena.ChangePhase(currPhase);
      }
      else if (currPhase < 3 && currHealth <= (maxHealth / 3)) {
         currPhase = 3;
         anim.SetTrigger("phase3Switch");
         AudioManager.GetInstance().Play("Enough!");
         arena.ChangePhase(currPhase);
      }

      if (Time.time >= nextAttackTime) {
         // Reset <nextAttackTime>
         nextAttackTime = Time.time + Random.Range(minTimeBtwnAttacks, maxTimeBtwnAttacks);

         // Choose random attack
         attackType = Random.Range(0, NUM_ATTACKS);

         // Perform random attack based on phase
         if (currPhase == 1) {
            switch(attackType) {
               case THORNRAIN:
                  anim.SetTrigger("thornRainAttack");
                  break;
               case THORNBLAST:
                  anim.SetTrigger("thornBlastAttack");
                  break;
               default:
                  break;
            }
         }
         else if (currPhase == 2) {
            switch(attackType) {
               case THORNRAIN:
                  anim.SetTrigger("thornRainAttackP2");
                  break;
               case THORNBLAST:
                  anim.SetTrigger("thornBlastAttackP2");
                  break;
               default:
                  break;
            }
         }
         else if (currPhase == 3) {
            switch(attackType) {
               case THORNRAIN:
                  anim.SetTrigger("thornRainAttackP3");
                  break;
               case THORNBLAST:
                  anim.SetTrigger("thornBlastAttackP3");
                  break;
               default:
                  break;
            }
         }
      }
   }
}
