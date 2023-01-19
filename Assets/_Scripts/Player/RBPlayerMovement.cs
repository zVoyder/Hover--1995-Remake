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


    // Start is called before the first frame update
    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            m_rigidBody.AddForce(transform.forward * m_maxSpeed, ForceMode.Acceleration);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            m_rigidBody.AddForce(-transform.forward * m_maxSpeed, ForceMode.Acceleration);
        }

        //rotation to left
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(Vector3.up, -m_rotationForce * Time.deltaTime); //effective left rotation
        }
        //rotation to right
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(Vector3.up, m_rotationForce * Time.deltaTime); //effective right rotation
        }
    }
}
