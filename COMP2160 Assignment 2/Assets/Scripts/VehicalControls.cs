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


    private float velocity;             // Car speed
    private Vector3 rotate;             //Degrees a second - car turning radius

    // To use in fixed update for controls
    private float accelerate;
    private float turning;


    private Quaternion deltaRotation;

    void Start()
    {
        rigidbodyComponent = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Control Axis
        accelerate = Input.GetAxis(InputAxes.Vertical);
        turning = Input.GetAxis(InputAxes.Horizontal);


        velocity = accelerate * force;
        rotate = turning * turningForce;
    }

    private void FixedUpdate()
    {


        // Car Accelerates with W and S keys with drag applied
        rigidbodyComponent.AddRelativeForce(velocity * Time.fixedDeltaTime * Vector3.forward);
        rigidbodyComponent.AddForce(rigidbodyComponent.drag * Time.fixedDeltaTime * Vector3.back);

        // Car turning with A / D keys
        deltaRotation = Quaternion.Euler(rotate * Time.fixedDeltaTime);
        rigidbodyComponent.MoveRotation(rigidbodyComponent.rotation * deltaRotation);
    }
}
