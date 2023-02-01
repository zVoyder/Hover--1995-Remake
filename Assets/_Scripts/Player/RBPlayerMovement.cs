using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RBPlayerMovement : MonoBehaviour
{
    public Rigidbody m_rigidBody;
    [Range(0, 100)]
    public float m_acceleration; //acceleration rate (can be setted in the inspetor)
    [Range(1, 100)]
    public float m_maxSpeed; //maximum speed reachable (can be setted in the inspetor)
    [Range(1, 100)]
    public float m_rotationForce; //force of the rotation (can be setted in the inspetor)
    [HideInInspector]
    public float m_timer = 0; //timer to count the seconds of the movement stop
    public bool CanMove { get; set; } = true;//wrapper to enable player to move
    [Range(1, 50)]
    public float m_movementStopDuration; //for how long does the player can't move
    public string m_immobilizerObjectTag = "Immobilize Platform"; //set the object that immobilize the player
    private AudioSource m_audioSource;


    void Start() // Start is called before the first frame update
    {
        m_rigidBody = GetComponent<Rigidbody>();
        m_audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate() //updates every 0.02 seconds to adapt to the physics
    {
        if (CanMove)
        {

            if (Input.GetKey(KeyCode.UpArrow))//forward acceleration key binding
            {
                if (m_rigidBody.velocity.magnitude < m_maxSpeed)
                {
                    //m_audioSource.Play(); //play movement audio
                    m_rigidBody.AddForce(transform.forward * m_acceleration, ForceMode.Acceleration); //forward acceleration
                }
                if (m_rigidBody.velocity.magnitude > m_maxSpeed)
                {
                    m_rigidBody.AddForce(transform.forward * m_maxSpeed, ForceMode.Acceleration); //forward max speed
                }
            }

            if (Input.GetKey(KeyCode.DownArrow))//backward acceleration key binding
            {
                if (m_rigidBody.velocity.magnitude < m_maxSpeed)
                {
                    //m_audioSource.Play(); //play movement audio
                    m_rigidBody.AddForce(-transform.forward * m_acceleration, ForceMode.Acceleration); //backward acceleration
                }
                if (m_rigidBody.velocity.magnitude > m_maxSpeed)
                {
                    m_rigidBody.AddForce(-transform.forward * m_maxSpeed, ForceMode.Acceleration); //backward max speed
                }
            }
        }
    }

    void Update() // Update is called once per frame
    {

        if (Input.GetKey(KeyCode.LeftArrow))//left rotation key binding
        {
            transform.Rotate(Vector3.up, -m_rotationForce * Time.deltaTime); //effective left rotation
        }

        if (Input.GetKey(KeyCode.RightArrow)) //right rotation key binding
        {
            transform.Rotate(Vector3.up, m_rotationForce * Time.deltaTime); //effective right rotation
        }
    }
}
