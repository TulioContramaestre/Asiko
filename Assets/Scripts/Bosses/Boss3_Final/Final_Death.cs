using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Final_Death : StateMachineBehaviour {
   // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
   override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
      AudioManager.GetInstance().Play("Boss3 Death");
      animator.GetComponent<Boss3_Final>().transform.Find("LeftHand/Battleaxe").GetComponent<PolygonCollider2D>().enabled = false;
      Destroy(animator.gameObject, stateInfo.length);
   }
}
