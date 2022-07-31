using System;
using System.Collections;
using UnityEngine;

public class Tutorial_GrapplingGun : MonoBehaviour
{
    [Header("Scripts Ref:")]
    public Tutorial_GrapplingRope grappleRope;

    [Header("Layers Settings:")]
    //[SerializeField] private bool grappleToAll = false;
    [SerializeField] private LayerMask nonGrappableLayer;

    [Header("Transform Ref:")]
    public Transform gunHolder;
    public Transform gunPivot;
    public Transform firePoint;

    [Header("Physics Ref:")]
    public SpringJoint2D m_springJoint2D;
    public Rigidbody2D m_rigidbody;

    //[Header("Rotation:")]
    //[SerializeField] private bool rotateOverTime = true;
    //[Range(0, 60)] [SerializeField] private float rotationSpeed = 4;

    [Header("Distance:")]
    [SerializeField] private bool hasMaxDistance = false;
    [SerializeField] private float maxDistance = 20;

    private enum LaunchType
    {
        Transform_Launch,
        Physics_Launch
    }

    [Header("Launching:")]
    [SerializeField] private bool launchToPoint = true;
    [SerializeField] private LaunchType launchType = LaunchType.Physics_Launch;
    [SerializeField] private float launchSpeed = 1;

    [Header("No Launch To Point")]
    [SerializeField] private bool autoConfigureDistance = false;
    [SerializeField] public float targetDistance = 1.3f;
    private float tempTargetDistance;
    [SerializeField] private float targetFrequncy = 1;

    [HideInInspector] public Vector2 grapplePoint;
    [HideInInspector] public Vector2 grappleDistanceVector;

    static Tutorial_GrapplingGun instance;

    public static Tutorial_GrapplingGun GetInstance()
    {
        return instance;
    }

    public static bool didGrapple; // Variable for the Cooldown.cs script to make it run.

    private void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        grappleRope.enabled = false;
        m_springJoint2D.enabled = false;
    }

    private void FixedUpdate()
    {
        if (grappleRope.enabled)
        {
            GrappleControl();
        }
    }

    private void Update()
    {
        if (!Cooldown.isGrappleCooldown && !PauseMenu.GamePaused)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                SetGrapplePoint();
            }
            else if (Input.GetKey(KeyCode.Mouse1))
            {
                if (grappleRope.enabled)
                {
                    //GrappleControl();
                    //RotateGun(grapplePoint, false);
                }
                else
                {
                    Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    //RotateGun(mousePos, true);
                }

                if (launchToPoint && grappleRope.isGrappling)
                {
                    if (launchType == LaunchType.Transform_Launch)
                    {
                        Vector2 firePointDistnace = firePoint.position - gunHolder.localPosition;
                        Vector2 targetPos = grapplePoint - firePointDistnace;
                        gunHolder.position = Vector2.Lerp(gunHolder.position, targetPos, Time.deltaTime * launchSpeed);
                    }
                }
            }
        }
        else if (!Input.GetKey(KeyCode.Mouse1) && grappleRope.isGrappling)
        {
            AudioManager.GetInstance().Play("Grapple Hook Done");
            // Once you grapple is cancelled, reset these variables.
            targetDistance = 1.3f;
            grappleRope.enabled = false;
            m_springJoint2D.enabled = false;
            m_rigidbody.gravityScale = 1;
        }
        else
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //RotateGun(mousePos, true);
        }
    }

    // RotateGun() and its calls are commented out because we want the gun to be
    // always attached to the player's hand
    //
    //void RotateGun(Vector3 lookPoint, bool allowRotationOverTime)
    //{
    //    Vector3 distanceVector = lookPoint - gunPivot.position;

    //    float angle = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;
    //    if (rotateOverTime && allowRotationOverTime)
    //    {
    //        gunPivot.rotation = Quaternion.Lerp(gunPivot.rotation, Quaternion.AngleAxis(angle, Vector3.forward), Time.deltaTime * rotationSpeed);
    //    }
    //    else
    //    {
    //        gunPivot.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    //    }
    //}

    void SetGrapplePoint()
    {
        // Ignore the Player, Enemy, and PlayerRangedWeapon layer when shooting the grapple hook
        int bitmask = ~(1 << 6) & ~(1 << 10) & ~(1 << 15);
        //Debug.Log(Convert.ToString(bitmask, 2).PadLeft(32, '0'));

        Vector2 distanceVector = Camera.main.ScreenToWorldPoint(Input.mousePosition) - gunPivot.position;
        if (Physics2D.Raycast(firePoint.position, distanceVector.normalized, maxDistance, bitmask))
        {
            RaycastHit2D _hit = Physics2D.Raycast(firePoint.position, distanceVector.normalized, maxDistance, bitmask);

                if ((Vector2.Distance(_hit.point, firePoint.position) <= maxDistance && !_hit.collider.CompareTag("Non-grappable")) || !hasMaxDistance)
                {
                    // Play the grappling hook sound effect
                    AudioManager.GetInstance().Play("Grapple Hook Fire");

                    grapplePoint = _hit.point;
                    grappleDistanceVector = grapplePoint - (Vector2)gunPivot.position;
                    grappleRope.enabled = true;
                    didGrapple = true;
                }
                else
                {
                    didGrapple = false;
                }
        }
        // Else if the Player shot at nothing then they didn't grapple. Don't run the Cooldown script.
        else
        {
            didGrapple = false;
        }
    }

    public void Grapple()
    {
        //Debug.Log("targetDistance = " + targetDistance);

        m_springJoint2D.autoConfigureDistance = false;
        if (!launchToPoint && !autoConfigureDistance)
        {
            m_springJoint2D.distance = targetDistance;
            m_springJoint2D.frequency = targetFrequncy;
        }
        if (!launchToPoint)
        {
            if (autoConfigureDistance)
            {
                m_springJoint2D.autoConfigureDistance = true;
                m_springJoint2D.frequency = 0;
            }

            m_springJoint2D.connectedAnchor = grapplePoint;
            m_springJoint2D.enabled = true;
        }
        else
        {
            switch (launchType)
            {
                case LaunchType.Physics_Launch:
                    m_springJoint2D.connectedAnchor = grapplePoint;

                    Vector2 distanceVector = firePoint.position - gunHolder.position;

                    m_springJoint2D.distance = distanceVector.magnitude;
                    m_springJoint2D.frequency = launchSpeed;
                    m_springJoint2D.enabled = true;
                    break;
                case LaunchType.Transform_Launch:
                    m_rigidbody.gravityScale = 0;
                    m_rigidbody.velocity = Vector2.zero;
                    break;
            }
        }
    }

    private void GrappleControl()
    {
        //tempTargetDistance = targetDistance;

        //Debug.Log("GrappleControl entered but not truly triggered");
        // Allow the player to retract from grappling.
        if (Input.GetKey(KeyCode.Q) && targetDistance > 0.3)
        {
            AudioManager.GetInstance().Play("Grapple Hook Retract");
            //Debug.Log("GrappleControl RETRACT (Q) triggered and targetDistance = " + targetDistance);
            targetDistance = targetDistance - 0.5f;
            m_springJoint2D.distance = targetDistance;

            if (!(targetDistance > 0.3))
                AudioManager.GetInstance().Play("Grapple Hook Retract Complete");
        }

        // Allow the player to protract from grappling.
        if (Input.GetKey(KeyCode.E) && targetDistance < maxDistance)
        {
            AudioManager.GetInstance().Play("Grapple Hook Protract");
            //Debug.Log("GrappleControl PROTRACT (E) triggered and targetDistance =" + targetDistance);
            targetDistance = targetDistance + 0.5f;
            m_springJoint2D.distance = targetDistance;

            if (!(targetDistance < maxDistance))
                AudioManager.GetInstance().Play("Grapple Hook Protract Complete");
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (firePoint != null && hasMaxDistance)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(firePoint.position, maxDistance);
        }
    }

}