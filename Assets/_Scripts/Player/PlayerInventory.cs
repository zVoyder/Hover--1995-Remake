using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]

public class PlayerInventory : MonoBehaviour
{
    public Text springText;
    public Text wallText;
    public Text cloackText;
    public GameObject placeableWall;
    [Range(0,10)]
    public float wallSpawnRange;
    [Range(1, 50)]
    public float wallLifeTime = 1;
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
    private new CapsuleCollider collider;
    RBPlayerMovement pm;

    // Start is called before the first frame update
    void Start()
    { 
        //getting Rigidbody and BoxCollidercomponents
        rigidBody = GetComponent<Rigidbody>();
        collider = GetComponent<CapsuleCollider>();
        pm = GetComponent<RBPlayerMovement>();
        UIUpdateCounter();
    }

    void FixedUpdate()
    {
        //bool for unable jumping if not colliding with ground objects as floor and stairs
        if (m_springCounter > 0 && isGrounded && Input.GetKeyDown(KeyCode.A))
        {
            //jump effective movement
            rigidBody.AddForce(Vector3.up * m_jumpForce * Time.deltaTime);
            //removing 1 spring counter each jump
            m_springCounter--;
            UIUpdateCounter();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S) && m_placeWallCounter > 0)//spawn placeable wall
        {
            WallSpawn();
            m_placeWallCounter--;
            UIUpdateCounter();
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
                pm.m_maxSpeed -= m_buffedMaxSpeed;
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
                pm.m_maxSpeed -= m_nerfedMaxSpeed;
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
            m_springCounter++;
            UIUpdateCounter();
        }

        if (other.gameObject.tag == "Cloack_Collectible") //what happens on collision with collectible cloack
        {
            m_cloackCounter++;
            UIUpdateCounter();
        }

        if (other.gameObject.tag == "Wall_Collectible") //what happens on collision with collectible wall
        {
            m_placeWallCounter++;
            UIUpdateCounter();
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
    private void WallSpawn()
    {
        Vector3 position = new Vector3(transform.position.x, transform.position.y, transform.position.z - wallSpawnRange);
        Quaternion rotation = transform.rotation;
        GameObject wall = Instantiate(placeableWall, position, rotation);
        Destroy(wall.gameObject, wallLifeTime);
    }
    private void UIUpdateCounter()
    {
        springText.text = m_springCounter.ToString();
        wallText.text = m_placeWallCounter.ToString();
        cloackText.text = m_cloackCounter.ToString();
    }
}