using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
   Rigidbody2D rb;

   [Header("Box Collider Control")]
   public BoxCollider2D initialCollider; // The collider should only be the tip when the arrow is first shot
   public BoxCollider2D restingCollider; // The collider should be the full length of the arrow to allow the player to climb it
   public GameObject bloodEffect;

   private bool shot; // This is purely so attacks only register once



    bool hasHit;

    // This is commented out because it is quite taxing in terms of memory. Although if we choose to "spawn" enemies in we
    // might have to opt in for this code until we find a better solution. As of now, having this is the Start() function
    // will not let it work with enemies that aren't spawned in.
    //
    //private void OnEnable()
    //{
    //    GameObject[] weapons = GameObject.FindGameObjectsWithTag("Weapon");

    //    foreach (GameObject obj in weapons)
    //    {
    //        Physics2D.IgnoreCollision(obj.GetComponent<Collider2D>(), GetComponent<Collider2D>());
    //        Debug.Log("OnEnable ran IN THE FOR LOOP");
    //    }
    //    Debug.Log("OnEnable ran");
    //}

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Get all the GameObjects with the tag Weapon and make it so there is no collision between them and the arrow
        GameObject[] weapons = GameObject.FindGameObjectsWithTag("Weapon");

        foreach (GameObject obj in weapons)
        {
            //Debug.Log("obj.GetComponent<Collider2D>() = " + obj.GetComponent<Collider2D>());
            //Debug.Log("GetComponent<Collider2D>() = " + GetComponent<Collider2D>());
            Physics2D.IgnoreCollision(obj.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }

        //Debug.Log("Start() was called");
        shot = true;
        Destroy(this.gameObject, 5f);
    }

    // Update is called once per frame
    void Update()
   {
      float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
      transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
   }

   // Check if bolt hit a collider
   private void OnCollisionEnter2D(Collision2D collision) {
      if (rb != null) {
           hasHit = true;
           rb.velocity = Vector2.zero;
           rb.isKinematic = true;

            // If what the Player hit is an average enemy.
            if (collision.gameObject.GetComponentInParent<EnemyHealth>() != null)
            {
                // Arrow hit Enemy
                if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy") && shot)
                {
                    // Allows the arrow to stick to whatever it collided with
                    gameObject.transform.SetParent(collision.transform);
                    initialCollider.enabled = false; // Disable your collider, otherwise it may stick to something else if it touches it

                    AudioManager.GetInstance().Play("Sword Hit Metal");
                    shot = false;

                    // If the enemy the player is hitting is alive
                    if (collision.gameObject.GetComponentInParent<EnemyHealth>().curHealth > 0)
                    {
                        gameObject.GetComponent<ParticleSystem>().Play(); // Play the arrow bleed effect
                        Instantiate(bloodEffect, collision.gameObject.transform.position, Quaternion.identity);
                        collision.gameObject.GetComponentInParent<EnemyHealth>().takeDamage(DataValues.playerArrowDamage);

                        // If the enemy the player is hitting gets killed
                        if (collision.gameObject.GetComponentInParent<EnemyHealth>().curHealth <= 0)
                        {
                            collision.gameObject.GetComponent<HingeJoint2D>().enabled = false;
                            collision.gameObject.GetComponent<ParticleSystem>().Play();
                            Instantiate(bloodEffect, collision.gameObject.transform.position, Quaternion.identity);
                        }
                    }
                    // If the enemy the player is hitting is dead, then still allow to dismember
                    else
                    {
                        //Debug.Log("That man is dead!");
                        collision.gameObject.GetComponent<HingeJoint2D>().enabled = false;
                        collision.gameObject.GetComponent<ParticleSystem>().Play();
                        Instantiate(bloodEffect, collision.gameObject.transform.position, Quaternion.identity);
                    }


                    //Destroy(this.gameObject);
                }
            }
            // Arrow hit the Environment that is not interactable.
            else if (collision.gameObject.layer != LayerMask.NameToLayer("InteractableProps") && collision.gameObject.layer != LayerMask.NameToLayer("Enemy"))
            {
                AudioManager.GetInstance().Play("Arrow Impact");

                // Change the layer and tag so it acts as ground and can be grapple hooked to
                gameObject.layer = LayerMask.NameToLayer("Default");
                gameObject.tag = "Untagged";

                initialCollider.enabled = false; // Disable the initial collider
                restingCollider.enabled = true;  // Enables the new collider
            }
            // Arrow hit an interactable prop, have it stick to it.
            else if (collision.gameObject.layer == LayerMask.NameToLayer("InteractableProps") && collision.gameObject.layer != LayerMask.NameToLayer("Enemy"))
            {
                // Allows the arrow to stick to whatever it collided with
                gameObject.transform.SetParent(collision.transform);
                initialCollider.enabled = false; // Disable your collider, otherwise it may stick to something else if it touches it
                AudioManager.GetInstance().Play("Arrow Impact");
            }
            // If what the Player hit is the Witch boss.
            else if (collision.gameObject.GetComponentInParent<Boss1_Witch>() != null)
            {
                if (collision.gameObject.GetComponentInParent<Boss1_Witch>().currHealth > 0 && shot)
                {
                    // Allows the arrow to stick to whatever it collided with
                    gameObject.transform.SetParent(collision.transform);
                    initialCollider.enabled = false; // Disable your collider, otherwise it may stick to something else if it touches it

                    AudioManager.GetInstance().Play("Sword Hit Metal");
                    shot = false;

                    Instantiate(bloodEffect, collision.gameObject.transform.position, Quaternion.identity);
                    collision.gameObject.GetComponentInParent<Boss1_Witch>().takeDamage(DataValues.playerArrowDamage);
                }

            }
            // If what the Player hit is the General Crain boss.
            else if (collision.gameObject.GetComponentInParent<Boss3_Final>() != null)
            {
                if (collision.gameObject.GetComponentInParent<Boss3_Final>().currHealth > 0 && shot)
                {
                    // Allows the arrow to stick to whatever it collided with
                    gameObject.transform.SetParent(collision.transform);
                    initialCollider.enabled = false; // Disable your collider, otherwise it may stick to something else if it touches it

                    AudioManager.GetInstance().Play("Sword Hit Metal");
                    shot = false;

                    Instantiate(bloodEffect, collision.gameObject.transform.position, Quaternion.identity);
                    collision.gameObject.GetComponentInParent<Boss3_Final>().takeDamage(DataValues.playerArrowDamage);
                }

            }

            Destroy(this);
      }
   }

}
