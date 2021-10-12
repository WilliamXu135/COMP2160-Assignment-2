using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class VehicalControls : MonoBehaviour
{
    private Rigidbody rigidbodyComponent;
   
    public float force;
    public float turningForce;

    private float velocity;
    private float rotate;

    // To use in fixed update for controls
    private float accelerate;
    private float turning;

    void Start()
    {
        rigidbodyComponent = GetComponent<Rigidbody>();
    }

    void Update()
    {
        accelerate = Input.GetAxis(InputAxes.Vertical);
        turning = Input.GetAxis(InputAxes.Horizontal);

        velocity = accelerate * force;
        rotate = turning * turningForce;
    }

    private void FixedUpdate()
    {
        // Car Accelerates forward with drag applied
        rigidbodyComponent.AddRelativeForce(velocity * Time.fixedDeltaTime * Vector3.forward);
        rigidbodyComponent.AddForce(rigidbodyComponent.drag * Time.fixedDeltaTime * Vector3.back);
    }
}
