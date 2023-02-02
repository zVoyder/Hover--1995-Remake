using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(RBPlayerMovement))]
public class PlayerInventory : MonoBehaviour
{
    public TextMeshProUGUI springText;
    public TextMeshProUGUI wallText;
    public TextMeshProUGUI cloackText;

    public GameObject placeableWall;
    [Range(0,10)]
    public float wallSpawnRange;
    [Range(1, 50)]
    public float wallLifeTime = 1;


    public int springCounter; //counter for how many spring the player have to jump

    public int placeWallCounter; // counter for how many placeable wall does the player have

    public int invisibilityCounter; //counter for how many cloack for invisibility does the player have

    public float m_jumpForce; //the force of the jump (can be setted in the inspector)
    private bool _isGrounded; //bool to be sure the player must be touching the ground or the stairs before jump
    public bool IsGrounded { get => _isGrounded; }

    [Range(1, 30)]
    public float speedBuffDuration = 1, speedNerfDuration = 1; //duration of the buff/debuff (can be setted in the inspector)
    [Range(0, 1000)]
    public float buffedSpeed; //bool to enable temporary buff of maximum speed
    [Range(0, 1000)]
    public float nerfedSpeed; //bool to enable temporary nerf of maximum speed

    [Range(1, 60)]public float invisibilityCooldown = 10f;
    public Color invisibilityColor = Color.blue;
    private float _alpha = 0f;

    private Rigidbody _rigidBody;
    RBPlayerMovement pm;

    public AudioSFX jumpSFX, invisibleSFX, wallSFX; // Audio SFXs for the skills

    public bool IsInvisible { get; set; } = false;

    public bool IsNerfed { get; set; } = false;
    public bool IsBuffed { get; set; } = false;

    // Start is called before the first frame update
    void Start()
    {

        //getting Rigidbody and BoxCollidercomponents
        _rigidBody = GetComponent<Rigidbody>();
        pm = GetComponent<RBPlayerMovement>();
        UIUpdateCounter();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(InputManager.WALL) && placeWallCounter > 0)//spawn placeable wall
        {
            WallSpawn();
        }

        //bool for unable jumping if not colliding with ground objects as floor and stairs
        if (Input.GetKeyDown(InputManager.JUMP) && springCounter > 0 && _isGrounded)
        {
            Jump();
        }

        if(Input.GetKeyDown(InputManager.INVISIBLE) && invisibilityCounter > 0 && !IsInvisible)
        {
            Invisibility();
        }
    }

    public void IncreaseSpringCounter()
    {
        springCounter++;
        UIUpdateCounter();
    }

    public void IncreaseCloakCounter()
    {
        invisibilityCounter++;
        UIUpdateCounter();
    }

    public void IncreaseWallCounter()
    {
        placeWallCounter++;
        UIUpdateCounter();
    }

    public void SpeedBuff()
    {
        StartCoroutine(StartSpeedBuff());
    }

    public void SpeedNerf()
    {
        StartCoroutine(StartSpeedNerf());
    }

    private IEnumerator StartSpeedBuff()
    {
        if (!IsBuffed)
        {
            IsBuffed = true;
            float tempMaxSpeed = pm.maxSpeed;
            pm.maxSpeed = buffedSpeed;
            yield return new WaitForSeconds(speedBuffDuration);
            IsBuffed = false;
            pm.maxSpeed = tempMaxSpeed;
        }
    }

    private IEnumerator StartSpeedNerf()
    {
        if (!IsNerfed) {
            IsNerfed = true;

            float tempMaxSpeed = pm.maxSpeed;
            pm.maxSpeed = nerfedSpeed;
            yield return new WaitForSeconds(speedBuffDuration);
            IsNerfed = false;
            pm.maxSpeed = tempMaxSpeed;
        }
    }

    private void WallSpawn()
    {
        placeWallCounter--;
        UIUpdateCounter();

        Extension.Audios.PlayClipAtPoint(wallSFX, transform.position);
        Vector3 position = transform.position + transform.forward * -wallSpawnRange;
        Quaternion rotation = Quaternion.LookRotation(-transform.forward);
        GameObject wall = Instantiate(placeableWall, position, rotation);
        Destroy(wall.gameObject, wallLifeTime);
    }

    private void Jump() 
    {
        Extension.Audios.PlayClipAtPoint(jumpSFX, transform.position);
        _rigidBody.AddForce(Vector3.up * m_jumpForce); //jump effective movement
        springCounter--; //removing 1 spring counter each jump
        UIUpdateCounter();
    }

    private void Invisibility()
    {
        Extension.Audios.PlayClipAtPoint(invisibleSFX, transform.position);
        invisibilityCounter--;
        UIUpdateCounter();
        StartCoroutine(GoInvisibleFor(invisibilityCooldown));
    }

    private void UIUpdateCounter()
    {
        springText.text = springCounter.ToString();
        wallText.text = placeWallCounter.ToString();
        cloackText.text = invisibilityCounter.ToString();
    }


    private IEnumerator GoInvisibleFor(float time)
    {
        IsInvisible = true;
        _alpha = invisibilityColor.a;
        yield return new WaitForSeconds(time);
        _alpha = 0f;
        IsInvisible = false;
    }

    private void OnGUI()
    {
        GUI.color = new Color(invisibilityColor.r, invisibilityColor.g, invisibilityColor.b, _alpha);
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture); // Draw Texture with the size of the screen
    }

    void OnCollisionExit(Collision other)
    {
        //isGrounded variable setting false for jumping
        if (other.gameObject.tag == "Ground")
        {
            _isGrounded = false;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == Constants.Tags.GROUND) //isGrounded variable setting true for jumping
        {
            _isGrounded = true;
        }
        
    }
}