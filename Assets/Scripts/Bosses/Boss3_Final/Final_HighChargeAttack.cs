using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Final_HighChargeAttack : StateMachineBehaviour {
   // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
   override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
      Boss3_Final boss = animator.GetComponent<Boss3_Final>();

      // Enable Battleaxe collider
      boss.transform.Find("LeftHand/Battleaxe").GetComponent<PolygonCollider2D>().enabled = true;
      AudioManager.GetInstance().Play("Boss3 Charge");
   }

   // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
   override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
      Boss3_Final boss = animator.GetComponent<Boss3_Final>();

      // Disable Battleaxe collider
      if (boss != null)
         boss.transform.Find("LeftHand/Battleaxe").GetComponent<PolygonCollider2D>().enabled = false;
   }
}
