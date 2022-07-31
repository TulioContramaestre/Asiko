using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Witch_ThornBlastAttackPhase3 : StateMachineBehaviour {
   GameObject thorn;
   float thornDamage;
   float thornBlastForce;
   Rigidbody2D target;
   Transform witch;
   const float MAGNITUDE = 7f;
   const float OFFSETY = 8f;
   const int RADIAL_ANGLE = 60;
   const int DEGREES_IN_CIRCLE = 360;

   // Shoot <DEGREES_IN_CIRCLE>/<RADIAL_ANGLE> thorns at Player
   void TargetedThornBlastAttack() {
      // Get direction & angle to player
      Vector2 dirToPlayer;
      float rotZ;

      // Summon <DEGREES_IN_CIRCLE>/<RADIAL_ANGLE> thorns around Witch
      for (int angle = 0; angle < DEGREES_IN_CIRCLE; angle += RADIAL_ANGLE) {
         // Get current radial offset
         float dirX = MAGNITUDE * Mathf.Cos(angle * Mathf.Deg2Rad);
         float dirY = MAGNITUDE * Mathf.Sin(angle * Mathf.Deg2Rad) + OFFSETY;

         dirToPlayer = (target.position - new Vector2(witch.position.x + dirX, witch.position.y + dirY)).normalized;
         rotZ = Mathf.Atan2(dirToPlayer.y, dirToPlayer.x) * Mathf.Rad2Deg;
         GameObject newThorn = Instantiate(thorn, new Vector2(witch.position.x + dirX, witch.position.y + dirY), Quaternion.Euler(0, 0, rotZ - 90));
         newThorn.GetComponent<Thorn>().SetThornForceDamageDirection(thornBlastForce, thornDamage, dirToPlayer);
      }
   }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
      // Get variables from <Boss1_Witch>
      Boss1_Witch boss = animator.GetComponent<Boss1_Witch>();
      thorn = boss.thorn;
      thornDamage = boss.thornDamage;
      thornBlastForce = boss.thornBlastForce;
      target = boss.target;
      witch = animator.GetComponent<Transform>();

      // Shoot thorns towards player in a radial pattern
      TargetedThornBlastAttack();
    }
}
