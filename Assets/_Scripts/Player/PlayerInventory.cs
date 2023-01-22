using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]

public class PlayerInventory : MonoBehaviour
{
    [HideInInspector]
    public int m_springCounter; //counter for how many spring the player have to jump
    [HideInInspector]
    public int m_placeWallCounter; // counter for how many placeable wall does the player have
    [HideInInspector]
    public int m_cloackCounter; //counter for how many cloack for invisibility does the player have
    [Range(0, 150)]
    public float m_jumpForce; //the force of the jump (can be setted in the inspector)
    private bool isGrounded; //bool to be sure the player must be touching the ground or the stairs before jump
    [Range(0, 50)]
    [SerializeField] private float m_buffedMaxSpeed; //max speed of the player when buffed (can be setted in the inspector)
    [Range(0, -50)]
    [SerializeField] private float m_nerfedMaxSpeed; //max speed of the player when nerfed (can be setted in the inspector)
    [Range(0, 30)]
    public float buffDurationSeconds; //duration of the buff/debuff (can be setted in the inspector)
    private bool speedBuff; //bool to enable temporary buff of maximum speed
    private bool speedNerf; //bool to enable temporary nerf of maximum speed
    private float m_buffTimer = 0; //timer to set a duration for the buffs
    private float m_nerfTimer = 0; //timer to set a duration for the nerfs
    private Rigidbody rigidBody;
    Collider sphereCollider;
    RBPlayerMovement pm;

    // Start is called before the first frame update
    void Start()
    { 
        //getting Rigidbody and BoxCollidercomponents
        rigidBody = GetComponent<Rigidbody>();
        sphereCollider = GetComponent<BoxCollider>();
        pm = GetComponent<RBPlayerMovement>();
    }

    void FixedUpdate()
    {
        //bool for unable jumping if not colliding with ground objects as floor and stairs
        if (isGrounded)
        {
            isGrounded = false;
            rigidBody.AddForce(0, m_jumpForce, 0, ForceMode.Impulse);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //capping min number of springs 
        if (m_springCounter < 0)
        {
            m_springCounter = 0;
        }
        //jumping input
        if (m_springCounter > 0 && isGrounded && Input.GetKeyDown(InputManager.JUMP))
        {
            //jump effective movement
            rigidBody.AddForce(Vector3.up * m_jumpForce * Time.deltaTime);
            //removing 1 spring counter each jump
            m_springCounter -= 1;
        }
        //Speed buff for Stoplight Green Light
        if (speedBuff)
        {
            pm.m_maxSpeed += m_buffedMaxSpeed;
            m_buffTimer += Time.deltaTime / 1000;
            //end of the speed buff
            if (m_buffTimer == buffDurationSeconds)
            {
                speedBuff = false;
                m_buffTimer = 0;
                pm.m_maxSpeed = pm.m_maxSpeed - m_buffedMaxSpeed;
            }
        }        
        
        //Speed nerf for Stoplight Red Light
        if (speedNerf)
        {
            pm.m_maxSpeed += m_nerfedMaxSpeed;
            m_nerfTimer += Time.deltaTime / 1000;
            //end of the speed nerf
            if (m_nerfTimer == buffDurationSeconds)
            {
                speedNerf = false;
                m_nerfTimer = 0;
                pm.m_maxSpeed = pm.m_maxSpeed - m_nerfedMaxSpeed;
            }
        }
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Ground") //isGrounded variable setting true for jumping
        {
            isGrounded = true;
        }
        
        if (other.gameObject.tag == "Spring_Collectible") //what happens on collision with collectible springs
        {
            m_springCounter += 1;
        }

        if (other.gameObject.tag == "Cloack_Collectible") //what happens on collision with collectible cloack
        {
            m_cloackCounter += 1;
        }

        if (other.gameObject.tag == "Wall_Collectible") //what happens on collision with collectible wall
        {
            m_placeWallCounter += 1;
        }

        if (other.gameObject.tag == "Stoplight_GreenLight") //what happens on collision with speed buff
        {
            speedBuff = true;
            m_nerfTimer = buffDurationSeconds;
        }

        if (other.gameObject.tag == "Stoplight_RedLight") //what happens on collision with speed nerf
        {
            speedNerf = true;
            m_buffTimer = buffDurationSeconds;
        }
    }

    void OnCollisionExit(Collision other)
    {
        //isGrounded variable setting false for jumping
        if (other.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }
    }
}