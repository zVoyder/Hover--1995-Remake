using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BoostOnTrigger : MonoBehaviour
{
    [Header("Sets")]
    [Tooltip("Rotation speed"), Range(1, 10), SerializeField]
    private float _rotation = 10f;
    [Tooltip("Transform in the Center Dragging Force"), Range(1, 10), SerializeField]
    private float _drag = 1f;
    [Tooltip("Launch Force"), SerializeField]
    private float _impulseForce = 100f;
    [Tooltip("Time before throwing the object"), SerializeField]
    private float _timeToLaunch = 1f;

    [Header("Thresholds")]
    [Tooltip("Precision of the rotation, lower values means more precision"), SerializeField, Range(0.01f, 1f)]
    private float _rotationPrecision = 0.02f;
    [Tooltip("Precision of the position, lower values means more precision "), SerializeField, Range(0.05f, 5f)]
    private float _centerPrecision = 1f;

    [SerializeField] AudioClip _trigger, _launch;

    private AudioSource _audio;

    private void Start()
    {
        _audio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<RBPlayerMovement>(out RBPlayerMovement pl))
        {
            if (other.TryGetComponent<PlayerInventory>(out PlayerInventory pli) 
                && !pli.IsShielded)
            {
                Play(_trigger); // Play the triggered audio

                pl.rigidBody.velocity = Vector3.zero;
                pl.enabled = false;
                StartCoroutine(SetLaunch(pl.rigidBody));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<RBPlayerMovement>(out RBPlayerMovement pl))
        {
            pl.enabled = true;
        }
    }


    /// <summary>
    /// Set the rotation and position of the rigidbody to launch
    /// </summary>
    /// <param name="toThrow">Rigidbody to launch</param>
    /// <returns>WaitForFixedUpdate</returns>
    private IEnumerator SetLaunch(Rigidbody toThrow)
    {
        //Do it until the rotation and position are not the same of this transform position and rotation
        while (
            !Extension.Mathematics.Approximately(toThrow.transform.rotation, transform.rotation, _rotationPrecision) //rotation check
            || !Extension.Mathematics.Approximately(toThrow.transform.position.x, transform.position.x, _centerPrecision)
            || !Extension.Mathematics.Approximately(toThrow.transform.position.z, transform.position.z, _centerPrecision)) //position check
        {
            toThrow.transform.SetPositionAndRotation(
                
                Vector3.Lerp(toThrow.transform.position,
                    new Vector3(transform.position.x, toThrow.transform.position.y, transform.position.z), _drag * Time.fixedDeltaTime), //Set position based only on the Y
                
                Quaternion.Lerp(toThrow.transform.rotation,
                    new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w), _rotation * Time.fixedDeltaTime) //Set rotation based only on the Y
            
                );
            yield return new WaitForFixedUpdate(); //WaitForFixedUpdate where defaul value is 0.2f so 0.2 seconds
        }

        //Debug.Log(toThrow.transform.rotation + "  " + transform.rotation);

        StartCoroutine(LaunchIn(_timeToLaunch, toThrow)); //StartCaroutine to wait before launch
        yield return null;
    }

    /// <summary>
    /// Timer before actually launch the rigidbody
    /// </summary>
    /// <param name="time">time needed</param>
    /// <param name="rb">rigidbody</param>
    /// <returns>WaitForFixedUpdate</returns>
    private IEnumerator LaunchIn(float time, Rigidbody rb)
    {
        while (time > 0)
        {
            //Debug.Log(time);
            time -= Time.fixedDeltaTime; // fixedDeltaTime so it scales the time smoothly
            yield return new WaitForFixedUpdate(); // Wait
        }

        Play(_launch); // Play the launch audio
        rb.AddForce(transform.forward * _impulseForce, ForceMode.VelocityChange); //Add force
        yield return null;
    }


    private void Play(AudioClip clip)
    {
        _audio.clip = clip;
        _audio.Play();
    }
}
