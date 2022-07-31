using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arena1 : MonoBehaviour {
   [Header("Arena Attacks")]
   [SerializeField] float minTimeBtwnAttacks = 5f;
   [SerializeField] float maxTimeBtwnAttacks = 10f;
   [SerializeField] GameObject indicator;
   [HideInInspector] Rigidbody2D target;
   [SerializeField] GameObject vineFloor;
   float nextAttackTime;
   int phase;
   Transform floor;
   Transform leftWall;
   Transform rightWall;
   const float OFFSETX = 25f;
   const float OFFSETY = 1.5f;

   // Change phases
   public void ChangePhase(int phase) {
      this.phase = phase;
   }

   // Make floor of Arena hazardous and stop this script
   public void VineifyFloor() {
      // Spawn vines on floor starting from <leftWall> and <rightWall>
      GameObject leftVineFloor = Instantiate(vineFloor, new Vector2(leftWall.position.x, floor.position.y+OFFSETY), vineFloor.transform.rotation);
      GameObject rightVineFloor = Instantiate(vineFloor, new Vector2(rightWall.position.x, floor.position.y+OFFSETY), vineFloor.transform.rotation * Quaternion.Euler(0, 180f, 0));

      // Stop script
      Destroy(this);
   }

   // Shoot vine from <floor> of arena after <windUpTime>
   // Vine shot locks-in after half of <windUpTime>
   void FloorVine(int phase) {
      if (phase == 1) {
         GameObject newIndicator = Instantiate(indicator, new Vector2(target.position.x, floor.position.y), indicator.transform.rotation);
         newIndicator.GetComponent<Indicator>().SetTarget(target, 0);
      }
      else if (phase == 2) {
         // Center indicator
         GameObject centerIndicator = Instantiate(indicator, new Vector2(target.position.x, floor.position.y), indicator.transform.rotation);
         centerIndicator.GetComponent<Indicator>().SetTarget(target, 0);

         // Left indicator
         GameObject leftIndicator = Instantiate(indicator, new Vector2(target.position.x - OFFSETX, floor.position.y), indicator.transform.rotation);
         leftIndicator.GetComponent<Indicator>().SetTarget(target, -OFFSETX);

         // Right indicator
         GameObject rightIndicator = Instantiate(indicator, new Vector2(target.position.x + OFFSETX, floor.position.y), indicator.transform.rotation);
         rightIndicator.GetComponent<Indicator>().SetTarget(target, OFFSETX);
      }
   }

   // Start is called before the first frame update
   void Start() {
      target = GameObject.Find("/Player_Stickman/Hips").GetComponent<Rigidbody2D>();

      // Get components from Hierarchy
      phase = 1;
      floor = this.transform.Find("Platforms/Floor").GetComponent<Transform>();
      leftWall = this.transform.Find("Walls/LeftWall").GetComponent<Transform>();
      rightWall = this.transform.Find("Walls/RightWall").GetComponent<Transform>();

      // Set <nextAttackTime>
      nextAttackTime = Random.Range(minTimeBtwnAttacks, maxTimeBtwnAttacks);
   }

   // Update is called once per frame
   void Update() {
      if (phase == 3)
         VineifyFloor();

      // Attack if off-cooldown
      if (Time.time >= nextAttackTime) {
         // Reset <nextAttackTime>
         nextAttackTime = Time.time + Random.Range(minTimeBtwnAttacks, maxTimeBtwnAttacks);

         // Perform attack based on phase
         FloorVine(phase);
      }
   }
}
