using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PortalTransporter : MonoBehaviour
{
    public string triggerTag = Constants.Tags.PLAYER;
    public Transform destination;
    public CameraFade fade;



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(triggerTag))
        {
            destination.rotation = other.transform.rotation;
            other.transform.SetPositionAndRotation(destination.position, destination.rotation);

            fade.DoFadeIn();
        }
    }

}
