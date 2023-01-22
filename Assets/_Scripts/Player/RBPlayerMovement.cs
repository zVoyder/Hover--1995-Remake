using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RBPlayerMovement : MonoBehaviour
{
    public Rigidbody m_rigidBody;
    [Range(0, 100)]
    public float m_maxSpeed; //maximum speed reachable (can be setted in the inspetor)
    [Range(1, 100)]
    public float m_rotationForce; //force of the rotation (can be setted in the inspetor)


    void Start() // Start is called before the first frame update
    {
        m_rigidBody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() //updates every 0.02 seconds to adapt to the physics
    {
        if (Input.GetKey(InputManager.FORWARD))//forward acceleration key binding
        {
            m_rigidBody.AddForce(transform.forward * m_maxSpeed, ForceMode.Acceleration); //forward acceleration
        }

        if (Input.GetKey(InputManager.BACKWARD))//backward acceleration key binding
        {
            m_rigidBody.AddForce(-transform.forward * m_maxSpeed, ForceMode.Acceleration); //backward acceleration
        }
    }

    void Update() // Update is called once per frame
    {

        if (Input.GetKey(InputManager.TURNLEFT))//left rotation key binding
        {
            transform.Rotate(Vector3.up, -m_rotationForce * Time.deltaTime); //effective left rotation
        }
        
        if (Input.GetKey(InputManager.TURNRIGHT)) //right rotation key binding
        {
            transform.Rotate(Vector3.up, m_rotationForce * Time.deltaTime); //effective right rotation
        }
    }
}
