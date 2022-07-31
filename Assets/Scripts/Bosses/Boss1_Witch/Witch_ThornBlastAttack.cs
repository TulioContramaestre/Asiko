using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Witch_ThornBlastAttack : StateMachineBehaviour
{
   GameObject thorn;
   float thornDamage;
   float thornBlastForce;
   Rigidbody2D target;
   Transform witch;
   const float ANGLE_DIFF = 10;

   // Shoot 3 thorns at Player
   void TargetedThornBlastAttack() {
      // Get direction & angle to player
      Vector2 dirToPlayer = (target.position - new Vector2(witch.position.x, witch.position.y + 18)).normalized;
      float rotZ = Mathf.Atan2(dirToPlayer.y, dirToPlayer.x) * Mathf.Rad2Deg;

      // Center thorn
      GameObject centerThorn = Instantiate(thorn, new Vector2(witch.position.x, witch.position.y + 18), Quaternion.Euler(0, 0, rotZ - 90));
      centerThorn.GetComponent<Thorn>().SetThornForceDamageDirection(thornBlastForce, thornDamage, dirToPlayer);

      // Upper thorn
      GameObject upperThorn = Instantiate(thorn, new Vector2(witch.position.x, witch.position.y + 21), Quaternion.Euler(0, 0, rotZ+ANGLE_DIFF - 90));
      upperThorn.GetComponent<Thorn>().SetThornForceDamageDirection(thornBlastForce, thornDamage, dirToPlayer);

      // Lower thorn
      GameObject lowerThorn = Instantiate(thorn, new Vector2(witch.position.x, witch.position.y + 15), Quaternion.Euler(0, 0, rotZ-ANGLE_DIFF - 90));
      lowerThorn.GetComponent<Thorn>().SetThornForceDamageDirection(thornBlastForce, thornDamage, dirToPlayer);
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
