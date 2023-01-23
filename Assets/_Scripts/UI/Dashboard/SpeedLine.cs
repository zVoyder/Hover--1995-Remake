using Extension;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// This script is used to display the velocity of
/// momentum of a Rigidbody as a line in the user interface.
/// It uses an image element to represent the line and uses
/// the velocity of the target Rigidbody to determine the height of the image.
/// </summary>
[RequireComponent(typeof(Image))]
public class SpeedLine : MonoBehaviour
{
    public Rigidbody rigidBody;
    
    [Range(1, 20), SerializeField,
        Tooltip("Set this float to scale the fill amount, higher values means less fill.")]private float _fillDump = 1f;
    [Range(0,1)] public float cap = 1.0f;

    private Image _image;

    void Start()
    {
        _image = GetComponent<Image>();
        _image.fillAmount = 0f;
    }

    void FixedUpdate()
    {
        float len = Mathematics.Percent(rigidBody.velocity.magnitude / (_fillDump * 1000f), 1); // Percentage calculation based on the magnitude of the rigidbody
        len = len > cap ? cap : len; //Check if it is higher of the cap
        _image.fillAmount = len;
    }
}
