using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour {
   // For VineShot
   [SerializeField] GameObject vineShot;
   [SerializeField] float windUpTime = 3f;
   [SerializeField] float stayTime = 2f;
   Rigidbody2D player;
   float offset;
   float speed = 5f;
   float stopTrackingTime;
   float endTime;

   // For Out-Of-Bounds (OOB) checking
   float OOBLeft;
   float OOBRight;

   public void SetTarget(Rigidbody2D player, float offset) {
      this.player = player;
      this.offset = offset;
   }

   // Move indicator towards Player
   void TrackPlayer() {
      // Make sure Indicator doesn't go out of arena
      float targetX = player.position.x + offset;
      if (targetX <= OOBLeft)
         targetX = OOBLeft;
      else if (targetX >= OOBRight)
         targetX = OOBRight;

      Vector2 target = new Vector2(targetX, player.position.y);
      Vector3 dirToPlayer = ((Vector3)target - this.transform.position).normalized;
      this.transform.position += new Vector3(dirToPlayer.x * speed, 0, 0);
   }

   // Shoot vine out of ground and delte indicator
   void VineShot() {
      // Shoot vine
      GameObject newVineShot = Instantiate(vineShot, this.transform.position, Quaternion.identity);

      // Delete indicator
      Destroy(this.gameObject);
   }

   // Start is called before the first frame update
   void Start() {
      // Set instance variables
      stopTrackingTime = Time.time + windUpTime;
      endTime = Time.time + windUpTime + stayTime;

      // Get components from Hierarchy
      OOBLeft = GameObject.Find("Environment/Arena/Walls/LeftWall").transform.position.x;
      OOBRight = GameObject.Find("Environment/Arena/Walls/RightWall").transform.position.x;
   }

   // Update is called once per frame
   void Update() {
      if (player != null && Time.time < stopTrackingTime)
         TrackPlayer();
      else if (Time.time >= endTime)
         VineShot();
   }
}
