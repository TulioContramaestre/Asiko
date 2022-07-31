using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3_Final : MonoBehaviour {
   [SerializeField] float maxHealth = 500f;
   public float currHealth;

   Animator anim;
   Rigidbody2D target;

   [Header("Attacks")]
   [SerializeField] float maxTimeBtwnAttacks = 6f;
   [SerializeField] float minTimeBtwnAttacks = 2f;
   int currPhase;
   float nextAttackTime;
   int NUM_ATTACKS = 2;
   int attackType;

   [Header("ChargeAttack")]
   [SerializeField] public float chargeDamage = 20f;
   [SerializeField] float offGroundDist = 13f;
   const int CHARGEATTACK = 0;

   [Header("ShockwaveAttack")]

   [SerializeField] public Transform rockSpawnpoint;
   [SerializeField] public GameObject rock;
   [SerializeField] public float rockDamage;
   [SerializeField] public int percentChance = 60;
   const int SHOCKWAVEATTACK = 1;

   [Header("AxeAttack")]
   [SerializeField] float attackRange = 20f;

   [Header("Phase2Switch")]
   [SerializeField] GameObject leftAmbusher;
   [SerializeField] GameObject rightAmbusher;
   const float ANIM_LENGTH = 1.2f;

   [Header("HealthBar")]
   public BossHealthBar healthbar;

   public static Boss3_Final instance;

   public static Boss3_Final GetInstance()
   {
       return instance;
   }

   // Checks if <target> is within <attackRange>
   bool PlayerTooClose() {
      float dist = ((Vector2)this.transform.position - target.position).magnitude;

      return (dist <= attackRange);
   }

   // Checks if <target> is on the ground
   bool PlayerOnGround() {
      float deltaY = Mathf.Abs(((Vector2)this.transform.position - target.position).y);
      //Debug.Log(deltaY);

      return (deltaY <= offGroundDist);
   }

   // Spawn ambush on Phase2Switch after ANIM_LENGTH
   IEnumerator SpawnAmbush() {
      yield return new WaitForSeconds(ANIM_LENGTH);
      leftAmbusher.SetActive(true);
      rightAmbusher.SetActive(true);
   }

   // Start is called before the first frame update
   void Start() {
      if (instance != null)
      {
          Destroy(gameObject);
          return;
      }
      instance = this;

      // Get component from Hierarchy
      anim = this.GetComponent<Animator>();
      target = GameObject.Find("Player_Stickman/Hips").transform.GetComponent<Rigidbody2D>();

      // Set status
      currHealth = maxHealth;
      currPhase = 1;
      healthbar.setMaxHealth(maxHealth);
      healthbar.setHealth(maxHealth);

      // Setup first attack time
      nextAttackTime = Random.Range(minTimeBtwnAttacks, maxTimeBtwnAttacks);
   }

   public void takeDamage(float damage)
   {
      currHealth -= damage;
      healthbar.setHealth(currHealth);
   }

    // Update is called once per frame
    void Update() {
      // If currHealth <= 0, kill boss
      if (currHealth <= 0) {
         anim.SetTrigger("death");
         Destroy(this);
      }

      // Switch phases if health is low enough
      if (currPhase < 2 && currHealth <= (maxHealth / 2)) {
         currPhase = 2;
         percentChance = 85;
         rock.GetComponent<Rock>().spawntime = 1f;
         AudioManager.GetInstance().Play("Boss3 Phase2 Switch");
         anim.SetTrigger("phase2Switch");
         StartCoroutine(SpawnAmbush());
      }

      // Check if Player is too close
      if (Time.time >= nextAttackTime && PlayerTooClose()) {
         // Reset <nextAttackTime>
         nextAttackTime = Time.time + Random.Range(minTimeBtwnAttacks, maxTimeBtwnAttacks);

         // Attack Player
         anim.SetTrigger("axeAttack");
      }

      if (Time.time >= nextAttackTime) {
         // Reset <nextAttackTime>
         nextAttackTime = Time.time + Random.Range(minTimeBtwnAttacks, maxTimeBtwnAttacks);

         // Choose random attack
         attackType = Random.Range(0, NUM_ATTACKS);

         // Perform random attack based on phase
         if (currPhase == 1) {
            switch(attackType) {
               case CHARGEATTACK:
                  if (PlayerOnGround())
                     anim.SetTrigger("chargeAttackPhase1");
                  else
                     anim.SetTrigger("highChargeAttackPhase1");
                  break;
               case SHOCKWAVEATTACK:
                  anim.SetTrigger("shockwaveAttackPhase1");
                  break;
               default:
                  break;
            }
         }
         else if (currPhase == 2) {
            switch(attackType) {
               case CHARGEATTACK:
                  if (PlayerOnGround())
                     anim.SetTrigger("chargeAttackPhase2");
                  else
                     anim.SetTrigger("highChargeAttackPhase1");
                  break;
               case SHOCKWAVEATTACK:
                  anim.SetTrigger("shockwaveAttackPhase2");
                  break;
               default:
                  break;
            }
         }
      }
   }
}
