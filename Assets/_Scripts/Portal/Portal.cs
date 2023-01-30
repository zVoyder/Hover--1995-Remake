using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public Transform playerCamera;
    public Transform portal;
    public Transform otherPortal;


    private void Update()
    {
        Vector3 playerOffsetFromPortal = playerCamera.position - otherPortal.position;
        transform.position = portal.position + playerOffsetFromPortal;

        float angle = Quaternion.Angle(portal.rotation, otherPortal.rotation);

        Quaternion rotationDifference = Quaternion.AngleAxis(angle, Vector3.up);
        Vector3 camDirection = rotationDifference * playerCamera.forward;
        transform.rotation = Quaternion.LookRotation(camDirection, Vector3.up);
    
    }
}
