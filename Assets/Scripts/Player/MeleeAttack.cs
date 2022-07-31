using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    //AudioManager audioManager;
    //public Camera cam;

    [SerializeField] private int speed = 50;
    [SerializeField] private float thrust = 200f;
    public GameObject PlayerHips;
    public Rigidbody2D rb;
    public GameObject Weapon;
    public GameObject bloodEffect;

    public static bool isSwinging;
    public static bool swung; // This is purely so attacks only register once

    //private void Awake()
    //{
    //    Application.targetFrameRate = 60;
    //}

    private void Start()
    {
        Weapon.GetComponent<TrailRenderer>().enabled = false;
        //audioManager = FindObjectOfType<AudioManager>();
        //Debug.Log("MeleeAttack.cs found the " + audioManager + " audiomanager!");
        //cam = FindObjectOfType<Camera>();
        //cam = CameraInstance.GetInstance();
    }

    private void FixedUpdate()
    {
        if (!isSwinging)
        {
            Vector3 mouse = Camera.main.WorldToScreenPoint(transform.position);
            var playerScreenPoint = Camera.main.WorldToScreenPoint(PlayerHips.transform.position);

            //Debug.Log("Mouse position x = " + mouse.x);
            //Debug.Log("Player position x = " + playerScreenPoint.x);

            if (mouse.x < playerScreenPoint.x || mouse.x > playerScreenPoint.x)
            {
                Vector3 mousePos = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
                //Debug.Log("Cursor NOT Close to player");
                Vector3 difference = mousePos - transform.position;
                float rotationZ = Mathf.Atan2(difference.x, -difference.y) * Mathf.Rad2Deg - 90;

                rb.MoveRotation(Mathf.LerpAngle(rb.rotation, rotationZ, speed * Time.deltaTime));
            }

            if (mouse.x == playerScreenPoint.x)
                Debug.Log("Cursor is near player");

            //if (mouse.x < playerScreenPoint.x)
                //Debug.Log("Cursor is to the left");


            //if (mouse.x > playerScreenPoint.x)
                //Debug.Log("Cursor is to the right");
        }
    }

    private void Update()
    {
        if (!isSwinging)
        {
            //Debug.Log("Player position = " + Player.transform.position);

            if (Input.GetKeyDown(KeyCode.Mouse0) && !PauseMenu.GamePaused)
            {
                AudioManager.GetInstance().Play("Sword Swing");

                Vector3 mouse = Camera.main.WorldToScreenPoint(transform.position);
                Vector3 direction = (Input.mousePosition - mouse);
                direction.Normalize();

                var playerScreenPoint = Camera.main.WorldToScreenPoint(PlayerHips.transform.position);
                if (mouse.x < playerScreenPoint.x)
                {
                    //Debug.Log("You clicked to the left of the player");
                    rb.AddForce(direction * thrust, ForceMode2D.Impulse);
                }
                else
                {
                    //Debug.Log("You clicked to the right of the player");
                    rb.AddForce(direction * thrust, ForceMode2D.Impulse);
                }

                StartCoroutine(PauseAttack());
            }
        }
    }

    IEnumerator PauseAttack()
    {
        Weapon.GetComponent<TrailRenderer>().enabled = true;
        isSwinging = true;
        swung = true;
        //gameObject.GetComponent<Rigidbody2D>().collisionDetectionMode = CollisionDetectionMode2D.Discrete;
        yield return new WaitForSeconds(0.3f);
        Weapon.GetComponent<TrailRenderer>().enabled = false;
        swung = false;
        yield return new WaitForSeconds(0.5f);
        isSwinging = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If what the player hit was the Enemy layer. We use swung here instead of isSwinging because Update() relies on
        // isSwinging to not abruptly be set to false in the collision check
        if (collision.gameObject.layer == 10 && swung)
        {
            //gameObject.GetComponent<Rigidbody2D>().collisionDetectionMode = CollisionDetectionMode2D.Continuous;

            AudioManager.GetInstance().Play("Sword Hit Metal");
            swung = false;

            // If what the Player hit is an average enemy.
            if (collision.gameObject.GetComponentInParent<EnemyHealth>() != null)
            {
                // If the enemy the player is hitting is alive
                if (collision.gameObject.GetComponentInParent<EnemyHealth>().curHealth > 0)
                {
                    collision.gameObject.GetComponentInParent<EnemyHealth>().takeDamage(DataValues.playerSwordDamage);
                    Instantiate(bloodEffect, collision.gameObject.transform.position, Quaternion.identity);

                    // If the enemy the player is hitting gets killed
                    if (collision.gameObject.GetComponentInParent<EnemyHealth>().curHealth <= 0)
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

            // If what the Player hit is the Witch boss.
            if (collision.gameObject.GetComponentInParent<Boss1_Witch>() != null)
            {
                if (collision.gameObject.GetComponentInParent<Boss1_Witch>().currHealth > 0)
                {
                    collision.gameObject.GetComponentInParent<Boss1_Witch>().takeDamage(DataValues.playerSwordDamage);
                    Instantiate(bloodEffect, collision.gameObject.transform.position, Quaternion.identity);
                }
            }

            // If what the Player hit is the General Crain boss.
            if (collision.gameObject.GetComponentInParent<Boss3_Final>() != null)
            {
                if (collision.gameObject.GetComponentInParent<Boss3_Final>().currHealth > 0)
                {
                    collision.gameObject.GetComponentInParent<Boss3_Final>().takeDamage(DataValues.playerSwordDamage);
                    Instantiate(bloodEffect, collision.gameObject.transform.position, Quaternion.identity);
                }
            }
        }
    }

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.gameObject.layer == 10 && !swung)
    //    {
    //        swung = true;
    //    }
    //}

}