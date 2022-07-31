using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAI_Attack : MonoBehaviour {
    [SerializeField] private int speed = 50;
    [SerializeField] private float thrust = 10f;
    Rigidbody2D rb; // The AI's Rigidbody2D
    Rigidbody2D target; // The Player's Rigidbody2D
    public GameObject Weapon;
    public GameObject bloodEffect;
    [Header("Check this box to have this unit do Honor Guard damage")]
    [SerializeField] private bool isHonorGuard;

    private bool isSwinging;
    private bool swung;

    private void Start() {
        rb = this.transform.GetComponent<Rigidbody2D>();
        //Weapon = GameObject.Find("EnemyManager/Grunt_Melee/RightHand/EnemySword");
        Weapon.GetComponent<TrailRenderer>().enabled = false;
        target = GameObject.Find("/Player_Stickman/Hips").GetComponent<Rigidbody2D>();

    }

    //private void Update()
    //{
    //    float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg;
    //    Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
    //    rb.transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, speed * Time.deltaTime);
    //}

    private void Update()
    {
        if (!isSwinging)
        {
            Vector3 mousePos = new Vector3(target.position.x, target.position.y, 0);
            Vector3 difference = mousePos - transform.position;
            float rotationZ = Mathf.Atan2(difference.x, -difference.y) * Mathf.Rad2Deg - 90;

            rb.MoveRotation(Mathf.LerpAngle(rb.rotation, rotationZ, speed * Time.deltaTime));
        }
    }

    // Attacks in direction based on <dir>
    // Negative <dir> => attack left, Positive <dir> => attack right
    public void Attack(int dir) {
      AudioManager.GetInstance().Play("Sword Swing");

      Vector3 force = new Vector3(dir * thrust, 0, 0);
      rb.AddForce(force * thrust, ForceMode2D.Impulse);

       StartCoroutine(PauseAttack());
    }

    public IEnumerator PauseAttack() {
      Weapon.GetComponent<TrailRenderer>().enabled = true;
      isSwinging = true;
      swung = true;
      yield return new WaitForSeconds(0.3f);
      Weapon.GetComponent<TrailRenderer>().enabled = false;
      swung = false;
      yield return new WaitForSeconds(0.5f);
      isSwinging = false;
    }

    private void OnCollisionStay2D(Collision2D collision) {
      if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && swung) {

         AudioManager.GetInstance().Play("Sword Hit Metal");
         swung = false;

         //Debug.Log("Hit Player!");

            // If the enemy the player is hitting is alive
            if (PlayerHealth.curHealth > 0)
            {
                if (isHonorGuard)
                    collision.gameObject.GetComponentInParent<PlayerHealth>().takeDamage(DataValues.honorGuardSwordDamage);
                else
                    collision.gameObject.GetComponentInParent<PlayerHealth>().takeDamage(DataValues.gruntSwordDamage);

                Instantiate(bloodEffect, collision.gameObject.transform.position, Quaternion.identity);

                // If the enemy the player is hitting gets killed
                if (PlayerHealth.curHealth <= 0)
                {
                    collision.gameObject.GetComponent<HingeJoint2D>().enabled = false;
                    collision.gameObject.GetComponent<ParticleSystem>().Play();
                    Instantiate(bloodEffect, collision.gameObject.transform.position, Quaternion.identity);
                }
            }
            else // If the enemy the player is hitting is dead, then still allow to dismember
            {
                if (collision.gameObject.GetComponent<HingeJoint2D>().enabled == true)
                {
                    //Debug.Log("The body part was still attached! But now it's not.");
                    collision.gameObject.GetComponent<HingeJoint2D>().enabled = false;
                    collision.gameObject.GetComponent<ParticleSystem>().Play();
                }
                //Debug.Log("That man is dead!");
                Instantiate(bloodEffect, collision.gameObject.transform.position, Quaternion.identity);
            }
        }
   }
}
