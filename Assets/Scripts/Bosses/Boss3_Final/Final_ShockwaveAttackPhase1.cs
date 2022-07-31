using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Final_ShockwaveAttackPhase1 : StateMachineBehaviour {
   GameObject rock;
   Transform rockSpawnpoint;
   int percentChance;
   const float FULLCOVER = 4;
   const int MAXROCKS = 8;

   float rockDamage;

   void ShockwaveAttack(float cover) {
      // Get start and end locations of spawn
      float offsetY = rockSpawnpoint.position.y;
      float startSpawn = rockSpawnpoint.GetChild(0).transform.position.x;
      float endSpawn = rockSpawnpoint.GetChild(1).transform.position.x;
      float currSpawn = startSpawn;

      // Make sure that MAXROCKS rocks don't spawn in a row
      int count = 0;

      // Spawn thorns throughout top of arena
      while (currSpawn < endSpawn) {
         // Skip spawning rock if too many in a row
         if (count >= MAXROCKS) {
            count = 0;
            currSpawn += cover;
            continue;
         }

         // Instantiate a rock by chance
         if (Random.Range(1, 101) <= percentChance){
            GameObject newRock = Instantiate(rock, new Vector2(currSpawn, offsetY), Quaternion.AngleAxis(180, Vector3.right));
            rock.GetComponent<Rock>().SetRockDamage(Boss3_Final.GetInstance().rockDamage);
            count++;
         }
         else
            count = 0;

         // Increment spawnpoint
         currSpawn += cover;
      }
   }

   // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
   override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
      // Get variables from <Boss1_Witch>
      Boss3_Final boss = animator.GetComponent<Boss3_Final>();
      rock = boss.rock;
      rockDamage = boss.rockDamage;
      rockSpawnpoint = boss.rockSpawnpoint;
      percentChance = boss.percentChance;

      // Enable Battleaxe collider
      boss.transform.Find("LeftHand/Battleaxe").GetComponent<PolygonCollider2D>().enabled = true;

      // Shoot thorns from arena staggered over 2 spawns
      ShockwaveAttack(FULLCOVER);
   }

   override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
      Boss3_Final boss = animator.GetComponent<Boss3_Final>();

      // Disable Battleaxe collider
      if (boss != null)
         boss.transform.Find("LeftHand/Battleaxe").GetComponent<PolygonCollider2D>().enabled = false;
   }
}
