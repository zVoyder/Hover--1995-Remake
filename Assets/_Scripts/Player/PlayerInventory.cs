using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(RBPlayerMovement))]
public class PlayerInventory : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI springText;
    public TextMeshProUGUI wallText;
    public TextMeshProUGUI cloackText;
    public Image speedImage, cloakImage, wallImage, shieldImage;
    public Color invisibilityColor = Color.blue;
    public Color colorBuffedSpeed = Color.green, colorNerfedSpeed = Color.red;

    [Header("Counters")]
    public int springCounter; //counter for how many spring the player have to jump
    public int placeWallCounter; // counter for how many placeable wall does the player have
    public int invisibilityCounter; //counter for how many cloack for invisibility does the player have

    [Header("Durations")]
    [Range(1, 30)] public float speedBuffDuration = 1;
    [Range(1, 30)] public float speedNerfDuration = 1;
    [Range(1, 30)] public float shieldDuration = 1;
    [Range(1, 30)] public float invisibilityDuration = 10f;
    [Range(1, 30)] public float breakoutDuration = 1;
    [Range(1, 30)] public float wallLifeTime = 1;

    [Header("Placeables")]
    public GameObject placeableWall;
    [Range(0, 10)] public float wallSpawnRange;

    [Header("MinimapBreakout")]
    public GameObject minimap;

    [Header("Speed Modyfiers")]
    [Range(0, 1000)] public float buffedSpeed;
    [Range(0, 1000)] public float nerfedSpeed;

    [Header("Abilities")]
    public float jumpForce; //the force of the jump (can be setted in the inspector)

    [Header("Audio")]
    public AudioSFX jumpSFX;
    public AudioSFX invisibleSFX;
    public AudioSFX wallSFX; // Audio SFXs for the skills

    private bool _isGrounded; //bool to be sure the player must be touching the ground or the stairs before jump
    public bool IsGrounded { get => _isGrounded; }
    public bool IsInvisible { get; set; } = false;
    public bool IsNerfed { get; set; } = false;
    public bool IsBuffed { get; set; } = false;
    public bool IsShielded { get; set; } = false;


    private float _startedMaxSpeed;
    private float _alpha = 0f;
    private Rigidbody _rigidBody;
    private RBPlayerMovement pm;


    // Start is called before the first frame update
    void Start()
    {
        //getting Rigidbody and BoxCollidercomponents
        _rigidBody = GetComponent<Rigidbody>();
        pm = GetComponent<RBPlayerMovement>();
        _startedMaxSpeed = pm.maxSpeed;
        UIUpdateCounter();

        speedImage.fillAmount = 0;
        cloakImage.fillAmount = 0;
        wallImage.fillAmount = 0;
        shieldImage.fillAmount = 0;
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
        speedImage.color = colorBuffedSpeed;
        StartCoroutine(SpeedChangeFor(speedBuffDuration, buffedSpeed));
    }

    public void SpeedNerf()
    {
        speedImage.color = colorNerfedSpeed;
        StartCoroutine(SpeedChangeFor(speedNerfDuration, nerfedSpeed));
    }

    public void Shield()
    {
        StartCoroutine(ActivateShieldFor(shieldDuration));
    }

    public void Breakout()
    {
        StartCoroutine(BreakOutFor(breakoutDuration));
    }

    private IEnumerator BreakOutFor(float time)
    {
        minimap.SetActive(false);
        yield return new WaitForSeconds(time);
        minimap.SetActive(true);
    }

    private IEnumerator ActivateShieldFor(float time)
    {
        float timer = 0f;

        while (timer < time)
        {
            IsShielded = true;
            timer += Time.fixedDeltaTime;
            shieldImage.fillAmount = 1 - (timer / shieldDuration);
            yield return new WaitForFixedUpdate();
        }

        shieldImage.fillAmount = 0;
        IsShielded = false;

        yield return null;
    }

    private IEnumerator SpeedChangeFor(float time, float newSpeed)
    {
        float timer = 0f;

        while (timer < time)
        {
            pm.maxSpeed = newSpeed;
            timer += Time.fixedDeltaTime;
            speedImage.fillAmount = 1 - (timer / time);
            yield return new WaitForFixedUpdate();
        }

        pm.maxSpeed = _startedMaxSpeed;

        yield return null;
    }

    private void WallSpawn()
    {
        placeWallCounter--;
        UIUpdateCounter();

        Extension.Audios.PlayClipAtPoint(wallSFX, transform.position);
        Vector3 position = transform.position + transform.forward * -wallSpawnRange;
        Quaternion rotation = Quaternion.LookRotation(-transform.forward);
        GameObject wall = Instantiate(placeableWall, position, rotation);
        StartCoroutine(WallSpawnDuration(wall));
    }

    private IEnumerator WallSpawnDuration(GameObject wall)
    {
        float timer = 0f;

        while (timer < invisibilityDuration)
        {
            timer += Time.fixedDeltaTime;
            wallImage.fillAmount = 1 - (timer / wallLifeTime);
            yield return new WaitForFixedUpdate();
        }

        Destroy(wall);
        yield return null;
    }

    private void Jump() 
    {
        Extension.Audios.PlayClipAtPoint(jumpSFX, transform.position);
        _rigidBody.AddForce(Vector3.up * jumpForce); //jump effective movement
        springCounter--; //removing 1 spring counter each jump
        UIUpdateCounter();
    }

    private void Invisibility()
    {
        Extension.Audios.PlayClipAtPoint(invisibleSFX, transform.position);
        invisibilityCounter--;
        UIUpdateCounter();
        StartCoroutine(GoInvisibleFor(invisibilityDuration));
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

        float timer = 0f;

        while (timer < invisibilityDuration)
        {
            timer += Time.fixedDeltaTime;
            cloakImage.fillAmount = 1 - (timer / time);
            yield return new WaitForFixedUpdate();
        }

        _alpha = 0f;
        IsInvisible = false;
        yield return null;
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