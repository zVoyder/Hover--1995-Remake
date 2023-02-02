using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RBPlayerMovement : MonoBehaviour
{
    internal Rigidbody rigidBody;

    [Header("Movement")]
    [Range(0, 100)]
    public float acceleration = 0; //acceleration rate (can be setted in the inspetor)
    [Range(1, 100)]
    public float maxSpeed = 15; //maximum speed reachable (can be setted in the inspetor)
    [Range(1, 100)]
    public float rotationForce = 70; //force of the rotation (can be setted in the inspetor)
    [HideInInspector]
    public float timer = 0; //timer to count the seconds of the movement stop
    public bool CanMove { get; set; } = true;//wrapper to enable player to move

    [Header("Stair Steps Raycasts")]
    public float upperDistance;
    public float upperOffset;
    public float lowerDistance;
    public float lowerOffset;
    public float stepHeight;

    public LayerMask climbableLayers;

    void Start() // Start is called before the first frame update
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() //updates every 0.02 seconds to adapt to the physics
    {
        if (CanMove)
        {
            if (Input.GetKey(InputManager.FORWARD))//forward acceleration key binding
            {
                if (rigidBody.velocity.magnitude < maxSpeed)
                {
                    rigidBody.AddForce(transform.forward * acceleration, ForceMode.Acceleration); //forward acceleration
                }

                if (rigidBody.velocity.magnitude > maxSpeed)
                {
                    rigidBody.AddForce(transform.forward * maxSpeed, ForceMode.Acceleration); //forward max speed
                }

                StepClimb(); // Only when he's moving forward can climb the stairs
            }

            if (Input.GetKey(InputManager.BACKWARD))//backward acceleration key binding
            {
                if (rigidBody.velocity.magnitude < maxSpeed)
                {
                    //m_audioSource.Play(); //play movement audio
                    rigidBody.AddForce(-transform.forward * acceleration, ForceMode.Acceleration); //backward acceleration
                }
                if (rigidBody.velocity.magnitude > maxSpeed)
                {
                    rigidBody.AddForce(-transform.forward * maxSpeed, ForceMode.Acceleration); //backward max speed
                }
            }
        }

    }

    void Update() // Update is called once per frame
    {

        if (Input.GetKey(InputManager.TURNLEFT))//left rotation key binding
        {
            
            transform.Rotate(Vector3.up, -rotationForce * Time.deltaTime); //effective left rotation
        }

        if (Input.GetKey(InputManager.TURNRIGHT)) //right rotation key binding
        {
            transform.Rotate(Vector3.up, rotationForce * Time.deltaTime); //effective right rotation
        }


#if DEBUG
        Vector3 lowerPos = new Vector3(transform.position.x, transform.position.y + lowerOffset, transform.position.z);
        Vector3 upperPos = new Vector3(transform.position.x, transform.position.y + upperOffset, transform.position.z);

        Debug.DrawRay(upperPos, transform.forward * lowerDistance);
        Debug.DrawRay(lowerPos, transform.forward * upperDistance);
#endif

    }

    void StepClimb()
    {
        Vector3 lowerPos = new Vector3(transform.position.x, transform.position.y + lowerOffset, transform.position.z);
        Vector3 upperPos = new Vector3(transform.position.x, transform.position.y + upperOffset, transform.position.z);

        if (Physics.Raycast(lowerPos, transform.forward, out RaycastHit hitLower, lowerDistance, climbableLayers))
        {
            //Debug.Log("hitLow");

            if (!Physics.Raycast(upperPos, transform.forward, out RaycastHit hitUpper, upperDistance, climbableLayers))
            {
                //Debug.Log("hitUpper");

                transform.position += new Vector3(0, stepHeight, 0);
            }
        }
    }


}
