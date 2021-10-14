using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class VehicalControls : MonoBehaviour
{
    private Rigidbody rb;

    // Acceleration and turning force
    public float force;
    public float turningForce;

    private float velocity;                             // Car speed
    private float rotate;                               // Degrees a second - car turning radius
    private float velScaler;                            // To scale the magnitude of velocity for
                                                        // controlling rotation

    // To use in fixed update for controls
    private float accelerate;
    private float turn;

    // Used for detecting if the car is on the ground
    public LayerMask layer;
    private float distance;
    private float dist;
    private Vector3 transformOffset;
    private float maxDistFromGround;
    private float turnDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        velScaler = 10.0f;

        transformOffset = new Vector3(0, 0.5f, 0);
        distance = float.PositiveInfinity;
        maxDistFromGround = 0.7f;
    }

    void Update()
    {
        // Control Axis
        accelerate = Input.GetAxis(InputAxes.Vertical);
        turn = Input.GetAxis(InputAxes.Horizontal);

        velocity = accelerate * force;
       
        if (accelerate <0)
        {
            turnDirection = -1;
        }
        else if(accelerate>0)
        {
            turnDirection = 1; 
        }
        rotate = turn * turningForce *turnDirection;
    }

    private void FixedUpdate()
    {
        float deltaTurnspeed;

        if (this.DistanceToGround() < maxDistFromGround)
        {
            // Car Accelerates with W and S keys with drag applied
            rb.AddRelativeForce(velocity * Time.fixedDeltaTime * Vector3.forward, ForceMode.Acceleration);
            rb.AddRelativeForce(rb.drag * Time.fixedDeltaTime * Vector3.back);

            //Allows turning when W/S key not being pressed
            rotate *= (rb.velocity.magnitude / velScaler);
            deltaTurnspeed = rotate * Time.fixedDeltaTime;

            rb.AddRelativeTorque(deltaTurnspeed * Vector3.up, ForceMode.VelocityChange);
        }
        // Stop car from sliding
    }

    private float DistanceToGround()
    {
        Vector3 pos = transform.position + transformOffset;
        Vector3 dir = Vector3.down;

        RaycastHit hit;

        if (Physics.Raycast(pos, dir, out hit, distance, layer))
        {
            dist = hit.distance;
        }
        return dist;
    }

}
