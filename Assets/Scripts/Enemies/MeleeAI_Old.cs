using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class MeleeAI_Old : MonoBehaviour {
   [Header("Movement")]
   [SerializeField] float chaseForce = 450f;
   [SerializeField] float jumpForce = 1000f;
   [SerializeField] float jumpDist = 6f;
   [SerializeField] float raycastDistance = 2f;
   [SerializeField] float stuckDist = 2f;
   [SerializeField] LayerMask whatIsGround;

   [Header("Animation")]
   public Animator anim;

   [Header("Patrolling")]
   [SerializeField] float patrolForce = 300f;
   [SerializeField] float patrolDist = 10f;
   [SerializeField] float maxPatrolDist = 30f;
   [SerializeField] float patrolWaitTime = 4f;
   int patrolDir = 1;
   float patrolDistTravelled = 0f;
   float timeUntilNextPatrol = 0f;
   bool patrolWaiting = false;

   [Header("Pathfinding")]
   [SerializeField] LayerMask lineOfSightLayers;
   [SerializeField] float nextWaypointDist = 3f;
   [SerializeField] float timeUntilPathUpdate = 0.5f;
   Rigidbody2D target;
   float nextUpdateTime = 0f;

   [Header("Line of Sight")]
   [SerializeField] float sightRange = 25f;
   [SerializeField] float attackRange = 5f;
   Rigidbody2D head;

   [Header("Attacking")]
   [SerializeField] float attackRate = 1f;
   float nextAttackTime = 0f;
   [HideInInspector] public MeleeAI_Attack Weapon;

   // Path info for enemy
   Path path;
   int currentWaypoint = 0;
   bool reachedEndOfPath = false;
   bool stuck = false;
   float stuckDir;

   // Enemy's info on player whereabouts
   bool playerSpotted = false;
   bool searching = false;

   // Status info for enemy
   float maxHealth = 100f;
   float currHealth;

   // For referencing various GameObjects
   Seeker seeker;
   Rigidbody2D rb;

   //---------------------- HELPER FUNCTIONS ----------------------//
   // Updates enemy's path to <target>
   void UpdatePath() {
      if (seeker.IsDone())
         seeker.StartPath(rb.position, target.position, OnPathComplete);
   }

   // Moves enemy towards <target> based on <path>
   void OnPathComplete(Path p) {
      if (!p.error) {
         path = p;
         currentWaypoint = 0;
      }
   }

   // Move back some distance to allow enemy to become unstuck
   void MoveBack() {
      RaycastHit2D hit = Physics2D.Raycast(rb.position, new Vector2(stuckDir, 0).normalized, jumpDist * 2f, whatIsGround);
      stuck = hit.collider != null;

      rb.velocity = new Vector2(-stuckDir * (chaseForce * Time.deltaTime), rb.velocity.y);
   }

   // Check which animation to play based on <xDir>
   void Animation(float xDir) {
      if (xDir >= 0.01f) {
         anim.SetBool("Walk", true);
         anim.SetBool("WalkBack", false);
      }
      else if (xDir <= 0.01f) {
         anim.SetBool("Walk", false);
         anim.SetBool("WalkBack", true);
      }
      else {
         anim.SetBool("Walk", false);
         anim.SetBool("WalkBack", false);
      }
   }

   // Returns -1 if <target> is to left of enemy
   // Returns 1 if <target> is to right of enemy
   int GetDirectionToPlayer() {
      Vector2 dir = (target.position - head.position).normalized;
      if (dir.x < 0)
         return -1;
      else
         return 1;
   }
   //--------------------------------------------------------------//

   //----------------------- BOOL FUNCTIONS -----------------------//
   // Check if enemy is currently grounded
   bool IsGrounded() {
      RaycastHit2D hit = Physics2D.Raycast(rb.position, Vector2.down, raycastDistance, whatIsGround);
      return hit.collider != null;
   }

   // Check if there is an obstacle along the path
   bool ObstacleInPath(float xDir) {
      RaycastHit2D hit = Physics2D.Raycast(rb.position, new Vector2(xDir, 0).normalized, jumpDist, whatIsGround);
      return hit.collider != null;
   }

   // Check if enemy is stuck against environment
   bool IsStuck(float xDir) {
      RaycastHit2D hit = Physics2D.Raycast(rb.position, new Vector2(xDir, 0).normalized, stuckDist, whatIsGround);
      return hit.collider != null;
   }

   // Check if player is within <range> of enemy
   // Enemy cannot see through <whatIsGround>
   bool PlayerInLineOfSight(float range) {
      // See if Raycast2D hits Player
      RaycastHit2D hit = Physics2D.Raycast(head.position, (target.position - head.position), range, lineOfSightLayers);

      if (hit.collider != null) {
         if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Player")) {
            if (Time.time >= nextUpdateTime) {
               UpdatePath();
               nextUpdateTime = Time.time + timeUntilPathUpdate;
            }
            return true;
         }
      }

      return false;
   }

   // Checks if attack timer is finished for enemy
   bool AttackNotOnCooldown() {
      if (Time.time >= nextAttackTime) {
         nextAttackTime = Time.time + (1 / attackRate);
         return true;
      }

      return false;
   }
   //--------------------------------------------------------------//

   //---------------------- STATE FUNCTIONS -----------------------//
   // Handles Animations + Physics for movement
   void Movement() {
      // No path, return out of function
      if (path == null)
         return;

      // Check if end of path was reached
      if (currentWaypoint >= path.vectorPath.Count) {
         reachedEndOfPath = true;
         return;
      }
      else
         reachedEndOfPath = false;

      // Set direction for enemy movement
      Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
      float xDir = direction.x;

      // Move backwards and retry path if stuck
      if (!stuck && IsStuck(xDir)) {
         stuck = true;
         stuckDir = xDir;
      }

      if (stuck) {
         MoveBack();
         xDir = rb.velocity.normalized.x;
      }
      else {
         // Check if obstacle in the way & jump over if necessary
         if (IsGrounded() && ObstacleInPath(xDir))
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.deltaTime);

         // Add x-velocity to enemy
         rb.velocity = new Vector2(xDir * (chaseForce * Time.deltaTime), rb.velocity.y);
      }

      Animation(xDir);

      // Check if need to move onto next waypoint along <path>
      float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
      if (distance < nextWaypointDist)
         currentWaypoint++;
   }

   // Default state for enemies (patrol)
   void Idle() {
      Vector2 startPos = rb.position;

      // Wait for <patrolWaitTime> seconds once reached end of patrol
      if (patrolDistTravelled >= patrolDist) {
         patrolDistTravelled = 0;
         patrolWaiting = true;
         timeUntilNextPatrol = Time.time + patrolWaitTime;
      }

      // See if currently waiting at patrol endpoint
      if (!patrolWaiting) {
         // Move <patrolDist> units to the right/left
         rb.velocity = new Vector2(patrolDir * (patrolForce * Time.deltaTime), rb.velocity.y);
         Animation(patrolDir);
         patrolDistTravelled += Mathf.Abs(rb.velocity.x * Time.deltaTime);
      }
      else {
         if (Time.time >= timeUntilNextPatrol) {
            patrolWaiting = false;
            patrolDir *= -1;
         }
      }
   }

   // State if enemy has previously seen player, but currently out of sight
   // Check last known location and patrol map more aggressively
   void Search() {
      // Move towards last known location
      if (!reachedEndOfPath)
         Movement();
      else {
         // Lost sight of player; patrol more aggressively in current location
         searching = false;
         patrolForce = chaseForce;
         patrolDist = maxPatrolDist;
      }
   }

   // Chase after Player based on A* Pathfinding
   void Chase() {
      // Move enemy along path
      Movement();
   }

   // Attacks Player once in <attackRange>
   void Attack() {

   }
   //--------------------------------------------------------------//

   //--------------------- RUNTIME FUNCTIONS ----------------------//
   // Start is called before the first frame update
   void Start() {
      // Get Seeker & Rigidbody2D instances
      seeker = GetComponent<Seeker>();
      rb = this.transform.Find("Hips").GetComponent<Rigidbody2D>();
      head = this.transform.Find("Head").GetComponent<Rigidbody2D>();

      // Get <target> from Player Asset
      target = GameObject.Find("/Player_Stickman/Head").GetComponent<Rigidbody2D>();

      // Get <weapon> from Enemy prefab
      Weapon = transform.Find("RightHand").GetComponent<MeleeAI_Attack>();
      // Initialize starting attributes
      currHealth = maxHealth;
   }

   // Update is called once per frame
   void FixedUpdate() {
      // Check if player within line-of-sight
      if (PlayerInLineOfSight(sightRange)) {
         playerSpotted = true;
         searching = true;
      }
      else
         playerSpotted = false;

      // Default to Idle state
      if (!playerSpotted && !searching)
         Idle();
      else if (!playerSpotted && searching)
         Search();
        else if (PlayerInLineOfSight(attackRange) && AttackNotOnCooldown())
            Weapon.Attack(GetDirectionToPlayer());
        else
         Chase();
   }
   //--------------------------------------------------------------//
}
