using Extension.Methods;
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

    private Image _image;

    void Start()
    {
        _image = GetComponent<Image>();
        _image.fillAmount = 0f;
    }

    void Update()
    {
        float curr = Mathematics.Percent(rigidBody.velocity.magnitude / 1000f, 1); // Percentage calculation based on the magnitude of the rigidbody
        _image.fillAmount = curr;
    }
}
