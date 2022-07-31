using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Witch_ThornRainAttackPhase3 : StateMachineBehaviour {
   GameObject thorn;
   float thornDamage = 5f;
   float thornRainForce = 10f;
   Transform thornRainSpawnpoint;
   const float FULLCOVER = 4;

   // Spawn line of thorns at top of arena
   void ThornRainAttack(float cover) {
      // Get start and end locations of spawn
      float offset = thornRainSpawnpoint.position.y;
      float startSpawn = thornRainSpawnpoint.GetChild(0).transform.position.x;
      float endSpawn = thornRainSpawnpoint.GetChild(1).transform.position.x;
      float currSpawn = startSpawn;

      // Spawn thorns throughout top of arena
      while (currSpawn < endSpawn) {
         // Instantiate a thorn
         GameObject newThorn = Instantiate(thorn, new Vector2(currSpawn, offset), Quaternion.AngleAxis(180, Vector3.right));
         newThorn.GetComponent<Thorn>().SetThornForceDamageDirection(thornRainForce, thornDamage, Vector3.down);

         // Increment spawnpoint
         currSpawn += cover;
      }
   }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
      // Get variables from <Boss1_Witch>
      Boss1_Witch boss = animator.GetComponent<Boss1_Witch>();
      thorn = boss.thorn;
      thornDamage = boss.thornDamage;
      thornRainForce = boss.thornRainForce;
      thornRainSpawnpoint = boss.thornRainSpawnpoint;

      // Shoot thorns from arena
      ThornRainAttack(FULLCOVER);
    }
}
