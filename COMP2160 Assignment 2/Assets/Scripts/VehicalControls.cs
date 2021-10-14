using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class VehicalControls : MonoBehaviour
{
    private Rigidbody rigidbodyComponent;
   
    // Acceleration and turning force
    public float force;
    public Vector3 turningForce;

    private float velocity;                     // Car speed
    private Vector3 rotate;                     // Degrees a second - car turning radius
    private float velocityScaler;               // To scale the magnitude of velocity for
                                                // controlling rotation

    // To use in fixed update for controls
    private float accelerate;
    private float turning;
    
    // Turning radius of car scaled with fixedDeltaTime
    private Quaternion deltaRotation;

    // Used for detecting if the car is on the ground
    private float clearanceError;
    private bool grounded;
    private float distanceToGround;
    private Collider carCollider;
    public LayerMask layer;

    void Start()
    {
        rigidbodyComponent = GetComponent<Rigidbody>();
        carCollider = GetComponent<Collider>();
        clearanceError = 0.1f;
        velocityScaler = 10.0f;
        distanceToGround = carCollider.bounds.extents.y + clearanceError;
    }

    void Update()
    {
        Debug.Log(distanceToGround);
        // Control Axis
        accelerate = Input.GetAxis(InputAxes.Vertical);
        turning = Input.GetAxis(InputAxes.Horizontal);

        velocity = accelerate * force;
        rotate = turning * turningForce;
    }

    private void FixedUpdate()
    {
        grounded = Physics.Raycast(rigidbodyComponent.position,
             rigidbodyComponent.transform.TransformDirection(Vector3.down),
             distanceToGround + clearanceError, layer);

        if (grounded)
        {
            // Car Accelerates with W and S keys with drag applied
            rigidbodyComponent.AddRelativeForce(velocity * Time.fixedDeltaTime * Vector3.forward,ForceMode.Acceleration);

            rigidbodyComponent.AddForce(rigidbodyComponent.drag * 
                rigidbodyComponent.angularDrag * Time.fixedDeltaTime * Vector3.back);

            // Car turning with A / D keys
            deltaRotation = Quaternion.Euler(rotate * Time.fixedDeltaTime *
                rigidbodyComponent.velocity.magnitude / velocityScaler);

            rigidbodyComponent.MoveRotation(rigidbodyComponent.rotation * deltaRotation);
        }
    }
}
