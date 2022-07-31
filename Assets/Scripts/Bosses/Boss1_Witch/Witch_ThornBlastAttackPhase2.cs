using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Witch_ThornBlastAttackPhase2 : StateMachineBehaviour
{
   GameObject thorn;
   float thornDamage;
   float thornBlastForce;
   Rigidbody2D target;
   Transform witch;
   float currOffset;
   const float ANGLE_DIFF = 10f;
   const int NUMSETS = 3;
   const float OFFSETX = 10f;

   // Shoot 3 sets of 3 thorns at Player
   void TargetedThornBlastAttack() {
      // Get direction & angle to player
      Vector2 dirToPlayer;
      float rotZ;

      // Center thorns
      for (int i = -1; i < NUMSETS-1; i++) {
         currOffset = OFFSETX * i;
         dirToPlayer = (target.position - new Vector2(witch.position.x + currOffset, witch.position.y + 18)).normalized;
         rotZ = Mathf.Atan2(dirToPlayer.y, dirToPlayer.x) * Mathf.Rad2Deg;
         GameObject centerThorn = Instantiate(thorn, new Vector2(witch.position.x + currOffset, witch.position.y + 18), Quaternion.Euler(0, 0, rotZ - 90));
         centerThorn.GetComponent<Thorn>().SetThornForceDamageDirection(thornBlastForce, thornDamage, dirToPlayer);
      }

      // Upper thorns
      for (int i = -1; i < NUMSETS-1; i++) {
         currOffset = OFFSETX * i;
         dirToPlayer = (target.position - new Vector2(witch.position.x + currOffset, witch.position.y + 18)).normalized;
         rotZ = Mathf.Atan2(dirToPlayer.y, dirToPlayer.x) * Mathf.Rad2Deg;
         GameObject upperThorn = Instantiate(thorn, new Vector2(witch.position.x + currOffset, witch.position.y + 21), Quaternion.Euler(0, 0, rotZ+ANGLE_DIFF - 90));
         upperThorn.GetComponent<Thorn>().SetThornForceDamageDirection(thornBlastForce, thornDamage, dirToPlayer);
      }

      // Lower thorns
      for (int i = -1; i < NUMSETS-1; i++) {
         currOffset = OFFSETX * i;
         dirToPlayer = (target.position - new Vector2(witch.position.x + currOffset, witch.position.y + 18)).normalized;
         rotZ = Mathf.Atan2(dirToPlayer.y, dirToPlayer.x) * Mathf.Rad2Deg;
         GameObject lowerThorn = Instantiate(thorn, new Vector2(witch.position.x + currOffset, witch.position.y + 15), Quaternion.Euler(0, 0, rotZ-ANGLE_DIFF - 90));
         lowerThorn.GetComponent<Thorn>().SetThornForceDamageDirection(thornBlastForce, thornDamage, dirToPlayer);
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

      // Shoot thorns towards player
      TargetedThornBlastAttack();
    }
}
