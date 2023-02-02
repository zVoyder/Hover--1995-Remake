using Extension;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// This script is used to display the direction of
/// momentum of a Rigidbody as a line in the user interface.
/// It uses an image element to represent the line and uses
/// the velocity of the target Rigidbody to determine the rotation angle of the image.
/// </summary>
[RequireComponent(typeof(Image))]
public class MomentumLine : MonoBehaviour
{
    private Rigidbody rigidBody;

    private Image _image;

    public Rigidbody RigidBody { get => rigidBody; set => rigidBody = value; }

    void Start()
    {
        _image = GetComponent<Image>();
        _image.fillAmount = 0f;
    }

    void FixedUpdate()
    {
        float curr = Mathematics.Percent(RigidBody.velocity.magnitude / 1000f, 1); // Percentage calculation based on the magnitude of the rigidbody
        _image.fillAmount = curr;

         // Calculate the SignedAngle beetween the Vector3 of the velocity end the forward Vector3 of the transform of the rigidbody
        float angle = Vector3.SignedAngle(RigidBody.velocity, RigidBody.transform.forward, RigidBody.transform.up);
        _image.rectTransform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
