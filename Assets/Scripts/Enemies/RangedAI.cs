using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class RangedAI : MonoBehaviour {
   [Header("Movement")]
   [SerializeField] float chaseForce = 1300f;
   [SerializeField] float jumpForce = 2000f;
   [SerializeField] float jumpDist = 7f;
   [SerializeField] float raycastDistance = 4f;
   [SerializeField] float stuckDist = 2f;
   [SerializeField] LayerMask whatIsGround;

   [Header("Retreat")]
   [SerializeField] float retreatForce = 800f;
   [SerializeField] float retreatDist = 10f;

   [Header("Animation")]
   public Animator anim;

   [Header("Pathfinding")]
   [SerializeField] LayerMask lineOfSightLayers;
   [SerializeField] float nextWaypointDist = 3f;
   [SerializeField] float timeUntilPathUpdate = 0.5f;
   [SerializeField] bool willChasePlayer = false;
   [SerializeField] bool willMove = false;
   float nextUpdateTime = 0f;
   Rigidbody2D target;

   [Header("Line of Sight")]
   [SerializeField] float sightRange = 40f;
   [SerializeField] float maxSightRange = 50f;
   Rigidbody2D head;

   [Header("Attacking")]
   [SerializeField] float attackRange = 25f;
   Rigidbody2D rightHand;
   RangedAI_Attack crossbow;
   bool aiming = false;
   bool shot = false;

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

   // For referencing this object's Seeker & Rigidbody2D components
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

   // Returns the angle needed to shoot Player
   float GetRotZ() {
      // float offset = 0;

      // float shotForce = crossbow.GetShotForce();
      // float dist = (target.position - head.position).x;
      // float angle = 0.5f * (Mathf.Asin ((-Physics.gravity.y * dist) / (shotForce * shotForce)) * Mathf.Rad2Deg);
      // return (angle + offset);

      Vector2 dist = (target.position - head.position);
      float angle = Mathf.Atan2(dist.y, dist.x) * Mathf.Rad2Deg;
      //Debug.Log(angle);
      return angle;
   }

   // Makes enemy stop moving to take aim at Player with <crossbow>
   IEnumerator TakeAim(float rotZ, float duration) {
      float progress = 0;

      while (progress < duration) {
         rightHand.MoveRotation(rotZ + 5f * progress);
         progress += Time.deltaTime;
         yield return null;
      }

      shot = false;
      aiming = false;
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

   // Check if player is within <sightRange> of enemy
   // Enemy cannot see through <whatIsGround>
   bool PlayerInLineOfSight() {
      // See if Raycast2D hits Player
      RaycastHit2D hit = Physics2D.Raycast(head.position, (target.position - head.position), sightRange, lineOfSightLayers);

      // Return true and update path if enemy can see player within <sightRange>
      if (hit.collider != null) {
         if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Player")) {
            if (Time.time >= nextUpdateTime) {
               UpdatePath();
               nextUpdateTime = Time.time + timeUntilPathUpdate;
            }
            return true;
         }
      }

      // Return false and do not update path otherwise
      return false;
   }

   // Check if player is within <retreatDist> of enemy
   bool PlayerTooClose() {
      // See if Raycast2D hits Player
      RaycastHit2D hit = Physics2D.Raycast(head.position, (target.position - head.position), retreatDist, lineOfSightLayers);

      // Return true if enemy can see player within <retreatDist>
      if (hit.collider != null)
         if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
            return true;

      // Return false otherwise
      return false;
   }

   // Check if player is within <attackRange> of enemy
   bool PlayerInAttackRange() {
      // See if Raycast2D hits Player
      RaycastHit2D hit = Physics2D.Raycast(head.position, (target.position - head.position), attackRange, lineOfSightLayers);

      // Return true if enemy can see player within <retreatDist>
      if (hit.collider != null)
         if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
            return true;

      // Return false otherwise
      return false;
   }
   //--------------------------------------------------------------//

   //---------------------- STATE FUNCTIONS -----------------------//
   // Handles Animations + Physics for movement
   void Movement(float xDir, float movementForce) {
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
         rb.velocity = new Vector2(xDir * (movementForce * Time.deltaTime), rb.velocity.y);
      }

      Animation(xDir);
   }

   // State if enemy has previously seen player, but currently out of sight
   // Check last known location and patrol map more aggressively
   void Search() {
      // Move towards last known location
      if (!reachedEndOfPath)
         Chase();
      else {
         // Lost sight of player; be more vigilant for player
         searching = false;
         sightRange = maxSightRange;
      }
   }

   // Chase after Player based on A* Pathfinding
   void Chase() {
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

      // Move enemy along path
      Movement(xDir, chaseForce);

      // Check if need to move onto next waypoint along <path>
      float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
      if (distance < nextWaypointDist)
         currentWaypoint++;
   }

   // Keep distance between enemy and player
   void Retreat() {
      // Get x-component of direction away from player
      float retreatDir = -((target.position - head.position).normalized.x);

      // Move enemy away from player with <retreatForce>
      // <retreatForce> is lower than <chaseForce> so enemy moves slower
      Movement(retreatDir, retreatForce);
   }

   // Shoot <crossbow> at Player
   IEnumerator ShootAtPlayer() {
      float rotZ = GetRotZ();
      int offset = 0;

      if (!float.IsNaN(rotZ)) {
            //print("rotZ is not NaN");
         // Player is to the left of enemy
         if ((target.position - head.position).x < 0) {
            offset = -20;
            //rotZ += 180;
         }

         rotZ += offset;

         // Fire <crossbow> if able and stop moving to take aim
         aiming = true;
         yield return TakeAim(rotZ, 2);
         StartCoroutine(crossbow.Shoot());
      }
   }
   //--------------------------------------------------------------//

   //--------------------- RUNTIME FUNCTIONS ----------------------//
   // Start is called before the first frame update
   void Start() {
      // Get Seeker & Rigidbody2D instances
      seeker = GetComponent<Seeker>();
      rb = this.transform.Find("Hips").GetComponent<Rigidbody2D>();
      head = this.transform.Find("Head").GetComponent<Rigidbody2D>();
      rightHand = this.transform.Find("RightHand").GetComponent<Rigidbody2D>();

      // Get <target> from Player Asset
      target = GameObject.Find("/Player_Stickman/Head").GetComponent<Rigidbody2D>();

      // Get reference to crossbow
      crossbow = this.transform.Find("RightHand/Crossbow").GetComponent<RangedAI_Attack>();

      // Initialize starting attributes
      currHealth = maxHealth;
   }

   // Update is called once per frame
   void FixedUpdate() {
      // Check if player within line-of-sight
      if (PlayerInLineOfSight()) {
         playerSpotted = true;
         searching = true;
      }
      else
         playerSpotted = false;

      // Default to Idling
      if (!playerSpotted && !searching)
         return;
      // Enemy knows player in area but not exact location
      else if (!aiming && !playerSpotted && searching && willMove)
         Search();
      // Enemy sees player
      else {
         // Only certain ranged enemies will hunt down player
         if (!aiming && willChasePlayer && willMove)
            Chase();

         // Move back from player if too close
         if (!aiming && willMove && PlayerTooClose())
            Retreat();

         // Shoot at Player if in range
         if (!shot && PlayerInAttackRange() && crossbow.CanShoot()) {
            shot = true;
            StartCoroutine(ShootAtPlayer());
         }
      }
   }
   //--------------------------------------------------------------//
}
